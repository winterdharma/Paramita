using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.UI.Input;
using System;
using Paramita.GameLogic.Utility;
using Paramita.UI.Base;

namespace Paramita.UI.Elements
{
    /// <summary>
    /// Handles the content and display logic for a discrete image drawn to the screen.
    /// </summary>
    public class Image : Element
    {
        private bool _mouseOver = false;
        private Point _mousePosition;

        public event EventHandler<EventArgs> LeftClicked;
        public event EventHandler<EventArgs> MouseOver;
        public event EventHandler<EventArgs> MouseGone;

        public Image(string id, Component parent, Vector2 position, Texture2D texture, 
            Color normal, Color highlighted, int drawOrder, float scale = 1) 
            : base(id, parent, position, normal, highlighted, drawOrder)
        {
            Id = id;
            Texture = texture;
            Visible = true;
            Enabled = true;
            Scale = scale;
            Rectangle = CreateRectangle();
        }

        public Texture2D Texture { get; set; }
        public float Scale { get; set; }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Visible)
                spriteBatch.Draw(Texture, Position, null, Color, 0f, new Vector2(0,0), Scale, SpriteEffects.None, 0f);
        }

        public override void Update(GameTime gameTime)
        {
            if(Enabled)
            {
            }
        }

        protected override Rectangle CreateRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, 
                (int)(Texture.Bounds.Width * Scale), (int)(Texture.Bounds.Height * Scale));
        }

        #region Events
        public override void SubscribeToEvents()
        {
            Parent.Input.LeftMouseClick += OnMouseClicked;
            Parent.Input.NewMousePosition += OnMouseMoved;
        }

        public override void UnsubscribeFromEvents()
        {
            Parent.Input.LeftMouseClick -= OnMouseClicked;
            Parent.Input.NewMousePosition -= OnMouseMoved;
        }

        private void OnMouseMoved(object sender, PointEventArgs e)
        {
            _mousePosition = e.Point;

            if (Rectangle.Contains(_mousePosition) && !_mouseOver)
            {
                _mouseOver = true;
                MouseOver?.Invoke(this, new EventArgs());

            }
            else if (!Rectangle.Contains(_mousePosition) && _mouseOver)
            {
                _mouseOver = false;
                MouseGone?.Invoke(this, new EventArgs());
            }
        }

        private void OnMouseClicked(object sender, EventArgs e)
        {
            if (Rectangle.Contains(_mousePosition))
                LeftClicked?.Invoke(this, new EventArgs());
        }
        #endregion
    }
}
