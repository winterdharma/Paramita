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
using System.Linq;

namespace Paramita.UI.Scenes
{
    public class TileMapPanel : Component
    {
        #region Fields
        private Vector2 _playerPosition;
        private Rectangle _drawFrame;
        public static Dictionary<string, Texture2D> _spritesheets = new Dictionary<string, Texture2D>();
        private Dictionary<string, Image> _tiles;
        private Dictionary<string, Image> _items;
        private Dictionary<string, Sprite> _actors;
        private const int TILE_SIZE = 32;
        private Point _mapSizeInPixels;
        private Rectangle _mapRectangle;
        private Rectangle _viewport;
        #endregion

        #region Constructors
        public TileMapPanel(Scene parent, int drawOrder) : base(parent, drawOrder)
        {
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
        protected override void Initialize()
        {
            InitializeLevelMap();
            base.Initialize();
        }
        protected override Rectangle UpdatePanelRectangle()
        {
            return _parentRectangle;
        }

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
            _mapRectangle = new Rectangle(0, 0, _mapSizeInPixels.X, _mapSizeInPixels.Y);
            _viewport = _panelRectangle;
            _tiles = CreateTileElements(tileLayer);
            _items = CreateItemElements(itemLayer);
            _actors = CreateActorSprites(actorLayer);
        }

        protected override Dictionary<string, Element> InitializeElements()
        {
            var elements = new Dictionary<string, Element>();

            _tiles.ToList().ForEach(x => elements.Add(x.Key, x.Value));
            _items.ToList().ForEach(x => elements.Add(x.Key, x.Value));
            _actors.ToList().ForEach(x => elements.Add(x.Key, x.Value));

            return elements;
        }

        private Dictionary<string, Image> CreateTileElements(TileType[,] tiles)
        {
            var tilesDict = new Dictionary<string, Image>();

            TileType tileType;
            for(int y = 0; y < tiles.GetLength(0); y++)
            {
                for (int x = 0; x < tiles.GetLength(1); x++)
                {
                    tileType = tiles[x, y];
                    string tileId = GetTileId(x, y);
                    tilesDict[tileId] = new Image(tileId, this, GetTilePosition(x,y), 
                        GetTexture(tileType.ToString()), 0);
                }
            }
            return tilesDict;
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

        private Dictionary<string, Image> CreateItemElements(ItemType[,] items)
        {
            var itemsDict = new Dictionary<string, Image>();

            ItemType itemType;
            for (int y = 0; y < items.GetLength(0); y++)
            {
                for (int x = 0; x < items.GetLength(1); x++)
                {
                    if (items[x, y] != ItemType.None)
                    {
                        itemType = items[x, y];
                        string itemId = GetItemId(x, y);
                        itemsDict[itemId] = new Image(itemId, this, GetTilePosition(x,y),
                            GetItemTexture(itemType), 1);
                    }
                }
            }
            return itemsDict;
        }

        private string GetItemId(int x, int y)
        {
            return "item [" + x + ", " + y + "]";
        }

        private Texture2D GetItemTexture(ItemType itemType)
        {
            return ItemTextures.ItemTextureMap[itemType];
        }

        private Dictionary<string, Sprite> CreateActorSprites(Tuple<ActorType, Compass, bool>[,] actors)
        {
            var actorsDict = new Dictionary<string, Sprite>();
            ActorType actorType; Compass facing; bool isPlayer;

            for(int y = 0; y < actors.GetLength(0); y++)
            {
                for(int x = 0; x < actors.GetLength(1); x++)
                {
                    var actor = actors[x, y];
                    if (actors[x, y] != null)
                    {
                        actorType = actor.Item1;
                        facing = actor.Item2;
                        isPlayer = actor.Item3;
                        string spriteId = GetSpriteId(x, y);
                        actorsDict[spriteId] = new Sprite(GetSpriteId(x, y), this, GetTilePosition(x, y),
                            GetTexture(actorType.ToString()), 2)
                        {
                            Facing = facing
                        };

                        if (isPlayer)
                            _playerPosition = actorsDict[spriteId].Position;
                    }
                }
            }

            return actorsDict;
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
            var sprite = (Sprite)Elements[GetSpriteId(origin.X, origin.Y)];

            bool isPlayer = false;
            if (sprite.Position == _playerPosition)
                isPlayer = true;

            sprite.Position =
                    new Vector2(destination.X * TILE_SIZE, destination.Y * TILE_SIZE);
            sprite.Facing = Direction.GetDirection(destination - origin);
            sprite.Id = GetSpriteId(destination.X, destination.Y);

            if (isPlayer)
                _playerPosition = sprite.Position;

            Elements[sprite.Id] = sprite;
            Elements.Remove(GetSpriteId(origin.X, origin.Y));
        }

        private void HandleActorWasRemoved(object sender, MoveEventArgs eventArgs)
        {
            var origin = eventArgs.Origin;
            var elements = new Dictionary<string, Element>(Elements);
            elements.Remove(GetSpriteId(origin.X, origin.Y));
            Elements[GetSpriteId(origin.X, origin.Y)].Hide();
            Elements = elements;
        }

        private void HandleItemAddedToMap(object sender, ItemEventArgs e)
        {
            int x = e.Location.X; int y = e.Location.Y;
            string itemId = GetItemId(x, y);
            var elements = new Dictionary<string, Element>(Elements);
            elements[itemId] = new Image(itemId, this, 
                GetTilePosition(x,y), GetItemTexture(e.ItemType), 1);
            Elements = elements;
            Elements[itemId].Show();
        }

        private void HandleItemRemovedFromMap(object sender, ItemEventArgs e)
        {
            var elements = new Dictionary<string, Element>(Elements);
            elements.Remove(GetItemId(e.Location.X, e.Location.Y));
            Elements[GetItemId(e.Location.X, e.Location.Y)].Hide();
            Elements = elements;
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
            CenterViewportOnPlayerSprite(_playerPosition, _mapRectangle);
            StopViewportAtTilemapEdges(_viewport, _mapRectangle);
        }

        // this needs to get simplfied with calls to Element.Draw() etc!
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach(var element in _visibleElements)
            {
                if (_viewport.Intersects(element.Rectangle))
                {
                    if (element is Sprite sprite)
                        sprite.Draw(gameTime, spriteBatch,
                            sprite.Position - new Vector2(_viewport.Location.X, _viewport.Location.Y));
                    else if (element is Image image)
                        image.Draw(gameTime, spriteBatch,
                            image.Position - new Vector2(_viewport.Location.X, _viewport.Location.Y));
                    
                }   
            }
            spriteBatch.End();
        }

        private void StopViewportAtTilemapEdges(Rectangle viewport, Rectangle mapRectangle)
        {
            var position = new Vector2(
                MathHelper.Clamp(viewport.Location.X, 0, mapRectangle.Width - viewport.Width),
                MathHelper.Clamp(viewport.Location.Y, 0, mapRectangle.Width - viewport.Height));
            _viewport.Location = new Point((int)position.X, (int)position.Y);
        }

        private void CenterViewportOnPlayerSprite(Vector2 playerPosition, Rectangle mapRectangle)
        {
            var position = new Vector2(
                (playerPosition.X + 16) - (_viewport.Width / 2),
                (playerPosition.Y + 16) - (_viewport.Height / 2));
            _viewport.Location = new Point((int)position.X, (int)position.Y);
        }
        #endregion
    }
}
