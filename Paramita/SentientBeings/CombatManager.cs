using Paramita.Items;
using RogueSharp.DiceNotation;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Paramita.SentientBeings
{
    public class CombatManager
    {
        Random random;



        // When we construct the CombatManager class we want to pass in references
        // to the player and the list of enemies.
        public CombatManager(Random random)
        {
            this.random = random;
        }




        // Use this method to resolve attacks between Figures
        public bool AttackRoll(SentientBeing attacker, SentientBeing defender)
        {
            bool IsHit = false;
            // Roll the die, add the attack bonus, and compare to the defender's armor class
            if ( DiceRoll("2d6", true) + attacker.AttackSkill - (attacker.Fatigue / 20) 
                 > DiceRoll("2d6", true) + defender.DefenseSkill - (defender.Fatigue / 10) )
            {
                IsHit = true;
            }
            return IsHit;
        }



        // This function returns the part of a regular humanoid hit by an attack
        // In future, size difference of attacker and defense should be factored into this algo
        // Also need to account for shield hits
        public HumanoidBodyParts GetHitLocationOnHumanoid(SentientBeing attacker, SentientBeing defender)
        {
            HumanoidBodyParts hitLocation = HumanoidBodyParts.Torso;

            int chance = DiceRoll("1d100", false);

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
         * This function conducts a DamageRoll after a successful AttackRoll roll
         * The calculations for the shield hit are missing at present
         * It also is missing the case of a head hit, which uses defender's head protection only
         */
        public int DamageRoll(SentientBeing attacker, Weapon attackWeapon, SentientBeing defender)
        {
            int damage = 0;

            int attack = attacker.Strength + attackWeapon.Damage + DiceRoll("2d6", true);
            int defense = defender.Protection + DiceRoll("2d6", true);

            if (attack - defense > 0)
                damage = attack - defense;

            return damage;
        }

        /*
         * This function simulates an open-ended roll of dice to generate a random number.
         * 
         * The function parses the dice type and number from @dice.
         * 
         * The flag @openEnded indicates whether bonus die rolls are gained with rolling
         * the highest number possible. OpenEnded rolls simulate improbable events
         * such as a weak attacker managing to hit a superior opponent by sheer luck. OpenEnded
         * dice rolls can theoretically go on forever since bonus rolls can result in more
         * bonus rolls.
         * 
         * The function parses the type of die and number of dice to roll plus a modifier from a 
         * string that is passed to it. 
         * 
         * It returns a total of the dice rolls as an integer.
         */
        private int DiceRoll(string dice, bool openEnded)
        {
            int totalRolled = 0;
            int roll = 0;
            int dieSize = 0;
            int dieNumber = 0;
            int modifier = 0;
             
            switch(dice)
            {
                case "1d6":
                    dieSize = 6;
                    dieNumber = 1;
                    break;
                case "1d6+1":
                    dieSize = 6;
                    dieNumber = 1;
                    modifier = 1;
                    break;
                case "2d6":
                    dieSize = 6;
                    dieNumber = 2;
                    break;
                case "1d100":
                    dieSize = 100;
                    dieNumber = 1;
                    break;
                default:
                    dieSize = 0;
                    dieNumber = 0;
                    break;
            }

            // Simulates an open-ended roll of dice. Any roll that comes out highest possible
            // yields another die roll and the total is reduced by 1. Dice continue to be rolled 
            // until all rolls are done.
            while(dieNumber != 0)
            {
                dieNumber--;
                roll = random.Next(1, dieSize) + modifier;
                if(roll == dieSize && openEnded == true)
                {
                    roll--;
                    dieNumber++;
                }
                totalRolled += roll;
            }

            return totalRolled;
        }
    }
}
