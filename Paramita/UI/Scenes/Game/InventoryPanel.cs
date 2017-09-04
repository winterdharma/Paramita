using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.GameLogic;
using Paramita.GameLogic.Actors;
using Paramita.GameLogic.Items;
using Paramita.GameLogic.Utility;
using Paramita.UI.Elements;
using System;
using System.Collections.Generic;

namespace Paramita.UI.Base.Game
{
    public enum InventoryActions
    {
        None,
        Drop,
        Use,
        Equip,
        Cancel
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
            { "left_hand", "right_hand", "head", "body", "feet",
                "other1", "other2", "other3", "other4", "other5"};

        private static Dictionary<string, Texture2D> _defaultSlotTextures 
            = new Dictionary<string, Texture2D>();

        private Dictionary<string, ItemType> _inventory 
            = new Dictionary<string, ItemType>();
        private int _gold = 0;

        private Rectangle _parentScreen;
        private const int PANEL_WIDTH_OPEN = 250;
        private const int PANEL_WIDTH_CLOSED = 150;
        private const int PANEL_HEIGHT_OPEN = 330;
        private const int PANEL_HEIGHT_CLOSED = 30;

        private Vector2 _itemInfoPosition;

        private const string HEADING = "(I)nventory";
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


        public InventoryPanel(Scene parent) : base(parent)
        {
            _parentScreen = Parent.ScreenRectangle;
            InitializePanel();
            SubscribeToInputEvents();
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
                UpdateItemImages();
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


        #region Initialization
        private void InitializePanel()
        {
            UpdatePanelRectangle();
            InitializeElements();
            InitializeInventoryData();
        }

        private void InitializeElements()
        {
            InitializeImageElements();
            InitializeTextElements();
        }

        private void InitializeInventoryData()
        {
            var parent = Parent as GameScene;
            parent.Dungeon.GetPlayerInventory(); 
            // data is received as an event from Dungeon
        }
        #endregion


        #region Panel State Change Logic
        private void TogglePanelOpenOrClosed()
        {
            IsOpen = !IsOpen;

            UpdateEventSubscriptions();
            UpdatePanelRectangle();

            UpdateBackground();
            UpdateHeading();
            UpdateElements();
        }

        /// <summary>
        /// Subscribes to mouse events when the panel is closed and unsubscribes from them when open.
        /// </summary>
        private void UpdateEventSubscriptions()
        {
            if(!IsOpen)
            {
                Input.LeftMouseClick += OnMouseClicked;
                Input.NewMousePosition += OnMouseMoved;
            }
            else
            {
                Input.LeftMouseClick -= OnMouseClicked;
                Input.NewMousePosition -= OnMouseMoved;
            }
            
        }

        private void UpdateBackground()
        {
            if (IsOpen)
            {
                Elements["background_open"].Show();
                Elements["background_closed"].Hide();
            }
            else
            {
                Elements["background_closed"].Show();
                Elements["background_open"].Hide();
            }
        }

        private void UpdateHeading()
        {
            Elements["heading"].Position = GetHeadingPosition(_headingFont);
        }

        private void UpdateElements()
        {
            var exceptions = new List<string>()
            { "heading", "background_open", "background_closed" };

            foreach (var key in Elements.Keys)
            {
                if (exceptions.Contains(key))
                    continue;

                if (IsOpen)
                    Elements[key].Show();
                else
                    Elements[key].Hide();
            }
            // if panel is open, show/hide hint texts depending on _itemSelected value
            if(IsOpen)
                UpdateHintTextElements();
        }
        #endregion


        #region Update PanelRectangle Methods
        private void UpdatePanelRectangle()
        {
            PanelRectangle = new Rectangle(GetPanelOrigin(), GetPanelSize());
        }

        private Point GetPanelOrigin(int offsetTop = 0, int offsetRight = 0)
        {
            if (IsOpen)
                return new Point(_parentScreen.Width - PANEL_WIDTH_OPEN - offsetRight, offsetTop);
            else
                return new Point(_parentScreen.Width - PANEL_WIDTH_CLOSED - offsetRight, offsetTop);
        }

        private Point GetPanelSize()
        {
            if (IsOpen)
                return new Point(PANEL_WIDTH_OPEN, PANEL_HEIGHT_OPEN);
            else
                return new Point(PANEL_WIDTH_CLOSED, PANEL_HEIGHT_CLOSED);
        }
        #endregion


        #region Event Handling
        public void SubscribeToInputEvents()
        {
            Input.D0KeyPressed += OnD0KeyPressed;
            Input.D1KeyPressed += OnD1KeyPressed;
            Input.D2KeyPressed += OnD2KeyPressed;
            Input.D3KeyPressed += OnD3KeyPressed;
            Input.D4KeyPressed += OnD4KeyPressed;
            Input.D5KeyPressed += OnD5KeyPressed;
            Input.D6KeyPressed += OnD6KeyPressed;
            Input.D7KeyPressed += OnD7KeyPressed;
            Input.D8KeyPressed += OnD8KeyPressed;
            Input.D9KeyPressed += OnD9KeyPressed;
            Input.DKeyPressed += OnDKeyPressed;
            Input.EKeyPressed += OnEKeyPressed;
            Input.CKeyPressed += OnCKeyPressed;
            Input.UKeyPressed += OnUKeyPressed;
            Input.IKeyPressed += OnIKeyPressed;
            SubscribeToMouseEvents();
            Dungeon.OnInventoryChangeUINotification += HandleInventoryChange;
        }

        public void UnsubscribeFromInputEvents()
        {
            Input.D0KeyPressed -= OnD0KeyPressed;
            Input.D1KeyPressed -= OnD1KeyPressed;
            Input.D2KeyPressed -= OnD2KeyPressed;
            Input.D3KeyPressed -= OnD3KeyPressed;
            Input.D4KeyPressed -= OnD4KeyPressed;
            Input.D5KeyPressed -= OnD5KeyPressed;
            Input.D6KeyPressed -= OnD6KeyPressed;
            Input.D7KeyPressed -= OnD7KeyPressed;
            Input.D8KeyPressed -= OnD8KeyPressed;
            Input.D9KeyPressed -= OnD9KeyPressed;
            Input.DKeyPressed -= OnDKeyPressed;
            Input.EKeyPressed -= OnEKeyPressed;
            Input.CKeyPressed -= OnCKeyPressed;
            Input.UKeyPressed -= OnUKeyPressed;
            Input.IKeyPressed -= OnIKeyPressed;
            Input.LeftMouseClick -= OnMouseClicked;
            Input.NewMousePosition -= OnMouseMoved;
            Dungeon.OnInventoryChangeUINotification -= HandleInventoryChange;
        }

        private void SubscribeToMouseEvents()
        {
            Input.LeftMouseClick += OnMouseClicked;
            Input.NewMousePosition += OnMouseMoved;

            foreach (var key in Elements.Keys)
            {
                if (Elements[key] is Background)
                    continue;
                else if (Elements[key] is Image)
                {
                    var image = Elements[key] as Image;
                    image.MouseOver += ImageMousedOver;
                    image.MouseGone += ImageMouseGone;
                    image.LeftClicked += ImageClicked;
                }
            }
        }

        private void ImageClicked(object sender, EventArgs e)
        {
            var image = sender as Image;
            if (image.Id.Equals("minimize_icon"))
                TogglePanelOpenOrClosed();
            else
                ItemSelected = _inventorySlots.FindIndex(slot => slot.Equals(image.Id)) + 1;
        }

        private void ImageMouseGone(object sender, EventArgs e)
        {
            var image = sender as Image;
            image.Unhighlight();
        }

        private void ImageMousedOver(object sender, EventArgs e)
        {
            var image = sender as Image;
            image.Highlight();
        }

        private void OnMouseMoved(object sender, PointEventArgs e)
        {
            _mousePosition = e.Point;
        }

        private void OnMouseClicked(object sender, EventArgs e)
        {
            if (!_isOpen && _panelRectangle.Contains(_mousePosition))
            {
                TogglePanelOpenOrClosed();
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

        private void HandleInput(InventoryActions action)
        {
            if (ItemSelected == 0)
                return;

            if (action == InventoryActions.Drop)
            {
                string slot = _inventorySlots[ItemSelected - 1];
                if (_inventory[slot] != ItemType.None)
                {
                    OnPlayerDroppedItem?.Invoke(null,
                    new InventoryEventArgs(slot, _inventory[slot]));
                }
            }
        }

        private void HandleInventoryChange(object sender, InventoryChangeEventArgs e)
        {
            Inventory = e.Inventory.Item1;
            _gold = e.Inventory.Item2;
        }
        #endregion


        #region Images
        private void InitializeImageElements()
        {
            Elements["background_closed"] = CreateClosedPanelBackground();
            Elements["background_open"] = CreateOpenPanelBackground();
            Elements["minimize_icon"] = CreateMinimizeIcon();
            Elements["minimize_icon"].Hide();
            CreateInventorySlotImages();            
        }

        private Image CreateClosedPanelBackground()
        {
            var background = new Background(
                "background_closed",
                this,
                new Vector2(PanelRectangle.X, PanelRectangle.Y),
                _defaultSlotTextures["white_background"],
                Color.DarkBlue,
                Color.White,
                PanelRectangle.Size
                );
            background.Show();
            return background;
        }

        private Image CreateOpenPanelBackground()
        {
            var background = new Background(
                "background_open",
                this,
                new Vector2(PanelRectangle.X, PanelRectangle.Y),
                _defaultSlotTextures["background"],
                Color.White,
                Color.White,
                PanelRectangle.Size
                );
            background.Hide();
            return background;
        }

        private Image CreateMinimizeIcon()
        {
            var image = new Image(
                "minimize_icon", 
                this, 
                new Vector2(PanelRectangle.Right - 20, PanelRectangle.Top + 5),
                DefaultTextures["minimize_icon"], 
                Color.Gray, 
                Color.White,
                0.0784f
                );
            image.Hide();
            return image;
        }

        private void UpdateItemImages()
        {
            RemoveItemImages();
            CreateItemImages();
        }

        private void CreateInventorySlotImages()
        {
            for (int i = 0; i < _inventorySlots.Count; i++)
            {
                Elements[_inventorySlots[i]] = new Image(
                    _inventorySlots[i],
                    this,
                    GetSpriteElementPosition(i),
                    _defaultSlotTextures[GetDefaultTextureKey(_inventorySlots[i])],
                    Color.White,
                    Color.Red
                    );
                Elements[_inventorySlots[i]].Hide();
            }
        }

        private void CreateItemImages()
        {
            int count = 0;

            foreach (var slot in _inventorySlots)
            {
                if (_inventory[slot] != ItemType.None
                    && _inventory[slot] != ItemType.Fist
                    && _inventory[slot] != ItemType.Bite)
                {
                    var slotImage = Elements[slot];
                    var item = new Image("item_" + ConvertItemTypeToString(_inventory[slot]) + ++count,
                        this, slotImage.Position, ItemTextures.ItemTextureMap[_inventory[slot]],
                        Color.White, Color.Red);
                    Elements[item.Id] = item;
                }
            }
        }

        private void RemoveItemImages()
        {
            var itemsToRemove = new List<string>();
            foreach (var key in Elements.Keys)
            {
                if (key.Contains("item_"))
                    itemsToRemove.Add(key);
            }
            
            if(itemsToRemove.Count > 0)
            {
                foreach(var key in itemsToRemove)
                {
                    Elements.Remove(key);
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


        #region Text Elements
        private void InitializeTextElements()
        {
            Elements["heading"] = ConstructHeadingElement();
            Elements["heading"].Show();
            Elements["unequip_hint"] = ConstructUnequipHintElement();
            Elements["unequip_hint"].Hide();
            Elements["equip_hint"] = ConstructEquipHintElement();
            Elements["equip_hint"].Hide();
            Elements["drop_hint"] = ConstructDropHintElement();
            Elements["drop_hint"].Hide();
            Elements["cancel_hint"] = ConstructCancelHintElement();
            Elements["cancel_hint"].Hide();

            // intended for future addition of text element showing info about selected item
            _itemInfoPosition = new Vector2(
                _panelRectangle.Right - (PANEL_WIDTH_OPEN - 10),
                _panelRectangle.Top + 140);
        }

        private LineOfText ConstructHeadingElement()
        {
            var heading = new LineOfText(
                "heading", this, GetHeadingPosition(_headingFont),
                HEADING, _headingFont, Color.White, Color.White);

            heading.Show();
            return heading;
        }

        private LineOfText ConstructUnequipHintElement()
        {
            var position = new Vector2(
               _panelRectangle.Right - (PANEL_WIDTH_OPEN - 10),
               150);

            var unequip = new LineOfText(
                "unequip_hint", this, position,
                UNEQUIP_HINT, GameController.NotoSans, Color.White, Color.White);

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
                EQUIP_HINT, GameController.NotoSans, Color.White, Color.White);
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
                DROP_HINT, font, Color.White, Color.White);
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
                CANCEL_HINT, font, Color.White, Color.White);
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

        private void UpdateHintTextElements()
        {
            if (_itemSelected == 0)
            {
                Elements["unequip_hint"].Hide();
                Elements["equip_hint"].Hide();
                Elements["cancel_hint"].Hide();
                Elements["drop_hint"].Hide();
            }
            else if (_itemSelected > 0 && _itemSelected < 6)
            {
                Elements["unequip_hint"].Show();
                Elements["equip_hint"].Hide();
            }
            else if (_itemSelected >= 6)
            {
                Elements["unequip_hint"].Hide();
                Elements["equip_hint"].Show();
            }

            if (_itemSelected > 0)
            {
                Elements["cancel_hint"].Show();
                Elements["drop_hint"].Show();
            }
        }
        #endregion


        // Called by GameScene.Update() to check for changes or input to handle
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }


        // Called by GameScene.Draw()
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }
    }
}
