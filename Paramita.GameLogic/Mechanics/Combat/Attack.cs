using Paramita.GameLogic.Items;
using Paramita.GameLogic.Actors;

namespace Paramita.GameLogic.Mechanics
{
    /*
     * This class handles and resolves an attack initiated by a SentientBeing on another
     * SentientBeing using a particular weapon. The exact type of attacks that are resolved
     * is determined and the appropriate combat objects created to perform the attack.
     */
    public class Attack
    {
        public Actor Attacker { get; private set; }
        public Actor Defender { get; private set; }
        public Weapon Weapon { get; private set; }
        



        public Attack(Actor attacker, Weapon weapon, Actor defender)
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
