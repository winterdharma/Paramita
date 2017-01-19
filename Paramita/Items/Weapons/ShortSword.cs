using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Paramita.SentientBeings;

namespace Paramita.Items.Weapons
{
    public class ShortSword : Weapon
    {
        string description;
        
        public ShortSword(Texture2D texture)
            : base(5, 0, 2, 1, texture)
        {
            name = "Short Sword";
            this.description = "A modest weapon to start with.";
            equipType = EquipType.Hand;
        }
    }
}
