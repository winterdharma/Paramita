using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Items;
using Paramita.Mechanics;
using Paramita.SentientBeings;

namespace Paramita.Scenes.Game
{
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
        private int panelHeight = 300;
        private string heading = "Inventory";
        private Vector2 headingPosition;

        private InputDevices input;
        private Player player;
        private Texture2D background;
        private SpriteFont font;

        

        public Inventory(SpriteFont font, Rectangle parentScreen, InputDevices inputDevices, Player player, Texture2D background, int maxItems)
        {
            this.font = font;
            input = inputDevices;
            this.player = player;
            this.background = background;
            this.maxItems = maxItems;
            labels = CreateItemLabels(maxItems);
            itemDescriptions = GetPlayerItemStrings();
            panelOrigin = new Point(parentScreen.Width - (panelWidth + 10), 10);
            panelRectangle = new Rectangle(panelOrigin.X, 
                panelOrigin.Y, panelWidth, panelHeight);

            Vector2 headingSize = font.MeasureString(heading);
            headingPosition = new Vector2( 
                panelRectangle.Left + ((panelWidth / 2) - (headingSize.X / 2)), 
                (panelRectangle.Top + 10) );
        }



        // Generates the item labels for the inventory list given the size of the inventory
        private string[] CreateItemLabels(int numberOfLabels)
        {
            string[] labels = new string[numberOfLabels];
            for(int x = 0; x < numberOfLabels; x++)
            {
                labels[x] = (x + 1).ToString() + ": ";
            }
            return labels;
        }



        // Gets the player's Item list and converts it to a string[] for display on the panel
        private string[] GetPlayerItemStrings()
        {
            Item[] playerItems = player.Items;
            string[] itemStrings = new string[playerItems.Length];

            for (int x = 0; x < playerItems.Length; x++)
            {
                if(playerItems[x] != null)
                {
                    itemStrings[x] = playerItems[x].ToString();
                }
                else
                {
                    itemStrings[x] = "";
                }
            }

            return itemStrings;
        }

        public void Update(GameTime gameTime)
        {
            itemDescriptions = GetPlayerItemStrings();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            
            spriteBatch.Draw(background, panelRectangle, Color.White);

            spriteBatch.DrawString(font, heading, headingPosition, Color.White);

            Vector2 itemOrigin = new Vector2(panelRectangle.Left + 10, headingPosition.Y + 20);
            for (int x = 0; x < itemDescriptions.Length; x++)
            {
                string line = labels[x] + itemDescriptions[x];
                spriteBatch.DrawString(font, line, itemOrigin, Color.White);
                itemOrigin.Y += 20;
            }
            spriteBatch.End();
        }
    }
}
