using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Paramita.GameLogic.Items
{
    public enum ItemType
    {
        None = 0,
        ShortSword,
        Shield,
        Coins,
        Meat,
        Fist,
        Bite
    }

    public enum EquipType
    {
        None = 0,
        Hand,
        Head,
        Body,
        Feet
    }


    public abstract class Item : IEquatable<Item>
    {
        private static int counter;
        private int id;
        protected string name;

        public EquipType EquipType { get; protected set; }
        public ItemType ItemType { get; protected set; }


        public Item(ItemType itemType)
        {
            id = counter;
            counter++;
            ItemType = itemType;
        }

        /*
         * IEquatable<Item> methods.
         * 
         * Items are not equal unless they both have the same id,
         * meaning that both are references to the same instance of
         * an Item derived class.
         * 
         * This is setup with the List<Item>.Remove() method in mind,
         * which matches the Item to remove with the Item in the collection
         * with Equals().
         */
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Item objAsItem = obj as Item;
            if (objAsItem == null) return false;
            else return Equals(objAsItem);
        }

        public override int GetHashCode()
        {
            return id;
        }

        public bool Equals(Item other)
        {
            if(other == null) return false;
            return (id.Equals(other.id));
        }

        public static bool operator ==(Item left, Item right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Item left, Item right)
        {
            return !Equals(left, right);
        }

        public abstract string GetDescription();
    }
}
