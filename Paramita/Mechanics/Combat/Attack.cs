using Paramita.Items;
using Paramita.SentientBeings;

namespace Paramita.Mechanics
{
    /*
     * This class handles and resolves an attack initiated by a SentientBeing on another
     * SentientBeing using a particular weapon. The exact type of attacks that are resolved
     * is determined and the appropriate combat objects created to perform the attack.
     */
    public class Attack
    {
        public SentientBeing Attacker { get; private set; }
        public SentientBeing Defender { get; private set; }
        public Weapon Weapon { get; private set; }
        



        public Attack(SentientBeing attacker, Weapon weapon, SentientBeing defender)
        {
            Attacker = attacker;
            Defender = defender;
            Weapon = weapon;

            ResolveAttack();
        }




        private void ResolveAttack()
        {
            var repelMeeleeAttack = new RepelMeeleeAttack(this);

            if (!repelMeeleeAttack.IsSuccessful)
            {
                Attacker.TakeDamage(repelMeeleeAttack.Damage);
                var meeleeAttack = new MeeleeAttack(this);
            }
        }
    }
}
