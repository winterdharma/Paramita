using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.GameLogic.Actors;
using Paramita.GameLogic.Items;
using Paramita.GameLogic.Levels;
using Paramita.GameLogic.Mechanics;
using System;
using System.Collections.Generic;
using Paramita.GameLogic;

namespace Paramita.UI.Scenes.Game
{
    public enum SpriteType
    {
        Tile_Floor,
        Tile_Wall,
        Tile_Door,
        Tile_StairsUp,
        Tile_StairsDown,
        Actor_GiantRat,
        Actor_Player,
        Item_Buckler,
        Item_ShortSword,
        Item_Coins,
        Item_Meat
    }

    public class TileMapPanel
    {
        private Rectangle _viewport;
        private Vector2 _playerPosition;
        private Rectangle _drawFrame;
        public static Dictionary<SpriteType, Texture2D> _spritesheets = new Dictionary<SpriteType, Texture2D>();
        private Sprite[,] _tileArray;
        private Sprite[,] _itemArray;
        private BeingSprite[,] _actorArray;
        private const int TILE_SIZE = 32;
        private Point _mapSizeInPixels;

        public static Dictionary<SpriteType, Texture2D> Spritesheets
        {
            get { return _spritesheets; }
            set { _spritesheets = value; }
        }



        public TileMapPanel(TileType[,] tileArray, Tuple<ItemType>[,] itemArray, 
            Tuple<BeingType, Compass, bool>[,] actorArray)
        {
            _viewport = GameController.ScreenRectangle;
            _drawFrame = new Rectangle(0, 0, TILE_SIZE, TILE_SIZE);
            _tileArray = CreateTileSprites(tileArray);
            _mapSizeInPixels = 
                new Point(tileArray.GetLength(0) * TILE_SIZE, tileArray.GetLength(1) * TILE_SIZE);
            _itemArray = CreateItemSprites(itemArray);
            _actorArray = CreateActorSprites(actorArray);
            _playerPosition = GetPlayerPosition(actorArray);
            SubscribeToDungeonNotifications();
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
                    spriteArray[i, j] = new Sprite(_spritesheets[GetSpriteType(type)], _drawFrame);
                    spriteArray[i, j].Position = new Vector2(j * TILE_SIZE, i * TILE_SIZE);
                }
            }
            return spriteArray;
        }

        private Sprite[,] CreateItemSprites(Tuple<ItemType>[,] typeArray)
        {
            var spriteArray = new Sprite[typeArray.GetLength(0), typeArray.GetLength(1)];

            ItemType type;

            for (int i = 0; i < typeArray.GetLength(0); i++)
            {
                for (int j = 0; j < typeArray.GetLength(1); j++)
                {
                    if(typeArray[i,j] != null)
                    {
                        type = typeArray[i, j].Item1;
                        spriteArray[i, j] = new Sprite(_spritesheets[GetSpriteType(type)], _drawFrame);
                        spriteArray[i, j].Position = new Vector2(j * TILE_SIZE, i * TILE_SIZE);
                    }
                }
            }

            return spriteArray;
        }

        private BeingSprite[,] CreateActorSprites(Tuple<BeingType, Compass, bool>[,] typeArray)
        {
            var spriteArray = new BeingSprite[typeArray.GetLength(0), typeArray.GetLength(1)];

            BeingType type; Compass facing;

            for (int i = 0; i < typeArray.GetLength(0); i++)
            {
                for (int j = 0; j < typeArray.GetLength(1); j++)
                {
                    if(typeArray[i, j] != null)
                    {
                        type = typeArray[i, j].Item1;
                        facing = typeArray[i, j].Item2;
                        spriteArray[i, j] = new BeingSprite(_spritesheets[GetSpriteType(type)], _drawFrame);
                        spriteArray[i, j].Facing = facing;
                        spriteArray[i, j].Position = new Vector2(i * TILE_SIZE, j * TILE_SIZE);
                    }
                }
            }

            return spriteArray;
        }

        private Vector2 GetPlayerPosition(Tuple<BeingType, Compass, bool>[,] array)
        {
            var position = Vector2.Zero;

            for(int i = 0; i < array.GetLength(0); i++)
			{
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if(array[i,j] != null && array[i,j].Item3)
                    {
                        position = new Vector2(i * TILE_SIZE, j * TILE_SIZE);
                    }
                }
            }

            return position;
        }

        private SpriteType GetSpriteType(TileType type)
        {
            switch (type)
            {
                case TileType.Door:
                    return SpriteType.Tile_Door;
                case TileType.Floor:
                    return SpriteType.Tile_Floor;
                case TileType.StairsDown:
                    return SpriteType.Tile_StairsDown;
                case TileType.StairsUp:
                    return SpriteType.Tile_StairsUp;
                case TileType.Wall:
                    return SpriteType.Tile_Wall;
                default:
                    throw new NotImplementedException("TileMapPanel.GetSpriteType(): TileType not implemented.");
            }
        }

        private SpriteType GetSpriteType(ItemType type)
        {
            switch (type)
            {
                case ItemType.Coins:
                    return SpriteType.Item_Coins;
                case ItemType.Meat:
                    return SpriteType.Item_Meat;
                case ItemType.Shield:
                    return SpriteType.Item_Buckler;
                case ItemType.ShortSword:
                    return SpriteType.Item_ShortSword;
                default:
                    throw new NotImplementedException("TileMapPanel.GetSpriteType(): ItemType not implemented.");
            }
        }

        private SpriteType GetSpriteType(BeingType type)
        {
            switch (type)
            {
                case BeingType.GiantRat:
                    return SpriteType.Actor_GiantRat;
                case BeingType.HumanPlayer:
                    return SpriteType.Actor_Player;
                default:
                    throw new NotImplementedException("TileMapPanel.GetSpriteType(): BeingType not implemented.");
            }
        }


        private void SubscribeToDungeonNotifications()
        {
            Dungeon.OnActorMoveUINotification += HandleOnActorWasMoved;
        }

        private void HandleOnActorWasMoved(object sender, MoveEventArgs eventArgs)
        {
            Point oldTile = eventArgs.TilePoint - Direction.GetPoint(eventArgs.Direction);
            var sprite = _actorArray[oldTile.X, oldTile.Y];

            if (sprite.Position == _playerPosition)
            {
                sprite.Position = new Vector2(eventArgs.TilePoint.X * TILE_SIZE, eventArgs.TilePoint.Y * TILE_SIZE);
                _playerPosition = sprite.Position;
            }
            else
                sprite.Position = new Vector2(eventArgs.TilePoint.X * TILE_SIZE, eventArgs.TilePoint.Y * TILE_SIZE);

            _actorArray[eventArgs.TilePoint.X, eventArgs.TilePoint.Y] = sprite;
            _actorArray[oldTile.X, oldTile.Y] = null;
        }


        public void Update(GameTime gameTime)
        {
            Camera.LockToSprite(_mapSizeInPixels, _playerPosition, _viewport);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Point min; Point max;
            int tilesWide = _tileArray.GetLength(0);
            int tilesHigh = _tileArray.GetLength(1);
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
            Sprite tileSprite;
            Sprite itemSprite;
            BeingSprite actorSprite;

            for (int i = min.X; i < max.X; i++)
            {
                drawFrame.X = i * TILE_SIZE;
                for (int j = min.Y; j < max.Y; j++)
                {
                    tileSprite = _tileArray[i, j];
                    itemSprite = _itemArray[i, j];
                    actorSprite = _actorArray[i, j];

                    drawFrame.Y = j * TILE_SIZE;

                    spriteBatch.Begin(
                     SpriteSortMode.Deferred,
                     BlendState.AlphaBlend,
                     SamplerState.PointClamp,
                     null, null, null,
                     Camera.Transformation);

                    spriteBatch.Draw(
                        tileSprite.Texture,
                        drawFrame,
                        Color.White);

                    if (itemSprite != null)
                    {
                        spriteBatch.Draw(
                            itemSprite.Texture,
                            drawFrame,
                            Color.White);
                    }

                    if (actorSprite != null)
                    {
                        spriteBatch.Draw(
                            actorSprite.Texture,
                            drawFrame,
                            actorSprite.Textures[actorSprite.Facing],
                            Color.White);
                    }

                    spriteBatch.End();                 
                }
            }
        }

        private Point PositionToArrayIndices(Vector2 position)
        {
            return new Point((int)position.X / TILE_SIZE, (int)position.Y / TILE_SIZE);
        }
    }
}
