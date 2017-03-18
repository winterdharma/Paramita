using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Paramita.GameLogic.Items;
using Paramita.GameLogic.Mechanics;
using Paramita.GameLogic.Utility;

namespace Paramita.GameLogic.Levels
{

    // These types are translated to a graphical equivalent when the TileMap is drawn to screen
    public enum TileType
    {
        None,
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

    public class Tile : IContainer, IEquatable<Tile>, IComparable<Tile>
    {
        #region Fields
        private Point _tilePoint;
        private TileType _tileType;
        private bool _isWalkable;
        private bool _isTransparent;
        private bool _isInLineOfSight;
        private bool _isExplored;
        private List<Item> _items = new List<Item>();
        #endregion


        #region Events
        public event EventHandler<ItemEventArgs> OnItemAddedToTile;
        public event EventHandler<ItemEventArgs> OnItemRemovedFromTile;
        #endregion


        public Tile(int x, int y, TileType type, bool isInLos = false, bool isExplored = false)
        {
            TilePoint = new Point(x, y);
            TileType = type;
            IsInLineOfSight = isInLos;
            IsExplored = isExplored;
        }


        #region Properties
        public Point TilePoint
        {
            get { return _tilePoint; }
            set { _tilePoint = value; }
        }

        public List<Item> Items
        {
            get { return _items; }
        }

        public TileType TileType
        {
            get { return _tileType; }
            private set
            {
                _tileType = value;
                SetFlagsForNewTileType();
            }
        }

        public bool IsTransparent
        {
            get { return _isTransparent; }
            private set { _isTransparent = value; }
        }

        public bool IsWalkable
        {
            get { return _isWalkable; }
            private set { _isWalkable = value; }
        }

        // future properties not currently used
        public bool IsInLineOfSight
        {
            get { return _isInLineOfSight; }
            private set { _isInLineOfSight = value; }
        }

        public bool IsExplored
        {
            get { return _isExplored; }
            private set { _isExplored = value; }
        }
        #endregion


        public bool AdjacentTo(Tile other, out Compass direction)
        {
            Point difference = other.TilePoint - TilePoint;
            List<Point> compassNESW = Direction.CardinalPoints;

            // if @difference is not found in @compassNESW, 
            // then Point(0,0) is returned by List.Find(), 
            // which is equal to Compass.None

            direction = Direction.GetDirection(compassNESW.Find(p => p == difference));

            var isAdjacent = direction != Compass.None ? true : false;
            return isAdjacent;
        }


        #region IContainer Implementation
        // Tiles will start with no limitations as containers
        public bool AddItem(Item item)
        {
            Utilities.ThrowExceptionIfNull(item);

            _items.Add(item);
            OnItemAddedToTile?.Invoke(this, new ItemEventArgs(TilePoint, item.ItemType));
            return true;
        }

        public void RemoveItem(Item item)
        {
            Utilities.ThrowExceptionIfNull(item);

            var wasRemoved = _items.Remove(item);
            if (wasRemoved)
            {
                OnItemRemovedFromTile?.Invoke(this, new ItemEventArgs(TilePoint, item.ItemType));
            }
            else
                throw new ArgumentException("Tried to remove an item that wasn't in Tile._items.");
        }


        // Makes the items in this Tile visible to the caller
        public Item[] InspectItems()
        {
            return _items.ToArray();
        }
        #endregion


        #region IEquatable Implementation
        public bool Equals(Tile other)
        {
            return TilePoint == other.TilePoint 
                && IsTransparent == other.IsTransparent 
                && IsWalkable == other.IsWalkable 
                && TileType == other.TileType;
        }

        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
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
        #endregion


        #region IComparable Implementation
        // this sorts tiles by their coordinates, such that (0,0) is first
        public int CompareTo(Tile other)
        {
            int i = TilePoint.Y.CompareTo(other.TilePoint.Y);
            if (i != 0)
                return i;

            i = TilePoint.X.CompareTo(other.TilePoint.X);
            if (i != 0)
                return i;
            return 0;
        }
        #endregion


        #region ToString() Override
        // this method redirects the standard ToString() call so a bool can be applied
        public override string ToString()
        {
            string str = TilePoint.ToString();
            return str;
        }

        
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
        #endregion


        #region Helper Methods
        private void SetFlagsForNewTileType()
        {
            switch (_tileType)
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
                    string message = "Tried to set flags for unimplemented " + _tileType + ".";
                    throw new NotImplementedException(message);
            }
        }
        #endregion
    }
}
