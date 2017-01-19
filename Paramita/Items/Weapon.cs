using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.SentientBeings;

namespace Paramita.Items
{
    /*
     * Weapons are used to engage in combat or otherwise harm living things.
     * This is a base class not intended to be directly instantiated.
     */
    public abstract class Weapon : Item
    {
        protected int damage;
        protected int attackModifier;
        protected int defenseModifier;
        protected int length;

        public int Damage { get { return damage; } }

        public int AttackModifier { get { return attackModifier; } }

        public int DefenseModifier { get { return defenseModifier; } }

        public int Length { get { return length; } }


        public Weapon(int damage, int attack, int defense, int length, Texture2D texture = null) 
            : base(texture)
        {
            this.damage = damage;
            attackModifier = attack;
            defenseModifier = defense;
            this.length = length;
        }



        // This override may belong in Weapon class (just need to pass @name to it?)
        public override string ToString()
        {
            return name;
        }

        public override string GetDescription()
        {
            return name + " (Damage: " + damage + " plus strength, Att:" + attackModifier + ", Def:" + defenseModifier +
                ", Length:" + length + ")";
        }
    }
}
