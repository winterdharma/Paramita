using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Paramita.Items
{
    /*
     * Weapons are used to engage in combat or otherwise harm living things.
     * This is a base class not intended to be directly instantiated.
     */
    public abstract class Weapon : Item
    {
        protected string name;
        protected int damage;
        protected int attackModifier;
        protected int defenseModifier;
        protected int length;
        protected bool isNatural;

        public int Damage { get { return damage; } }

        public int AttackModifier { get { return attackModifier; } }

        public int DefenseModifier { get { return defenseModifier; } }

        public int Length { get { return length; } }

        // a flag for checking if a weapon is a natural (default) weapon
        // rather than a equiped item
        public bool IsNatural
        {
            get { return isNatural; }
            private set { isNatural = value; }
        }


        public Weapon(Texture2D texture, Rectangle rect, int damage, int attack, int defense, int length, bool isNatural) 
            : base(texture, rect)
        {
            this.damage = damage;
            attackModifier = attack;
            defenseModifier = defense;
            this.length = length;
            this.isNatural = isNatural;
        }



        // This override may belong in Weapon class (just need to pass @name to it?)
        public override string ToString()
        {
            return name + " (Att:" + attackModifier + ", Def:" + defenseModifier +
                ", Length:" + length + ")";
        }
    }
}
