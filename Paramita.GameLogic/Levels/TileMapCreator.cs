using Microsoft.Xna.Framework;
using Paramita.GameLogic.Data.Levels;
using Paramita.GameLogic.Mechanics;
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
        private IRandom random = Dungeon._random;
        #endregion


        #region Constructors
        public TileMapCreator() { }
        #endregion


        public Tile[,] CreateMap(TileMapData data)
        {
            ValidateTileMapData(data);

            int cols = data.LevelWidth;
            int rows = data.LevelHeight;

            var tiles = new Tile[cols, rows];
            var rooms = CreateRoomRectangles(data);
            
            foreach(Rectangle room in rooms)
            {
                MakeRoom(tiles, room);
            }

            CreateConnectingTunnels(rooms, tiles);
            
            AddStairsTiles(rooms, tiles);
            AddWallTiles(tiles);

            return tiles;
        }

        #region Helper Methods
        private void ValidateTileMapData(TileMapData data)
        {
            ThrowExceptionIfMaxRoomsInvalid(data.MaxRooms);
            ThrowExceptionIfMinSizeLargerThanMaxSize(data.MinRoomSize, data.MaxRoomSize);
            ThrowExceptionIfMapDimensionsTooSmall(data.LevelWidth, data.LevelHeight);
        }

        private void ThrowExceptionIfMaxRoomsInvalid(int maxRooms)
        {
            if (maxRooms < 1)
                throw new ArgumentOutOfRangeException("MaxRooms was less than 1.");
        }
        
        private void ThrowExceptionIfMinSizeLargerThanMaxSize(int minSize, int maxSize)
        {
            if (minSize > maxSize)
                throw new ArgumentOutOfRangeException("MinRoomSize was greater than MaxRoomSize");
        }

        private void ThrowExceptionIfMapDimensionsTooSmall(int width, int height)
        {
            if(width < 10 || height < 10)
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        private List<Rectangle> CreateRoomRectangles(TileMapData data)
        {
            int minSize = data.MinRoomSize;
            int maxSize = data.MaxRoomSize;

            var rooms = new List<Rectangle>();
            int attempts = 3;
            for (int x = 0; x < data.MaxRooms + 1; x++)
            {
                int roomWidth = random.Next(minSize, maxSize);
                int roomHeight = random.Next(minSize, maxSize);
                int roomXPosition = random.Next(0, data.LevelWidth - roomWidth - 1);
                int roomYPosition = random.Next(0, data.LevelHeight - roomHeight - 1);

                var newRoom = new Rectangle(
                    roomXPosition, roomYPosition, roomWidth, roomHeight);
                bool newRoomIntersects = rooms.Any(room => newRoom.Intersects(room));

                if (newRoomIntersects == false) { rooms.Add(newRoom); }
                else
                {
                    attempts--;
                    if (attempts == 0)
                    {
                        attempts = 3;
                        continue;
                    }
                    else x--;
                }
            }

            return rooms;
        }

        private void CreateConnectingTunnels(List<Rectangle> rooms, Tile[,] tiles)
        {
            for (int x = 1; x < rooms.Count; x++)
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

        // not implemented yet. Requires addition of a black tile to GameLogic to represent
        // tiles left empty because cannot be seen by player through walls.
        private bool AdjacentToWalkableTile(int x, int y,Tile[,] tiles)
        {
            var standPoint = new Point(x, y);
            foreach(var compassPoint in Direction.EightCompassPoints)
            {
                var checkPoint = standPoint + compassPoint;

                if(checkPoint.X < 0 || checkPoint.X >= tiles.GetLength(0)
                    || checkPoint.Y < 0 || checkPoint.Y >= tiles.GetLength(1))
                {
                    continue;
                }

                if(tiles[checkPoint.X, checkPoint.Y] != null 
                    && tiles[checkPoint.X, checkPoint.Y].IsWalkable)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
