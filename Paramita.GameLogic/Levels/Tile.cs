using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Paramita.GameLogic.Items;
using Paramita.GameLogic.Mechanics;

namespace Paramita.GameLogic.Levels
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

    public class ItemEventArgs : EventArgs
    {
        public Point Location;
        public ItemType ItemType;

        public ItemEventArgs(Point tileCoords, ItemType itemType)
        {
            Location = tileCoords;
            ItemType = itemType;
        }
    }

    public class Tile : IContainer
    {
        TileType _tileType;
        //Tile coordinates
        public Point TilePoint { get; private set; }

        //Tile property flags
        public TileType TypeOfTile
        {
            get { return _tileType; }
            private set
            {
                _tileType = value;
                SetFlagsForNewTileType(value);
            }
        }
        public bool IsTransparent { get; private set; }
        public bool IsWalkable { get; private set; }
        public bool IsInLineOfSight { get; private set; }
        public bool IsExplored { get; set; }

        //The Items on the ground here
        private List<Item> items;

        public event EventHandler<ItemEventArgs> OnItemAddedToTile;
        public event EventHandler<ItemEventArgs> OnItemRemovedFromTile;

        // full constructor with all properties provided
        public Tile(int x, int y, TileType type, bool isInLos = false, bool isExplored = false)
        {
            TilePoint = new Point(x, y);
            TypeOfTile = type;
            IsInLineOfSight = isInLos;
            IsExplored = isExplored;
            items = new List<Item>();
        }


        private void SetFlagsForNewTileType(TileType type)
        {
            switch(type)
            {
                case TileType.StairsUp:
                case TileType.StairsDown:
                case TileType.Floor:
                    IsTransparent = true;
                    IsWalkable = true;
                    break;
                case TileType.Door:
                case TileType.Wall:
                    IsTransparent = false;
                    IsWalkable = false;
                    break;
                default:
                    string message = "Tile.SetFlagsForNewTileType() Tried to set flags for" +
                       " unimplemented " + type + ".";
                    throw new NotImplementedException(message);
            }
        }


        public void SetTileType(TileType newType)
        {
            TypeOfTile = newType;
        }

        // Tiles will start with no limitations as containers
        public bool AddItem(Item item)
        {
            items.Add(item);
            OnItemAddedToTile?.Invoke(this, new ItemEventArgs(TilePoint, item.ItemType));
            return true;
        }

        // Returning null will means something went wrong (tried to remove
        // something not actually present here).
        public void RemoveItem(Item item)
        {
            items.Remove(item);
            OnItemRemovedFromTile?.Invoke(this, new ItemEventArgs(TilePoint, item.ItemType));
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
        // Returns true if the other tile is next to this one (not diagonally)
        // Otherwise, returns false 
        public bool AdjacentTo(Tile other, out Compass direction)
        {
            direction = Compass.None;
            Point difference = other.TilePoint - TilePoint;

            if( ((difference.X == 1 || difference.X == -1) && difference.Y == 0)
                || ((difference.Y == 1 || difference.Y == -1) && difference.X == 0))
            {
                if (difference == Direction.GetPoint(Compass.North))
                    direction = Compass.North;
                else if (difference == Direction.GetPoint(Compass.South))
                    direction = Compass.South;
                else if (difference == Direction.GetPoint(Compass.East))
                    direction = Compass.East;
                else if (difference == Direction.GetPoint(Compass.West))
                    direction = Compass.West;

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

       
        public static bool operator ==(Tile left, Tile right)
        {
            return Equals(left, right);
        }

        
        public static bool operator !=(Tile left, Tile right)
        {
            return !Equals(left, right);
        }

        
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
