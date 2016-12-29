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
        protected int damage;
        protected int attackModifier;
        protected int defenseModifier;
        protected int length;

        public int Damage { get { return damage; } }

        public int AttackModifier { get { return attackModifier; } }

        public int DefenseModifier { get { return defenseModifier; } }

        public int Length { get { return length; } }




        public Weapon(Texture2D texture, Rectangle rect, int damage, int attack, int defense, int length) 
            : base(texture, rect)
        {
            this.damage = damage;
            attackModifier = attack;
            defenseModifier = defense;
            this.length = length;
        }
    }
}
