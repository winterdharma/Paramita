using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Paramita.Items
{
    public abstract class Item : DrawableGameComponent, IEquatable<Item>
    {
        private int id; // used to identify specific items
        
        // private vector2 position; <= needed for drawing?

        // Basic Properties all Items should have
        public Texture2D Texture { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        


        public Item(GameController game, Texture2D texture, string name, string description)
            : base(game)
        {
            Texture = texture;
            Name = name;
            Description = description;
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
    }
}
