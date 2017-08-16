using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input.InputListeners;
using Paramita.GameLogic;
using Paramita.GameLogic.Items;
using Paramita.UI.Input;
using Paramita.UI.Scenes.Game;
using Paramita.GameLogic.Mechanics;

namespace Paramita.UI.Scenes
{

    public class GameScene : Scene
    {
        private Dungeon _dungeon;
        private TileMapPanel _tileMapPanel;        
        private static StatusPanel _statusPanel;
        private InventoryPanel _inventoryPanel;


        public GameScene(GameController game) : base(game) { }


        public override void Initialize()
        {
            base.Initialize(); // This calls LoadContent()

            _dungeon = new Dungeon();

            _tileMapPanel = new TileMapPanel(_dungeon.GetCurrentLevelLayers(), _screenRectangle);
            _statusPanel = new StatusPanel(GameController.ArialBold, 10, new Point(0,720));
            _inventoryPanel = new InventoryPanel(_input, _screenRectangle);
            _dungeon.GetPlayerInventory(); // raises event that inventoryPanel catches
        }

        protected override void LoadContent()
        {
            ItemTextures.ItemTextureMap[ItemType.Coins] = _content.Load<Texture2D>("Images\\Items\\coins");
            ItemTextures.ItemTextureMap[ItemType.Meat] = _content.Load<Texture2D>("Images\\Items\\meat");
            ItemTextures.ItemTextureMap[ItemType.Shield] = _content.Load<Texture2D>("Images\\Items\\buckler");
            ItemTextures.ItemTextureMap[ItemType.ShortSword] = _content.Load<Texture2D>("Images\\Items\\short_sword");

            TileMapPanel.Spritesheets.Add(SpriteType.Tile_Floor, _content.Load<Texture2D>("Images\\Tiles\\floor"));
            TileMapPanel.Spritesheets.Add(SpriteType.Tile_Door, _content.Load<Texture2D>("Images\\Tiles\\door"));
            TileMapPanel.Spritesheets.Add(SpriteType.Tile_Wall, _content.Load<Texture2D>("Images\\Tiles\\wall"));
            TileMapPanel.Spritesheets.Add(SpriteType.Tile_StairsUp, _content.Load<Texture2D>("Images\\Tiles\\stairs_up"));
            TileMapPanel.Spritesheets.Add(SpriteType.Tile_StairsDown, _content.Load<Texture2D>("Images\\Tiles\\stairs_down"));
            TileMapPanel.Spritesheets.Add(SpriteType.Actor_GiantRat, _content.Load<Texture2D>("Images\\SentientBeings\\giant_rat"));
            TileMapPanel.Spritesheets.Add(SpriteType.Actor_Player, _content.Load<Texture2D>("Images\\SentientBeings\\human_player"));

            InventoryPanel.DefaultTextures.Add("background", _content.Load<Texture2D>("black_background1"));
            InventoryPanel.DefaultTextures["white_background"] = _content.Load<Texture2D>("Images\\Scenes\\inventory_bg_white");
            InventoryPanel.DefaultTextures.Add("default_hand", _content.Load<Texture2D>("Images\\Scenes\\inventory_hand"));
            InventoryPanel.DefaultTextures.Add("default_head", _content.Load<Texture2D>("Images\\Scenes\\inventory_head"));
            InventoryPanel.DefaultTextures.Add("default_body", _content.Load<Texture2D>("Images\\Scenes\\inventory_body"));
            InventoryPanel.DefaultTextures.Add("default_feet", _content.Load<Texture2D>("Images\\Scenes\\inventory_feet"));
            InventoryPanel.DefaultTextures.Add("default_other", _content.Load<Texture2D>("Images\\Scenes\\inventory_other"));
        }


        public override void Update(GameTime gameTime)
        {
            _dungeon.Update();

            _tileMapPanel.Update(gameTime);
            _statusPanel.Update(gameTime);
            _inventoryPanel.Update(gameTime);

            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            _tileMapPanel.Draw(gameTime, _spriteBatch);
            _statusPanel.Draw(gameTime, _spriteBatch);
            _inventoryPanel.Draw(gameTime, _spriteBatch);
        }

        public override void Show()
        {
            base.Show();
            SubscribeToEvents();
        }

        public override void Hide()
        {
            base.Hide();
            UnsubscribeFromEvents();
        }


        #region Event Handling
        private void SubscribeToEvents()
        {
            _input.LeftKeyPressed += MovePlayerWest;
            _input.RightKeyPressed += MovePlayerEast;
            _input.UpKeyPressed += MovePlayerNorth;
            _input.DownKeyPressed += MovePlayerSouth;
            _inventoryPanel.OnPlayerDroppedItem += PlayerDropItemEventHandler;
            _inventoryPanel.OnPlayerEquippedItem += PlayerEquipItemEventHandler;
            _inventoryPanel.OnPlayerUsedItem += PlayerUseItemEventHandler;
        }

        private void UnsubscribeFromEvents()
        {
            _input.LeftKeyPressed -= MovePlayerWest;
            _input.RightKeyPressed -= MovePlayerEast;
            _input.UpKeyPressed -= MovePlayerNorth;
            _input.DownKeyPressed -= MovePlayerSouth;
            _inventoryPanel.OnPlayerDroppedItem -= PlayerDropItemEventHandler;
            _inventoryPanel.OnPlayerEquippedItem -= PlayerEquipItemEventHandler;
            _inventoryPanel.OnPlayerUsedItem -= PlayerUseItemEventHandler;
        }

        private void MovePlayerWest(object sender, EventArgs e)
        {
            _dungeon.MovePlayer(Compass.West);
        }

        private void MovePlayerEast(object sender, EventArgs e)
        {
            _dungeon.MovePlayer(Compass.East);
        }

        private void MovePlayerNorth(object sender, EventArgs e)
        {
            _dungeon.MovePlayer(Compass.North);
        }

        private void MovePlayerSouth(object sender, EventArgs e)
        {
            _dungeon.MovePlayer(Compass.South);
        }

        private void PlayerDropItemEventHandler(object sender, InventoryEventArgs e)
        {
            _dungeon.PlayerDropItem(e.InventorySlot, e.InventoryItem);
        }

        private void PlayerEquipItemEventHandler(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PlayerUseItemEventHandler(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
