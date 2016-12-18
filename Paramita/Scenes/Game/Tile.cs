﻿namespace Paramita.Scenes
{

    // These types are translated to a graphical equivalent when the TileMap is drawn to screen
    public enum TileType
    {
        Door,
        Wall,
        Floor
    }

    public class Tile
    {
        //Tile coordinates
        private int x;
        private int y;

        public int X { get; private set; }
        public int Y { get; private set; }


        //Tile property flags
        private TileType tileType; // symbol for the tile's graphical depiction
        private bool isTransparent; // player can see through
        private bool isWalkable; // player can walk over
        private bool isInLineOfSight; // tile is in player's line of sight
        private bool isExplored; // player has seen this tile

        public TileType TileType { get; private set; }
        public bool IsTransparent { get; private set; }
        public bool IsWalkable { get; private set; }
        public bool IsInLineOfSight { get; private set; }
        public bool IsExplored { get; set; }



        // full constructor with all properties provided
        public Tile(int x, int y, bool isTransparent, bool isWalkable, bool isInLos, bool isExplored = false)
        {
            X = x;
            Y = y;
            IsTransparent = isTransparent;
            IsWalkable = isWalkable;
            IsInLineOfSight = isInLos;
            IsExplored = isExplored;
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
            return X == other.X && Y == other.Y && IsTransparent == other.IsTransparent && IsWalkable == other.IsWalkable && IsInLineOfSight == other.IsInLineOfSight && IsExplored == other.IsExplored;
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
                var hashCode = X;
                hashCode = (hashCode * 397) ^ Y;
                hashCode = (hashCode * 397) ^ IsTransparent.GetHashCode();
                hashCode = (hashCode * 397) ^ IsWalkable.GetHashCode();
                hashCode = (hashCode * 397) ^ IsInLineOfSight.GetHashCode();
                hashCode = (hashCode * 397) ^ IsExplored.GetHashCode();
                return hashCode;
            }
        }
    }
}