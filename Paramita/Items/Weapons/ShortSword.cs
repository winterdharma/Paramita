using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Paramita.SentientBeings;

namespace Paramita.Items.Weapons
{
    public class ShortSword : Weapon
    {
        string description;
        
        public ShortSword(Texture2D texture, Rectangle rect, string description)
            : base(texture, rect, 5, 0, 2, 1)
        {
            name = "Short Sword";
            this.description = description;
            equipType = EquipType.Hand;
        }
    }
}
