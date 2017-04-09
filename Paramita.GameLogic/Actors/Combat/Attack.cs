using Paramita.GameLogic.Items;

namespace Paramita.GameLogic.Actors
{
    public class Attack
    {
        public Attack(Weapon weapon, int strength, int attackskill)
        {
            Weapon = weapon;
            Damage = strength + weapon.Damage;
            TotalAttack = attackskill + weapon.AttackModifier;
            //Fatigue = weapon.Encumbrance; (not implemented yet)
        }

        public Weapon Weapon { get; set; }
        // Actor's strength + weapon damage
        public int Damage { get; set; }
        // Actor's attack skill + weapon attack mod
        public int TotalAttack { get; set; }
        // Actor's encumbrance + weapon encumbrance (not implemented yet)
        //public int Fatigue { get; set; }
    }
}
