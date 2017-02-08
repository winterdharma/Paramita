using Microsoft.Xna.Framework.Graphics;
using Paramita.GameLogic.Items.Armors;
using Paramita.GameLogic.Items.Consumables;
using Paramita.GameLogic.Items.Valuables;
using Paramita.GameLogic.Items.Weapons;
using System.Collections.Generic;

namespace Paramita.GameLogic.Items
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
            return new ShortSword();
        }



        public static Coins CreateCoins(int number)
        {
            return new Coins(number);
        }



        public static Meat CreateMeat()
        {
            return new Meat();
        }



        public static Buckler CreateBuckler()
        {
            return new Buckler();
        }
    }
}
