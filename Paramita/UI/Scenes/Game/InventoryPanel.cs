using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.GameLogic;
using Paramita.GameLogic.Actors;
using Paramita.GameLogic.Items;
using Paramita.GameLogic.Utility;
using Paramita.UI.Input;
using System;
using System.Collections.Generic;

namespace Paramita.UI.Base.Game
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

        private List<Image> _images;
        private List<LineOfText> _textElements;

        private Rectangle _panelRectangle;
        private Rectangle _parentScreen;
        private const int PANEL_WIDTH_OPEN = 250;
        private const int PANEL_WIDTH_CLOSED = 150;
        private const int PANEL_HEIGHT_OPEN = 330;
        private const int PANEL_HEIGHT_CLOSED = 30;

        private Vector2 _itemInfoPosition;

        private const string HEADING = "(I)nventory";
        private const string TOGGLE_CLOSED = "[X]";
        private const string SELECT_HINT = "Press (0-9) to Select Item ";
        private const string DROP_HINT = "(D)rop Item";
        private const string USE_HINT = "(U)se Item";
        private const string EQUIP_HINT = "(E)quip Item";
        private const string UNEQUIP_HINT = "Un(e)quip Item";
        private const string CANCEL_HINT = "(C)ancel Selection";

        private SpriteFont _headingFont = GameController.ArialBold;

        private int _itemSelected = 0;
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
                InitializeImages();
            }
        }

        public bool IsOpen
        {
            get { return _isOpen; }
            set { _isOpen = value; }
        }

        public int ItemSelected
        {
            get => _itemSelected;
            private set
            {
                _itemSelected = value;
                UpdateHintTextElements();
            }
        }

        private void UpdateHintTextElements()
        {
            if (_itemSelected == 0)
            {
                _textElements.Find(t => t.Id.Equals("unequip_hint")).Hide();
                _textElements.Find(t => t.Id.Equals("equip_hint")).Hide();
                _textElements.Find(t => t.Id.Equals("cancel_hint")).Hide();
                _textElements.Find(t => t.Id.Equals("drop_hint")).Hide();
            }
            else if (_itemSelected > 0 && _itemSelected < 6)
            {
                _textElements.Find(t => t.Id.Equals("unequip_hint")).Show();
                _textElements.Find(t => t.Id.Equals("equip_hint")).Hide();
            }
            else if (_itemSelected >= 6)
            {
                _textElements.Find(t => t.Id.Equals("unequip_hint")).Hide();
                _textElements.Find(t => t.Id.Equals("equip_hint")).Show();
            }

            if(_itemSelected > 0)
            {
                _textElements.Find(t => t.Id.Equals("cancel_hint")).Show();
                _textElements.Find(t => t.Id.Equals("drop_hint")).Show();
            }
        }

        private void ActivateTextElements()
        {
            UpdateHintTextElements();
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
            UpdatePanelRectangle();
            _images = new List<Image>(CreateInventorySlotImages());
            _textElements = new List<LineOfText>(InitializeTextElements());
        }

        private void TogglePanelOpenOrClosed()
        {
            IsOpen = !IsOpen;
            // IsOpen property setter calls UpdatePanelRectangle() and UpdateHeadingPosition()

            UpdatePanelRectangle();
            UpdateBackgroundElement();
            UpdateImageVisibility();
            UpdateHintTextVisibility();
            UpdateHeadingElement();
            UpdateToggleButtonVisibility();
        }

        private void UpdateHeadingElement()
        {
            var heading = _textElements.Find(t => t.Id.Equals("heading"));
            heading.Position = GetHeadingPosition(_headingFont);
            heading.Visible = true;
            heading.Enabled = true;
        }

        private void UpdateImageVisibility()
        {
            if (IsOpen)
                ActivateImageElements();
            else
                DeactivateImageElements();
        }

        private void UpdateHintTextVisibility()
        {
            if (IsOpen)
                ActivateTextElements();
            else
                DeactivateTextElements();
        }

        private void UpdateBackgroundElement()
        {

        }

        private void UpdateToggleButtonVisibility()
        {
            var toggle = _textElements.Find(t => t.Id.Equals("toggle_text"));

            if (IsOpen)
                toggle.Show();
            else
                toggle.Hide();
        }

        #region PanelRectangle
        private void UpdatePanelRectangle()
        {
            _panelRectangle.Location = GetPanelOrigin();
            _panelRectangle.Size = GetPanelSize();
        }

        private Point GetPanelOrigin(int offsetFromTop = 0, int offsetFromRight = 0)
        {
            if (IsOpen)
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

        private Point GetPanelSize()
        {
            if (IsOpen)
            {
                return new Point(PANEL_WIDTH_OPEN, PANEL_HEIGHT_OPEN);
            }
            else
            {
                return new Point(PANEL_WIDTH_CLOSED, PANEL_HEIGHT_CLOSED);
            }
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
            foreach (var image in _images)
            {
                if (image.Rectangle.Contains(_mousePosition))
                    image.Color = Color.Red;
                else
                    image.Color = Color.White;
            }
        }

        private void OnMouseClicked(object sender, EventArgs e)
        {
            if (!_isOpen && _panelRectangle.Contains(_mousePosition))
            {
                TogglePanelOpenOrClosed();
            }
            else if (_textElements.Find(t => t.Id.Equals("toggle_text")).Rectangle.Contains(_mousePosition))
            {
                TogglePanelOpenOrClosed();
            }
            else // add 1 because _itemSelected begins at 1 not 0
            {   
                _itemSelected = 
                    _images.FindIndex(image => image.Rectangle.Contains(_mousePosition)) + 1;
            }
        }

        private void OnD0KeyPressed(object sender, EventArgs e)
        {
            ItemSelected = 10;
        }

        private void OnD1KeyPressed(object sender, EventArgs e)
        {
            ItemSelected = 1;
        }

        private void OnD2KeyPressed(object sender, EventArgs e)
        {
            ItemSelected = 2;
        }

        private void OnD3KeyPressed(object sender, EventArgs e)
        {
            ItemSelected = 3;
        }

        private void OnD4KeyPressed(object sender, EventArgs e)
        {
            ItemSelected = 4;
        }

        private void OnD5KeyPressed(object sender, EventArgs e)
        {
            ItemSelected = 5;
        }

        private void OnD6KeyPressed(object sender, EventArgs e)
        {
            ItemSelected = 6;
        }

        private void OnD7KeyPressed(object sender, EventArgs e)
        {
            ItemSelected = 7;
        }

        private void OnD8KeyPressed(object sender, EventArgs e)
        {
            ItemSelected = 8;
        }

        private void OnD9KeyPressed(object sender, EventArgs e)
        {
            ItemSelected = 9;
        }

        private void OnDKeyPressed(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Drop);
            ItemSelected = 0;
        }

        private void OnUKeyPressed(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Use);
            ItemSelected = 0;
        }

        private void OnEKeyPressed(object sender, EventArgs e)
        {
            HandleInput(InventoryActions.Equip);
            ItemSelected = 0;
        }

        private void OnIKeyPressed(object sender, EventArgs e)
        {
            TogglePanelOpenOrClosed();
        }

        private void OnCKeyPressed(object sender, EventArgs e)
        {
            ItemSelected = 0;
        }
        #endregion

        private void UpdateInventoryData(Tuple<Dictionary<string, ItemType>, int> inventoryData)
        {
            Inventory = inventoryData.Item1;
            _gold = inventoryData.Item2;
        }



        #region Images
        private void InitializeImages()
        {
            // clear the previous item images
            if(_images.Count > 10)
                _images.RemoveRange(10, _images.Count-10);
            // add the new set of item images
            _images.AddRange(CreateItemImages(_images));       
        }

        private List<Image> CreateInventorySlotImages()
        {
            var images = new List<Image>();
            for (int i = 1; i < _inventorySlots.Count; i++)
            {
                var image = new Image(
                    _inventorySlots[i],
                    this,
                    GetSpriteElementPosition(i - 1),
                    _defaultSlotTextures[GetDefaultTextureKey(_inventorySlots[i])],
                    Color.White
                    );
                image.Rectangle = new Rectangle((int)image.Position.X, (int)image.Position.Y, 32, 32);
                image.Hide();
                images.Add(image);
            }
            return images;
        }

        private List<Image> CreateItemImages(List<Image> images)
        {
            var items = new List<Image>();
            var tempList = images;
            foreach (var slotImage in tempList)
            {
                if (_inventory[slotImage.Id] != ItemType.None
                    && _inventory[slotImage.Id] != ItemType.Fist
                    && _inventory[slotImage.Id] != ItemType.Bite)
                {
                    var item = new Image(ConvertItemTypeToString(_inventory[slotImage.Id]),
                        this, slotImage.Position, ItemTextures.ItemTextureMap[_inventory[slotImage.Id]],
                        Color.White);
                    item.Rectangle = slotImage.Rectangle;
                    items.Add(item);
                }
            } 
            return items;
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

        private void ActivateImageElements()
        {
            foreach (var image in _images)
            {
                image.Visible = true;
                image.Enabled = true;
            }
        }

        private void DeactivateImageElements()
        {
            foreach(var image in _images)
            {
                image.Visible = false;
                image.Enabled = false;
            }
        }
        #endregion



        #region Text Elements
        private List<LineOfText> InitializeTextElements()
        {
            var textElements = new List<LineOfText>
            {
                ConstructHeadingElement(),
                ConstructToggleElement(),
                ConstructUnequipHintElement(),
                ConstructEquipHintElement(),
                ConstructDropHintElement(),
                ConstructCancelHintElement()
            };

            // intended for future addition of text element showing info about selected item
            _itemInfoPosition = new Vector2(
                _panelRectangle.Right - (PANEL_WIDTH_OPEN - 10),
                _panelRectangle.Top + 140);

            return textElements;
        }

        private LineOfText ConstructHeadingElement()
        {
            var heading = new LineOfText(
                "heading", this, GetHeadingPosition(_headingFont),
                HEADING, _headingFont, Color.White);

            heading.Show();
            return heading;
        }

        private LineOfText ConstructToggleElement()
        {
            var font = GameController.NotoSans;
            var toggleSize = font.MeasureString(TOGGLE_CLOSED);
            var position = new Vector2(
                _panelRectangle.Right - toggleSize.X, (_panelRectangle.Top + 5));

            var toggle = new LineOfText(
                "toggle_text", this, position,
                TOGGLE_CLOSED, font, Color.White);
            toggle.Hide();
            return toggle;
        }

        private LineOfText ConstructUnequipHintElement()
        {
            var position = new Vector2(
               _panelRectangle.Right - (PANEL_WIDTH_OPEN - 10),
               150);

            var unequip = new LineOfText(
                "unequip_hint", this, position,
                UNEQUIP_HINT, GameController.NotoSans, Color.White);

            unequip.Hide();
            return unequip;
        }

        private LineOfText ConstructEquipHintElement()
        {
            var position = new Vector2(
              _panelRectangle.Right - (PANEL_WIDTH_OPEN - 10),
              150);

            var equip = new LineOfText(
                "equip_hint", this, position,
                EQUIP_HINT, GameController.NotoSans, Color.White);
            equip.Hide();
            return equip;
        }

        private LineOfText ConstructDropHintElement()
        {
            var font = GameController.NotoSans;
            var position = new Vector2(
              _panelRectangle.Right - (PANEL_WIDTH_OPEN - 10),
              150);
            position.Y += font.MeasureString(EQUIP_HINT).Y + 5;

            var drop = new LineOfText(
                "drop_hint", this, position,
                DROP_HINT, font, Color.White);
            drop.Hide();
            return drop;
        }

        private LineOfText ConstructCancelHintElement()
        {
            var font = GameController.NotoSans;
            var position = new Vector2(
              _panelRectangle.Right - (PANEL_WIDTH_OPEN - 10),
              150);
            position.Y += font.MeasureString(EQUIP_HINT).Y + 5;
            position.Y += font.MeasureString(CANCEL_HINT).Y + 5;

            var cancel = new LineOfText(
                "cancel_hint", this, position,
                CANCEL_HINT, font, Color.White);
            cancel.Hide();
            return cancel;
        }

        private Vector2 GetHeadingPosition(SpriteFont font, int offsetTop = 5)
        {
            var headingSize = font.MeasureString(HEADING);
            return new Vector2(
                _panelRectangle.Left + ((_panelRectangle.Width / 2) - (headingSize.X / 2)),
                (_panelRectangle.Top + offsetTop));
        }
        #endregion




        #region Input Handlers
        private void HandleInput(InventoryActions action)
        {
            if (ItemSelected == 0)
                return;

            if(action == InventoryActions.Drop)
            {
                string slot = _inventorySlots[ItemSelected];
                if(_inventory[slot] != ItemType.None)
                {
                    OnPlayerDroppedItem?.Invoke(null,
                    new InventoryEventArgs(slot, _inventory[slot]));
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
        }



        #region Draw Methods
        // Called by GameScene.Draw()
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            if(IsOpen)
                spriteBatch.Draw(_defaultSlotTextures["background"], _panelRectangle, Color.White);
            else
                spriteBatch.Draw(_defaultSlotTextures["white_background"], _panelRectangle, Color.DarkBlue);

            foreach (var element in _images)
            {
                element.Draw(gameTime, spriteBatch);
            }

            foreach (var element in _textElements)
            {
                element.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }
        #endregion
    }
}
