using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.GameLogic.Levels;
using System;
using System.Collections.Generic;

namespace Paramita.UI.Scenes.Game
{
    class TileMapPanel
    {
        private Rectangle _viewport;
        private Rectangle _drawFrame;
        public static Dictionary<TileType, Texture2D> _spritesheets = new Dictionary<TileType, Texture2D>();
        private Sprite[,] _spriteArray2D;
        private const int TILE_SIZE = 32;

        public static Dictionary<TileType, Texture2D> Spritesheets
        {
            get { return _spritesheets; }
            set { _spritesheets = value; }
        }



        public TileMapPanel(TileType[,] typeArray)
        {
            _viewport = GameController.ScreenRectangle;
            _spriteArray2D = CreateTileSprites(typeArray);
            _drawFrame = new Rectangle(0, 0, TILE_SIZE, TILE_SIZE);
        }



        private Sprite[,] CreateTileSprites(TileType[,] typeArray)
        {
            var spriteArray = new Sprite[typeArray.GetLength(0), typeArray.GetLength(1)];

            TileType type;

            for (int i = 0; i < typeArray.GetLength(0); i++)
            {
                for(int j = 0; j < typeArray.GetLength(1); j++)
                {
                    type = typeArray[i, j];
                    spriteArray[i, j] = new Sprite(_spritesheets[type], _drawFrame);
                    spriteArray[i, j].Position = new Vector2(j * TILE_SIZE, i * TILE_SIZE);
                }
            }
            return spriteArray;
        }


        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Point min; Point max;
            int tilesHigh = _spriteArray2D.GetLength(0);
            int tilesWide = _spriteArray2D.GetLength(1);
            Point cameraPoint = PositionToArrayIndices(Camera.Position);
            Point viewPoint = PositionToArrayIndices(
                new Vector2(
                    (Camera.Position.X + _viewport.Width),
                    (Camera.Position.Y + _viewport.Height)
                )
            );

            min.X = Math.Max(0, cameraPoint.X - 1);
            min.Y = Math.Max(0, cameraPoint.Y - 1);
            max.X = Math.Min(viewPoint.X + 1, tilesWide);
            max.Y = Math.Min(viewPoint.Y + 1, tilesHigh);
            var drawFrame = _drawFrame; 
            Sprite sprite;

            for (int i = min.X; i < max.X; i++)
            {
                drawFrame.Y = i * TILE_SIZE;
                for (int j = min.Y; j < max.Y; j++)
                {
                    sprite = _spriteArray2D[i, j];
                    //Item[] tileItems = tile.InspectItems();
                    drawFrame.X = j * TILE_SIZE;

                    spriteBatch.Begin(
                     SpriteSortMode.Deferred,
                     BlendState.AlphaBlend,
                     SamplerState.PointClamp,
                     null, null, null,
                     Camera.Transformation);

                    spriteBatch.Draw(
                        sprite.Texture,
                        drawFrame,
                        Color.White);

                    spriteBatch.End();

                    //if (tileItems.Length > 0)
                    //{
                    //    tileItems[0].Sprite.Draw(gameTime, spriteBatch);
                    //}
                }
            }
        }

        private Point PositionToArrayIndices(Vector2 position)
        {
            return new Point((int)position.Y / TILE_SIZE, (int)position.X / TILE_SIZE);
        }
    }
}
