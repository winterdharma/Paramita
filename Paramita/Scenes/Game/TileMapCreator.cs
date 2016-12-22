using Microsoft.Xna.Framework;
using Paramita.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Paramita.Scenes
{
    class TileMapCreator
    {
        private Random random;
        private int rows;
        private int cols;
        private int maxRooms;
        private int maxRoomSize;
        private int minRoomSize;

        public TileMapCreator(int rows, int cols, int maxRooms, 
            int maxSize, int minSize, Random random)
        {
            this.rows = rows;
            this.cols = cols;
            this.maxRooms = maxRooms;
            maxRoomSize = maxSize;
            minRoomSize = minSize;
            this.random = random;
        }

        public Tile[,] CreateMap()
        {
            Tile[,] tiles = new Tile[cols, rows];
            var rooms = new List<Rectangle>();
            for (int x = 0; x < maxRooms; x++)
            {
                int roomWidth = random.Next(minRoomSize, maxRoomSize);
                int roomHeight = random.Next(minRoomSize, maxRoomSize);
                int roomXPosition = random.Next(0, cols - roomWidth - 1);
                int roomYPosition = random.Next(0, rows - roomHeight - 1);

                var newRoom = new Rectangle(
                    roomXPosition, roomYPosition, roomWidth, roomHeight);
                bool newRoomIntersects = rooms.Any(room => newRoom.Intersects(room));

                if (newRoomIntersects == false) { rooms.Add(newRoom); }
            }

            foreach(Rectangle room in rooms)
            {
                MakeRoom(tiles, room);
            }

            for(int x = 1; x < rooms.Count; x++)
            {
                var previousRoomCenterX = rooms[x - 1].Center.X;
                var previousRoomCenterY = rooms[x - 1].Center.Y;
                var currentRoomCenterX = rooms[x].Center.X;
                var currentRoomCenterY = rooms[x].Center.Y;

                if (random.Next(0, 2) == 0) // why this random number?
                {
                    MakeHorizontalTunnel(tiles, previousRoomCenterX, currentRoomCenterX, previousRoomCenterY);
                    MakeVerticalTunnel(tiles, previousRoomCenterY, currentRoomCenterY, currentRoomCenterX);
                }
                else
                {
                    MakeVerticalTunnel(tiles, previousRoomCenterY, currentRoomCenterY, previousRoomCenterX);
                    MakeHorizontalTunnel(tiles, previousRoomCenterX, currentRoomCenterX, currentRoomCenterY);
                }

                AddWallTiles(tiles);
            }

            

            return tiles;
        }


        // This method creates Tiles for a room and inserts them into the Tile[,] array
        private void MakeRoom(Tile[,] tiles, Rectangle room)
        {
            for(int x = room.Left + 1; x < room.Right; x++)
            {
                for(int y = room.Top + 1; y < room.Bottom; y++)
                {
                    tiles[x, y] = new Tile(x,y,TileType.Floor,true,true);
                }
            }
        }


        private void MakeHorizontalTunnel(Tile[,] tiles, int xStart, int xEnd, int yPosition)
        {
            for (var x = Math.Min(xStart, xEnd); x <= Math.Max(xStart, xEnd); x++)
            {
                tiles[x,yPosition] = new Tile(x,yPosition, TileType.Floor, true, true);
            }
        }

        private void MakeVerticalTunnel(Tile[,] tiles, int yStart, int yEnd, int xPosition)
        {
            for (var y = Math.Min(yStart, yEnd); y <= Math.Max(yStart, yEnd); y++)
            {
                tiles[xPosition, y] = new Tile(xPosition, y, TileType.Floor, true, true); ;
            }
        }

        // This method finds tiles that weren't rooms or tunnels and sets them to walls
        private void AddWallTiles(Tile[,] tiles)
        {
            for(int x = 0; x < rows; x++)
            {
                for(int y = 0; y < cols; y++)
                {
                    if(tiles[x,y] == null)
                    {
                        tiles[x, y] = new Tile(x,y,TileType.Wall,false,false);
                    } 
                } 
            }
        }
    }
}
