using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Paramita.Items.Weapons
{
    public class ShortSword : Weapon
    {
        string name;
        string description;
        
        public ShortSword(Texture2D texture, Rectangle rect, string description)
            : base(texture, rect)
        {
            name = "Short Sword";
            this.description = description;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
