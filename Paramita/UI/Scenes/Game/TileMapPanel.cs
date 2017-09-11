using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.GameLogic.Actors;
using Paramita.GameLogic.Items;
using Paramita.GameLogic.Levels;
using Paramita.GameLogic.Mechanics;
using System;
using System.Collections.Generic;
using Paramita.GameLogic;
using Paramita.GameLogic.Utility;
using Paramita.UI.Input;

namespace Paramita.UI.Base.Game
{
    public class TileMapPanel : Component
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



        public TileMapPanel(Scene parent, Tuple<TileType[,], ItemType[,], 
            Tuple<ActorType, Compass, bool>[,]> tileMapLayers, int drawOrder) : base(parent, drawOrder)
        {
            _viewport = parent.ScreenRectangle;
            _drawFrame = new Rectangle(0, 0, TILE_SIZE, TILE_SIZE);
            InitializeLevelMap(tileMapLayers.Item1, tileMapLayers.Item2, tileMapLayers.Item3);
            SubscribeToDungeonNotifications();
        }


        private void InitializeLevelMap(TileType[,] tileLayer, ItemType[,] itemLayer, 
            Tuple<ActorType, Compass, bool>[,] actorLayer)
        {
            _mapSizeInPixels =
                new Point(tileLayer.GetLength(0) * TILE_SIZE, tileLayer.GetLength(1) * TILE_SIZE);
            _tileArray = CreateTileSprites(tileLayer);
            _itemArray = CreateItemSprites(itemLayer);
            _actorArray = CreateActorSprites(actorLayer);
            _playerPosition = GetPlayerPosition(actorLayer);
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
                    spriteArray[i, j] = new Sprite(_spritesheets[Sprite.GetSpriteType(type)], _drawFrame);
                    spriteArray[i, j].Position = new Vector2(j * TILE_SIZE, i * TILE_SIZE);
                }
            }
            return spriteArray;
        }

        private Sprite[,] CreateItemSprites(ItemType[,] typeArray)
        {
            var spriteArray = new Sprite[typeArray.GetLength(0), typeArray.GetLength(1)];

            ItemType type;

            for (int i = 0; i < typeArray.GetLength(0); i++)
            {
                for (int j = 0; j < typeArray.GetLength(1); j++)
                {
                    if(typeArray[i,j] != ItemType.None)
                    {
                        type = typeArray[i, j];
                        spriteArray[i, j] = CreateItemSprite(new Point(j, i), type);
                    }
                }
            }

            return spriteArray;
        }

        // Constructs an individual item sprite object for the array of item sprites
        private Sprite CreateItemSprite(Point position, ItemType type)
        {
            var sprite = new Sprite(ItemTextures.ItemTextureMap[type], _drawFrame);
            sprite.Position = new Vector2(position.X * TILE_SIZE, position.Y * TILE_SIZE);
            return sprite;
        }


        private BeingSprite[,] CreateActorSprites(Tuple<ActorType, Compass, bool>[,] typeArray)
        {
            var spriteArray = new BeingSprite[typeArray.GetLength(0), typeArray.GetLength(1)];

            ActorType type; Compass facing;

            for (int i = 0; i < typeArray.GetLength(0); i++)
            {
                for (int j = 0; j < typeArray.GetLength(1); j++)
                {
                    if(typeArray[i, j] != null)
                    {
                        type = typeArray[i, j].Item1;
                        facing = typeArray[i, j].Item2;
                        spriteArray[i, j] = new BeingSprite(_spritesheets[Sprite.GetSpriteType(type)], _drawFrame);
                        spriteArray[i, j].Facing = facing;
                        spriteArray[i, j].Position = new Vector2(i * TILE_SIZE, j * TILE_SIZE);
                    }
                }
            }

            return spriteArray;
        }

        private Vector2 GetPlayerPosition(Tuple<ActorType, Compass, bool>[,] array)
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


        private void SubscribeToDungeonNotifications()
        {
            Dungeon.OnActorMoveUINotification += HandleOnActorWasMoved;
            Dungeon.OnItemDroppedUINotification += HandleItemAddedToMap;
            Dungeon.OnItemPickedUpUINotification += HandleItemRemovedFromMap;
            Dungeon.OnLevelChangeUINotification += HandleLevelChange;
            Dungeon.OnActorRemovedUINotification += HandleActorWasRemoved;
        }

        private void HandleOnActorWasMoved(object sender, MoveEventArgs eventArgs)
        {
            var origin = eventArgs.Origin;
            var destination = eventArgs.Destination;
            var sprite = _actorArray[origin.X, origin.Y];

            if (sprite.Position == _playerPosition)
            {
                sprite.Position = 
                    new Vector2(destination.X * TILE_SIZE, destination.Y * TILE_SIZE);
                _playerPosition = sprite.Position;
            }
            else
                sprite.Position = 
                    new Vector2(destination.X * TILE_SIZE, destination.Y * TILE_SIZE);

            _actorArray[destination.X, destination.Y] = sprite;
            _actorArray[origin.X, origin.Y] = null;
        }

        private void HandleActorWasRemoved(object sender, MoveEventArgs eventArgs)
        {
            var origin = eventArgs.Origin;
            _actorArray[origin.X, origin.Y] = null;
        }

        private void HandleItemAddedToMap(object sender, ItemEventArgs e)
        {
            _itemArray[e.Location.X, e.Location.Y] = 
                CreateItemSprite(new Point(e.Location.Y, e.Location.X), e.ItemType);
        }

        private void HandleItemRemovedFromMap(object sender, ItemEventArgs e)
        {
            _itemArray[e.Location.X, e.Location.Y] = null;
        }

        private void HandleLevelChange(object sender, NewLevelEventArgs e)
        {
            InitializeLevelMap(e.Layers.Item1, e.Layers.Item2, e.Layers.Item3);
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

        protected override Dictionary<string, Element> InitializeElements()
        {
            return new Dictionary<string, Element>();
        }

        protected override Rectangle UpdatePanelRectangle()
        {
            return new Rectangle();
        }
    }
}
