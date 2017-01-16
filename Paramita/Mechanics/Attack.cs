using Paramita.Items;
using Paramita.SentientBeings;

namespace Paramita.Mechanics
{
    public class Attack
    {
        private SentientBeing attacker;
        private SentientBeing defender;
        private Weapon weapon;

        public Attack(SentientBeing attacker, Weapon weapon, SentientBeing defender)
        {
            this.attacker = attacker;
            this.defender = defender;
            this.weapon = weapon;

            var repelMeeleeAttack = new RepelMeeleeAttack(attacker, weapon, defender);

            if(!repelMeeleeAttack.IsSuccessful)
            {
                attacker.TakeDamage(repelMeeleeAttack.Damage);
                var meeleeAttack = new MeeleeAttack(attacker, weapon, defender);
            }
        }
    }
}
