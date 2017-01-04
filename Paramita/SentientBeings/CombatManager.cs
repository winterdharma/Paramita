using Paramita.Items;
using Paramita.Scenes;
using System;

namespace Paramita.SentientBeings
{
    public class CombatManager
    {
        Random random;
        GameScene scene;



        // When we construct the CombatManager class we want to pass in references
        // to the player and the list of enemies.
        public CombatManager(Random random, GameScene gameScene)
        {
            this.random = random;
            scene = gameScene;
        }




        // Conducts an attack roll for a single attack on a defending SentientBeing
        // Returns a bool indicating whether the attack hit or not.
        // It also posts messages to the GameScene indicating what happened.
        public bool AttackRoll(SentientBeing attacker, Weapon weapon, SentientBeing defender)
        {
            // check to see if defender gets a repel attack and resolve it if it does
            if(CheckForRepelAttempt(attacker, weapon, defender) == true)
            {
                Weapon repelWeapon = defender.GetLongestWeapon();

                scene.PostNewStatus(defender + " trys to repel attack. (Att length: " + weapon.Length 
                    + ", Def length: " + repelWeapon.Length + ")");


                int attackRepelled = RepelAttackRoll(defender, repelWeapon, attacker);
                if(attackRepelled > 0)
                {
                    scene.PostNewStatus(defender + " succeeds!");
                    bool moraleCheck = MoraleCheck(attacker, defender, 10, attackRepelled);
                    if(moraleCheck == false)
                    {
                        // attacker aborts the attack
                        scene.PostNewStatus(attacker + " aborts attack!");
                        return false;
                    }
                    else
                    {
                        scene.PostNewStatus(attacker + " continues to attack.");
                        // attacker goes ahead with attack, but might take damage from
                        // the defender's weapon
                        int damage = DamageRoll(defender, repelWeapon, attacker);
                        if (damage > 1)
                        {
                            // no more than 1 pt of damage is possible
                            damage = 1;
                            attacker.TakeDamage(damage);
                            scene.PostNewStatus(attacker + " takes " + damage + "pt of dmg!");
                            if(attacker.IsDead == true)
                            {
                                scene.PostNewStatus(attacker + " is killed!");
                                return false;
                            }
                        }
                    }
                }
            }

            bool isHit = false;

            int attackSkill = attacker.AttackSkill + weapon.AttackModifier;
            int attFatigue = attacker.FatigueAttPenalty;
            int defenseSkill = defender.DefenseSkill; // SentientBeing.DefenseSkill.Get() includes weapons modifiers
            int defFatigue = defender.FatigueDefPenalty;

            // roll the dice
            int attRoll = DiceRoll("2d6", true);
            int defRoll = DiceRoll("2d6", true);
            
            // add the dice rolls to other factors to arrive at total attack and defense scores
            int attack = attRoll + attackSkill - attFatigue;
            int defense = defRoll + defenseSkill - defFatigue;

            // check for a hit
            if (attack > defense)
                isHit = true;

            string verb;
            if (isHit == true)
                verb = "hit";
            else
                verb = "missed";

            scene.PostNewStatus(attacker + " rolled " + attRoll + " plus attack skill "
                + attackSkill + " and weapon mod of " + weapon.AttackModifier + " minus fatigue mod of " 
                + attFatigue + ".");
            scene.PostNewStatus(defender + " rolled " + defRoll + " plus total defense skill of "
                + defenseSkill + " minus fatigue mod of " + defFatigue + ".");
            scene.PostNewStatus(attacker + " " + verb + "!");

            return isHit;
        }



        // checks to see if the defender gets a repel attempt
        // when a defender has a weapon longer than the one the attacker is using,
        // he is given a chance to use it to block the attack
        private bool CheckForRepelAttempt(SentientBeing attacker, Weapon weapon, SentientBeing defender)
        {
            // first, find the length of the longest weapon the defender has available
            int maxDefWeaponLength = defender.GetLongestWeapon().Length;

            // check if the defender's longest weapon is longer than the attacker's weapon
            if (maxDefWeaponLength > weapon.Length)
                return true;

            return false;
        }



        // RepelAttack is the same as a normal AttackRoll, except that the difference between
        // the attack and defense scores is returned instead of a bool
        private int RepelAttackRoll(SentientBeing attacker, Weapon weapon, SentientBeing defender)
        {
            int attackSkill = attacker.AttackSkill + weapon.AttackModifier;
            int attFatigue = attacker.FatigueAttPenalty;
            int defenseSkill = defender.DefenseSkill; // SentientBeing.DefenseSkill.Get() includes weapons modifiers
            int defFatigue = defender.FatigueDefPenalty;

            // roll the dice
            int attRoll = DiceRoll("2d6", true);
            int defRoll = DiceRoll("2d6", true);

            // add the dice rolls to other factors to arrive at total attack and defense scores
            int attack = attRoll + attackSkill - attFatigue;
            int defense = defRoll + defenseSkill - defFatigue;

            // check for a hit
            return attack - defense;
        }



        // perform a morale check against a supplied number and an optional bonus difficulty
        private bool MoraleCheck(SentientBeing checker, SentientBeing other, int checkAgainst, int bonus = 0)
        {
            int checkerMorale = checker.Morale;
            int sizeDifference = checker.Size - other.Size;
            // roll the dice
            int checkerRoll = DiceRoll("2d6", true);
            int checkAgainstRoll = DiceRoll("2d6", true);

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
         * This function conducts a DamageRoll after a successful AttackRoll hit
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
         * This function simulates rolling dice to generate a random number.
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
                roll = random.Next(1, dieSize + 1) + modifier;
                Console.WriteLine("Rolled " + roll);
                if(roll == dieSize && openEnded == true)
                {
                    Console.WriteLine("Bonus roll!");
                    roll--;
                    dieNumber++;
                }
                totalRolled += roll;
            }

            return totalRolled;
        }
    }
}
