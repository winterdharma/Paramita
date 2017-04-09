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
        Feet,
        Tail
    }


    public abstract class Item : IEquatable<Item>
    {
        private static int counter;
        private int id;
        protected string name;

        public EquipType EquipType { get; protected set; }
        public ItemType ItemType { get; protected set; }
        public string Name { get; protected set; }


        public Item(ItemType itemType, string name)
        {
            id = counter;
            counter++;
            ItemType = itemType;
            this.name = name;
        }


        #region IEquatable Methods
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
            if (this.GetType() == other.GetType())
                return true;
            return false;
        }

        public static bool operator ==(Item left, Item right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Item left, Item right)
        {
            return !Equals(left, right);
        }
        #endregion

        public override string ToString()
        {
            return id + " : " + name;
        }

        public string DisplayText()
        {
            return name;
        }

        public abstract string GetDescription();
    }
}
