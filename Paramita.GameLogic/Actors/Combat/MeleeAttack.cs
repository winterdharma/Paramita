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
            attacker.AddEncumbranceToFatigue();

            var attackRoll = new AttackRoll(attacker, attack.Weapon, defender);

            Report.AddRange(
                new List<string>() { attackRoll.AttackerReport, attackRoll.DefenderReport });

            ResolveAttackResult(attackRoll);
        }


        private void ResolveAttackResult(AttackRoll attackRoll)
        {
            if (attackRoll.Result > 0)
            {
                var damageRoll = new DamageRoll(attackRoll);
                attackRoll.Defender.TakeDamage(damageRoll.Damage);

                string damage = damageRoll.Damage < 1 ? "no" : damageRoll.Damage.ToString();
                Report.Add(attackRoll.Attacker + " hit " + attackRoll.Defender + " doing " + damage + " damage!");

                if (attackRoll.Defender.HitPoints < 1)
                    Report.Add(attackRoll.Defender + " was killed!");
            }
            else
            {
                Report.Add(attackRoll.Attacker + " missed!");
            }
        }
    }
}
