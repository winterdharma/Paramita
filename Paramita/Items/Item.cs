using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.UI;
using System;

namespace Paramita.Items
{
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
        protected Sprite sprite;

        // Basic Properties all Items should have
        public Sprite Sprite
        {
            get { return sprite; }
            private set { sprite = value; }
        }

        protected EquipType equipType;

        public EquipType EquipType
        {
            get { return equipType; }
            protected set { equipType = value; } 
        }

        public Item(Texture2D texture = null)
        {
            if(texture != null)
            {
                sprite = new Sprite(texture, new Rectangle(0, 0, 32, 32));
            }

            id = counter;
            counter++;
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
