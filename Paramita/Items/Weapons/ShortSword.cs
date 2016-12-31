using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Paramita.Items.Weapons
{
    public class ShortSword : Weapon
    {
        string name;
        string description;
        
        public ShortSword(Texture2D texture, Rectangle rect, string description)
            : base(texture, rect, 5, 0, 2, 1, false)
        {
            name = "Short Sword";
            this.description = description;
        }

        // This override may belong in Weapon class (just need to pass @name to it?)
        public override string ToString()
        {
            return name + "(Att:" + attackModifier + ", Def:" + defenseModifier + 
                ", Length:" + length + ")";
        }
    }
}
