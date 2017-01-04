using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.SentientBeings;

namespace Paramita.Items.Weapons
{
    public class Fist : NaturalWeapon
    {
        public Fist(Texture2D texture, Rectangle rect) : base(texture, rect, -2, -1, -1, 0)
        {
            name = "Fist";
            equipType = EquipType.Hand;
        }
    }
}
