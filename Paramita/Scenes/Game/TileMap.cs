using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

// need to continue getting this working - merging RogueSharp Map
// and the TileLayer class together.

namespace Paramita.Scenes
{
    public class TileMap
    {
        string mapName;
        Dictionary<string, Point> characters; // list of NPCs by name and tile coords
        [ContentSerializer]
        int mapWidth;
        [ContentSerializer]
        int mapHeight;
        TileSet tileSet;

        [ContentSerializer(CollectionItemName = "Tiles")]
        Tile[][] tiles;
        
        Point cameraPoint;
        Point viewPoint;
        Point min;
        Point max;
        Rectangle destination;

        
        

        [ContentSerializer]
        public string MapName
        {
            get { return mapName; }
            private set { mapName = value; }
        }

        [ContentSerializer]
        public TileSet TileSet
        {
            get { return tileSet; }
            set { tileSet = value; }
        }



        [ContentSerializer]
        public Dictionary<string, Point> Characters
        {
            get { return characters; }
            private set { characters = value; }
        }

        public int MapWidth
        {
            get { return mapWidth; }
        }

        public int MapHeight
        {
            get { return mapHeight; }
        }

        public int WidthInPixels
        {
            get { return mapWidth * TileEngine.TileWidth; }
        }

        public int HeightInPixels
        {
            get { return mapHeight * TileEngine.TileHeight; }
        }





        public TileMap(TileSet tileSet, int width, int height, string mapName)
        {
            this.characters = new Dictionary<string, Point>();
            mapWidth = width;
            mapHeight = height;
            this.tileSet = tileSet;
            this.mapName = mapName;
        }




        public void SetTile(int x, int y, Tile newTile)
        {
            tiles[x][y] = newTile;
        }


        public Tile GetTile(int x, int y)
        {
            return tiles[x][y];
        }


        


        public void Update(GameTime gameTime)
        {
            
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
        {
            cameraPoint = TileEngine.VectorToCell(camera.Position);
            viewPoint = TileEngine.VectorToCell(
                new Vector2(
                    (camera.Position.X + TileEngine.ViewportRectangle.Width),
                    (camera.Position.Y + TileEngine.ViewportRectangle.Height)
                )
            );

            min.X = Math.Max(0, cameraPoint.X - 1);
            min.Y = Math.Max(0, cameraPoint.Y - 1);
            max.X = Math.Min(viewPoint.X + 1, MapWidth);
            max.Y = Math.Min(viewPoint.Y + 1, MapHeight);
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
                    tile = GetTile(x, y);
                    destination.X = x * TileEngine.TileWidth;
                    spriteBatch.Draw(
                    tileSet.Texture,
                    destination,
                    //tileSet.TilesheetRects[tile],
                    Color.White);
                }
            }
            spriteBatch.End();
        }
    }
}
