using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.UI.Input;
using System;
using System.Collections.Generic;

namespace Paramita.Scenes
{
    public class MenuComponent //: Scene
    {
        SpriteFont spriteFont;
        readonly List<string> menuItems = new List<string>();
        Rectangle[] menuItemRects;
        int selectedIndex = 0;
        bool mouseOver;
        int width;
        int height;
        Color normalColor = Color.White;
        Color hiliteColor = Color.Red;
        Texture2D texture;
        Vector2 _menuPosition;


        public int SelectedIndex
        {
            get { return selectedIndex; }
        }


        public bool MouseOver
        {
            get { return mouseOver; }
        }



        public MenuComponent(GameController game, SpriteFont spriteFont, Texture2D texture, string[] menuItems, Vector2 position)
        //: base(game)
        {
            mouseOver = false;
            this.spriteFont = spriteFont;
            this.texture = texture;
            _menuPosition = position;

            InputDevices.OnUpKeyWasPressed += HandleUpInput;
            InputDevices.OnDownKeyWasPressed += HandleDownInput;
            InputDevices.OnMousePositionChanged += HandleMouseMove;

            // create an array of rectangles that are used to detect which button is selected with a mouse
            menuItemRects = new Rectangle[menuItems.Length];
            Vector2 menuItemPos = position;
            for (int x = 0; x < menuItems.Length; x++)
            {
                this.menuItems.Add(menuItems[x]);
                menuItemRects[x] = new Rectangle((int)menuItemPos.X, (int)menuItemPos.Y,
                texture.Width, texture.Height);

                menuItemPos.Y += texture.Height + 50; // adjust for the next button rectangle
            }

            MeasureMenu();
        }



        // Sets the component's width and height fields and checks that the width will be large enough
        // to accomodate the strings that are in menuItems.
        private void MeasureMenu()
        {
            width = texture.Width;
            height = 0;
            foreach (string s in menuItems)
            {
                Vector2 size = spriteFont.MeasureString(s);
                if (size.X > width)
                    width = (int)size.X;
                height += texture.Height + 50;
            }
            height -= 50;
        }



        //public override void Update(GameTime gameTime)
        //{
            
        //}


        private void HandleMouseMove(object sender, MouseEventArgs e)
        {
            var mousePosition = e.Position;
            mouseOver = false;

            for (int i = 0; i < menuItems.Count; i++)
            {
                if (menuItemRects[i].Contains(mousePosition))
                {
                    selectedIndex = i;
                    mouseOver = true;
                }
            }
        }

        private void HandleUpInput(object sender, EventArgs e)
        {
            selectedIndex--;
            if (selectedIndex < 0)
                selectedIndex = menuItems.Count - 1;
        }

        private void HandleDownInput(object sender, EventArgs e)
        {
            selectedIndex++;
            if (selectedIndex > menuItems.Count - 1)
                selectedIndex = 0;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 menuPosition = _menuPosition;
            Color myColor;
            for (int i = 0; i < menuItems.Count; i++)
            {
                if (i == selectedIndex)
                    myColor = hiliteColor;
                else
                    myColor = normalColor;

                spriteBatch.Draw(texture, menuPosition, Color.White);

                Vector2 textSize = spriteFont.MeasureString(menuItems[i]);
                Vector2 textPosition = menuPosition + new Vector2((int)(texture.Width -
                    textSize.X) / 2, (int)(texture.Height - textSize.Y) / 2);

                spriteBatch.DrawString(spriteFont,
                    menuItems[i],
                    textPosition,
                    myColor);
                menuPosition.Y += texture.Height + 50;
            }
        }
    }
}
