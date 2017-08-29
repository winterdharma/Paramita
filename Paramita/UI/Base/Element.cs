using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Paramita.UI.Base
{
    /// <summary>
    /// An Element handles the logic and content for Update() and Draw() on a discrete part of a 
    /// UI Component.
    /// </summary>
    public abstract class Element
    {
        protected string _id;
        protected Component _parent;
        protected Vector2 _position;
        protected Rectangle _rectangle;
        protected Color _color;

        public bool Visible { get; set; }
        public bool Enabled { get; set; }
        public Rectangle Rectangle { get => _rectangle; set => _rectangle = value; }

        public Element(string id, Component parent, Vector2 position)
        {
            _id = id;
            _parent = parent;
            _position = position;
            Visible = false;
            Enabled = false;
        }

        protected abstract Rectangle CreateRectangle();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public virtual void Show()
        {
            Visible = true;
            Enabled = true;
        }

        public virtual void Hide()
        {
            Visible = false;
            Enabled = false;
        }
    }
}
