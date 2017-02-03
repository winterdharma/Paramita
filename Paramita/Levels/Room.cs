using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita.Levels
{
    /*
     *  A room is a pre-defined set of tiles that is used to construct the tilemap of a level
     *  by fitting them together like puzzle peices.
     *  
     *  Usually a room represents a single room of a dungeon, but also represents pre-designed
     *  areas that can represent several rooms or corridors.
     */
    public abstract class Room
    {
        private Tile[,] tileMap;

        public Room()
        {

        }

        public abstract Tile[,] Rotate90Clockwise();

        public abstract Tile[,] Rotate90CounterClockwise();

        public abstract Tile[,] FlipHorizontally();

        public abstract Tile[,] FlipVertically();
    }
}
