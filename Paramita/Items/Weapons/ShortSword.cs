using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Paramita.Items.Weapons
{
    public class ShortSword : Weapon
    {
        string name;
        string description;
        
        public ShortSword(Texture2D texture, Rectangle rect, string name, string description)
            : base(texture, rect)
        {
            this.name = name;
            this.description = description;
        }
    }
}
