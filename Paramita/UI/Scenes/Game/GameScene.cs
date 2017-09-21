using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.GameLogic;
using Paramita.GameLogic.Items;
using Paramita.UI.Base.Game;
using Paramita.GameLogic.Mechanics;
using System.Collections.Generic;
using Paramita.UI.Input;
using Microsoft.Xna.Framework.Input;

namespace Paramita.UI.Base
{

    public class GameScene : Scene
    {
        private TileMapPanel _tileMapPanel;        
        private static StatusPanel _statusPanel;
        private InventoryPanel _inventoryPanel;


        public GameScene(GameController game) : base(game) { }

        public Dungeon Dungeon { get; set; }

        public override void Initialize()
        {
            base.Initialize(); // This calls LoadContent()

            Dungeon = new Dungeon();

            _tileMapPanel = new TileMapPanel(this, Dungeon.GetCurrentLevelLayers(), 0);
            _statusPanel = new StatusPanel(this, GameController.ArialBold, 10, new Point(0,720), 1);
            _inventoryPanel = new InventoryPanel(this, 1);

            Components = InitializeComponents(_tileMapPanel, _statusPanel, _inventoryPanel);

            UserActions = InitializeUserActions(Components);
        }

        protected override void LoadContent()
        {
            ItemTextures.ItemTextureMap[ItemType.Coins] = _content.Load<Texture2D>("Images\\Items\\coins");
            ItemTextures.ItemTextureMap[ItemType.Meat] = _content.Load<Texture2D>("Images\\Items\\meat");
            ItemTextures.ItemTextureMap[ItemType.Shield] = _content.Load<Texture2D>("Images\\Items\\buckler");
            ItemTextures.ItemTextureMap[ItemType.ShortSword] = _content.Load<Texture2D>("Images\\Items\\short_sword");
            ItemTextures.ItemTextureMap[ItemType.Bite] = _content.Load<Texture2D>("transparent");
            ItemTextures.ItemTextureMap[ItemType.Fist] = _content.Load<Texture2D>("transparent");
            ItemTextures.ItemTextureMap[ItemType.None] = _content.Load<Texture2D>("transparent");

            TileMapPanel.Spritesheets.Add(SpriteType.Tile_Floor, _content.Load<Texture2D>("Images\\Tiles\\floor"));
            TileMapPanel.Spritesheets.Add(SpriteType.Tile_Door, _content.Load<Texture2D>("Images\\Tiles\\door"));
            TileMapPanel.Spritesheets.Add(SpriteType.Tile_Wall, _content.Load<Texture2D>("Images\\Tiles\\wall"));
            TileMapPanel.Spritesheets.Add(SpriteType.Tile_StairsUp, _content.Load<Texture2D>("Images\\Tiles\\stairs_up"));
            TileMapPanel.Spritesheets.Add(SpriteType.Tile_StairsDown, _content.Load<Texture2D>("Images\\Tiles\\stairs_down"));
            TileMapPanel.Spritesheets.Add(SpriteType.Actor_GiantRat, _content.Load<Texture2D>("Images\\SentientBeings\\giant_rat"));
            TileMapPanel.Spritesheets.Add(SpriteType.Actor_Player, _content.Load<Texture2D>("Images\\SentientBeings\\human_player"));

            InventoryPanel.DefaultTextures["background"] = _content.Load<Texture2D>("black_background1");
            InventoryPanel.DefaultTextures["minimize_icon"] = _content.Load<Texture2D>("Images\\Scenes\\minimize_icon");
            InventoryPanel.DefaultTextures["white_background"] = _content.Load<Texture2D>("Images\\Scenes\\inventory_bg_white");
            InventoryPanel.DefaultTextures["left_hand"] = _content.Load<Texture2D>("Images\\Scenes\\inventory_hand");
            InventoryPanel.DefaultTextures["right_hand"] = _content.Load<Texture2D>("Images\\Scenes\\inventory_hand");
            InventoryPanel.DefaultTextures["head"] = _content.Load<Texture2D>("Images\\Scenes\\inventory_head");
            InventoryPanel.DefaultTextures["body"] = _content.Load<Texture2D>("Images\\Scenes\\inventory_body");
            InventoryPanel.DefaultTextures["feet"] = _content.Load<Texture2D>("Images\\Scenes\\inventory_feet");
            InventoryPanel.DefaultTextures["other1"] = _content.Load<Texture2D>("Images\\Scenes\\inventory_other");
            InventoryPanel.DefaultTextures["other2"] = _content.Load<Texture2D>("Images\\Scenes\\inventory_other");
            InventoryPanel.DefaultTextures["other3"] = _content.Load<Texture2D>("Images\\Scenes\\inventory_other");
            InventoryPanel.DefaultTextures["other4"] = _content.Load<Texture2D>("Images\\Scenes\\inventory_other");
            InventoryPanel.DefaultTextures["other5"] = _content.Load<Texture2D>("Images\\Scenes\\inventory_other");
        }

        #region User Actions
        protected override List<UserAction> InitializeUserActions(List<Component> components)
        {
            var inventoryPanel = (InventoryPanel)components.Find(c => c is InventoryPanel);
            var actionsList = new List<UserAction>
            {
                new UserAction(this, ToggleInventoryPanel, CanToggleInventoryPanel)
            };
            return actionsList;
        }

        private bool CanSelectInventoryItem(Tuple<Scene, UserInputEventArgs> context)
        {
            return true;
        }

        private void SelectInventoryItem(Scene obj)
        {
            throw new NotImplementedException();
        }

        private bool CanToggleInventoryPanel(Tuple<Scene, UserInputEventArgs> context)
        {
            var scene = context.Item1;
            var eventArgs = context.Item2;

            var inventory = (InventoryPanel)scene.Components.Find(c => c is InventoryPanel);
            var inputSources = new InputSource(new List<Element>()
            {
                inventory.Elements["minimize_icon"],
                inventory.Elements["background_closed"],
                inventory.Elements["heading"]
            }, Keys.I);


            if (!inputSources.Contains(eventArgs.EventSource))
                return false;

            if (eventArgs.EventType != EventType.LeftClick && 
                eventArgs.EventType != EventType.Keyboard)
                return false;

            if (eventArgs.EventSource == inventory.Elements["heading"] && inventory.IsOpen)
                return false;

            return true;
        }

        private void ToggleInventoryPanel(Scene parent)
        {
            var panel = (InventoryPanel)parent.Components.Find(c => c is InventoryPanel);
            panel.TogglePanelState();
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            Dungeon.Update();

            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        #region Event Handling
        private void OnIKeyPressed(object sender, EventArgs e)
        {
            InvokeUserInputEvent(new UserInputEventArgs(EventType.Keyboard, null, Keys.I));
        }

        private void MovePlayerWest(object sender, EventArgs e)
        {
            Dungeon.MovePlayer(Compass.West);
        }

        private void MovePlayerEast(object sender, EventArgs e)
        {
            Dungeon.MovePlayer(Compass.East);
        }

        private void MovePlayerNorth(object sender, EventArgs e)
        {
            Dungeon.MovePlayer(Compass.North);
        }

        private void MovePlayerSouth(object sender, EventArgs e)
        {
            Dungeon.MovePlayer(Compass.South);
        }

        private void PlayerDropItemEventHandler(object sender, InventoryEventArgs e)
        {
            Dungeon.PlayerDropItem(e.InventorySlot, e.InventoryItem);
        }

        private void PlayerEquipItemEventHandler(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PlayerUseItemEventHandler(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void SubscribeToKeyboardEvents()
        {
            Input.IKeyPressed += OnIKeyPressed;
            Input.LeftKeyPressed += MovePlayerWest;
            Input.RightKeyPressed += MovePlayerEast;
            Input.UpKeyPressed += MovePlayerNorth;
            Input.DownKeyPressed += MovePlayerSouth;
            _inventoryPanel.OnPlayerDroppedItem += PlayerDropItemEventHandler;
            _inventoryPanel.OnPlayerEquippedItem += PlayerEquipItemEventHandler;
            _inventoryPanel.OnPlayerUsedItem += PlayerUseItemEventHandler;
        }

        protected override void UnsubscribeFromKeyboardEvents()
        {
            Input.IKeyPressed -= OnIKeyPressed;
            Input.LeftKeyPressed -= MovePlayerWest;
            Input.RightKeyPressed -= MovePlayerEast;
            Input.UpKeyPressed -= MovePlayerNorth;
            Input.DownKeyPressed -= MovePlayerSouth;
            _inventoryPanel.OnPlayerDroppedItem -= PlayerDropItemEventHandler;
            _inventoryPanel.OnPlayerEquippedItem -= PlayerEquipItemEventHandler;
            _inventoryPanel.OnPlayerUsedItem -= PlayerUseItemEventHandler;
        }
        #endregion
    }
}
