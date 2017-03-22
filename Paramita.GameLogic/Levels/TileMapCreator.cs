using Microsoft.Xna.Framework;
using Paramita.GameLogic.Data.Levels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Paramita.GameLogic.Levels
{
    public interface ITileMapCreator
    {
        Tile[,] CreateMap(TileMapData data);
    }

    public class TileMapCreator : ITileMapCreator
    {
        #region Fields
        private Random random = Dungeon._random;
        #endregion


        #region Constructors
        public TileMapCreator() { }
        #endregion


        public Tile[,] CreateMap(TileMapData data)
        {
            int cols = data.LevelWidth;
            int rows = data.LevelHeight;
            int maxRoomSize = data.MaxRoomSize;
            int minRoomSize = data.MinRoomSize;

            Tile[,] tiles = new Tile[cols, rows];
            var rooms = new List<Rectangle>();
            for (int x = 0; x < data.MaxRooms; x++)
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
                
            }

            AddStairsTiles(rooms, tiles);
            AddWallTiles(tiles);

            return tiles;
        }

        #region Helper Methods
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

        private void AddStairsTiles(List<Rectangle> rooms, Tile[,] tiles)
        {
            int roomCenterX = rooms[0].Center.X;
            int roomCenterY = rooms[0].Center.Y;
            tiles[roomCenterX, roomCenterY] = new Tile(roomCenterX, roomCenterY, TileType.StairsUp);

            roomCenterX = rooms[rooms.Count - 1].Center.X;
            roomCenterY = rooms[rooms.Count - 1].Center.Y;
            tiles[roomCenterX, roomCenterY] = new Tile(roomCenterX, roomCenterY, TileType.StairsDown);

        }

        // This method finds tiles that weren't rooms or tunnels and sets them to walls
        private void AddWallTiles(Tile[,] tiles)
        {
            for(int x = 0; x < tiles.GetLength(0); x++)
            {
                for(int y = 0; y < tiles.GetLength(1); y++)
                {
                    if(tiles[x,y] == null)
                    {
                        tiles[x, y] = new Tile(x,y,TileType.Wall,false,false);
                    } 
                } 
            }
        }
        #endregion
    }
}
