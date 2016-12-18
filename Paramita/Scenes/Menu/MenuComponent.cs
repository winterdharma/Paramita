using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Paramita.Components;
using System.Collections.Generic;

namespace Paramita.Scenes
{
    public class MenuComponent
    {
        SpriteFont spriteFont;
        readonly List<string> menuItems = new List<string>();
        int selectedIndex = -1;
        bool mouseOver;
        int width;
        int height;
        Color normalColor = Color.White;
        Color hiliteColor = Color.Red;
        Texture2D texture;
        Vector2 position;


        public Vector2 Postion
        {
            get { return position; }
            set { position = value; }
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = (int)MathHelper.Clamp(
                value,
                0,
                menuItems.Count - 1);
            }
        }

        public Color NormalColor
        {
            get { return normalColor; }
            set { normalColor = value; }
        }

        public Color HiliteColor
        {
            get { return hiliteColor; }
            set { hiliteColor = value; }
        }

        public bool MouseOver
        {
            get { return mouseOver; }
        }



        public MenuComponent(SpriteFont spriteFont, Texture2D texture)
        {
            mouseOver = false;
            this.spriteFont = spriteFont;
            this.texture = texture;
        }


        public MenuComponent(SpriteFont spriteFont, Texture2D texture, string[] menuItems)
        : this(spriteFont, texture)
        {
            selectedIndex = 0;
            foreach (string s in menuItems)
            {
                this.menuItems.Add(s);
            }
            MeasureMenu();
        }


        public void SetMenuItems(string[] items)
        {
            menuItems.Clear();
            menuItems.AddRange(items);
            MeasureMenu();
            selectedIndex = 0;
        }


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


        public void Update(GameTime gameTime)
        {
            Vector2 menuPosition = position;
            Point p = InputDevices.MouseState.Position;
            Rectangle buttonRect;
            mouseOver = false;

            for (int i = 0; i < menuItems.Count; i++)
            {
                buttonRect = new Rectangle((int)menuPosition.X, (int)menuPosition.Y,
                texture.Width, texture.Height);
                if (buttonRect.Contains(p))
                {
                    selectedIndex = i;
                    mouseOver = true;
                }
                menuPosition.Y += texture.Height + 50;
            }
            if (!mouseOver && (InputDevices.CheckKeyReleased(Keys.Up)))
            {
                selectedIndex--;
                if (selectedIndex < 0)
                    selectedIndex = menuItems.Count - 1;
            }
            else if (!mouseOver && (InputDevices.CheckKeyReleased(Keys.Down)))
            {
                selectedIndex++;
                if (selectedIndex > menuItems.Count - 1)
                    selectedIndex = 0;
            }
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 menuPosition = position;
            Color myColor;
            for (int i = 0; i < menuItems.Count; i++)
            {
                if (i == SelectedIndex)
                    myColor = HiliteColor;
                else
                    myColor = NormalColor;
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
