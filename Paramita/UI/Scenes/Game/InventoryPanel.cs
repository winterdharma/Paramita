using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.GameLogic;
using Paramita.GameLogic.Actors;
using Paramita.GameLogic.Items;
using Paramita.UI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

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
        #region Fields
        private static Dictionary<string, Texture2D> _defaultSlotTextures 
            = new Dictionary<string, Texture2D>();

        internal Dictionary<string, ItemType> _inventory 
            = new Dictionary<string, ItemType>();
        private int _gold = 0;

        private const int PANEL_WIDTH_OPEN = 250;
        private const int PANEL_WIDTH_CLOSED = 150;
        private const int PANEL_HEIGHT_OPEN = 330;
        private const int PANEL_HEIGHT_CLOSED = 30;

        private Vector2 _itemInfoPosition;

        private const string HEADING_ID = "heading";
        private const string HEADING_TEXT = "(I)nventory";
        private const string DROP_HINT_ID = "drop_hint";
        private const string DROP_HINT_TEXT = "(D)rop Item";
        private const string USE_HINT_ID = "use_hint";
        private const string USE_HINT_TEXT = "(U)se Item";
        private const string EQUIP_HINT_ID = "equip_hint";
        private const string EQUIP_HINT_TEXT = "(E)quip Item";
        private const string UNEQUIP_HINT_ID = "unequip_hint";
        private const string UNEQUIP_HINT_TEXT = "Un(e)quip Item";
        private const string CANCEL_HINT_ID = "cancel_hint";
        private const string CANCEL_HINT_TEXT = "(C)ancel Selection";
        private const string BACKGROUND_OPEN_ID = "background_open";
        private const string BACKGROUND_CLOSED_ID = "background_closed";
        private const string MINIMIZE_ID = "minimize_icon";
        private const string LEFT_HAND_SLOT = "left_hand";
        private const string RIGHT_HAND_SLOT = "right_hand";
        private const string HEAD_SLOT = "head";
        private const string BODY_SLOT = "body";
        private const string FEET_SLOT = "feet";
        private const string OTHER1_SLOT = "other1";
        private const string OTHER2_SLOT = "other2";
        private const string OTHER3_SLOT = "other3";
        private const string OTHER4_SLOT = "other4";
        private const string OTHER5_SLOT = "other5";
        private const string LEFT_HAND_ITEM = "left_hand_item";
        private const string RIGHT_HAND_ITEM = "right_hand_item";
        private const string HEAD_ITEM = "head_item";
        private const string BODY_ITEM = "body_item";
        private const string FEET_ITEM = "feet_item";
        private const string OTHER1_ITEM = "other1_item";
        private const string OTHER2_ITEM = "other2_item";
        private const string OTHER3_ITEM = "other3_item";
        private const string OTHER4_ITEM = "other4_item";
        private const string OTHER5_ITEM = "other5_item";

        internal List<string> _inventorySlots = new List<string>()
        {   LEFT_HAND_SLOT, RIGHT_HAND_SLOT, HEAD_SLOT, BODY_SLOT, FEET_SLOT,
            OTHER1_SLOT, OTHER2_SLOT, OTHER3_SLOT, OTHER4_SLOT, OTHER5_SLOT };

        internal List<string> _inventoryItems = new List<string>()
        {   LEFT_HAND_ITEM, RIGHT_HAND_ITEM, HEAD_ITEM, BODY_ITEM, FEET_ITEM,
            OTHER1_ITEM, OTHER2_ITEM, OTHER3_ITEM, OTHER4_ITEM, OTHER5_ITEM };

        private readonly SpriteFont HEADING_FONT = GameController.ArialBold;
        private readonly SpriteFont HINT_FONT = GameController.NotoSans;

        private readonly Color WHITE = Color.White;
        private readonly Color RED = Color.Red;
        private readonly Color GRAY = Color.Gray;
        private readonly Color DARK_BLUE = Color.DarkBlue;

        private int _itemSelected = 0;
        private Point _mousePosition = new Point(0, 0);
        #endregion

        #region Constructors
        public InventoryPanel(Scene parent, int drawOrder) : base(parent, drawOrder)
        {
            IsOpen = false;
            SubscribeToInputEvents();
            InitializeInventoryData();
        }
        #endregion

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

        public bool IsOpen { get; set; }

        public int ItemSelected
        {
            get => _itemSelected;
            set
            {
                _itemSelected = value;
                UpdateHintTextElements();
            }
        }
        #endregion


        #region Initialization
        protected override Dictionary<string, Element> InitializeElements()
        {
            var images = InitializeImageElements();
            var texts = InitializeTextElements();
            texts.ToList().ForEach(x => images.Add(x.Key, x.Value));
            return images;
        }

        private Dictionary<string, Element> InitializeImageElements()
        {
            var images = new Dictionary<string, Element>
            {
                ["background_closed"] = CreateClosedPanelBackground(),
                ["background_open"] = CreateOpenPanelBackground(),
                ["minimize_icon"] = CreateMinimizeIcon()
            };
            var slotImages = CreateInventorySlotImages();
            var itemImages = CreateItemImages();
            slotImages.ToList().ForEach(x => images.Add(x.Key, x.Value));
            itemImages.ToList().ForEach(x => images.Add(x.Key, x.Value));
            return images;
        }

        private Dictionary<string, Element> InitializeTextElements()
        {
            var texts = new Dictionary<string, Element>();
            var textIds = new List<string>
            {
                HEADING_ID, EQUIP_HINT_ID, UNEQUIP_HINT_ID, USE_HINT_ID, DROP_HINT_ID,
                 CANCEL_HINT_ID
            };
            var textContents = new List<string>
            {
                HEADING_TEXT, EQUIP_HINT_TEXT, UNEQUIP_HINT_TEXT, USE_HINT_TEXT, DROP_HINT_TEXT,
                 CANCEL_HINT_TEXT
            };
            var fonts = new List<SpriteFont>
            {
                HEADING_FONT, HINT_FONT, HINT_FONT, HINT_FONT, HINT_FONT, HINT_FONT
            };
            var highlights = new List<Color> { WHITE, WHITE, WHITE, WHITE, WHITE, WHITE };
            var unhighlights = new List<Color> { WHITE, WHITE, WHITE, WHITE, WHITE, WHITE };
            var drawOrder = new List<int> { 1, 1, 1, 1, 1, 1 };

            for (var i = 0; i < textIds.Count; i++)
            {
                var id = textIds[i];
                var str = textContents[i];
                texts[id] = new LineOfText(id, this, GetElementPosition(id, false), str, fonts[i],
                    highlights[i], unhighlights[i], drawOrder[i]);

                texts[id].Hide();
            }

            // intended for future addition of text element showing info about selected item
            _itemInfoPosition = new Vector2(
                _panelRectangle.Right - (PANEL_WIDTH_OPEN - 10),
                _panelRectangle.Top + 140);

            return texts;
        }

        private void InitializeInventoryData()
        {
            var parent = Parent as GameScene;
            parent.Dungeon.GetPlayerInventory(); 
            // data is received as an event from Dungeon
        }
        #endregion


        #region Panel State Change Logic
        public void TogglePanelState()
        {
            IsOpen = !IsOpen;
            UpdatePanel();
        }

        private void UpdatePanel()
        {
            Rectangle = UpdatePanelRectangle();

            UpdateBackground();
            UpdateHeading();
            UpdateItemSlots();
            UpdateElements();
        }

        private void UpdateItemSlots()
        {
            foreach(var item in _inventory)
            {
                if(item.Value != ItemType.None && 
                    item.Value != ItemType.Bite && item.Value != ItemType.Fist)
                {
                    if (IsOpen)
                        Elements[item.Key + "_item"].Show();
                    else
                        Elements[item.Key + "_item"].Hide();
                }
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
            Elements["heading"].Position = GetElementPosition(HEADING_ID, Elements[USE_HINT_ID].Visible);
        }

        private void UpdateElements()
        {
            var exceptions = new List<string>()
            { "heading", "background_open", "background_closed" };
            exceptions.AddRange(_inventoryItems);

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
        protected override Rectangle UpdatePanelRectangle()
        {
            return new Rectangle(GetPanelOrigin(), GetPanelSize());
        }

        private Point GetPanelOrigin(int offsetTop = 0, int offsetRight = 0)
        {
            if (IsOpen)
                return new Point(_parentRectangle.Width - PANEL_WIDTH_OPEN - offsetRight, offsetTop);
            else
                return new Point(_parentRectangle.Width - PANEL_WIDTH_CLOSED - offsetRight, offsetTop);
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
            Dungeon.OnInventoryChangeUINotification += HandleInventoryChange;
        }

        public void UnsubscribeFromInputEvents()
        {
            Dungeon.OnInventoryChangeUINotification -= HandleInventoryChange;
        }

        private void HandleInventoryChange(object sender, InventoryChangeEventArgs e)
        {
            Inventory = e.Inventory.Item1;
            _gold = e.Inventory.Item2;
        }
        #endregion


        #region Images
        private Image CreateClosedPanelBackground()
        {
            var background = new Background(
                "background_closed",
                this,
                new Vector2(Rectangle.X, Rectangle.Y),
                _defaultSlotTextures["white_background"],
                Color.DarkBlue,
                Color.DarkBlue,
                Rectangle.Size,
                0
                );
            background.Hide();
            return background;
        }

        private Image CreateOpenPanelBackground()
        {
            var background = new Background(
                "background_open",
                this,
                new Vector2(Rectangle.X, Rectangle.Y),
                _defaultSlotTextures["background"],
                Color.White,
                Color.White,
                Rectangle.Size,
                0
                );
            background.Hide();
            return background;
        }

        private Image CreateMinimizeIcon()
        {
            var image = new Image(
                "minimize_icon", 
                this, 
                new Vector2(Rectangle.Right - 20, Rectangle.Top + 5),
                DefaultTextures["minimize_icon"], 
                Color.Gray, 
                Color.White,
                1, 
                0.0784f
                );
            image.Hide();
            return image;
        }

        private Dictionary<string, Element> CreateInventorySlotImages()
        {
            var slotImages = new Dictionary<string, Element>();
            for (int i = 0; i < _inventorySlots.Count; i++)
            {
                slotImages[_inventorySlots[i]] = new Image(
                    _inventorySlots[i],
                    this,
                    GetSpriteElementPosition(i),
                    _defaultSlotTextures[_inventorySlots[i]],
                    Color.White,
                    Color.Red,
                    1
                    );
                slotImages[_inventorySlots[i]].Hide();
            }
            return slotImages;
        }

        private Dictionary<string, Element> CreateItemImages()
        {
            var itemImages = new Dictionary<string, Element>();
            for (var i = 0; i < _inventoryItems.Count; i++)
            {
                var slotID = _inventorySlots[i];
                var itemID = _inventoryItems[i];

                var item = new Image(itemID, this, GetSpriteElementPosition(i), 
                    ItemTextures.ItemTextureMap[ItemType.None], Color.White, Color.Red, 2);

                item.Hide();
                itemImages[itemID] = item;
            }
            return itemImages;
        }

        private void UpdateItemImages()
        {
            foreach(var item in _inventory)
            {
                string itemID = item.Key + "_item";
                var itemImage = (Image)Elements[itemID];
                itemImage.Texture = ItemTextures.ItemTextureMap[item.Value];

                if (item.Value == ItemType.None || item.Value == ItemType.Bite ||
                    item.Value == ItemType.Fist || !IsOpen)
                {
                    itemImage.Hide();
                }
                else if (IsOpen)
                {
                    itemImage.Show();
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
                texture = _defaultSlotTextures[slot];
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
        private Vector2 GetElementPosition(string id, bool isUseHintVisible )
        {
            var hintsLeftMargin = Rectangle.Right - (PANEL_WIDTH_OPEN - 10);
            var hintsTopMargin = 150;
            var hintsLineHeight = HINT_FONT.MeasureString("Hint").Y;
            var lines = 1;
            var headingSize = HEADING_FONT.MeasureString(HEADING_TEXT);
            var headingLeftMargin = Rectangle.Left + ((Rectangle.Width / 2) - (headingSize.X / 2));
            var headingTopMargin = Rectangle.Top + 5;

            switch (id)
            {
                case HEADING_ID:
                    return new Vector2(headingLeftMargin, headingTopMargin);
                case UNEQUIP_HINT_ID:
                case EQUIP_HINT_ID:
                    return new Vector2(hintsLeftMargin, hintsTopMargin);
                case USE_HINT_ID:
                    return new Vector2(hintsLeftMargin, hintsTopMargin + hintsLineHeight);
                case DROP_HINT_ID:
                    if (isUseHintVisible)
                        lines = 2;
                    return new Vector2(hintsLeftMargin, hintsTopMargin + (hintsLineHeight * lines));
                case CANCEL_HINT_ID:
                    if (isUseHintVisible)
                        lines = 2;
                    return new Vector2(hintsLeftMargin, hintsTopMargin + (hintsLineHeight * (lines + 1)));
                default:
                    throw new ArgumentException("Element Id unimplemented");
            }
        }

        private void UpdateHintTextElements()
        {
            Elements[USE_HINT_ID].Hide(); // unimplemented feature

            if (_itemSelected == 0)
            {
                Elements[UNEQUIP_HINT_ID].Hide();
                Elements[EQUIP_HINT_ID].Hide();
                Elements[CANCEL_HINT_ID].Hide();
                Elements[DROP_HINT_ID].Hide();
            }
            else if (_itemSelected > 0 && _itemSelected < 6)
            {
                Elements[UNEQUIP_HINT_ID].Show();
                Elements[EQUIP_HINT_ID].Hide();
            }
            else if (_itemSelected >= 6)
            {
                Elements[UNEQUIP_HINT_ID].Hide();
                Elements[EQUIP_HINT_ID].Show();
            }

            if (_itemSelected > 0)
            {
                Elements[CANCEL_HINT_ID].Position 
                    = GetElementPosition(CANCEL_HINT_ID, Elements[USE_HINT_ID].Visible);
                Elements[CANCEL_HINT_ID].Show();
                Elements[DROP_HINT_ID].Position 
                    = GetElementPosition(DROP_HINT_ID, Elements[USE_HINT_ID].Visible);
                Elements[DROP_HINT_ID].Show();
            }
        }
        #endregion

        #region Public API
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

        public override void Show()
        {
            Elements[HEADING_ID].Show();
            UpdatePanel();
            Enabled = true;
            Visible = true;
        }
        #endregion
    }
}