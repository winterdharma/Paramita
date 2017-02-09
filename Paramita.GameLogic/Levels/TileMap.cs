using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;


namespace Paramita.GameLogic.Levels
{
    public class TileMap
    {
        [ContentSerializer(CollectionItemName = "Tiles")]
        private Tile[,] _tiles;

        private int _tilesWide;
        private int _tilesHigh;

        private const string unsupportedTileType = "A TileType was supplied that is not supported for this search.";

        [ContentSerializer]
        public string MapName { get; private set; }

        // Number of tiles wide and high
        public int TilesWide { get { return _tilesWide; } }
        public int TilesHigh { get { return _tilesHigh; } }
        


        public TileMap(Tile[,] tiles, string name)
        {
            this._tiles = tiles;
            _tilesWide = tiles.GetLength(0);
            _tilesHigh = tiles.GetLength(1);
            MapName = name;
        }



        public TileType[,] ConvertMapToTileTypes()
        {
            var typeArray = new TileType[_tilesHigh, _tilesWide];
            for(int i = 0; i < _tiles.GetLength(0); i++)
            {
                for(int j = 0; j < _tiles.GetLength(1); j++)
                {
                    typeArray[i, j] = _tiles[i, j].TypeOfTile;
                }
            }
            return typeArray;
        }


        // Sets a specific Tile in tiles[,] to a new Tile
        public void SetTile(Point coord, Tile newTile)
        {
            _tiles[coord.X, coord.Y] = newTile;
        }



        // Returns the Tile in tiles[,] corresponding to the coord parameter
        // or null if coord is outside the bounds of tiles[,]
        public Tile GetTile(Point coord)
        {
            // return null, if coord is outside the TileMap's bounds
            if (coord.X < 0 || coord.X > TilesWide - 1
                || coord.Y < 0 || coord.Y > TilesHigh - 1)
            {
                return null;
            }

            return _tiles[coord.X, coord.Y];
        }



        // return the value of IsWalkable property on Tile at (x,y) on the map
        public bool IsTileWalkable(int x, int y)
        {
            return _tiles[x, y].IsWalkable;
        }
        

        public Tile FindTileType(TileType type)
        {
            switch(type)
            {
                case TileType.StairsUp:
                    return FindTileByType(TileType.StairsUp);
                case TileType.StairsDown:
                    return FindTileByType(TileType.StairsDown);
                default:
                    Console.WriteLine(unsupportedTileType + "(" + type + ")");
                    break;
            }
            return null;
        }



        // conducts a linear search of @tiles and returns the first StairsDown tile encountered
        private Tile FindTileByType(TileType type)
        {
            for(int x = 0; x < TilesWide; x++)
            {
                for(int y = 0; y < TilesHigh; y++)
                {
                    if (_tiles[x, y].TypeOfTile == type)
                        return _tiles[x, y];
                }
            }
            return null;
        }
    }
}
