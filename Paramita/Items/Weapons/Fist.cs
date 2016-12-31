using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Paramita.Items.Weapons
{
    public class Fist : Weapon
    {
        public Fist(Texture2D texture, Rectangle rect) : base(texture, rect, -2, -1, -1, 0, true)
        {
            name = "Fist";
        }
    }
}
