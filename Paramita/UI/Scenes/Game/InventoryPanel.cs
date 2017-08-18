using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;
using Paramita.GameLogic;
using Paramita.GameLogic.Actors;
using Paramita.GameLogic.Items;
using Paramita.GameLogic.Utility;
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
    public class InventoryPanel : Component
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
        private Rectangle _parentScreen;
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
        private Point _mousePosition = new Point(0, 0);

        public event EventHandler<InventoryEventArgs> OnPlayerDroppedItem;
        public event EventHandler<InventoryEventArgs> OnPlayerEquippedItem;
        public event EventHandler<InventoryEventArgs> OnPlayerUsedItem;



        public InventoryPanel(InputResponder input, Rectangle screen)
        {
            _parentScreen = screen;
            InitializePanel();
            SubscribeToInputEvents(input);
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

                if (_isOpen)
                    ActivateTextElements();
                else
                    DeactivateTextElements();
            }
        }

        

        private void ActivateTextElements()
        {
            foreach(var element in _textElements)
            {
                element.Enabled = true;
                element.Visible = true;
            }
        }

        private void DeactivateTextElements()
        {
            foreach (var element in _textElements)
            {
                element.Enabled = false;
                element.Visible = false;
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

        #endregion

        #endregion


        
        private void InitializePanel()
        {
            _isOpen = false;
            _itemSelected = 0;
            UpdatePanelRectangle();
            InitializeTextElements();
        }



        #region PanelRectangle
        private void UpdatePanelRectangle()
        {
            _panelRectangle.Location = GetPanelOrigin(_isOpen);
            _panelRectangle.Size = GetPanelSize(_isOpen);
        }

        private Point GetPanelOrigin(bool isOpen, int offsetFromTop = 0, int offsetFromRight = 0)
        {
            if (isOpen)
            {
                return new Point(
                _parentScreen.Width - PANEL_WIDTH_OPEN - offsetFromRight,
                offsetFromTop);
            }
            else
            {
                return new Point(
                _parentScreen.Width - PANEL_WIDTH_CLOSED - offsetFromRight,
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

        private void TogglePanelOpenOrClosed()
        {
            IsOpen = !IsOpen; 
            // IsOpen property setter calls UpdatePanelRectangle() and UpdateHeadingPosition()
        }

        #endregion


        #region Event Handling
        public void SubscribeToInputEvents(InputResponder input)
        {
            input.D0KeyPressed += OnD0KeyPressed;
            input.D1KeyPressed += OnD1KeyPressed;
            input.D2KeyPressed += OnD2KeyPressed;
            input.D3KeyPressed += OnD3KeyPressed;
            input.D4KeyPressed += OnD4KeyPressed;
            input.D5KeyPressed += OnD5KeyPressed;
            input.D6KeyPressed += OnD6KeyPressed;
            input.D7KeyPressed += OnD7KeyPressed;
            input.D8KeyPressed += OnD8KeyPressed;
            input.D9KeyPressed += OnD9KeyPressed;
            input.DKeyPressed += OnDKeyPressed;
            input.EKeyPressed += OnEKeyPressed;
            input.CKeyPressed += OnCKeyPressed;
            input.UKeyPressed += OnUKeyPressed;
            input.IKeyPressed += OnIKeyPressed;
            input.LeftMouseClick += OnMouseClicked;
            input.NewMousePosition += OnMouseMoved;
            Dungeon.OnInventoryChangeUINotification += HandleInventoryChange;
        }

        public void UnsubscribeFromInputEvents(InputResponder input)
        {
            input.D0KeyPressed -= OnD0KeyPressed;
            input.D1KeyPressed -= OnD1KeyPressed;
            input.D2KeyPressed -= OnD2KeyPressed;
            input.D3KeyPressed -= OnD3KeyPressed;
            input.D4KeyPressed -= OnD4KeyPressed;
            input.D5KeyPressed -= OnD5KeyPressed;
            input.D6KeyPressed -= OnD6KeyPressed;
            input.D7KeyPressed -= OnD7KeyPressed;
            input.D8KeyPressed -= OnD8KeyPressed;
            input.D9KeyPressed -= OnD9KeyPressed;
            input.DKeyPressed -= OnDKeyPressed;
            input.EKeyPressed -= OnEKeyPressed;
            input.CKeyPressed -= OnCKeyPressed;
            input.UKeyPressed -= OnUKeyPressed;
            input.IKeyPressed -= OnIKeyPressed;
            input.LeftMouseClick -= OnMouseClicked;
            input.NewMousePosition -= OnMouseMoved;
            Dungeon.OnInventoryChangeUINotification -= HandleInventoryChange;
        }

        private void OnMouseMoved(object sender, PointEventArgs e)
        {
            // update SpriteElement tints to indicate getting or losing focus
            _mousePosition = e.Point;
            foreach (var sprite in _spriteElements)
            {
                if (sprite.Rectangle.Contains(_mousePosition))
                {
                    sprite.Color = Color.Red;
                }
                else
                    sprite.Color = Color.White;
            }
        }

        private void OnMouseClicked(object sender, EventArgs e)
        {
            if (!_isOpen && _panelRectangle.Contains(_mousePosition))
            {
                TogglePanelOpenOrClosed();
            }
            else if (_toggleText.Rectangle.Contains(_mousePosition))
            {
                TogglePanelOpenOrClosed();
            }
            else
            {
                foreach (var sprite in _spriteElements)
                {
                    if (sprite.Rectangle.Contains(_mousePosition))
                    {
                        for (int i = 0; i < _inventorySlots.Count; i++)
                        {
                            if (sprite.Label.Equals(_inventorySlots[i]))
                            {
                                _itemSelected = i;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void OnD0KeyPressed(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Select0);
        }

        private void OnD1KeyPressed(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Select1);
        }

        private void OnD2KeyPressed(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Select2);
        }

        private void OnD3KeyPressed(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Select3);
        }

        private void OnD4KeyPressed(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Select4);
        }

        private void OnD5KeyPressed(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Select5);
        }

        private void OnD6KeyPressed(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Select6);
        }

        private void OnD7KeyPressed(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Select7);
        }

        private void OnD8KeyPressed(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Select8);
        }

        private void OnD9KeyPressed(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Select9);
        }

        private void OnDKeyPressed(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Drop);
        }

        private void OnUKeyPressed(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Use);
        }

        private void OnEKeyPressed(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Equip);
        }

        private void OnIKeyPressed(object sender, EventArgs e)
        {
            TogglePanelOpenOrClosed();
        }

        private void OnCKeyPressed(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Cancel);
        }
        #endregion

        private void UpdateInventoryData(Tuple<Dictionary<string, ItemType>, int> inventoryData)
        {
            Inventory = inventoryData.Item1;
            _gold = inventoryData.Item2;
        }



        #region SpriteElements
        private void CreateSpriteElements()
        {
            _spriteElements = new List<SpriteElement>();

            for (int i = 1; i < _inventorySlots.Count; i++)
            {
                var sprite = new SpriteElement();
                sprite.Label = _inventorySlots[i];
                sprite.Texture = _defaultSlotTextures[GetDefaultTextureKey(_inventorySlots[i])];
                sprite.Position = GetSpriteElementPosition(i - 1);
                sprite.Color = GetSpriteElementColor(i);
                sprite.Rectangle = new Rectangle((int)sprite.Position.X, (int)sprite.Position.Y, 32, 32);
                _spriteElements.Add(sprite);
            }

            var tempSpriteElements = new List<SpriteElement>(_spriteElements);
            foreach(var sprite in tempSpriteElements)
            {
                if(_inventory[sprite.Label] != ItemType.None 
                    && _inventory[sprite.Label] != ItemType.Fist 
                    && _inventory[sprite.Label] != ItemType.Bite)
                {
                    sprite.Color = Color.DarkSlateGray;
                    var item = new SpriteElement();
                    item.Label = ConvertItemTypeToString(_inventory[sprite.Label]);
                    item.Texture = ItemTextures.ItemTextureMap[_inventory[sprite.Label]];
                    item.Position = sprite.Position;
                    item.Color = Color.White;
                    item.Rectangle = sprite.Rectangle;
                    _spriteElements.Add(item);
                }
            }
        }

        private string ConvertItemTypeToString(ItemType type)
        {
            switch (type)
            {
                case ItemType.Meat:
                    return "meat";
                case ItemType.Shield:
                    return "buckler";
                case ItemType.ShortSword:
                    return "short_sword";
                default:
                    return "unknown item";
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
        #endregion



        #region TextElements
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
                "heading", this, GetHeadingPosition(HEADING, _panelRectangle, font),
                HEADING, font, Color.White);
        }

        private void ConstructToggleElement()
        {
            var font = GameController.NotoSans;
            var toggleSize = font.MeasureString(TOGGLE_CLOSED);
            var position = new Vector2(
                _panelRectangle.Right - toggleSize.X, (_panelRectangle.Top + 5));

            _toggleText = new TextElement(
                "toggle_text", this, position,
                TOGGLE_CLOSED, font, Color.White);
        }

        private void ConstructUnequipHintElement()
        {
            var position = new Vector2(
               _panelRectangle.Right - (PANEL_WIDTH_OPEN - 10),
               150);

            _unequipText = new TextElement(
                "unequip_text", this, position,
                UNEQUIP_HINT, GameController.NotoSans, Color.White);
        }

        private void ConstructEquipHintElement()
        {
            _equipText = new TextElement(
                "equip_hint", this, _unequipText.Position,
                EQUIP_HINT, GameController.NotoSans, Color.White);
        }

        private void ConstructDropHintElement()
        {
            var font = GameController.NotoSans;
            var position = _unequipText.Position;
            position.Y += font.MeasureString(EQUIP_HINT).Y + 5;

            _dropText = new TextElement(
                "drop_hint", this, position,
                DROP_HINT, font, Color.White);
        }

        private void ConstructCancelHintElement()
        {
            var font = GameController.NotoSans;
            var position = _dropText.Position;
            position.Y += font.MeasureString(CANCEL_HINT).Y + 5;

            _cancelText = new TextElement(
                "cancel_hint", this, position,
                CANCEL_HINT, font, Color.White);
        }

        private Vector2 GetHeadingPosition(string heading, Rectangle panelRect, SpriteFont font, int offsetTop = 5)
        {
            var headingSize = font.MeasureString(heading);
            return new Vector2(
                panelRect.Left + ((panelRect.Width / 2) - (headingSize.X / 2)),
                (panelRect.Top + offsetTop));
        }

        private void UpdateHeadingPosition()
        {
            _heading.Position = GetHeadingPosition(_heading.Text, _panelRectangle, _heading.Font);
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

        private void HandleInventoryChange(object sender, InventoryChangeEventArgs e)
        {
            UpdateInventoryData(e.Inventory);
            
        }
        #endregion



        // Called by GameScene.Update() to check for changes or input to handle
        public override void Update(GameTime gameTime)
        {
            CreateTextElements();
        }



        #region Draw Methods
        // Called by GameScene.Draw()
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            if (IsOpen)
                DrawOpenPanel(gameTime, spriteBatch);
            else
                DrawClosedPanel(gameTime, spriteBatch);

            spriteBatch.End();
        }

        private void DrawOpenPanel(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // draw background
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
                textElement.Draw(gameTime, spriteBatch);
            }
        }


        private void DrawClosedPanel(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_defaultSlotTextures["white_background"], _panelRectangle, Color.DarkBlue);
            
            spriteBatch.DrawString(_heading.Font, _heading.Text, _heading.Position, _heading.Color);
        }

        #endregion
    }
}
