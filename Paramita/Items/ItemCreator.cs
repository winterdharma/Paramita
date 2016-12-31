using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Items.Armors;
using Paramita.Items.Consumables;
using Paramita.Items.Valuables;
using Paramita.Items.Weapons;

namespace Paramita.Items
{
    public enum ItemType
    {
        ShortSword = 0,
        Shield,
        Coins,
        Meat,
        Fist,
        Bite
    }


    public static class ItemCreator
    {
        private static Rectangle[] spritesheetMap = new Rectangle[5] {
                new Rectangle(0,0,32,32),
                new Rectangle(0,31,32,32),
                new Rectangle(0,63,32,32),
                new Rectangle(0,95,32,32),
                // This rectangle is the lower right cornor of the spritesheet
                // and should be left transparent for items that need no sprite
                // like natural weapons, etc.
                new Rectangle(447,447,32,32)
            };
        private static Texture2D spritesheet;

        public static Texture2D Sprites
        {
            get { return spritesheet; }
            set { spritesheet = value; }
        }

 
        // This item has no visible sprite
        public static Fist CreateFist()
        {
            return new Fist(spritesheet, spritesheetMap[4]);
        }

        // This item has no visible sprite
        public static Bite CreateBite()
        {
            return new Bite(spritesheet, spritesheetMap[4]);
        }

        public static ShortSword CreateShortSword()
        {
            return new ShortSword(spritesheet, spritesheetMap[(int)ItemType.ShortSword],
                "A short sword for test games.");
        }



        public static Coins CreateCoins(int number)
        {
            return new Coins(spritesheet, spritesheetMap[(int)ItemType.Coins], 
                "A quantity of gold coins.", number);
        }



        public static Meat CreateMeat()
        {
            return new Meat(spritesheet, spritesheetMap[(int)ItemType.Meat],
                "An edible chunk of salted shank.");
        }



        public static Shield CreateShield()
        {
            return new Shield(spritesheet, spritesheetMap[(int)ItemType.Shield],
                "A handy shield.");
        }
    }
}
