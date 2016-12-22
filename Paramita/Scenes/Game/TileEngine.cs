using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Paramita.Scenes
{
    /*
     * This class will become a LevelManager that handles the saving of TileMaps
     * as they are created and handles level transitions.
     */

    public class TileEngine
    {
        private static Rectangle viewPortRectangle;
        private static int tileWidth = 32;
        private static int tileHeight = 32;
        private TileMap map;


        public static int TileWidth
        {
            get { return tileWidth; }
            set { tileWidth = value; }
        }

        public static int TileHeight
        {
            get { return tileHeight; }
            set { tileHeight = value; }
        }

        public TileMap Map
        {
            get { return map; }
        }

        public static Rectangle ViewportRectangle
        {
            get { return viewPortRectangle; }
            set { viewPortRectangle = value; }
        }
        



        public TileEngine(Rectangle viewPort)
        {
            ViewportRectangle = viewPort;
        }

        public TileEngine(Rectangle viewPort, int tileWidth, int tileHeight) : this(viewPort)
        {
            TileWidth = tileWidth;
            TileHeight = tileHeight;
        }
       



        public static Point PixelsToTileCoords(Vector2 position)
        {
            return new Point(
                (int)position.X / tileWidth, 
                (int)position.Y / tileHeight
            );
        }


        public void SetMap(TileMap newMap)
        {
            if (newMap == null)
            {
                throw new ArgumentNullException("newMap");
            }
            map = newMap;
        }


        public void Update(GameTime gameTime)
        {
            Map.Update(gameTime);
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }
    }
}
