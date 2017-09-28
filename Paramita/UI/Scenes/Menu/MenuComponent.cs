using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.GameLogic.Utility;
using Paramita.UI.Input;
using System;
using System.Collections.Generic;

namespace Paramita.UI.Scenes
{
    public class MenuComponent
    {
        SpriteFont _spriteFont;
        readonly List<string> _menuItems;
        Rectangle[] _menuItemRectangles;
        int _selectedIndex;
        bool _mouseOver;
        int _menuWidth;
        int _menuHeight;
        Color normalColor = Color.White;
        Color hiliteColor = Color.Red;
        Texture2D _texture;
        Vector2 _menuPosition;


        public int SelectedIndex
        {
            get { return _selectedIndex; }
        }


        public bool MouseOver
        {
            get { return _mouseOver; }
        }

        public MenuComponent(SpriteFont spriteFont, Texture2D texture, string[] menuItems, Vector2 position,
            InputResponder input)
        {
            _spriteFont = spriteFont;
            _texture = texture;
            _menuPosition = position;
            _menuItems = new List<string>(menuItems);

            InitializeMenuItemRectangles();

            SubscribeToInputEvents(input);

            MeasureMenu();
        }

        #region Event Handling
        private void SubscribeToInputEvents(InputResponder input)
        {
            input.UpKeyPressed += HandleUpInput;
            input.DownKeyPressed += HandleDownInput;
            input.NewMousePosition += HandleMouseMove;
        }

        private void HandleMouseMove(object sender, PointEventArgs e)
        {
            var mousePosition = e.Point;
            _mouseOver = false;

            for (int i = 0; i < _menuItems.Count; i++)
            {
                if (_menuItemRectangles[i].Contains(mousePosition))
                {
                    _selectedIndex = i;
                    _mouseOver = true;
                }
            }
        }

        private void HandleUpInput(object sender, EventArgs e)
        {
            _selectedIndex--;
            if (_selectedIndex < 0)
                _selectedIndex = _menuItems.Count - 1;
        }

        private void HandleDownInput(object sender, EventArgs e)
        {
            _selectedIndex++;
            if (_selectedIndex > _menuItems.Count - 1)
                _selectedIndex = 0;
        }
        #endregion

        private void InitializeMenuItemRectangles()
        {
            _menuItemRectangles = new Rectangle[_menuItems.Count];
            Vector2 menuItemPos = _menuPosition;
            for (int x = 0; x < _menuItems.Count; x++)
            {
                _menuItemRectangles[x] = new Rectangle((int)menuItemPos.X, (int)menuItemPos.Y,
                _texture.Width, _texture.Height);

                menuItemPos.Y += _texture.Height + 50; // adjust for the next button rectangle
            }
        }


        // Sets the component's width and height fields and checks that the width will be large enough
        // to accomodate the strings that are in menuItems.
        private void MeasureMenu()
        {
            _menuWidth = _texture.Width;
            _menuHeight = 0;
            foreach (string str in _menuItems)
            {
                Vector2 size = _spriteFont.MeasureString(str);
                if (size.X > _menuWidth)
                    _menuWidth = (int)size.X;
                _menuHeight += _texture.Height + 50;
            }
            _menuHeight -= 50;
        }


        

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 menuPosition = _menuPosition;
            Color myColor;
            for (int i = 0; i < _menuItems.Count; i++)
            {
                if (i == _selectedIndex)
                    myColor = hiliteColor;
                else
                    myColor = normalColor;

                spriteBatch.Draw(_texture, menuPosition, Color.White);

                Vector2 textSize = _spriteFont.MeasureString(_menuItems[i]);
                Vector2 textPosition = menuPosition + new Vector2((int)(_texture.Width -
                    textSize.X) / 2, (int)(_texture.Height - textSize.Y) / 2);

                spriteBatch.DrawString(_spriteFont,
                    _menuItems[i],
                    textPosition,
                    myColor);
                menuPosition.Y += _texture.Height + 50;
            }
        }
    }
}
