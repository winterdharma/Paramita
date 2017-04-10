using Paramita.GameLogic.Mechanics;
using System.Collections.Generic;

namespace Paramita.GameLogic.Actors.Combat
{
    internal class MeleeAttack
    {

        public MeleeAttack()
        {
            Report = new List<string>();
        }

        public List<string> Report { get; set; }

        public void Execute(Combatant attacker, Attack attack, Combatant defender)
        {
            Report.Clear();
            defender.IncrementTimesAttacked();
            attacker.AddAttackEncumbranceToFatigue();
            defender.AddDefenseEncumbranceToFatigue();

            var attackRoll = new AttackRoll(attacker, attack.Weapon, defender);

            Report.AddRange(attackRoll.AttackRollReport);

            ResolveAttackResult(attackRoll.Result, attack.Weapon.Damage, attacker, defender);
        }

        private void ResolveAttackResult(int attackResult, int weaponDmg, Combatant attacker, 
            Combatant defender)
        {
            if (attackResult > 0)
            {
                var damageRoll = new DamageRoll(attackResult, weaponDmg, attacker, defender);
                Report.AddRange(damageRoll.DamageRollReport);
                defender.TakeDamage(damageRoll.Damage);

                string damage = damageRoll.Damage < 1 ? "no" : damageRoll.Damage.ToString();
                Report.Add(attacker + " hit " + defender + " doing " + damage + " damage!");

                if (defender.HitPoints < 1)
                    Report.Add(defender + " was killed!");
            }
            else
            {
                Report.Add(attacker + " missed!");
            }
        }
    }
}
