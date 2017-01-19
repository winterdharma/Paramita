using Microsoft.Xna.Framework;
using Paramita.Items;
using System.Collections.Generic;

namespace Paramita.Scenes
{

    // These types are translated to a graphical equivalent when the TileMap is drawn to screen
    public enum TileType
    {
        Door,
        Wall,
        Floor,
        StairsUp,
        StairsDown
    }

    public class Tile : IContainer
    {
        //Tile coordinates
        public Point TilePoint { get; private set; }
        public Vector2 Position { get; private set; }
        private const int TILE_SIZE = 32;

        //Tile property flags
        public TileType TileType { get; private set; }
        public bool IsTransparent { get; private set; }
        public bool IsWalkable { get; private set; }
        public bool IsInLineOfSight { get; private set; }
        public bool IsExplored { get; set; }

        //The Items on the ground here
        private List<Item> items;

        // full constructor with all properties provided
        public Tile(int x, int y, TileType type, bool isTransparent, bool isWalkable,
            bool isInLos = false, bool isExplored = false)
        {
            TilePoint = new Point(x, y);
            Position = GetTilePosition();
            TileType = type;
            IsTransparent = isTransparent;
            IsWalkable = isWalkable;
            IsInLineOfSight = isInLos;
            IsExplored = isExplored;
            items = new List<Item>();
        }



        // Converts a Tile's XY coordinates on the tilemap to a Vector2
        // of its position in pixels
        public Vector2 GetTilePosition()
        {
            return new Vector2(
                TilePoint.X * TILE_SIZE,
                TilePoint.Y * TILE_SIZE);
        }


        public void SetTileType(TileType newType)
        {
            TileType = newType;
        }

        // Tiles will start with no limitations as containers
        public bool AddItem(Item item)
        {
            item.Sprite.Position = Position;
            items.Add(item);
            return true;
        }

        // Returning null will means something went wrong (tried to remove
        // something not actually present here).
        public void RemoveItem(Item item)
        {
            items.Remove(item);
        }


        // Makes the items in this Tile visible to the caller
        public Item[] InspectItems()
        {
            return items.ToArray();
        }

        // this method redirects the standard ToString() call so a bool can be applied
        public override string ToString()
        {
            return ToString(false);
        }

        /// <summary>
        /// Provides a simple visual representation of the Cell using the following symbols:
        /// - `%`: `Cell` is not in field-of-view
        /// - `.`: `Cell` is transparent, walkable, and in field-of-view
        /// - `s`: `Cell` is walkable and in field-of-view (but not transparent)
        /// - `o`: `Cell` is transparent and in field-of-view (but not walkable)
        /// - `#`: `Cell` is in field-of-view (but not transparent or walkable)
        /// </summary>
        /// <param name="useFov">True if field-of-view calculations will be used when creating the string represenation of the Cell. False otherwise</param>
        /// <returns>A string representation of the Cell using special symbols to denote Cell properties</returns>
        public string ToString(bool useFov)
        {
            if (useFov && !IsInLineOfSight) { return "%"; }
            if (IsWalkable)
            {
                if (IsTransparent) { return "."; }
                else { return "s"; }
            }
            else
            {
                if (IsTransparent) { return "o"; }
                else { return "#"; }
            }
        }


        // Checks to see if a tile is next to this one
        // Returns true if the other tile is next to this one (in any of the eight Directions)
        // Otherwise, returns false 
        public bool AdjacentTo(Tile other)
        {
            // exit if @other and @this tile are the same tile
            if(this == other)
            {
                return false;
            }

            int thisX = this.TilePoint.X;
            int thisY = this.TilePoint.Y;
            int otherX = other.TilePoint.X;
            int otherY = other.TilePoint.Y;

            if( (thisX - otherX < 2 && thisX - otherX > -2 ) 
               && ( thisY - otherY < 2 && thisY - otherY > -2 ))
            {
                return true;
            }
            return false;
        }


        public bool Equals(Tile other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return TilePoint == other.TilePoint && IsTransparent == other.IsTransparent && IsWalkable == other.IsWalkable && IsInLineOfSight == other.IsInLineOfSight && IsExplored == other.IsExplored;
        }

        /// <summary>
        /// Determines whether two Cell instances are equal
        /// </summary>
        /// <param name="obj">The Object to compare this instance to</param>
        /// <returns>True if the instances are equal; False otherwise</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((Tile)obj);
        }

        /// <summary>
        /// Determines whether two Cell instances are equal
        /// </summary>
        /// <param name="left">Cell on the left side of the equal sign</param>
        /// <param name="right">Cell on the right side of the equal sign</param>
        /// <returns>True if a and b are equal; False otherwise</returns>
        public static bool operator ==(Tile left, Tile right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Determines whether two Cell instances are not equal
        /// </summary>
        /// <param name="left">Cell on the left side of the equal sign</param>
        /// <param name="right">Cell on the right side of the equal sign</param>
        /// <returns>True if a and b are not equal; False otherwise</returns>
        public static bool operator !=(Tile left, Tile right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// Gets the hash code for this object which can help for quick checks of equality
        /// or when inserting this Cell into a hash-based collection such as a Dictionary or Hashtable 
        /// </summary>
        /// <returns>An integer hash used to identify this Cell</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = TilePoint.X;
                hashCode = (hashCode * 397) ^ TilePoint.Y;
                hashCode = (hashCode * 397) ^ IsTransparent.GetHashCode();
                hashCode = (hashCode * 397) ^ IsWalkable.GetHashCode();
                hashCode = (hashCode * 397) ^ IsInLineOfSight.GetHashCode();
                hashCode = (hashCode * 397) ^ IsExplored.GetHashCode();
                return hashCode;
            }
        }
    }
}
