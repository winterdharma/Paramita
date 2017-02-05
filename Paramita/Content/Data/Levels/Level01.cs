using Microsoft.Xna.Framework;
using Paramita.Levels;
using System;
using System.Collections.Generic;

namespace Paramita.Data.Levels
{
    public static class Level01
    {
        static int _tilesHigh = 40;
        static int _tilesWide = 40;
        static int _entries = 1;
        static int _exits = 1;

        static TileType[,] _entryArea = new TileType[10, 10]
        {
            { TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall,
              TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall },
            { TileType.Wall, TileType.StairsUp, TileType.Floor, TileType.Wall, TileType.Floor, TileType.Floor,
              TileType.Wall, TileType.Floor, TileType.Floor, TileType.Wall },
            { TileType.Wall, TileType.Floor, TileType.Floor, TileType.Wall, TileType.Floor, TileType.Floor,
              TileType.Wall, TileType.Floor, TileType.Floor, TileType.Wall },
            { TileType.Wall, TileType.Wall, TileType.Door, TileType.Wall, TileType.Door, TileType.Wall,
              TileType.Wall, TileType.Door, TileType.Wall, TileType.Wall },
            { TileType.Wall, TileType.Wall, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Floor,
              TileType.Floor, TileType.Floor, TileType.Floor, TileType.Floor },
            { TileType.Wall, TileType.Wall, TileType.Door, TileType.Wall, TileType.Door, TileType.Wall,
              TileType.Wall, TileType.Door, TileType.Wall, TileType.Wall },
            { TileType.Wall, TileType.Floor, TileType.Floor, TileType.Wall, TileType.Floor, TileType.Floor,
              TileType.Wall, TileType.Floor, TileType.Floor, TileType.Wall },
            { TileType.Wall, TileType.Floor, TileType.Floor, TileType.Wall, TileType.Floor, TileType.Floor,
              TileType.Wall, TileType.Floor, TileType.Floor, TileType.Wall },
            { TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall,
              TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall },
            { TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall,
              TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall }
        };

        static TileType[,] _exitArea = new TileType[10, 10]
        {
            { TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall,
              TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall },
            { TileType.Wall, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Wall, TileType.Wall,
              TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall },
            { TileType.Wall, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Wall, TileType.Wall,
              TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall },
            { TileType.Wall, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Floor,
              TileType.Floor, TileType.Floor, TileType.Floor, TileType.Wall },
            { TileType.Wall, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Floor,
              TileType.Floor, TileType.Floor, TileType.StairsDown, TileType.Wall },
            { TileType.Floor, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Floor,
              TileType.Floor, TileType.Floor, TileType.Floor, TileType.Wall },
            { TileType.Wall, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Wall, TileType.Wall,
              TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall },
            { TileType.Wall, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Wall, TileType.Wall,
              TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall },
            { TileType.Wall, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Wall, TileType.Wall,
              TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall },
            { TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall,
              TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall }
        };

        static TileType[,] _fourWayRoom = new TileType[10, 10]
        {
            { TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Floor, TileType.Wall,
              TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall },
            { TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Floor, TileType.Wall,
              TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall },
            { TileType.Wall, TileType.Wall, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Floor,
              TileType.Floor, TileType.Floor, TileType.Wall, TileType.Wall },
            { TileType.Wall, TileType.Wall, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Floor,
              TileType.Floor, TileType.Floor, TileType.Wall, TileType.Wall },
            { TileType.Wall, TileType.Wall, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Floor,
              TileType.Floor, TileType.Floor, TileType.Floor, TileType.Floor },
            { TileType.Floor, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Floor,
              TileType.Floor, TileType.Floor, TileType.Wall, TileType.Wall },
            { TileType.Wall, TileType.Wall, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Floor,
              TileType.Floor, TileType.Floor, TileType.Wall, TileType.Wall },
            { TileType.Wall, TileType.Wall, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Floor,
              TileType.Floor, TileType.Floor, TileType.Wall, TileType.Wall },
            { TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Floor,
              TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall },
            { TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Floor,
              TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall }
        };

        static List<TileType[,]> _areas = new List<TileType[,]>() { _entryArea, _exitArea, _fourWayRoom };




        public static Tuple<int,int> TileMapSize
        {
            get { return new Tuple<int, int>(_tilesHigh, _tilesWide); }
        }

        public static List<TileType[,]> TileMapAreas
        {
            get { return _areas; }
        }

        public static int NumOfEntries
        {
            get { return _entries; }
        }

        public static int NumOfExits
        {
            get { return _exits; }
        }
    }
}
