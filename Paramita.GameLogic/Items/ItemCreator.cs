using Paramita.GameLogic.Items.Armors;
using Paramita.GameLogic.Items.Consumables;
using Paramita.GameLogic.Items.Valuables;
using Paramita.GameLogic.Items.Weapons;
using System;

namespace Paramita.GameLogic.Items
{
    public static class ItemCreator
    {
        public static Item CreateItem(ItemType type, int coins = 0)
        {
            switch (type)
            {
                case ItemType.Meat:
                    return CreateMeat();
                case ItemType.Shield:
                    return CreateBuckler();
                case ItemType.ShortSword:
                    return CreateShortSword();
                case ItemType.ScaleMailCuirass:
                    return CreateScaleMailCuirass();
                case ItemType.Coins:
                    return CreateCoins(coins);
                case ItemType.Bite:
                    return CreateBite();
                case ItemType.Fist:
                    return CreateFist();
                default:
                    throw new NotImplementedException(type + " not implemented yet.");
            }
        }

        public static ScaleMailCuirass CreateScaleMailCuirass()
        {
            return new ScaleMailCuirass();
        }

        public static Fist CreateFist()
        {
            return new Fist();
        }

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
