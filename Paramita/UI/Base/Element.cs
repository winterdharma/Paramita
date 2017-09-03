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
        protected Rectangle _rectangle;
        protected Color _color;
        private bool _enabled;

        public string Id { get; set; }
        public bool Visible { get; set; }
        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                if (_enabled)
                    SubscribeToEvents();
                else
                    UnsubscribeFromEvents();
            }
        }
        public Vector2 Position { get; set; }
        public Rectangle Rectangle { get => _rectangle; set => _rectangle = value; }
        public Component Parent { get; set; }

        public Element(string id, Component parent, Vector2 position)
        {
            Id = id;
            Parent = parent;
            Position = position;
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

        public abstract void SubscribeToEvents();
        public abstract void UnsubscribeFromEvents();
    }
}
