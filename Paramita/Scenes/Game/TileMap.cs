using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Items;
using System;
using System.Collections.Generic;

// need to continue getting this working - merging RogueSharp Map
// and the TileLayer class together.

namespace Paramita.Scenes
{
    public class TileMap
    {
        [ContentSerializer(CollectionItemName = "Tiles")]
        private Tile[,] tiles;
        
        Point cameraPoint;
        Point viewPoint;
        Point min;
        Point max;
        Rectangle destination;
        Rectangle viewport;

        
        [ContentSerializer]
        public string MapName { get; private set; }

        [ContentSerializer]
        public TileSet TileSet { get; set; }

        // Number of tiles wide and high
        public int TilesWide { get; private set; }
        public int TilesHigh { get; private set; }
        public int TileSize { get; private set; }

        // Size of map in pixels
        public Point MapSizeInPixels { get; private set; }





        public TileMap(Rectangle parentScreen, TileSet tileSet, Tile[,] tiles, int width, int height, string name)
        {
            viewport = parentScreen;
            TilesWide = width;
            TilesHigh = height;
            TileSet = tileSet;
            TileSize = tileSet.TileSize;
            MapSizeInPixels = new Point(width * tileSet.TileSize, height * tileSet.TileSize);
            MapName = name;
            this.tiles = tiles; 
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



        // Converts a Tile's XY coordinates on the tilemap to a Vector2
        // of its position in pixels
        public Vector2 GetTilePosition(Tile tile)
        {
            return new Vector2(
                tile.TilePoint.X * TileSize,
                tile.TilePoint.Y * TileSize);
        }



        // Converts a Vector2 position to a Tile's XY coordinates on the tilemap
        public Point PositionToTileCoords(Vector2 position)
        {
            return new Point(
                (int)position.X / TileSize,
                (int)position.Y / TileSize
            );
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
                    return FindStairsUpTile();
                case TileType.StairsDown:
                    return FindStairsDownTile();
            }
            return null;
        }



        // conducts a linear search of @tiles and returns the first StairsDown tile encountered
        private Tile FindStairsDownTile()
        {
            for(int x = 0; x < TilesWide; x++)
            {
                for(int y = 0; y < TilesHigh; y++)
                {
                    if (tiles[x, y].TileType == TileType.StairsDown)
                        return tiles[x, y];
                }
            }
            return null;
        }

        // conducts a linear search of @tiles and returns the first StairsUp tile encountered
        private Tile FindStairsUpTile()
        {
            for (int x = 0; x < TilesWide; x++)
            {
                for (int y = 0; y < TilesHigh; y++)
                {
                    if (tiles[x, y].TileType == TileType.StairsUp)
                        return tiles[x, y];
                }
            }
            return null;
        }

        public void Update(GameTime gameTime)
        {
            // implement in future if the map needs to change its tiles dynamically
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            cameraPoint = PositionToTileCoords(Camera.Position);
            viewPoint = PositionToTileCoords(
                new Vector2(
                    (Camera.Position.X + viewport.Width),
                    (Camera.Position.Y + viewport.Height)
                )
            );

            min.X = Math.Max(0, cameraPoint.X - 1);
            min.Y = Math.Max(0, cameraPoint.Y - 1);
            max.X = Math.Min(viewPoint.X + 1, TilesWide);
            max.Y = Math.Min(viewPoint.Y + 1, TilesHigh);
            destination = new Rectangle(0, 0, TileSize, TileSize);
            Tile tile;

            

            for (int y = min.Y; y < max.Y; y++)
            {
                destination.Y = y * TileSize;
                for (int x = min.X; x < max.X; x++)
                {
                    tile = GetTile(new Point(x, y));
                    Item[] tileItems = tile.InspectItems();
                    destination.X = x * TileSize;

                    spriteBatch.Begin(
                     SpriteSortMode.Deferred,
                     BlendState.AlphaBlend,
                     SamplerState.PointClamp,
                     null, null, null,
                     Camera.Transformation);

                    spriteBatch.Draw(
                    TileSet.Texture,
                    destination,
                    TileSet.GetRectForTileType(tile.TileType),
                    Color.White);

                    spriteBatch.End();

                    if (tileItems.Length > 0)
                    {
                        tileItems[0].Sprite.Draw(gameTime, spriteBatch);
                    }
                }
            }
        }
    }
}
