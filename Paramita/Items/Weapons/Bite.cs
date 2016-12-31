using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Paramita.Items.Weapons
{
    public class Bite : Weapon
    {
        public Bite(Texture2D texture, Rectangle rect) 
            : base(texture, rect, 2, 0, -1, 0, true)
        {
            name = "Bite";
        }
    }
}
