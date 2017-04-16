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
        ScaleMailCuirass,
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
        #region Fields
        private static int _counter;
        private int _id;
        protected string _name;
        #endregion


        public Item(ItemType itemType, string name)
        {
            _id = _counter;
            _counter++;
            ItemType = itemType;
            _name = name;
        }


        #region Properties
        public int Id { get { return _id; } }
        public EquipType EquipType { get; protected set; }
        public ItemType ItemType { get; protected set; }
        #endregion


        /*
         * Item Equals, ==, and != methods function like string comparisons.
         * == and != check for identity; Equals() checks for equivalency.
         * Equivalency is currently defined as being of the same Type.
         */
        #region IEquatable Methods
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Item objAsItem = obj as Item;
            if (objAsItem == null) return false;
            else return Equals(objAsItem);
        }

        public bool Equals(Item other)
        {
            if (GetType() == other.GetType())
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            return _id;
        }

        public static bool operator ==(Item left, Item right)
        {
            return ReferenceEquals(left, right);
        }

        public static bool operator !=(Item left, Item right)
        {
            return !(left == right);
        }
        #endregion

        public override string ToString()
        {
            return _id + " : " + _name;
        }

        public string DisplayText()
        {
            return _name;
        }

        public abstract string GetDescription();
    }
}
