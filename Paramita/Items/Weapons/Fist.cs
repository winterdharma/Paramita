using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Paramita.Items.Weapons
{
    class Fist : Weapon
    {
        private string name = "Fist";

        public Fist(Texture2D texture, Rectangle rect) : base(texture, rect, -2, -1, -1, 0, true)
        {
        }
    }
}
