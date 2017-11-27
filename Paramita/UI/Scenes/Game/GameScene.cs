using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Paramita.GameLogic;
using Paramita.GameLogic.Actors;
using Paramita.GameLogic.Items;
using Paramita.GameLogic.Mechanics;
using Paramita.GameLogic.Utility;
using Paramita.GameLogic.Levels;
using System.Collections.Generic;
using MonoGameUI;
using MonoGameUI.Input;
using MonoGameUI.Elements;
using MonoGameUI.Base;
using MonoGameUI.Events;

namespace Paramita.UI.Scenes
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

            _tileMapPanel = new TileMapPanel(this, 0);
            _statusPanel = new StatusPanel(this, 1);
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

            TileMapPanel.Spritesheets.Add("Floor", _content.Load<Texture2D>("Images\\Tiles\\floor"));
            TileMapPanel.Spritesheets.Add("Door", _content.Load<Texture2D>("Images\\Tiles\\door"));
            TileMapPanel.Spritesheets.Add("Wall", _content.Load<Texture2D>("Images\\Tiles\\wall"));
            TileMapPanel.Spritesheets.Add("StairsUp", _content.Load<Texture2D>("Images\\Tiles\\stairs_up"));
            TileMapPanel.Spritesheets.Add("StairsDown", _content.Load<Texture2D>("Images\\Tiles\\stairs_down"));
            TileMapPanel.Spritesheets.Add("GiantRat", _content.Load<Texture2D>("Images\\SentientBeings\\giant_rat"));
            TileMapPanel.Spritesheets.Add("HumanPlayer", _content.Load<Texture2D>("Images\\SentientBeings\\human_player"));

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
            return new List<UserAction>()
            {
                new UserAction(this, ToggleInventoryPanel, CanToggleInventoryPanel),
                new UserAction(this, SelectInventoryItem, CanSelectInventoryItem),
                new UserAction(this, CancelInventorySelection, CanCancelInventorySelection),
                new UserAction(this, FocusOnElement, CanFocusOnElement),
                new UserAction(this, StopFocusOnElement, CanStopFocusOnElement),
                new UserAction(this, DropSelectedItem, CanDropSelectedItem),
                new UserAction(this, MovePlayer, CanMovePlayer)
            };
        }

        private bool CanMovePlayer(Tuple<Scene, UserInputEventArgs> context)
        {
            var sources = new InputSource(Keys.Up, Keys.Right, Keys.Left, Keys.Down);

            if(sources.Contains(context.Item2.EventSource))
                return true;
            return false;
        }

        private void MovePlayer(Scene parent, UserInputEventArgs eventArgs)
        {
            var source = (Keys)eventArgs.EventSource;

            if (source == Keys.Up)
                Dungeon.MovePlayer(Compass.North);
            else if(source == Keys.Down)
                Dungeon.MovePlayer(Compass.South);
            else if(source == Keys.Right)
                Dungeon.MovePlayer(Compass.East);
            else if(source == Keys.Left)
                Dungeon.MovePlayer(Compass.West);
        }

        private bool CanCancelInventorySelection(Tuple<Scene, UserInputEventArgs> context)
        {
            var source = new InputSource(Keys.C);

            if (source.Contains(context.Item2.EventSource) && _inventoryPanel.ItemSelected != 0)    
                return true;
            return false;
        }

        private void CancelInventorySelection(Scene parent, UserInputEventArgs eventArgs)
        {
            _inventoryPanel.ItemSelected = 0;
        }

        private bool CanDropSelectedItem(Tuple<Scene, UserInputEventArgs> context)
        {
            var scene = context.Item1;
            var eventArgs = context.Item2;
            InventoryPanel panel = _inventoryPanel;
            var inputSources = new InputSource(Keys.D);

            if (!inputSources.Contains(eventArgs.EventSource))
                return false;

            if (eventArgs.EventType != EventType.Keyboard)
                return false;

            if (panel.ItemSelected == 0)
                return false;

            return true;
        }

        private void DropSelectedItem(Scene parent, UserInputEventArgs eventArgs)
        {
            string slot = _inventoryPanel._inventorySlots[_inventoryPanel.ItemSelected - 1];
            ItemType itemType = _inventoryPanel._inventory[slot];

            Dungeon.PlayerDropItem(slot, itemType);
            _inventoryPanel.ItemSelected = 0;
        }

        private bool CanFocusOnElement(Tuple<Scene, UserInputEventArgs> context)
        {
            var eventArgs = context.Item2;            

            if (!(eventArgs.EventSource is Image))
                return false;

            if (eventArgs.EventType != EventType.MouseOver)
                return false;

            return true;
        }

        private void FocusOnElement(Scene parent, UserInputEventArgs eventArgs)
        {
            var image = (Image)eventArgs.EventSource;
            if(_inventoryPanel.Elements.ContainsKey(image.Id))
                _inventoryPanel.Elements[image.Id].Highlight();
        }

        private bool CanStopFocusOnElement(Tuple<Scene, UserInputEventArgs> context)
        {
            var eventArgs = context.Item2;

            if (!(eventArgs.EventSource is Image))
                return false;

            if (eventArgs.EventType != EventType.MouseGone)
                return false;

            return true;
        }

        private void StopFocusOnElement(Scene parent, UserInputEventArgs eventArgs)
        {
            var image = (Image)eventArgs.EventSource;
            if (_inventoryPanel.Elements.ContainsKey(image.Id))
                _inventoryPanel.Elements[image.Id].Unhighlight();
        }

        private bool CanSelectInventoryItem(Tuple<Scene, UserInputEventArgs> context)
        {
            var scene = context.Item1;
            var eventArgs = context.Item2;

            var inventory = (InventoryPanel)scene.Components.Find(c => c is InventoryPanel);
            var inputSources = new InputSource(new List<Element>()
            {
                inventory.Elements["left_hand_item"],
                inventory.Elements["right_hand_item"],
                inventory.Elements["head_item"],
                inventory.Elements["body_item"],
                inventory.Elements["feet_item"],
                inventory.Elements["other1_item"],
                inventory.Elements["other2_item"],
                inventory.Elements["other3_item"],
                inventory.Elements["other4_item"],
                inventory.Elements["other5_item"]
            }, 
                Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, 
                Keys.D0);


            if (!inputSources.Contains(eventArgs.EventSource))
                return false;

            if (eventArgs.EventType != EventType.LeftClick &&
                eventArgs.EventType != EventType.Keyboard)
                return false;

            // don't select an item with the keyboard if no item is visible on that slot
            if(eventArgs.EventSource is Keys key)
            {
                if (key == Keys.D1 && !inventory.Elements["left_hand_item"].Visible)    return false;
                if (key == Keys.D2 && !inventory.Elements["right_hand_item"].Visible)   return false;
                if (key == Keys.D3 && !inventory.Elements["head_item"].Visible)         return false;
                if (key == Keys.D4 && !inventory.Elements["body_item"].Visible)         return false;
                if (key == Keys.D5 && !inventory.Elements["feet_item"].Visible)         return false;
                if (key == Keys.D6 && !inventory.Elements["other1_item"].Visible)       return false;
                if (key == Keys.D7 && !inventory.Elements["other2_item"].Visible)       return false;
                if (key == Keys.D8 && !inventory.Elements["other3_item"].Visible)       return false;
                if (key == Keys.D9 && !inventory.Elements["other4_item"].Visible)       return false;
                if (key == Keys.D0 && !inventory.Elements["other5_item"].Visible)       return false;
            }

            return true;
        }

        private void SelectInventoryItem(Scene parent, UserInputEventArgs eventArgs)
        {
            GameScene scene = (GameScene)parent;
            if (eventArgs.EventSource is Image image)
            {
                int index = scene._inventoryPanel._inventoryItems.FindIndex(id => id.Equals(image.Id));
                
                scene._inventoryPanel.ItemSelected = index + 1;
            }
            else
            {
                if(eventArgs.EventSource is Keys key)
                {
                    if (key is Keys.D0)
                        scene._inventoryPanel.ItemSelected = (int)key - 38;
                    else
                        scene._inventoryPanel.ItemSelected = (int)key - 48;
                }
            }
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

        private void ToggleInventoryPanel(Scene parent, UserInputEventArgs eventArgs)
        {
            var panel = (InventoryPanel)parent.Components.Find(c => c is InventoryPanel);
            panel.TogglePanelState();
        }
        #endregion

        #region Event Handling
        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            Dungeon.OnInventoryChangeUINotification += HandleInventoryChange;
            Dungeon.OnActorMoveUINotification += HandleOnActorWasMoved;
            Dungeon.OnItemDroppedUINotification += HandleItemAddedToMap;
            Dungeon.OnItemPickedUpUINotification += HandleItemRemovedFromMap;
            Dungeon.OnLevelChangeUINotification += HandleLevelChange;
            Dungeon.OnActorRemovedUINotification += HandleActorWasRemoved;
            Dungeon.OnStatusMsgUINotification += HandleNewStatusMessage;
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            Dungeon.OnInventoryChangeUINotification -= HandleInventoryChange;
            Dungeon.OnActorMoveUINotification -= HandleOnActorWasMoved;
            Dungeon.OnItemDroppedUINotification -= HandleItemAddedToMap;
            Dungeon.OnItemPickedUpUINotification -= HandleItemRemovedFromMap;
            Dungeon.OnLevelChangeUINotification -= HandleLevelChange;
            Dungeon.OnActorRemovedUINotification -= HandleActorWasRemoved;
            Dungeon.OnStatusMsgUINotification -= HandleNewStatusMessage;
        }

        private void HandleInventoryChange(object sender, InventoryChangeEventArgs e)
        {
            _inventoryPanel.Inventory = e.Inventory.Item1;
            _inventoryPanel.Gold = e.Inventory.Item2;
        }

        private void HandleOnActorWasMoved(object sender, MoveEventArgs eventArgs)
        {
            var origin = eventArgs.Origin;
            var destination = eventArgs.Destination;
            _tileMapPanel.MoveSprite(origin, destination);
        }

        private void HandleItemAddedToMap(object sender, ItemEventArgs e)
        {
            int x = e.Location.X; int y = e.Location.Y;
            var itemType = e.ItemType;
            _tileMapPanel.AddImage(x, y, itemType);
        }

        private void HandleItemRemovedFromMap(object sender, ItemEventArgs e)
        {
            _tileMapPanel.RemoveImage(e.Location.X, e.Location.Y);
        }

        private void HandleLevelChange(object sender, NewLevelEventArgs e)
        {
            _tileMapPanel.ChangeLevel(e.Layers);
        }

        private void HandleActorWasRemoved(object sender, MoveEventArgs eventArgs)
        {
            var origin = eventArgs.Origin;
            _tileMapPanel.RemoveSprite(origin.X, origin.Y);
        }

        private void HandleNewStatusMessage(object sender, StatusMessageEventArgs e)
        {
            if (e.Message.Count == 0)
                return;

            if (e.Message.Count == 1)
            {
                _statusPanel.AddMessage(e.Message[0]);
            }
            else
            {
                foreach (var msg in e.Message)
                {
                    _statusPanel.AddMessage(msg);
                }
            }
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
    }
}
