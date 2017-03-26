using System;
using Paramita.GameLogic.Items;
using Paramita.GameLogic.Actors;
using System.Collections.Generic;
using Paramita.GameLogic.Mechanics;

namespace Paramita.GameLogic.Actions
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
    public class MeleeAttack
    {
        private Dice _dice2d6;
        private List<string> _meleeAttackLog = new List<string>();
        // used in combat resolution
        private Actor _attacker;
        private Actor _defender;
        private Weapon _attackWeapon;

        public List<string> MeleeAttackLog { get { return _meleeAttackLog; } }



        public MeleeAttack(Attack attack)
        {
            _attacker = attack.Attacker;
            _attackWeapon = attack.Weapon;
            _defender = attack.Defender;
            _dice2d6 = new Dice(2);

            ResolveAttack();
        }



        public void ResolveAttack()
        {
            _defender.IncrementTimesAttacked();
            _attacker.AddEncumbranceToFatigue();

            var attackRoll = new AttackRoll(_attacker, _attackWeapon, _defender);

            _meleeAttackLog.AddRange( 
                new List<string>() { attackRoll.AttackerReport, attackRoll.DefenderReport });

            ResolveAttackResult(attackRoll);
            CheckIfAttackKilledDefender();
        }


        private void ResolveAttackResult(AttackRoll attackRoll)
        {
            if (attackRoll.Result > 0)
            {
                var damageRoll = new DamageRoll(attackRoll);
                _defender.TakeDamage(damageRoll.Damage);
                _meleeAttackLog.Add(_attacker + " hit " + _defender + " doing " + damageRoll.Damage + " damage!");
            }
            else
            {
                _meleeAttackLog.Add(_attacker + " missed!");
            }
        }


        private void CheckIfAttackKilledDefender()
        {
            if (_defender.IsDead)
            {
                _meleeAttackLog.Add(_defender + " is killed!");
            }
        }
    }
}
