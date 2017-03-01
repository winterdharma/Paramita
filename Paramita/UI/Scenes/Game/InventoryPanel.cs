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

        private List<SpriteElement> _spriteElements;
        private List<TextElement> _textElements;

        private Rectangle _panelRectangle;
        private const int PANEL_WIDTH_OPEN = 250;
        private const int PANEL_WIDTH_CLOSED = 150;
        private const int PANEL_HEIGHT_OPEN = 330;
        private const int PANEL_HEIGHT_CLOSED = 30;

        private Vector2 _itemInfoPosition;

        private TextElement _heading;
        private TextElement _toggleText;
        private TextElement _unequipText;
        private TextElement _equipText;
        private TextElement _dropText;
        private TextElement _cancelText;

        private const string HEADING = "(I)nventory";
        private const string TOGGLE_CLOSED = "[X]";
        private const string SELECT_HINT = "Press (0-9) to Select Item ";
        private const string DROP_HINT = "(D)rop Item";
        private const string USE_HINT = "(U)se Item";
        private const string EQUIP_HINT = "(E)quip Item";
        private const string UNEQUIP_HINT = "Un(e)quip Item";
        private const string CANCEL_HINT = "(C)ancel Selection";

        private int _itemSelected;
        private bool _isOpen = false;

        public static event EventHandler<InventoryEventArgs> OnPlayerDroppedItem;
        public static event EventHandler<InventoryEventArgs> OnPlayerEquippedItem;
        public static event EventHandler<InventoryEventArgs> OnPlayerUsedItem;

        public InventoryPanel()
        {
            InitializePanel();
            SubscribeToInputEvents();
            GetPlayerData();
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
                CreateSpriteElements();
            }
        }

        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                _isOpen = value;
                UpdatePanelRectangle();
                UpdateHeadingPosition();
            }
        }

        #region Inventory Property helpers

        

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

        private Point GetPanelOrigin(bool isOpen, int offsetFromTop = 0, int offsetFromRight = 0)
        {
            Rectangle parentScreen = GameController.ScreenRectangle;
            if (isOpen)
            {
                return new Point(
                parentScreen.Width - PANEL_WIDTH_OPEN - offsetFromRight,
                offsetFromTop);
            }
            else
            {
                return new Point(
                parentScreen.Width - PANEL_WIDTH_CLOSED - offsetFromRight,
                offsetFromTop);
            }

        }

        private Point GetPanelSize(bool isOpen)
        {
            if (isOpen)
            {
                return new Point(PANEL_WIDTH_OPEN, PANEL_HEIGHT_OPEN);
            }
            else
            {
                return new Point(PANEL_WIDTH_CLOSED, PANEL_HEIGHT_CLOSED);
            }
        }

        #endregion

        #endregion

        #region Constructor Helpers

        private void InitializePanel()
        {
            _isOpen = false;
            _itemSelected = 0;
            UpdatePanelRectangle();
            InitializeTextElements();
            
        }

        private void UpdatePanelRectangle()
        {
            _panelRectangle.Location = GetPanelOrigin(_isOpen);
            _panelRectangle.Size = GetPanelSize(_isOpen);
        }

        private void UpdateHeadingPosition()
        {
            _heading.Position = GetHeadingPosition(_heading.Text, _panelRectangle, _heading.Font);
        }

        private void SubscribeToInputEvents()
        {
            InputListener.OnLeftMouseButtonClicked += HandleLeftClick;
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


        private void CreateSpriteElements()
        {
            _spriteElements = new List<SpriteElement>();
            string slot;
            for(int i = 1; i < _inventorySlots.Count; i++)
            {
                slot = _inventorySlots[i];
                var spriteElement = new SpriteElement();
                spriteElement.Label = slot;
                spriteElement.Texture = GetSpriteElementTexture(slot, _inventory[slot]);
                spriteElement.Position = GetSpriteElementPosition(i-1);
                spriteElement.Color = GetSpriteElementColor(i);
                _spriteElements.Add(spriteElement);
            }
        }

        private Texture2D GetSpriteElementTexture(string slot, ItemType itemType)
        {
            Texture2D texture;
            if(itemType == ItemType.None || itemType == ItemType.Fist || itemType == ItemType.Bite)
                texture = _defaultSlotTextures[GetDefaultTextureKey(slot)];
            else
                texture = ItemTextures.ItemTextureMap[itemType];

            return texture;
        }

        private Vector2 GetSpriteElementPosition(int position)
        {
            int spritesPerRow = 5;

            var spritePosition = new Vector2();
            spritePosition.X = _panelRectangle.Right - ((PANEL_WIDTH_OPEN / 2) + 90);
            spritePosition.Y = _panelRectangle.Top + 60;

            spritePosition.X += (position % spritesPerRow) * 37;
            spritePosition.Y += (position / spritesPerRow) * 37;
            return spritePosition;
            
        }

        private Color GetSpriteElementColor(int index)
        {
            if (_itemSelected == index)
                return Color.Red;
            else
                return Color.White;

        }

        private void CreateTextElements()
        {
            _textElements = new List<TextElement>();

            _textElements.Add(_heading);
            _textElements.Add(_toggleText);
            _textElements.AddRange(GetHintTextElements());
        }

        private void InitializeTextElements()
        {
            ConstructHeadingElement();
            ConstructToggleElement();
            ConstructUnequipHintElement();
            ConstructEquipHintElement();
            ConstructDropHintElement();
            ConstructCancelHintElement();           

            _itemInfoPosition = new Vector2(
                _panelRectangle.Right - (PANEL_WIDTH_OPEN - 10),
                _panelRectangle.Top + 140); 
        }

        private void ConstructHeadingElement()
        {
            SpriteFont font = GameController.ArialBold;
            _heading = new TextElement(
                "heading",
                HEADING,
                font,
                GetHeadingPosition(HEADING, _panelRectangle, font),
                Color.White);
        }

        private void ConstructToggleElement()
        {
            var font = GameController.NotoSans;
            var toggleSize = font.MeasureString(TOGGLE_CLOSED);
            var position = new Vector2(
                _panelRectangle.Right - toggleSize.X, (_panelRectangle.Top + 5));

            _toggleText = new TextElement(
                "toggle_text",
                TOGGLE_CLOSED,
                font,
                position,
                Color.White);
        }

        private void ConstructUnequipHintElement()
        {
            var position = new Vector2(
               _panelRectangle.Right - (PANEL_WIDTH_OPEN - 10),
               150);

            _unequipText = new TextElement(
                "unequip_text",
                UNEQUIP_HINT,
                GameController.NotoSans,
                position,
                Color.White);
        }

        private void ConstructEquipHintElement()
        {
            _equipText = new TextElement(
                "equip_hint",
                EQUIP_HINT,
                GameController.NotoSans,
                _unequipText.Position,
                Color.White);
        }

        private void ConstructDropHintElement()
        {
            var font = GameController.NotoSans;
            var position = _unequipText.Position;
            position.Y += font.MeasureString(EQUIP_HINT).Y + 5;

            _dropText = new TextElement(
                "drop_hint",
                DROP_HINT,
                font,
                position,
                Color.White);
        }

        private void ConstructCancelHintElement()
        {
            var font = GameController.NotoSans;
            var position = _dropText.Position;
            position.Y += font.MeasureString(CANCEL_HINT).Y + 5;

            _cancelText = new TextElement(
                "cancel_hint",
                CANCEL_HINT,
                font,
                position,
                Color.White);
        }

        private Vector2 GetHeadingPosition(string heading, Rectangle panelRect, SpriteFont font, int offsetTop = 5)
        {
            var headingSize = font.MeasureString(heading);
            return new Vector2(
                panelRect.Left + ((panelRect.Width / 2) - (headingSize.X / 2)),
                (panelRect.Top + offsetTop));
        }


        private List<TextElement> GetHintTextElements()
        {
            var hints = new List<TextElement>();

            if (_itemSelected > 0 && _itemSelected < 6)
            {
                hints.Add(_unequipText);
            }
            else if (_itemSelected > 5)
            {
                hints.Add(_equipText);
            }

            if (_itemSelected > 0)
            {
                hints.Add(_dropText);
                hints.Add(_cancelText);
            }

            return hints;
        }

        #endregion

        #region Input Handlers
        private void HandleInput(InventoryActions action)
        {
            if (action == InventoryActions.TogglePanel)
                IsOpen = !IsOpen;

            int selectionInput = 0;
            if((int)action > 0 && (int)action < 11)
                selectionInput = (int)action;

            if(selectionInput > 0)
            {
                _itemSelected = selectionInput;
            }

            if (_itemSelected > 0 && _itemSelected <= _inventory.Count)
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

        private void HandleLeftClick(object sender, MouseEventArgs e)
        {
            if(!_isOpen && e.Position.X > _panelRectangle.X && e.Position.Y < _panelRectangle.Bottom)
            {
                IsOpen = true;
            }
            else
            {
                var toggleSize = _toggleText.Font.MeasureString(_toggleText.Text);
                if(e.Position.X > _toggleText.Position.X && e.Position.Y < _toggleText.Position.Y + toggleSize.Y)
                {
                    IsOpen = false;
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
            CreateTextElements();
        }


        // Called by GameScene.Draw()
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            if (IsOpen)
                DrawOpenPanel(spriteBatch);
            else
                DrawClosedPanel(spriteBatch);

            spriteBatch.End();
        }

        #region Draw Method Helpers
        private void DrawOpenPanel(SpriteBatch spriteBatch)
        {
            // draw background
            _panelRectangle.Height = PANEL_HEIGHT_OPEN;
            spriteBatch.Draw(_defaultSlotTextures["background"], _panelRectangle, Color.White);

            // draw sprite elements
            for(int i = 0; i < _spriteElements.Count; i++)
            {
                var sprite = _spriteElements[i];
                
                spriteBatch.Draw(
                    sprite.Texture,
                    sprite.Position,
                    sprite.Color
                );
            }

            // Draw text elements
            foreach(TextElement textElement in _textElements)
            {
                spriteBatch.DrawString(textElement.Font, 
                    textElement.Text, 
                    textElement.Position, 
                    textElement.Color);
            }
        }


        private void DrawClosedPanel(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_defaultSlotTextures["white_background"], _panelRectangle, Color.DarkBlue);
            
            spriteBatch.DrawString(_heading.Font, _heading.Text, _heading.Position, _heading.Color);
        }
        #endregion
    }
}
