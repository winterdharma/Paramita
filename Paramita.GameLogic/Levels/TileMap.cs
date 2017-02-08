using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;


namespace Paramita.GameLogic.Levels
{
    public class TileMap
    {
        [ContentSerializer(CollectionItemName = "Tiles")]
        private Tile[,] tiles;

        private int tilesWide;
        private int tilesHigh;

        private const string unsupportedTileType = "A TileType was supplied that is not supported for this search.";

        [ContentSerializer]
        public string MapName { get; private set; }

        // Number of tiles wide and high
        public int TilesWide { get { return tilesWide; } }
        public int TilesHigh { get { return tilesHigh; } }
        


        public TileMap(Tile[,] tiles, string name)
        {
            this.tiles = tiles;
            tilesWide = tiles.GetLength(0);
            tilesHigh = tiles.GetLength(1);
            MapName = name;
        }



        // Sets a specific Tile in tiles[,] to a new Tile
        public void SetTile(Point coord, Tile newTile)
        {
            tiles[coord.X, coord.Y] = newTile;
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

            return tiles[coord.X, coord.Y];
        }



        // return the value of IsWalkable property on Tile at (x,y) on the map
        public bool IsTileWalkable(int x, int y)
        {
            return tiles[x, y].IsWalkable;
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
                    if (tiles[x, y].TypeOfTile == type)
                        return tiles[x, y];
                }
            }
            return null;
        }
    }
}
