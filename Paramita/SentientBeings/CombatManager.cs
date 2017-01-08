using Paramita.Items;
using Paramita.Scenes;
using System;

namespace Paramita.SentientBeings
{
    public class CombatManager
    {
        Random random;
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
        int attackDamage;
        bool isShieldHit;

        // used in repel attack resolution
        bool attackRepelled;
        const int repelAttackDamage = 1;

        // used for rolling dice
        bool openEndedRoll;
        int dieSize;
        int diceLeftToRoll;
        int rollModifier;
        int totalRolled;
        int roll;




        // When we construct the CombatManager class we want to pass in references
        // to the player and the list of enemies.
        public CombatManager(Random random, GameScene gameScene)
        {
            this.random = random;
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
                AttackRoll(attacker, weapon, defender);
                ResolveAttackResult();
                CheckIfAttackKilledDefender();
            }
        }



        private void InitializeAttackVariables(SentientBeing att, Weapon weapon, SentientBeing def)
        {
            attacker = att;
            defender = def;
            attackResult = 0;
            attackDamage = 0;
            attWeapon = weapon;
            repelWeapon = defender.GetLongestWeapon();
            defCritHitFatPenalty = defender.FatigueCriticalPenalty;
            defParry = defender.Parry;
            defShieldProt = defender.ShieldProtection;
            isShieldHit = false;
        }



        private void CheckIfAttackRepelled()
        {
            if (CheckForRepelAttempt() == true)
                ResolveRepelAttack();
        }



        private void ResolveAttackResult()
        {
            if (attackResult > 0)
            {
                CheckForShieldHit();
                attackDamage = DamageRoll(attacker, attWeapon, defender);
                defender.TakeDamage(attackDamage);
                scene.PostNewStatus(attacker + " hit " + defender + ", doing " + attackDamage + " damage!");
            }
            else
            {
                scene.PostNewStatus(attacker + " missed!");
            }
        }



        private void CheckForShieldHit()
        {
            if (attackResult <= defParry)
            {
                isShieldHit = true;
                Console.WriteLine("Shield hit.");
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
            attackRepelled = false;
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
            if (DamageRoll(defender, repelWeapon, attacker) > 0)
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



        private void AttackRoll(SentientBeing attacker, Weapon weapon, SentientBeing defender)
        {
            int attackSkill = attacker.AttackSkill + weapon.AttackModifier;
            int attFatigue = attacker.FatigueAttPenalty;
            int defenseSkill = defender.DefenseSkill; // SentientBeing.DefenseSkill.Get() includes weapons modifiers
            int defFatigue = defender.FatigueDefPenalty;

            // roll the dice
            int attRoll = OpenEndedDiceRoll("2d6");
            int defRoll = OpenEndedDiceRoll("2d6");

            // add the dice rolls to other factors to arrive at total attack and defense scores
            int attack = attRoll + attackSkill - attFatigue;
            int defense = defRoll + defenseSkill - defFatigue;

            // report the details of the attack and defense scores
            scene.PostNewStatus(attacker + " rolled " + attRoll + " plus attack skill "
                + attackSkill + " and weapon mod of " + weapon.AttackModifier + " minus fatigue mod of "
                + attFatigue + ".");

            scene.PostNewStatus(defender + " rolled " + defRoll + " plus total defense skill of "
                + defenseSkill + " minus fatigue mod of " + defFatigue + ".");

            // return the difference of the attack and defense scores
            attackResult = attack - defense;
        }



        /*
         * This function conducts a DamageRoll after a successful AttackRoll hit
         * The calculations for the shield hit are missing at present
         * It also is missing the case of a head hit, which uses defender's head protection only
         */
        public int DamageRoll(SentientBeing attacker, Weapon attackWeapon, SentientBeing defender)
        {
            int defProtection = CalculateDefenderProtection(defender);

            int damage = 0;

            int attack = attacker.Strength + attackWeapon.Damage + OpenEndedDiceRoll("2d6");
            int defense = defProtection + OpenEndedDiceRoll("2d6");

            if (attack - defense > 0)
                damage = attack - defense;

            return damage;
        }



        private int CalculateDefenderProtection(SentientBeing defender)
        {
            int protection = defender.Protection;

            if (isShieldHit == true)
                protection += defShieldProt;

            if (CriticalHitCheck(defender) == true)
            {
                protection = protection / 2;
            }

            return protection;
        }



        // When a hit is scored, a critical hit check is rolled. If the hit is a critical,
        // the defender's protection is halved.
        private bool CriticalHitCheck(SentientBeing defender)
        {
            int threshold = 3;

            int criticalCheckRoll = OpenEndedDiceRoll("2d6") - defCritHitFatPenalty;
            if (criticalCheckRoll < threshold)
            {
                scene.PostNewStatus("A critical hit was scored! (" + roll + "-" + defCritHitFatPenalty + ")");
                return true;
            }

            scene.PostNewStatus("No critical hit. (" + roll + "-" + defCritHitFatPenalty + ")");
            return false;
        }



        // perform a morale check against a supplied number and an optional bonus difficulty
        private bool MoraleCheck(SentientBeing checker, SentientBeing other, int checkAgainst, int bonus = 0)
        {
            int checkerMorale = checker.Morale;
            int sizeDifference = checker.Size - other.Size;
            // roll the dice
            int checkerRoll = OpenEndedDiceRoll("2d6");
            int checkAgainstRoll = OpenEndedDiceRoll("2d6");

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

            int chance = ClosedEndedDiceRoll("1d100");

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



        /*
         *   Dice Roll Functions
         *   
         *   Currently, a dice roll must correspond to a code found in the ParseDiceCode() function!
         *
         *   @diceCode should use D&D conventions:
         *     * number of dice + "d" + max roll of die + any after-roll modifier (+/- integer)
         *   
         *   There are two kinds of dice rolls: Open-ended and closed-ended.
         *   
         *   Open-ended dice rolls get bonus rolls whenever any roll is the max for that die size.
         *   Example: Roll 2 d6 dice. Both roll 6. An open-ended roll means 2 more bonus rolls are
         *            made and added to final total. Bonus rolls can yield more bonus rolls.
         *   
         *   Closed-ended dice rolls only roll the initial dice indicated by the dice code.
         */
        private int OpenEndedDiceRoll(string dice)
        {
            openEndedRoll = true;
            DiceRoll(dice);

            return totalRolled;
        }



        private int ClosedEndedDiceRoll(string dice)
        {
            openEndedRoll = false;
            DiceRoll(dice);

            return totalRolled;
        }



        private void DiceRoll(string dice)
        {
            InitializeDiceRollVariables();
            ParseDiceRollCode(dice);
            RollDice();
        }



        private void InitializeDiceRollVariables()
        {
            totalRolled = 0;
            roll = 0;
            dieSize = 0;
            diceLeftToRoll = 0;
            rollModifier = 0;
        }



        // Sets the variables for a dice roll based on @diceCode
        //
        // Presently, I am just adding roll types to a switch statement
        // In the future, this should be turned into a string parser if the switch statement
        // gets longer than 10 items
        private void ParseDiceRollCode(string diceCode)
        {
            switch (diceCode)
            {
                case "1d6":
                    dieSize = 6;
                    diceLeftToRoll = 1;
                    break;
                case "1d6+1":
                    dieSize = 6;
                    diceLeftToRoll = 1;
                    rollModifier = 1;
                    break;
                case "2d6":
                    dieSize = 6;
                    diceLeftToRoll = 2;
                    break;
                case "1d100":
                    dieSize = 100;
                    diceLeftToRoll = 1;
                    break;
                default:
                    dieSize = 0;
                    diceLeftToRoll = 0;
                    Console.WriteLine("Unknown DiceCode.");
                    break;
            }
        }



        private void RollDice()
        {
            while (diceLeftToRoll > 0)
            {
                RollDie();
                if(openEndedRoll == true)
                    CheckForBonusRoll();
                totalRolled += roll;
            }
        }



        private void RollDie()
        {
            diceLeftToRoll--;
            roll = random.Next(1, dieSize + 1) + rollModifier;
            Console.WriteLine("Rolled " + roll);
        }



        private void CheckForBonusRoll()
        {
            if (roll == dieSize)
            {
                Console.WriteLine("Bonus roll!");
                roll--;
                diceLeftToRoll++;
            }
        }
    }
}
