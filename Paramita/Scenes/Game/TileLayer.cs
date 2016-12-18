using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Paramita.Scenes
{
    public class TileLayer
    {
        [ContentSerializer(CollectionItemName = "Tiles")]
        int[] tiles;
        int width;
        int height;
        Point cameraPoint;
        Point viewPoint;
        Point min;
        Point max;
        Rectangle destination;

        
        [ContentSerializerIgnore]
        public bool Enabled { get; set; }

        [ContentSerializerIgnore]
        public bool Visible { get; set; }

        [ContentSerializer]
        public int Width
        {
            get { return width; }
            private set { width = value; }
        }

        [ContentSerializer]
        public int Height
        {
            get { return height; }
            private set { height = value; }
        }
       


        private TileLayer()
        {
            Enabled = true;
            Visible = true;
        }


        public TileLayer(int[] tiles, int width, int height) : this()
        {
            this.tiles = (int[])tiles.Clone();
            this.width = width;
            this.height = height;
        }


        public TileLayer(int width, int height) : this()
        {
            tiles = new int[height * width];
            this.width = width;
            this.height = height;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tiles[y * width + x] = 0;
                }
            }
        }


        public TileLayer(int width, int height, int fill) : this()
        {
            tiles = new int[height * width];
            this.width = width;
            this.height = height;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tiles[y * width + x] = fill;
                }
            }
        }
        




        public int GetTile(int x, int y)
        {
            if (x < 0 || y < 0)
                return -1;
            if (x >= width || y >= height)
                return -1;
            return tiles[y * width + x];
        }


        public void SetTile(int x, int y, int tileIndex)
        {
            if (x < 0 || y < 0)
                return;
            if (x >= width || y >= height)
                return;
            tiles[y * width + x] = tileIndex;
        }


        public void Update(GameTime gameTime)
        {
            if (!Enabled)
                return;
        }



        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, TileSet tileSet, Camera camera)
        {
            if (!Visible)
            {
                return;
            }

            cameraPoint = TileEngine.VectorToCell(camera.Position);
            viewPoint = TileEngine.VectorToCell(
                new Vector2(
                    (camera.Position.X + TileEngine.ViewportRectangle.Width),
                    (camera.Position.Y + TileEngine.ViewportRectangle.Height)
                )
            );

            min.X = Math.Max(0, cameraPoint.X - 1);
            min.Y = Math.Max(0, cameraPoint.Y - 1);
            max.X = Math.Min(viewPoint.X + 1, Width);
            max.Y = Math.Min(viewPoint.Y + 1, Height);
            destination = new Rectangle(0, 0, TileEngine.TileWidth, TileEngine.TileHeight);
            int tile;

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
                    if (tile == -1)
                        continue;
                    destination.X = x * TileEngine.TileWidth;
                    spriteBatch.Draw(
                    tileSet.Texture,
                    destination,
                    tileSet.TilesheetRects[tile],
                    Color.White);
                }
            }
            spriteBatch.End();
        }
    }
}
