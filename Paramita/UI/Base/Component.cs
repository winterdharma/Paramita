using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.GameLogic.Utility;
using Paramita.UI.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita.UI.Base
{
    /// <summary>An collection of several elements that combine to form a single component of a 
    /// Scene.</summary>
    public abstract class Component
    {
        protected List<Element> _elements = new List<Element>();
        protected Rectangle _panelRectangle = new Rectangle();

        public event EventHandler<RectangleEventArgs> RectangleUpdated;

        public Component(InputResponder input)
        {
            Input = input;
        }

        public InputResponder Input { get; private set; }
        public Rectangle PanelRectangle
        {
            get => _panelRectangle;
            protected set
            {
                _panelRectangle = value;
                RectangleUpdated?.Invoke(this, new RectangleEventArgs(_panelRectangle));
            }
        }
        public List<Element> Elements
        {
            get { return _elements; }
            set { _elements = value; }
        }

        public bool Enabled { get; internal set; }
        public bool Visible { get; internal set; }

        public virtual void Update(GameTime gameTime)
        {
            foreach(var element in _elements)
            {
                element.Update(gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach(var element in _elements)
            {
                element.Draw(gameTime, spriteBatch);
            }
        }
    }
}
