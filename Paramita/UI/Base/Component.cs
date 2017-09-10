using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.GameLogic.Utility;
using Paramita.UI.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Paramita.UI.Base
{
    /// <summary>An collection of several elements that combine to form a single component of a 
    /// Scene.</summary>
    public abstract class Component : IDrawable
    {
        protected Dictionary<string, Element> _elements = new Dictionary<string, Element>();
        protected List<Element> _visibleElements = new List<Element>();
        protected List<Element> _enabledElements = new List<Element>();
        protected Rectangle _panelRectangle = new Rectangle();

        public event EventHandler<RectangleEventArgs> RectangleUpdated;

        public Component(Scene parent, int drawOrder)
        {
            Parent = parent;
            Input = parent.Input;
            DrawOrder = drawOrder;
        }

        public Scene Parent { get; set; }
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
        public Dictionary<string, Element> Elements
        {
            get { return _elements; }
            set
            {
                _elements = value;              
            }
        }
        public bool Enabled { get; internal set; }
        public bool Visible { get; internal set; }
        public int DrawOrder { get; set; }

        public virtual void Update(GameTime gameTime)
        {
            _visibleElements = GetVisibleElements();
            _enabledElements = GetEnabledElements();

            foreach (var element in _enabledElements)
            {
                element.Update(gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (var element in _visibleElements)
            {
                element.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }

        #region Private Helper Methods
        private List<Element> GetEnabledElements()
        {
            var enabledElements = new List<Element>();
            enabledElements = _elements.Values.ToList();
            enabledElements.RemoveAll(e => !e.Enabled);
            return enabledElements;
        }

        private List<Element> GetVisibleElements()
        {
            var visibleElements = new List<Element>();
            visibleElements = _elements.Values.ToList();
            visibleElements.RemoveAll(e => !e.Visible);
            visibleElements.Sort(new Comparison<Element>(Parent.CompareDrawOrders));
            return visibleElements;
        }
        #endregion
    }
}
