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

        
        [ContentSerializer]
        public string MapName { get; private set; }

        [ContentSerializer]
        public TileSet TileSet { get; set; }

        // Number of tiles wide and high
        public int TilesWide { get; private set; }
        public int TilesHigh { get; private set; }

        // Size of map in pixels
        public int WidthInPixels
        {
            get { return TilesWide * TileEngine.TileWidth; }
        }
        public int HeightInPixels
        {
            get { return TilesHigh * TileEngine.TileHeight; }
        }





        public TileMap(TileSet tileSet, Tile[,] tiles, int width, int height, string name)
        {
            TilesWide = width;
            TilesHigh = height;
            TileSet = tileSet;
            MapName = name;
            this.tiles = tiles; 
        }




        public void SetTile(Point coord, Tile newTile)
        {
            tiles[coord.X, coord.Y] = newTile;
        }

        public Tile GetTile(Point coord)
        {
            return tiles[coord.X, coord.Y];
        }

        public Vector2 GetTilePosition(Tile tile)
        {
            return new Vector2(
                tile.TilePoint.X * TileSet.TileSize,
                tile.TilePoint.Y * TileSet.TileSize);
        }

        // return the value of IsWalkable property on Tile at (x,y) on the map
        public bool IsTileWalkable(int x, int y)
        {
            return tiles[x, y].IsWalkable;
        }
        


        public void Update(GameTime gameTime)
        {
            // implement in future if the map needs to change its tiles dynamically
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
        {
            cameraPoint = TileEngine.PixelsToTileCoords(camera.Position);
            viewPoint = TileEngine.PixelsToTileCoords(
                new Vector2(
                    (camera.Position.X + TileEngine.ViewportRectangle.Width),
                    (camera.Position.Y + TileEngine.ViewportRectangle.Height)
                )
            );

            min.X = Math.Max(0, cameraPoint.X - 1);
            min.Y = Math.Max(0, cameraPoint.Y - 1);
            max.X = Math.Min(viewPoint.X + 1, TilesWide);
            max.Y = Math.Min(viewPoint.Y + 1, TilesHigh);
            destination = new Rectangle(0, 0, TileEngine.TileWidth, TileEngine.TileHeight);
            Tile tile;

            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null, null, null,
                camera.Transformation);

            for (int y = min.Y; y < max.Y; y++)
            {
                destination.Y = y * TileEngine.TileHeight;
                for (int x = min.X; x < max.X; x++)
                {
                    tile = GetTile(new Point(x, y));
                    Item[] tileItems = tile.InspectItems();
                    destination.X = x * TileEngine.TileWidth;

                    spriteBatch.Draw(
                    TileSet.Texture,
                    destination,
                    TileSet.GetRectForTileType(tile.TileType),
                    Color.White);

                    if(tileItems.Length > 0)
                    {
                        spriteBatch.Draw(
                        tileItems[0].Texture,
                        destination,
                        tileItems[0].TextureRect,
                        Color.White);
                    }
                }
            }
            spriteBatch.End();
        }
    }
}
