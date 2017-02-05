using Microsoft.Xna.Framework.Graphics;
using Paramita.Items.Armors;
using Paramita.Items.Consumables;
using Paramita.Items.Valuables;
using Paramita.Items.Weapons;
using System.Collections.Generic;

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
        private static Dictionary<ItemType, Texture2D> spritesheets = new Dictionary<ItemType, Texture2D>();

        public static Dictionary<ItemType, Texture2D> Spritesheets
        {
            get { return spritesheets; }
            set { spritesheets = value; }
        }

 
        // This item has no visible sprite
        public static Fist CreateFist()
        {
            return new Fist();
        }

        // This item has no visible sprite
        public static Bite CreateBite()
        {
            return new Bite();
        }

        public static ShortSword CreateShortSword()
        {
            return new ShortSword(spritesheets[ItemType.ShortSword]);
        }



        public static Coins CreateCoins(int number)
        {
            return new Coins(spritesheets[ItemType.Coins], number);
        }



        public static Meat CreateMeat()
        {
            return new Meat(spritesheets[ItemType.Meat]);
        }



        public static Buckler CreateBuckler()
        {
            return new Buckler(spritesheets[ItemType.Shield]);
        }
    }
}
