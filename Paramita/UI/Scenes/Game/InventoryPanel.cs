using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Items;
using Paramita.SentientBeings;
using Paramita.UI.Input;
using System;

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


    /*
     * This class:
     *         Handles the display of the player's inventory
     *         Polls InputDevices for user input
     *         Provides the UI for equiping and dropping items
     */
    public class InventoryPanel
    {
        private int maxItems;
        private string[] labels;
        private string[] itemDescriptions;

        private Point panelOrigin;
        private Rectangle panelRectangle;
        private int panelWidth = 250;
        private int panelHeightOpen = 330;
        private int panelHeightClosed = 30;


        private string heading = "Inventory";
        private Vector2 headingPosition;
        
        private string toggleOpen = "[+]";
        private string toggleClosed = "[-]";
        private Vector2 togglePosition;

        private string selectHint =     "Press (0-9) to Select Item";
        private string dropHint =       "Press (d) to Drop Item";
        private string useHint =        "Press (u) to Use Item";
        private string equipHint =      "Press (e) to Equip Item";
        private string unequipHint =    "Press (e) to Unequip Item";
        private string cancelHint =     "Press (c) to Cancel Selection";
        private Vector2 hintPosition;

        private int _itemSelected;
        private Player player;
        private Texture2D background;
        private SpriteFont fontHeader = GameController.ArialBold;
        private SpriteFont fontText = GameController.NotoSans;
        private bool isOpen = false;

        

        public InventoryPanel(Player player, Texture2D background, int maxItems)
        {
            this.player = player;
            this.background = background;
            this.maxItems = maxItems;

            InputDevices.OnD0KeyWasPressed += HandleSelect0Input;
            InputDevices.OnD1KeyWasPressed += HandleSelect1Input;
            InputDevices.OnD2KeyWasPressed += HandleSelect2Input;
            InputDevices.OnD3KeyWasPressed += HandleSelect3Input;
            InputDevices.OnD4KeyWasPressed += HandleSelect4Input;
            InputDevices.OnD5KeyWasPressed += HandleSelect5Input;
            InputDevices.OnD6KeyWasPressed += HandleSelect6Input;
            InputDevices.OnD7KeyWasPressed += HandleSelect7Input;
            InputDevices.OnD8KeyWasPressed += HandleSelect8Input;
            InputDevices.OnD9KeyWasPressed += HandleSelect9Input;
            InputDevices.OnDKeyWasPressed += HandleDropInput;
            InputDevices.OnEKeyWasPressed += HandleEquipInput;
            InputDevices.OnUKeyWasPressed += HandleUseInput;
            InputDevices.OnCKeyWasPressed += HandleCancelInput;
            InputDevices.OnIKeyWasPressed += HandleToggleInput;

            labels = CreateItemLabels(maxItems);
            itemDescriptions = GetPlayerItemStrings();

            Rectangle parentScreen = GameController.ScreenRectangle;
            panelOrigin = new Point(parentScreen.Width - (panelWidth), 0);
            panelRectangle = new Rectangle(panelOrigin.X, 
                panelOrigin.Y, panelWidth, panelHeightClosed);

            Vector2 headingSize = fontHeader.MeasureString(heading);
            headingPosition = new Vector2( 
                panelRectangle.Left + ((panelWidth / 2) - (headingSize.X / 2)), 
                (panelRectangle.Top + 5) );

            togglePosition = new Vector2(
                panelRectangle.Right - 30, (panelRectangle.Top + 5));

            hintPosition = new Vector2(
                panelRectangle.Left + 10, panelHeightOpen - 60);
            _itemSelected = 0;
        }



        // Generates the item labels for the inventory list given the size of the inventory
        private string[] CreateItemLabels(int numberOfLabels)
        {
            string[] labels = new string[numberOfLabels + 1];

            labels[0] = "L Hand: ";
            labels[1] = "R Hand: ";
            labels[2] = "Head: ";
            labels[3] = "Body: ";
            labels[4] = "Feet: ";

            for (int x = 5; x < numberOfLabels; x++)
            {
                labels[x] = "Misc" + (x - 4).ToString() + ": ";
            }

            labels[numberOfLabels] = "Gold: ";
            return labels;
        }



        // Gets the player's Item list and converts it to a string[] for display on the panel
        private string[] GetPlayerItemStrings()
        {
            Item[] playerItems = player.UnequipedItems;
            string[] itemStrings = new string[11];

            // check for player's equiped item slots
            if(player.LeftHandItem != null)
                itemStrings[0] = player.LeftHandItem.ToString();
            else
                itemStrings[0] = "";

            if (player.RightHandItem != null)
                itemStrings[1] = player.RightHandItem.ToString();
            else
                itemStrings[1] = "";

            if (player.HeadItem != null)
                itemStrings[2] = player.HeadItem.ToString();
            else
                itemStrings[2] = "";

            if (player.BodyItem != null)
                itemStrings[3] = player.BodyItem.ToString();
            else
                itemStrings[3] = "";
 
            if (player.FeetItem != null)
                itemStrings[4] = player.FeetItem.ToString();
            else
                itemStrings[4] = "";

            // check for the player's unequiped slots
            for (int x = 5; x < itemStrings.Length-1; x++)
            {
                if(playerItems[x-5] != null)
                {
                    itemStrings[x] = playerItems[x-5].ToString();
                }
                else
                {
                    itemStrings[x] = "";
                }
            }
            itemStrings[10] = player.Gold.ToString();

            return itemStrings;
        }


        private void HandleInput(InventoryActions action)
        {
            if (action == InventoryActions.TogglePanel)
                isOpen = !isOpen;

            int selectionInput = 0;
            if((int)action > 0 && (int)action < 11)
                selectionInput = (int)action;

            if(selectionInput > 0)
                _itemSelected = selectionInput;

            if(_itemSelected > 0 && _itemSelected <= maxItems)
            {
                if(action == InventoryActions.Drop)
                {
                    player.DropItem(GetPlayerItem(_itemSelected));
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


        // Maps the item number in the Inventory list to the corresponding Item in the
        // player object. (The first five are equipment slots and second five are in
        // player.Items.
        // Accepts an integer and returns the proper item from Player.
        private Item GetPlayerItem(int itemSelected)
        {
            switch (itemSelected)
            {
                case 1:
                    return player.LeftHandItem;
                case 2:
                    return player.RightHandItem;
                case 3:
                    return player.HeadItem;
                case 4:
                    return player.BodyItem;
                case 5:
                    return player.FeetItem;
                case 6:
                    return player.UnequipedItems[0];
                case 7:
                    return player.UnequipedItems[1];
                case 8:
                    return player.UnequipedItems[2];
                case 9:
                    return player.UnequipedItems[3];
                case 10:
                    return player.UnequipedItems[4];
                default:
                    return null;
            }
        }


        // Called by GameScene.Update() to check for changes or input to handle
        public void Update(GameTime gameTime)
        {
            itemDescriptions = GetPlayerItemStrings();
            //HandleInput();
        }


        // Called by GameScene.Draw()
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            if (isOpen)
                DrawOpenPanel(spriteBatch);
            else
                DrawClosedPanel(spriteBatch);

            spriteBatch.End();
        }


        private void DrawOpenPanel(SpriteBatch spriteBatch)
        {
            // draw background
            panelRectangle.Height = panelHeightOpen;
            spriteBatch.Draw(background, panelRectangle, Color.White);

            // draw header and toggle symbol
            spriteBatch.DrawString(fontHeader, heading, headingPosition, Color.White);
            spriteBatch.DrawString(fontText, toggleClosed, togglePosition, Color.White);

            // draw the list of items in player's inventory
            Vector2 itemOrigin = new Vector2(panelRectangle.Left + 10, headingPosition.Y + 20);
            Color fontColor = Color.White;
            for (int x = 0; x < itemDescriptions.Length; x++)
            {
                if (_itemSelected == (x + 1) && GetPlayerItem(_itemSelected) != null)
                {
                    fontColor = Color.Red;
                }

                string line = labels[x] + itemDescriptions[x];
                spriteBatch.DrawString(fontText, line, itemOrigin, fontColor);
                fontColor = Color.White;
                itemOrigin.Y += 20;
            }

            // check if an item has been selected and draw appropos hint text
            if (_itemSelected != 0 && _itemSelected <= maxItems && GetPlayerItem(_itemSelected) != null)
            {
                spriteBatch.DrawString(fontText, dropHint, hintPosition, fontColor);
                spriteBatch.DrawString(fontText, cancelHint,
                    new Vector2(hintPosition.X, hintPosition.Y + 15), fontColor);
            }
            else
            {
                spriteBatch.DrawString(fontText, selectHint, hintPosition, fontColor);
            }
        }


        private void DrawClosedPanel(SpriteBatch spriteBatch)
        {
            // draw panel background
            panelRectangle.Height = panelHeightClosed;
            spriteBatch.Draw(background, panelRectangle, Color.White);

            // draw the header and toggle symbol
            spriteBatch.DrawString(fontHeader, heading, headingPosition, Color.White);
            spriteBatch.DrawString(fontText, toggleOpen, togglePosition, Color.White);
        }
    }
}
