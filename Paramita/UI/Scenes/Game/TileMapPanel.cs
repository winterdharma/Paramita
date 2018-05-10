using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.GameLogic.Actors;
using Paramita.GameLogic.Items;
using Paramita.GameLogic.Levels;
using Paramita.GameLogic.Mechanics;
using System;
using System.Collections.Generic;
using MonoGameUI.Base;
using MonoGameUI.Elements;
using System.Linq;
using Paramita.UI.Elements;
using Paramita.UI.Assets;

namespace Paramita.UI.Scenes
{
    public class TileMapPanel : Component
    {
        #region Fields
        private Vector2 _playerPosition;
        private Rectangle _drawFrame;
        private TileType[,] _tileLayer;
        private ItemType[,] _itemLayer;
        private Tuple<ActorType, Compass, bool>[,] _actorLayer;
        private const int TILE_SIZE = 32;
        private Point _mapSizeInPixels;
        private Rectangle _mapRectangle;
        private Rectangle _viewport;
        #endregion

        #region Constructors
        public TileMapPanel(Scene parent, int drawOrder) : base(parent, drawOrder)
        {
            _drawFrame = new Rectangle(0, 0, TILE_SIZE, TILE_SIZE);
            _viewport = _panelRectangle;
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

        private void InitializeLevelMap()
        {
            var parent = (GameScene)Parent;
            var tileMapLayers = parent.Dungeon.GetCurrentLevelLayers();
            InitializeLevelMap(tileMapLayers.Item1, tileMapLayers.Item2, tileMapLayers.Item3);
        }

        private void InitializeLevelMap(TileType[,] tileLayer, ItemType[,] itemLayer, 
            Tuple<ActorType, Compass, bool>[,] actorLayer)
        {
            _tileLayer = tileLayer;
            _itemLayer = itemLayer;
            _actorLayer = actorLayer;

            _mapSizeInPixels =
                new Point(_tileLayer.GetLength(0) * TILE_SIZE, _tileLayer.GetLength(1) * TILE_SIZE);
            _mapRectangle = new Rectangle(0, 0, _mapSizeInPixels.X, _mapSizeInPixels.Y);
        }

        protected override Dictionary<string, Element> InitializeElements()
        {
            var elements = new Dictionary<string, Element>();

            CreateTileElements(_tileLayer).ToList().ForEach(x => elements.Add(x.Key, x.Value));
            CreateItemElements(_itemLayer).ToList().ForEach(x => elements.Add(x.Key, x.Value));
            CreateActorSprites(_actorLayer).ToList().ForEach(x => elements.Add(x.Key, x.Value));

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
                        TileTextures.Get(tileType), 0);
                }
            }
            return tilesDict;
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
                            ItemTextures.Get(itemType), 1);
                    }
                }
            }
            return itemsDict;
        }

        private string GetItemId(int x, int y)
        {
            return "item [" + x + ", " + y + "]";
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
                            ActorTextures.Get(actorType), 2)
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

        
        #region Public API
        public override void Update(GameTime gameTime)
        {
            CenterViewportOnPlayerSprite(_playerPosition, _mapRectangle);
            StopViewportAtTilemapEdges(_viewport, _mapRectangle);
        }

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

        public void MoveSprite(Point origin, Point destination)
        {
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

        public void RemoveSprite(int x, int y)
        {
            var elements = new Dictionary<string, Element>(Elements);
            elements.Remove(GetSpriteId(x, y));
            Elements[GetSpriteId(x, y)].Hide();
            Elements = elements;
        }

        public void AddImage(int x, int y, ItemType itemType)
        {
            string itemId = GetItemId(x, y);
            var elements = new Dictionary<string, Element>(Elements);
            elements[itemId] = new Image(itemId, this,
                GetTilePosition(x, y), ItemTextures.Get(itemType), 1);
            Elements = elements;
            Elements[itemId].Show();
        }

        public void RemoveImage(int x, int y)
        {
            var elements = new Dictionary<string, Element>(Elements);
            elements.Remove(GetItemId(x, y));
            Elements[GetItemId(x, y)].Hide();
            Elements = elements;
        }

        public void ChangeLevel(
            Tuple<TileType[,], ItemType[,], Tuple<ActorType, Compass, bool>[,]> tileMapLayers)
        {
            InitializeLevelMap(tileMapLayers.Item1, tileMapLayers.Item2, tileMapLayers.Item3);
        }
        #endregion

        #region Helper Methods
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
