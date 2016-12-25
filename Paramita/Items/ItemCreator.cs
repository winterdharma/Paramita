using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Items.Weapons;

namespace Paramita.Items
{
    public enum ItemType
    {
        ShortSword = 0,
        SmallShield,
        GoldCoins,
        Food
    }


    public class ItemCreator
    {
        Rectangle[] spritesheetMap;
        Texture2D spritesheet;

        public ItemCreator(Texture2D spritesheet)
        {
            this.spritesheet = spritesheet;
            spritesheetMap = new Rectangle[4] {
                new Rectangle(0,0,32,32),
                new Rectangle(0,31,32,32),
                new Rectangle(0,63,32,32),
                new Rectangle(0,95,32,32)
            };
        }


        public ShortSword CreateShortSword()
        {
            ShortSword shortsword = new ShortSword(spritesheet, spritesheetMap[(int)ItemType.ShortSword],
                "A short sword for test games.");

            return shortsword;
        }
    }
}
