using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.GameLogic;
using Paramita.GameLogic.Items;
using Paramita.UI.Input;
using System;
using System.Collections.Generic;

namespace Paramita.UI.Scenes.Game
{
    public enum InventoryActions
    {
        None,
        Select1,
        Select2,
        Select3,
        Select4,
        Select5,
        Select6,
        Select7,
        Select8,
        Select9,
        Select0,
        Drop,
        Use,
        Equip,
        Cancel,
        TogglePanel
    }


    public class InventoryEventArgs : EventArgs
    {
        public string InventorySlot { get; private set; }
        public ItemType InventoryItem { get; private set; }

        public InventoryEventArgs(string inventorySlot, ItemType inventoryItem)
        {
            InventorySlot = inventorySlot;
            InventoryItem = inventoryItem;
        }
    }

    /*
     * This class:
     *     Displays the player's inventory and other stats
     *     Provides the UI for equiping, using, and dropping items
     *     Responds to player inventory input events
     *     Raises player inventory change events 
     */
    public class InventoryPanel
    {
        private List<string> _inventorySlots = new List<string>()
            { "none","left_hand", "right_hand", "head", "body", "feet",
                "other1", "other2", "other3", "other4", "other5"};

        private static Dictionary<string, Texture2D> _defaultSlotTextures 
            = new Dictionary<string, Texture2D>();

        private Dictionary<string, ItemType> _inventory 
            = new Dictionary<string, ItemType>();
        private int _gold = 0;

        private Dictionary<string, Sprite> _inventorySprites
            = new Dictionary<string, Sprite>();

        private Point _panelOrigin;
        private Rectangle panelRectangle;
        private const int PANEL_WIDTH = 250;
        private const int PANEL_HEIGHT_OPEN = 330;
        private const int PANEL_HEIGHT_CLOSED = 30;

        private Vector2 _headingPosition;
        private Vector2 _togglePosition;
        private Vector2 _hintPosition;
        private Vector2 _spritePosition;

        private SpriteFont fontHeader = GameController.ArialBold;
        private SpriteFont fontText = GameController.NotoSans;

        private const string HEADING = "Inventory";
        private const string TOGGLE_OPEN = "[+]";
        private const string TOGGLE_CLOSED = "[-]";
        private string selectHint = "Press (0-9) to Select Item";
        private string dropHint = "Press (d) to Drop Item";
        private string useHint = "Press (u) to Use Item";
        private string equipHint = "Press (e) to Equip Item";
        private string unequipHint = "Press (e) to Unequip Item";
        private string cancelHint = "Press (c) to Cancel Selection";

        private int _itemSelected;
        private bool _isOpen = false;

        public static event EventHandler<InventoryEventArgs> OnPlayerDroppedItem;
        public static event EventHandler<InventoryEventArgs> OnPlayerEquippedItem;
        public static event EventHandler<InventoryEventArgs> OnPlayerUsedItem;

        public InventoryPanel()
        {
            SubscribeToInputEvents();

            GetPlayerData();

            InitializePanel();
        }

        #region Properties
        public static Dictionary<string, Texture2D> DefaultTextures
        {
            get { return _defaultSlotTextures; }
            set { _defaultSlotTextures = value; }
        }

        public Dictionary<string, ItemType> Inventory
        {
            set
            {
                _inventory = value;
                CreateInventorySprites(_inventory);
            }
        }

        #region Inventory Property helpers

        private void CreateInventorySprites(Dictionary<string, ItemType> _inventory)
        {
            Rectangle frame = new Rectangle(0, 0, 32, 32);
            foreach (string type in _inventory.Keys)
            {
                var spriteType = Sprite.GetSpriteType(_inventory[type]);
                if (spriteType == SpriteType.None)
                {
                    _inventorySprites[type] =
                        new Sprite(_defaultSlotTextures[GetDefaultTextureKey(type)], frame);
                }
                else
                {
                    _inventorySprites[type] =
                        new Sprite(ItemTextures.ItemTextureMap[spriteType], frame);
                }
            }
        }

        private string GetDefaultTextureKey(string str)
        {
            switch (str)
            {
                case "right_hand":
                case "left_hand":
                    return "default_hand";
                case "head":
                    return "default_head";
                case "body":
                    return "default_body";
                case "feet":
                    return "default_feet";
                case "other1":
                case "other2":
                case "other3":
                case "other4":
                case "other5":
                    return "default_other";
                default:
                    throw new NotImplementedException("InventoryPanel.GetDefaultTextureType():"
                        + " Unknown type from Dungeon.GetPlayerInventory()");
            }
        }
        #endregion

        #endregion

        #region Constructor Helpers

        private void SubscribeToInputEvents()
        {
            InputListener.OnD0KeyWasPressed += HandleSelect0Input;
            InputListener.OnD1KeyWasPressed += HandleSelect1Input;
            InputListener.OnD2KeyWasPressed += HandleSelect2Input;
            InputListener.OnD3KeyWasPressed += HandleSelect3Input;
            InputListener.OnD4KeyWasPressed += HandleSelect4Input;
            InputListener.OnD5KeyWasPressed += HandleSelect5Input;
            InputListener.OnD6KeyWasPressed += HandleSelect6Input;
            InputListener.OnD7KeyWasPressed += HandleSelect7Input;
            InputListener.OnD8KeyWasPressed += HandleSelect8Input;
            InputListener.OnD9KeyWasPressed += HandleSelect9Input;
            InputListener.OnDKeyWasPressed += HandleDropInput;
            InputListener.OnEKeyWasPressed += HandleEquipInput;
            InputListener.OnUKeyWasPressed += HandleUseInput;
            InputListener.OnCKeyWasPressed += HandleCancelInput;
            InputListener.OnIKeyWasPressed += HandleToggleInput;
        }


        private void GetPlayerData()
        {
            var playerData = Dungeon.GetPlayerInventory();
            Inventory = playerData.Item1;
            _gold = playerData.Item2;
        }


        private void InitializePanel()
        {
            Rectangle parentScreen = GameController.ScreenRectangle;
            _panelOrigin = new Point(parentScreen.Width - (PANEL_WIDTH), 0);
            panelRectangle = new Rectangle(_panelOrigin.X,
                _panelOrigin.Y, PANEL_WIDTH, PANEL_HEIGHT_CLOSED);

            Vector2 headingSize = fontHeader.MeasureString(HEADING);
            _headingPosition = new Vector2(
                panelRectangle.Left + ((PANEL_WIDTH / 2) - (headingSize.X / 2)),
                (panelRectangle.Top + 5));

            _togglePosition = new Vector2(
                panelRectangle.Right - 30, (panelRectangle.Top + 5));

            _spritePosition = new Vector2( // (5 sprites * 37 pixels - 5 / 2) = 90 pixels
                panelRectangle.Left + ((PANEL_WIDTH / 2) - 90),
                _headingPosition.Y + 30);

            _hintPosition = new Vector2(
                panelRectangle.Left + 10, PANEL_HEIGHT_OPEN - 60);
            _itemSelected = 0;
        }
        #endregion

        #region Input Handlers
        private void HandleInput(InventoryActions action)
        {
            if (action == InventoryActions.TogglePanel)
                _isOpen = !_isOpen;

            int selectionInput = 0;
            if((int)action > 0 && (int)action < 11)
                selectionInput = (int)action;

            if(selectionInput > 0)
                _itemSelected = selectionInput;

            if(_itemSelected > 0 && _itemSelected <= _inventory.Count)
            {
                if(action == InventoryActions.Drop)
                {
                    string slot = _inventorySlots[_itemSelected];
                    OnPlayerDroppedItem?.Invoke(null,
                        new InventoryEventArgs(slot, _inventory[slot]) );
                    _itemSelected = 0;
                }
                else if(action == InventoryActions.Cancel)
                {
                    _itemSelected = 0;
                }
            }
        }

        private void HandleSelect0Input(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Select0);
        }

        private void HandleSelect1Input(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Select1);
        }

        private void HandleSelect2Input(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Select2);
        }

        private void HandleSelect3Input(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Select3);
        }

        private void HandleSelect4Input(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Select4);
        }

        private void HandleSelect5Input(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Select5);
        }

        private void HandleSelect6Input(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Select6);
        }

        private void HandleSelect7Input(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Select7);
        }

        private void HandleSelect8Input(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Select8);
        }

        private void HandleSelect9Input(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Select9);
        }

        private void HandleDropInput(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Drop);
        }

        private void HandleEquipInput(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Equip);
        }

        private void HandleUseInput(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Use);
        }

        private void HandleCancelInput(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Cancel);
        }

        private void HandleToggleInput(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.TogglePanel);
        }
        #endregion

        

        // Called by GameScene.Update() to check for changes or input to handle
        public void Update(GameTime gameTime)
        {
            GetPlayerData();
        }


        // Called by GameScene.Draw()
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            if (_isOpen)
                DrawOpenPanel(spriteBatch);
            else
                DrawClosedPanel(spriteBatch);

            spriteBatch.End();
        }

        #region Draw Method Helpers
        private void DrawOpenPanel(SpriteBatch spriteBatch)
        {
            // draw background
            panelRectangle.Height = PANEL_HEIGHT_OPEN;
            spriteBatch.Draw(_defaultSlotTextures["background"], panelRectangle, Color.White);

            // draw header and toggle symbol
            spriteBatch.DrawString(fontHeader, HEADING, _headingPosition, Color.White);
            spriteBatch.DrawString(fontText, TOGGLE_CLOSED, _togglePosition, Color.White);

            // draw the list of items in player's inventory
            string slot = "";
            Sprite sprite;
            Vector2 position = _spritePosition;
            Color color = Color.White;
            Color normalColor = Color.White;
            Color selectedColor = Color.Red;
            for(int i = 1; i < 6; i++)
            {
                if (i == _itemSelected)
                    color = selectedColor;
                else
                    color = normalColor;

                slot = _inventorySlots[i];
                sprite = _inventorySprites[slot];
                sprite.Position = position;
                spriteBatch.Draw(
                    sprite.Texture,
                    sprite.Position,
                    sprite.Frame,
                    color
                );
                position.X += 37;
            }

            position = _spritePosition;
            position.Y += 37;
            for(int j = 6; j < 11; j++)
            {
                if (j == _itemSelected)
                    color = selectedColor;
                else
                    color = normalColor;

                slot = _inventorySlots[j];
                sprite = _inventorySprites[slot];
                sprite.Position = position;
                spriteBatch.Draw(
                    sprite.Texture,
                    sprite.Position,
                    sprite.Frame,
                    color
                );
                position.X += 37;
            }

            // Add other information items like amount of gold, weapon & armor stats, etc
        }


        private void DrawClosedPanel(SpriteBatch spriteBatch)
        {
            // draw panel background
            panelRectangle.Height = PANEL_HEIGHT_CLOSED;
            spriteBatch.Draw(_defaultSlotTextures["background"], panelRectangle, Color.White);

            // draw the header and toggle symbol
            spriteBatch.DrawString(fontHeader, HEADING, _headingPosition, Color.White);
            spriteBatch.DrawString(fontText, TOGGLE_OPEN, _togglePosition, Color.White);
        }
        #endregion
    }
}
