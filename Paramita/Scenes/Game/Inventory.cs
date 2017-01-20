using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Items;
using Paramita.Mechanics;
using Paramita.SentientBeings;

namespace Paramita.Scenes.Game
{
    public enum InventoryActions
    {
        Drop,
        Use,
        Equip,
        Cancel,
        TogglePanel,
        None
    }


    /*
     * This class:
     *         Handles the display of the player's inventory
     *         Polls InputDevices for user input
     *         Provides the UI for equiping and dropping items
     */
    public class Inventory
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

        private InputDevices input;
        private int itemSelected;
        private Player player;
        private Texture2D background;
        private SpriteFont font;
        private bool isOpen = false;

        

        public Inventory(SpriteFont font, Rectangle parentScreen, InputDevices inputDevices, Player player, Texture2D background, int maxItems)
        {
            this.font = font;
            input = inputDevices;
            this.player = player;
            this.background = background;
            this.maxItems = maxItems;

            labels = CreateItemLabels(maxItems);
            itemDescriptions = GetPlayerItemStrings();

            panelOrigin = new Point(parentScreen.Width - (panelWidth), 0);
            panelRectangle = new Rectangle(panelOrigin.X, 
                panelOrigin.Y, panelWidth, panelHeightClosed);

            Vector2 headingSize = font.MeasureString(heading);
            headingPosition = new Vector2( 
                panelRectangle.Left + ((panelWidth / 2) - (headingSize.X / 2)), 
                (panelRectangle.Top + 10) );

            togglePosition = new Vector2(
                panelRectangle.Right - 30, (panelRectangle.Top + 10));

            hintPosition = new Vector2(
                panelRectangle.Left + 10, panelRectangle.Bottom - 60);
            itemSelected = 0;
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


        private void HandleInput()
        {
            InventoryActions action = InputDevices.CheckForInventoryAction();
            int selectionInput = InputDevices.CheckIfItemSelected();

            if (action == InventoryActions.TogglePanel)
            {
                isOpen = !isOpen;
            }

            if(selectionInput > 0)
                itemSelected = InputDevices.CheckIfItemSelected();

            if(itemSelected > 0 && itemSelected <= maxItems)
            {
                if(action == InventoryActions.Drop)
                {
                    player.DropItem(GetPlayerItem(itemSelected));
                    itemSelected = 0;
                }
                else if(action == InventoryActions.Cancel)
                {
                    itemSelected = 0;
                }
            }
        }



        // Called by GameScene.Update() to check for changes or input to handle
        public void Update(GameTime gameTime)
        {
            itemDescriptions = GetPlayerItemStrings();
            HandleInput();
        }


        // Called by GameScene.Draw()
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(isOpen)
            {
                spriteBatch.Begin();

                panelRectangle.Height = panelHeightOpen;
                spriteBatch.Draw(background, panelRectangle, Color.White);

                spriteBatch.DrawString(font, heading, headingPosition, Color.White);
                spriteBatch.DrawString(font, toggleClosed, togglePosition, Color.White);

                Vector2 itemOrigin = new Vector2(panelRectangle.Left + 10, headingPosition.Y + 20);
                Color fontColor = Color.White;
                for (int x = 0; x < itemDescriptions.Length; x++)
                {
                    if (itemSelected == (x + 1) && GetPlayerItem(itemSelected) != null)
                    {
                        fontColor = Color.Red;
                    }

                    string line = labels[x] + itemDescriptions[x];
                    spriteBatch.DrawString(font, line, itemOrigin, fontColor);
                    fontColor = Color.White;
                    itemOrigin.Y += 20;
                }

                if (itemSelected != 0 && itemSelected <= maxItems && GetPlayerItem(itemSelected) != null)
                {
                    spriteBatch.DrawString(font, dropHint, hintPosition, fontColor);
                    spriteBatch.DrawString(font, cancelHint,
                        new Vector2(hintPosition.X, hintPosition.Y + 15), fontColor);
                }
                else
                {
                    spriteBatch.DrawString(font, selectHint, hintPosition, fontColor);
                }

                spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin();

                panelRectangle.Height = panelHeightClosed;
                spriteBatch.Draw(background, panelRectangle, Color.White);

                spriteBatch.DrawString(font, heading, headingPosition, Color.White);
                spriteBatch.DrawString(font, toggleOpen, togglePosition, Color.White);

                spriteBatch.End();
            }
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
    }
}
