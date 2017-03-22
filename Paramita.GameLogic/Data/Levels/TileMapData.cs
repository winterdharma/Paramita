using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita.GameLogic.Data.Levels
{
    public class TileMapData
    {
        public int LevelWidth { get; set; }
        public int LevelHeight { get; set; }
        public int MaxRooms { get; set; }
        public int MaxRoomSize { get; set; }
        public int MinRoomSize { get; set; }

        public TileMapData() { }

        public TileMapData(int width, int height, int maxRooms, int maxSize, int minSize)
        {
            LevelWidth = width;
            LevelHeight = height;
            MaxRooms = maxRooms;
            MaxRoomSize = maxSize;
            MinRoomSize = minSize;
        }
    }
}
