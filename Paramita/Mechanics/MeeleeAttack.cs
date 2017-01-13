using Paramita.Items;
using Paramita.Scenes;
using Paramita.SentientBeings;
using System;

namespace Paramita.Mechanics
{
    public class MeeleeAttack
    {
        Dice dice2d6;
        Dice dice1d100;

        // used in combat resolution
        SentientBeing attacker;
        SentientBeing defender;
        Weapon attackWeapon;
        Weapon repelWeapon;

        // used in repel attack resolution
        private const int repelMoraleCheck = 10;
        private const int repelAttackDamage = 1;



        public MeeleeAttack(SentientBeing attacker, Weapon weapon, SentientBeing defender)
        {
            this.attacker = attacker;
            attackWeapon = weapon;
            this.defender = defender;
            repelWeapon = defender.GetLongestWeapon();
            dice2d6 = new Dice(2);
            dice1d100 = new Dice(1, Die.d100);
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
        public void ResolveAttack()
        {
            if (CheckIfAttackRepelled())
                return;

            defender.IncrementTimesAttacked();
            attacker.AddEncumbranceToFatigue();

            var attackRoll = new AttackRoll(attacker, attackWeapon, defender);

            GameScene.PostNewStatus(attackRoll.AttackerReport);
            GameScene.PostNewStatus(attackRoll.DefenderReport);

            ResolveAttackResult(attackRoll);
            CheckIfAttackKilledDefender(defender);
        }



        private bool CheckIfAttackRepelled()
        {
            if (CheckForRepelAttempt())
            {
                attacker.IncrementTimesAttacked();
                defender.AddEncumbranceToFatigue();

                GameScene.PostNewStatus(defender + " trys to repel attack. (Att length: " + attackWeapon.Length
                + ", Def length: " + repelWeapon.Length + ")");

                return ResolveRepelAttack();
            }
            return false;
        }

        private void ResolveAttackResult(AttackRoll attackRoll)
        {
            if (attackRoll.AttackRollResult > 0)
            {
                var damageRoll = new DamageRoll(attackRoll);
                defender.TakeDamage(damageRoll.Damage);
                GameScene.PostNewStatus(attacker + " hit " + defender + " doing " + damageRoll.Damage + " damage!");
            }
            else
            {
                GameScene.PostNewStatus(attacker + " missed!");
            }
        }


        private void CheckIfAttackKilledDefender(SentientBeing defender)
        {
            if (defender.IsDead == true)
                GameScene.PostNewStatus(defender + " is killed!");
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
            if (repelWeapon.Length > attackWeapon.Length)
                return true;
            return false;
        }


        private bool ResolveRepelAttack()
        {
            var attackRoll = new AttackRoll(defender, repelWeapon, attacker);

            GameScene.PostNewStatus(attackRoll.AttackerReport);
            GameScene.PostNewStatus(attackRoll.DefenderReport);

            return ResolveRepelAttackResult(attackRoll);
        }


        private bool ResolveRepelAttackResult(AttackRoll attackRoll)
        {
            bool attackRepelled = false;
            if (attackRoll.AttackRollResult > 0)
                attackRepelled = RepelAttackMoraleCheck(attackRoll);

            if (CheckIfRepelAttackKilledAttacker())
                attackRepelled = true;

            return attackRepelled;
        }


        private bool RepelAttackMoraleCheck(AttackRoll attackRoll)
        {
            if (MoraleCheck(attackRoll, repelMoraleCheck))
            {
                RepelAttackDamageRoll(attackRoll);
                return false;
            }
            else
                return true;
        }


        private void RepelAttackDamageRoll(AttackRoll attackRoll)
        {
            var damageRoll = new DamageRoll(attackRoll);
            if (damageRoll.Damage > 0)
            {
                attacker.TakeDamage(repelAttackDamage);
                GameScene.PostNewStatus(attacker + " takes " + repelAttackDamage + " pt of damage!");
            }
        }


        private bool CheckIfRepelAttackKilledAttacker()
        {
            if (attacker.IsDead)
            {
                GameScene.PostNewStatus(attacker + " is killed!");
                return true;
            }
            return false;
        }


        


        private bool MoraleCheck(AttackRoll attackRoll, int checkAgainst, int bonus = 0)
        {
            int checkerMorale = attackRoll.Defender.Morale;
            int sizeDifference = attackRoll.Defender.Size - attackRoll.Attacker.Size;
            int checkerRoll = dice2d6.OpenEndedDiceRoll();
            int checkAgainstRoll = dice2d6.OpenEndedDiceRoll();

            int checkerTotal = checkerRoll + checkerMorale + sizeDifference;
            int checkAgainstTotal = checkAgainstRoll + checkAgainst + (bonus / 2);

            if (checkerTotal > checkAgainstTotal)
                return true;

            return false;
        }
    }
}
