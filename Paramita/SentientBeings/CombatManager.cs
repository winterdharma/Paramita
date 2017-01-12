﻿using Paramita.Items;
using Paramita.Mechanics;
using Paramita.Scenes;
using System;

namespace Paramita.SentientBeings
{
    public class CombatManager
    {
        GameScene scene;

        // used in combat resolution
        SentientBeing attacker;
        SentientBeing defender;
        Weapon attWeapon;
        Weapon repelWeapon;

        // used in attack resolution
        int defCritHitFatPenalty;
        int defParry;
        int defShieldProt;
        int attackResult;

        // used in repel attack resolution
        bool attackRepelled;
        const int repelAttackDamage = 1;

        // used in attack rolls
        int attackSkill;
        int attFatigue;
        int defenseSkill;
        int defFatigue;
        int attackScore;
        int defenseScore;
        int multipleAttackPenalty;

        //used in damage rolls
        int defProtection;
        int damage;



        // When we construct the CombatManager class we want to pass in references
        // to the player and the list of enemies.
        public CombatManager(Random random, GameScene gameScene)
        {
            scene = gameScene;
        }




        /*
         *  Resolve Attack Functions
         *  
         *  When a sentient being attacks another sentient being, a repel attack
         *  check is first done to see if the attack is aborted.
         *  
         *  If not, an attack roll is made to see if the attack hits the defender.
         *  
         *  If so, a damage roll is made to see if damage is done to the defender.
         *  
         *  Finally, a check is done to see if the defender was killed.
         */
        public void ResolveAttack(SentientBeing attacker, Weapon weapon, SentientBeing defender)
        {
            InitializeAttackVariables(attacker, weapon, defender);
            CheckIfAttackRepelled();
            if (attackRepelled == false)
            {
                defender.IncrementTimesAttacked();
                attacker.AddEncumbranceToFatigue();

                AttackRoll(attacker, weapon, defender);
                ResolveAttackResult();
                CheckIfAttackKilledDefender();
            }
        }


        private void InitializeAttackVariables(SentientBeing att, Weapon weapon, SentientBeing def)
        {
            attacker = att;
            defender = def;
            attWeapon = weapon;
            repelWeapon = defender.GetLongestWeapon();
            defCritHitFatPenalty = defender.FatigueCriticalPenalty;
            defParry = defender.Parry;
            defShieldProt = defender.ShieldProtection;
            attackRepelled = false;
        }


        private void CheckIfAttackRepelled()
        {
            if (CheckForRepelAttempt() == true)
            {
                attacker.IncrementTimesAttacked();
                defender.AddEncumbranceToFatigue();

                ResolveRepelAttack();
            }
        }


        private void ResolveAttackResult()
        {
            if (attackResult > 0)
            {
                DamageRoll(attacker, attWeapon, defender);
                defender.TakeDamage(damage);
                scene.PostNewStatus(attacker + " hit " + defender + ", doing " + damage + " damage!");
            }
            else
            {
                scene.PostNewStatus(attacker + " missed!");
            }
        }


        private void CheckIfAttackKilledDefender()
        {
            if (defender.IsDead == true)
                scene.PostNewStatus(defender + " is killed!");
        }



        /*
         *  Repel Attack Functions
         *  
         *  When a defender is attacked, he can attempt to repel the attack if it has a weapon
         *  with greater length than the attack weapon.
         *  
         *  If so, the defender attempts to interrupt the original attack with a repel attack.
         *  
         *  If the repel attack is successful, the attacker rolls a morale check to see if
         *  it gives up on the original attack.
         *  
         *  If the attacker passes the morale check, it continues to attack, but the defender
         *  did hit it with the repel attack and a damage roll is made. If the damage is greater
         *  than zero, the attacker takes damage equal to @repelAttackDamage.
         *  
         *  If the attacker happened to be killed by the repel attack damage, his attack
         *  is aborted.
         */
        private bool CheckForRepelAttempt()
        {
            if (repelWeapon.Length > attWeapon.Length)
                return true;
            return false;
        }


        private void ResolveRepelAttack()
        {
            InitializeRepelAttackVariables();

            scene.PostNewStatus(defender + " trys to repel attack. (Att length: " + attWeapon.Length
                + ", Def length: " + repelWeapon.Length + ")");

            RepelAttackRoll();
            ResolveRepelAttackResult();
        }


        private void InitializeRepelAttackVariables()
        {
        }


        private void RepelAttackRoll()
        {
            AttackRoll(defender, repelWeapon, attacker);
        }


        private void ResolveRepelAttackResult()
        {
            if (attackResult > 0)
                RepelAttackMoraleCheck();

            CheckIfRepelAttackKilledAttacker();
        }


        private void RepelAttackMoraleCheck()
        {
            if(MoraleCheck(attacker, defender, 10, attackResult) == true)
                RepelAttackDamageRoll();
            else
                attackRepelled = true;
        }


        private void RepelAttackDamageRoll()
        {
            DamageRoll(defender, repelWeapon, attacker);
            if ( damage > 0)
            {
                attacker.TakeDamage(repelAttackDamage);
                scene.PostNewStatus(attacker + " takes " + repelAttackDamage + " pt of damage!");
            }
        }


        private void CheckIfRepelAttackKilledAttacker()
        {
            if (attacker.IsDead == true)
            {
                scene.PostNewStatus(attacker + " is killed!");
                attackRepelled = true;
            }
        }



        /*
         *  An attack roll consists of a competitive roll of 2d6 (open-ended) between the attacker
         *  and defender. 
         *  
         *  The attacker's attack skill, weapon modifiers, and fatigue penalties are added to its roll.
         *  The defender's defense skill, weapon modifiers, and fatigue penalities are added to its roll.
         *  
         *  A message is displayed on the GameScene's Status Message panel indicated the totals.
         *  
         *  The difference between the two scores is stored for use in further resolution. A positive
         *  result indicates the defender was hit.
         */
        private void AttackRoll(SentientBeing attacker, Weapon weapon, SentientBeing defender)
        {
            InitializeAttackRollVariables(attacker, weapon, defender);
            Dice dice = new Dice(2);
            attackScore = dice.OpenEndedDiceRoll() + attackSkill - attFatigue;
            defenseScore = dice.OpenEndedDiceRoll() + defenseSkill - defFatigue - multipleAttackPenalty;

            ReportAttackRollToScene();
     
            attackResult = attackScore - defenseScore;
        }

        
        private void InitializeAttackRollVariables(SentientBeing attacker, Weapon weapon, SentientBeing defender)
        {
            attackSkill = attacker.AttackSkill + weapon.AttackModifier;
            attFatigue = attacker.FatigueAttPenalty;
            defenseSkill = defender.DefenseSkill;
            defFatigue = defender.FatigueDefPenalty;
            multipleAttackPenalty = (defender.TimesAttacked - 1) * 2;
        }


        private void ReportAttackRollToScene()
        {
            scene.PostNewStatus(attacker + " rolled " + attackScore + "(attSkill: " + attackSkill
                + ", fatigue: " + attFatigue + ")");
            scene.PostNewStatus(defender + " rolled " + defenseScore + "(defSkill: " + defenseSkill
                + ", fatigue: " + defFatigue + ", multAtt: " + multipleAttackPenalty + ")");
        }



        /*
         * This function conducts a DamageRoll after a successful AttackRoll hit
         * The calculations for the shield hit are missing at present
         * It also is missing the case of a head hit, which uses defender's head protection only
         */
        public void DamageRoll(SentientBeing attacker, Weapon attackWeapon, SentientBeing defender)
        {
            CalculateDefenderProtection(defender);
            Dice dice = new Dice(2);
            attackScore = attacker.Strength + attackWeapon.Damage + dice.OpenEndedDiceRoll();
            defenseScore = defProtection + dice.OpenEndedDiceRoll();

            damage = attackScore - defenseScore;
        }


        private void CalculateDefenderProtection(SentientBeing defender)
        {
            defProtection = defender.Protection;

            if (CheckForShieldHit() == true)
                defProtection += defShieldProt;

            if (CriticalHitCheck(defender) == true)
            {
                defProtection = defProtection / 2;
            }
        }


        // If an attacker rolls higher than the defender's total defense score, the
        // defender is hit, but if the margin of the win doesn't beat the defender's
        // shield parry, then defender was able to block the hit with his shield.
        private bool CheckForShieldHit()
        {
            if (attackResult <= defParry)
            {
                Console.WriteLine("Shield hit.");
                return true;
            }
            return false;
        }


        // When a hit is scored, a critical hit check is rolled. If the hit is a critical,
        // the defender's protection is halved.
        private bool CriticalHitCheck(SentientBeing defender)
        {
            int threshold = 3;
            Dice dice = new Dice(2);
            int criticalCheckRoll = dice.OpenEndedDiceRoll() - defCritHitFatPenalty;
            if (criticalCheckRoll < threshold)
            {
                scene.PostNewStatus("A critical hit was scored! (" + (criticalCheckRoll - defCritHitFatPenalty) 
                    + "-" + defCritHitFatPenalty + ")");
                return true;
            }
            return false;
        }


        private bool MoraleCheck(SentientBeing checker, SentientBeing other, int checkAgainst, int bonus = 0)
        {
            int checkerMorale = checker.Morale;
            int sizeDifference = checker.Size - other.Size;
            Dice dice = new Dice(2);
            int checkerRoll = dice.OpenEndedDiceRoll();
            int checkAgainstRoll = dice.OpenEndedDiceRoll();

            int checkerTotal = checkerRoll + checkerMorale + sizeDifference;
            int checkAgainstTotal = checkAgainstRoll + checkAgainst + (bonus / 2);

            if (checkerTotal > checkAgainstTotal)
                return true;

            return false;
        }



        // This function returns the part of a regular humanoid hit by an attack
        // In future, size difference of attacker and defense should be factored into this algo
        // Also need to account for shield hits
        public HumanoidBodyParts GetHitLocationOnHumanoid(SentientBeing attacker, SentientBeing defender)
        {
            HumanoidBodyParts hitLocation = HumanoidBodyParts.Torso;
            Dice dice = new Dice(1, Die.d100);
            int chance = dice.ClosedEndedDiceRoll();

            // chance < 51 is a Torso hit, which is the default value for hitLocation
            if(chance > 50 && chance < 71) 
            {
                if (chance < 61)
                    hitLocation = HumanoidBodyParts.RightArm;
                else
                    hitLocation = HumanoidBodyParts.LeftArm;
            }
            else if(chance > 70 && chance < 91)
            {
                if (chance < 81)
                    hitLocation = HumanoidBodyParts.RightLeg;
                else
                    hitLocation = HumanoidBodyParts.LeftLeg;
            }
            else if(chance > 90)
            {
                hitLocation = HumanoidBodyParts.Head;
            }

            return hitLocation;
        }
    }
}
