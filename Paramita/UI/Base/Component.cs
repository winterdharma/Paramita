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
        #region Fields
        protected Dictionary<string, Element> _elements = new Dictionary<string, Element>();
        protected List<Element> _visibleElements = new List<Element>();
        protected List<Element> _enabledElements = new List<Element>();
        protected Rectangle _panelRectangle = new Rectangle();
        protected Rectangle _parentRectangle = new Rectangle();
        protected bool _enabled;
        #endregion

        #region Events
        public event EventHandler<RectangleEventArgs> RectangleUpdated;
        public event EventHandler<ElementEventArgs> ElementLeftClicked;
        public event EventHandler<ElementEventArgs> ElementRightClicked;
        public event EventHandler<ElementEventArgs> ElementDoubleClicked;
        public event EventHandler<ElementEventArgs> ElementMousedOver;
        public event EventHandler<ElementEventArgs> ElementMouseGone;
        public event EventHandler<ElementEventArgs> ElementScrollWheelMoved;
        #endregion

        #region Constructors
        public Component(Scene parent, int drawOrder)
        {
            Parent = parent;
            _parentRectangle = parent.ScreenRectangle;
            Input = parent.Input;
            DrawOrder = drawOrder;
            Initialize();
        }
        #endregion

        #region Properties
        public Scene Parent { get; set; }
        public InputResponder Input { get; private set; }
        public Rectangle Rectangle
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
        public bool Visible { get; internal set; }
        public bool IsMouseOver { get; private set; }
        public int DrawOrder { get; set; }
        #endregion

        #region Initialization
        protected virtual void Initialize()
        {
            Rectangle = UpdatePanelRectangle();
            Elements = InitializeElements();
        }

        protected abstract Rectangle UpdatePanelRectangle();
        protected abstract Dictionary<string, Element> InitializeElements();
        #endregion

        #region Event Handling
        protected void SubscribeToEvents()
        {
            foreach(var element in _enabledElements)
            {
                element.LeftClick += OnElementLeftClicked;
                element.RightClick += OnElementRightClicked;
                element.DoubleClick += OnElementDoubleClicked;
                element.MouseOver += OnElementMousedOver;
                element.MouseGone += OnElementMouseGone;
                element.ScrollWheelChange += OnElementScrollWheelMoved;
            }
        }

        protected void UnsubscribeFromEvents()
        {
            foreach (var element in _enabledElements)
            {
                element.LeftClick -= OnElementLeftClicked;
                element.RightClick -= OnElementRightClicked;
                element.DoubleClick -= OnElementDoubleClicked;
                element.MouseOver -= OnElementMousedOver;
                element.MouseGone -= OnElementMouseGone;
                element.ScrollWheelChange -= OnElementScrollWheelMoved;
            }
        }
        
        protected virtual void OnElementLeftClicked(object sender, EventArgs e)
        {
            var element = sender as Element;

            if (Parent.IsDrawableOnTopDrawLayer(element, _visibleElements.ToList<IDrawable>()))
                ElementLeftClicked?.Invoke(this, new ElementEventArgs(element));
        }

        protected virtual void OnElementRightClicked(object sender, EventArgs e)
        {
            var element = sender as Element;

            if (Parent.IsDrawableOnTopDrawLayer(element, _visibleElements.ToList<IDrawable>()))
                ElementRightClicked?.Invoke(this, new ElementEventArgs(element));
        }

        protected virtual void OnElementDoubleClicked(object sender, EventArgs e)
        {
            var element = sender as Element;

            if (Parent.IsDrawableOnTopDrawLayer(element, _visibleElements.ToList<IDrawable>()))
                ElementDoubleClicked?.Invoke(this, new ElementEventArgs(element));
        }

        protected virtual void OnElementMousedOver(object sender, EventArgs e)
        {
            IsMouseOver = true;
            var element = sender as Element;

            if (Parent.IsDrawableOnTopDrawLayer(element, _visibleElements.ToList<IDrawable>()))
                ElementMousedOver?.Invoke(this, new ElementEventArgs(element));
        }

        protected virtual void OnElementMouseGone(object sender, EventArgs e)
        {
            if(!Rectangle.Contains(Input.CurrentMousePosition))
                IsMouseOver = false;

            var element = sender as Element;

            if (Parent.IsDrawableOnTopDrawLayer(element, _visibleElements.ToList<IDrawable>()))
                ElementMouseGone?.Invoke(this, new ElementEventArgs(element));
        }

        protected virtual void OnElementScrollWheelMoved(object sender, IntegerEventArgs e)
        {
            var element = sender as Element;

            if (Parent.IsDrawableOnTopDrawLayer(element, _visibleElements.ToList<IDrawable>()))
                ElementScrollWheelMoved?.Invoke(this, new ElementEventArgs(element, e.Value));
        }
        #endregion

        public virtual void Show()
        {
            Visible = true;
            Enabled = true;

            Elements.Values.ToList().ForEach(e => e.Show());
        }

        public virtual void Hide()
        {
            Visible = false;
            Enabled = false;

            Elements.Values.ToList().ForEach(e => e.Hide());
        }

        public virtual void Update(GameTime gameTime)
        {
            UpdateEnabledAndVisibleElements();

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

        #region Helper Methods
        protected void UpdateEnabledAndVisibleElements()
        {
            _enabledElements = GetEnabledElements();
            _visibleElements = GetVisibleElements();
        }

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
