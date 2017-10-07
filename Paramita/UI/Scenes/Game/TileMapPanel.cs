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
using Paramita.UI.Base;
using Paramita.UI.Elements;

namespace Paramita.UI.Scenes
{
    public class TileMapPanel : Component
    {
        #region Fields
        private Rectangle _viewport;
        private Vector2 _playerPosition;
        private Rectangle _drawFrame;
        public static Dictionary<string, Texture2D> _spritesheets = new Dictionary<string, Texture2D>();
        private Image[,] _tileArray;
        private Image[,] _itemArray;
        private Sprite[,] _actorArray;
        private const int TILE_SIZE = 32;
        private Point _mapSizeInPixels;
        #endregion

        #region Constructors
        public TileMapPanel(Scene parent, int drawOrder) : base(parent, drawOrder)
        {
            _viewport = parent.ScreenRectangle;
            _drawFrame = new Rectangle(0, 0, TILE_SIZE, TILE_SIZE);
            InitializeLevelMap();
            SubscribeToDungeonNotifications();
        }
        #endregion

        #region Properties
        public static Dictionary<string, Texture2D> Spritesheets
        {
            get { return _spritesheets; }
            set { _spritesheets = value; }
        }
        #endregion

        #region Initialization
        // at some point, combine all the elements into the Elements collection with correct positions
        private void InitializeLevelMap()
        {
            var parent = (GameScene)Parent;
            var tileMapLayers = parent.Dungeon.GetCurrentLevelLayers();
            var tileLayer = tileMapLayers.Item1;
            var itemLayer = tileMapLayers.Item2;
            var actorLayer = tileMapLayers.Item3;

            InitializeLevelMap(tileLayer, itemLayer, actorLayer);
        }

        // overload provided for HandleLevelChange() 
        private void InitializeLevelMap(TileType[,] tileLayer, ItemType[,] itemLayer, 
            Tuple<ActorType, Compass, bool>[,] actorLayer)
        {
            _mapSizeInPixels =
                new Point(tileLayer.GetLength(0) * TILE_SIZE, tileLayer.GetLength(1) * TILE_SIZE);
            _tileArray = CreateTileElements(tileLayer);
            _itemArray = CreateItemElements(itemLayer);
            _actorArray = CreateActorSprites(actorLayer);
        }

        private Image[,] CreateTileElements(TileType[,] tiles)
        {
            var tileImages = new Image[tiles.GetLength(0), tiles.GetLength(1)];

            TileType tileType;
            for(int y = 0; y < tileImages.GetLength(0); y++)
            {
                for (int x = 0; x < tileImages.GetLength(1); x++)
                {
                    tileType = tiles[x, y];
                    tileImages[x, y] = new Image(GetTileId(x,y), this, GetTilePosition(x,y), 
                        GetTexture(tileType.ToString()), 0);
                    tileImages[x, y].Show();
                }
            }
            return tileImages;
        }

        // current way of getting textures needs to change since Sprite class is going away 
        // & changing to an Element with animations in the future
        private Texture2D GetTexture(string key)
        {
            if (key.Equals("None"))
                throw new ArgumentException("None is not a valid key to fetch a texture from Spritesheets");

            return Spritesheets[key];
        }

        private Vector2 GetTilePosition(int x, int y)
        {
            return new Vector2(x * TILE_SIZE, y * TILE_SIZE);
        }

        private string GetTileId(int x, int y)
        {
            return "tile [" + x + ", " + y + "]";
        }

        private Image[,] CreateItemElements(ItemType[,] typeArray)
        {
            var imageArray = new Image[typeArray.GetLength(0), typeArray.GetLength(1)];

            ItemType itemType;
            for (int y = 0; y < typeArray.GetLength(0); y++)
            {
                for (int x = 0; x < typeArray.GetLength(1); x++)
                {
                    if (typeArray[x, y] != ItemType.None)
                    {
                        itemType = typeArray[x, y];
                        imageArray[x, y] = new Image(GetItemId(x,y), this, GetTilePosition(x,y),
                            GetItemTexture(itemType), 1);
                        imageArray[x, y].Show();
                    }
                }
            }
            return imageArray;
        }

        private string GetItemId(int x, int y)
        {
            return "item [" + x + ", " + y + "]";
        }

        private Texture2D GetItemTexture(ItemType itemType)
        {
            return ItemTextures.ItemTextureMap[itemType];
        }

        private Sprite[,] CreateActorSprites(Tuple<ActorType, Compass, bool>[,] actorTypeArray)
        {
            var spriteArray = new Sprite[actorTypeArray.GetLength(0), actorTypeArray.GetLength(1)];
            ActorType actorType; Compass facing; bool isPlayer;

            for(int y = 0; y < actorTypeArray.GetLength(0); y++)
            {
                for(int x = 0; x < actorTypeArray.GetLength(1); x++)
                {
                    var actor = actorTypeArray[x, y];
                    if (actorTypeArray[x, y] != null)
                    {
                        actorType = actor.Item1;
                        facing = actor.Item2;
                        isPlayer = actor.Item3;

                        spriteArray[x, y] = new Sprite(GetSpriteId(x, y), this, GetTilePosition(x, y), 
                            GetTexture(actorType.ToString()), 2);
                        spriteArray[x, y].Facing = facing;
                        spriteArray[x, y].Show();

                        if (isPlayer)
                            _playerPosition = spriteArray[x, y].Position;
                    }
                }
            }

            return spriteArray;
        }

        private string GetSpriteId(int x, int y)
        {
            return "actor [" + x + ", " + y + "]";
        }
        #endregion

        #region Event Handling
        // should be handled by GameScene!
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

            bool isPlayer = false;
            if (sprite.Position == _playerPosition)
                isPlayer = true;

            sprite.Position =
                    new Vector2(destination.X * TILE_SIZE, destination.Y * TILE_SIZE);
            sprite.Facing = Direction.GetDirection(destination - origin);

            if (isPlayer)
                _playerPosition = sprite.Position;

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
            int x = e.Location.X; int y = e.Location.Y;
            _itemArray[x, y] = new Image(GetItemId(x,y), this, 
                GetTilePosition(x,y), GetItemTexture(e.ItemType), 1);
            _itemArray[x, y].Show();
        }

        private void HandleItemRemovedFromMap(object sender, ItemEventArgs e)
        {
            _itemArray[e.Location.X, e.Location.Y] = null;
        }

        // use properties that automatically update the data structures?
        private void HandleLevelChange(object sender, NewLevelEventArgs e)
        {
            InitializeLevelMap(e.Layers.Item1, e.Layers.Item2, e.Layers.Item3);
        }
        #endregion

        #region Public API
        public override void Update(GameTime gameTime)
        {
            Camera.LockToSprite(_mapSizeInPixels, _playerPosition, _viewport);
        }

        // this needs to get simplfied with calls to Element.Draw() etc!
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
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
            Image tileSprite;
            Image itemSprite;
            Sprite actorSprite;

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

                    tileSprite.Draw(gameTime, spriteBatch);

                    if (itemSprite != null)
                    {
                        itemSprite.Draw(gameTime, spriteBatch);
                    }

                    if (actorSprite != null)
                    {
                        actorSprite.Draw(gameTime, spriteBatch);
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
        #endregion
    }
}
