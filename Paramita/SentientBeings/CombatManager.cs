using Paramita.Items;
using Paramita.Scenes;
using System;

namespace Paramita.SentientBeings
{
    public class CombatManager
    {
        Random random;
        GameScene scene;
        int repelAttackDmg = 1;

        // used for rolling dice
        bool openEndedRoll;
        int dieSize;
        int dieNumber;
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




        // Conducts an attack roll for a single attack on a defending SentientBeing
        // Returns a bool indicating whether the attack hit or not.
        // It also posts messages to the GameScene indicating what happened.
        public void ResolveAttack(SentientBeing attacker, Weapon weapon, SentientBeing defender)
        {
            // check to see if defender gets a repel attack and resolve it if it does
            // A return of true indicates the original attack was stopped
            if (ResolveRepelAttack(attacker, weapon, defender) == true)
                return;

            // if the repel attempt failed to stop the attack,
            // conduct an AttackRoll - a result > 0 indicates a hit is scored
            int attackResult = AttackRoll(attacker, weapon, defender);

            // resolve the DamageRoll if a hit was scored and apply damage
            int damage = 0;
            if(attackResult > 0)
            {
                damage = DamageRoll(attacker, weapon, defender);
                defender.TakeDamage(damage);
                scene.PostNewStatus(attacker + " hit " + defender + ", doing " + damage + " damage!");
            }
            else
            {
                scene.PostNewStatus(attacker + " missed!");
            }
        }



        // Conducts the repel attack check and resolves the repel attack if it is attempted
        // Returns:
        //      * True if the attacker's original attack is stopped
        //      * False if the attacker's original attack is not stopped
        private bool ResolveRepelAttack(SentientBeing attacker, Weapon weapon, SentientBeing defender)
        {
            int repelAttResult = 0;
            bool moraleCheck = false;
            int damage = 0;

            // check if the defender can attempt a repel attack
            if (CheckForRepelAttempt(attacker, weapon, defender) == false)
                return false;
            
            // get the weapon the defender will use for the repel attack
            Weapon repelWeapon = defender.GetLongestWeapon();

            // report that the defender is attempting to repel the attack
            scene.PostNewStatus(defender + " trys to repel attack. (Att length: " + weapon.Length
                + ", Def length: " + repelWeapon.Length + ")");

            // conduct an AttackRoll with the defender as attacker
            repelAttResult = AttackRoll(defender, repelWeapon, attacker);

            // if the repel attack succeeds, attacker rolls a morale check
            if (repelAttResult > 0)
                moraleCheck = MoraleCheck(attacker, defender, 10, repelAttResult);
            // if the repel attack fails, return false
            else
                return false;

            // if the attacker fails the morale check, he aborts his attack
            if (moraleCheck == false)
            {
                scene.PostNewStatus(defender + " succeeds!");
                scene.PostNewStatus(attacker + " aborts attack!");
                return true;
            }
            // if the attacker passes the morale check, he presses on and is hit
            // by defender's repel weapon
            else
            {
                scene.PostNewStatus(attacker + " continues to attack.");
                damage = DamageRoll(defender, repelWeapon, attacker);
            }

            // check if the attacker takes repel attack damage
            if (damage > 0)
            {
                attacker.TakeDamage(repelAttackDmg);
                scene.PostNewStatus(attacker + " takes 1 pt of damage!");
            }

            // check if the attack was killed by the repel attack
            if (attacker.IsDead == true)
            {
                scene.PostNewStatus(attacker + " is killed!");
                return true;
            }
            // if not returned yet, the attacker still gets to attack
            return false;
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
        private int AttackRoll(SentientBeing attacker, Weapon weapon, SentientBeing defender)
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
            return attack - defense;
        }



        /*
         * This function conducts a DamageRoll after a successful AttackRoll hit
         * The calculations for the shield hit are missing at present
         * It also is missing the case of a head hit, which uses defender's head protection only
         */
        public int DamageRoll(SentientBeing attacker, Weapon attackWeapon, SentientBeing defender)
        {
            int defProtection = defender.Protection;

            // check to see if the attacker scored a critical hit
            if (CriticalHitCheck(defender) == true)
            {
                defProtection = defProtection / 2;
            }

            int damage = 0;

            int attack = attacker.Strength + attackWeapon.Damage + OpenEndedDiceRoll("2d6");
            int defense = defProtection + OpenEndedDiceRoll("2d6");

            if (attack - defense > 0)
                damage = attack - defense;

            return damage;
        }



        // When a hit is scored, a critical hit check is rolled. If the hit is a critical,
        // the defender's protection is halved.
        private bool CriticalHitCheck(SentientBeing defender)
        {
            int fatiguePenalty = defender.Fatigue / 15;
            int roll = OpenEndedDiceRoll("2d6");
            int threshold = 3;

            int criticalCheckRoll = roll - fatiguePenalty;
            if (criticalCheckRoll < threshold)
            {
                scene.PostNewStatus("A critical hit was scored! (" + roll + "-" + fatiguePenalty + ")");
                return true;
            }
            scene.PostNewStatus("No critical hit. (" + roll + "-" + fatiguePenalty + ")");
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
         *   Example: Roll 2 d6 dice. Both roll 6. An open-ended rolls means 2 more bonus rolls are
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
            dieNumber = 0;
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
                    dieNumber = 1;
                    break;
                case "1d6+1":
                    dieSize = 6;
                    dieNumber = 1;
                    rollModifier = 1;
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
                    Console.WriteLine("Unknown DiceCode.");
                    break;
            }
        }



        private void RollDice()
        {
            while (dieNumber != 0)
            {
                RollDie();
                if(openEndedRoll == true)
                    CheckForBonusRoll();
                totalRolled += roll;
            }
        }



        private void RollDie()
        {
            dieNumber--;
            roll = random.Next(1, dieSize + 1) + rollModifier;
            Console.WriteLine("Rolled " + roll);
        }



        private void CheckForBonusRoll()
        {
            if (roll == dieSize)
            {
                Console.WriteLine("Bonus roll!");
                roll--;
                dieNumber++;
            }
        }
    }
}
