using Paramita.Items;
using Paramita.Scenes;
using Paramita.SentientBeings;
using System;

namespace Paramita.Mechanics
{
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
    public class MeeleeAttack
    {
        Dice dice2d6;

        // used in combat resolution
        SentientBeing attacker;
        SentientBeing defender;
        Weapon attackWeapon;



        public MeeleeAttack(SentientBeing attacker, Weapon weapon, SentientBeing defender)
        {
            this.attacker = attacker;
            attackWeapon = weapon;
            this.defender = defender;
            dice2d6 = new Dice(2);

            ResolveAttack();
        }




        public void ResolveAttack()
        {
            defender.IncrementTimesAttacked();
            attacker.AddEncumbranceToFatigue();

            var attackRoll = new AttackRoll(attacker, attackWeapon, defender);

            GameScene.PostNewStatus(attackRoll.AttackerReport);
            GameScene.PostNewStatus(attackRoll.DefenderReport);

            ResolveAttackResult(attackRoll);
            CheckIfAttackKilledDefender(defender);
        }


        private void ResolveAttackResult(AttackRoll attackRoll)
        {
            if (attackRoll.Result > 0)
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
    }
}
