using Paramita.GameLogic.Items;
using Paramita.GameLogic.Actors;
using System.Collections.Generic;

namespace Paramita.GameLogic.Actions
{
    /*
     * This class handles and resolves an attack initiated by a SentientBeing on another
     * SentientBeing using a particular weapon. The exact type of attacks that are resolved
     * is determined and the appropriate combat objects created to perform the attack.
     */
    public class Attack
    {
        private List<string> _attackReport = new List<string>();

        public Actor Attacker { get; private set; }
        public Actor Defender { get; private set; }
        public Weapon Weapon { get; private set; }
        

        public List<string> AttackReport { get { return _attackReport; } }

        public Attack(Actor attacker, Weapon weapon, Actor defender)
        {
            Attacker = attacker;
            Defender = defender;
            Weapon = weapon;

            ResolveAttack();
        }




        private void ResolveAttack()
        {
            var repelMeleeAttack = new RepelMeleeAttack(this);
            _attackReport.AddRange(repelMeleeAttack.RepelAttackLog);

            if (!repelMeleeAttack.IsSuccessful)
            {
                Attacker.TakeDamage(repelMeleeAttack.Damage);
                var meleeAttack = new MeleeAttack(this);
                _attackReport.AddRange(meleeAttack.MeleeAttackLog);
            }
        }
    }
}
