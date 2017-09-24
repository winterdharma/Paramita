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
        protected bool _visible;
        #endregion

        #region Events
        public event EventHandler<RectangleEventArgs> RectangleUpdated;
        public event EventHandler EnabledChanged;
        public event EventHandler VisibleChanged;
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
                if (value != _enabled)
                {
                    _enabled = value;
                    EnabledChanged?.Invoke(this, new EventArgs());
                }
            }
        }
        public bool Visible
        {
            get => _visible;
            set
            {
                if (value != _visible)
                {
                    _visible = value;
                    VisibleChanged?.Invoke(this, new EventArgs());
                }
            }
        }
        public bool IsMouseOver { get; private set; }
        public int DrawOrder { get; set; }
        #endregion

        #region Initialization
        protected virtual void Initialize()
        {
            Rectangle = UpdatePanelRectangle();
            Elements = InitializeElements();
            SubscribeToElementPropertyEvents();
        }

        protected abstract Rectangle UpdatePanelRectangle();
        protected abstract Dictionary<string, Element> InitializeElements();
        #endregion

        #region Event Handling
        protected virtual void OnElementEnabledChange(object sender, EventArgs e)
        {
            var element = (Element)sender;
            if(element.Enabled)
            {
                _enabledElements.Add(element);
                SubscribeToElementEvents(element);
            }
            else
            {
                _enabledElements.Remove(element);
                UnsubscribeFromElementEvents(element);
            }
        }

        protected void OnElementVisibleChange(object sender, EventArgs e)
        {
            var element = (Element)sender;
            if (element.Visible)
            {
                _visibleElements.Add(element);
                _visibleElements.Sort(new Comparison<Element>(Parent.CompareDrawOrders));
            }
            else
            {
                _visibleElements.Remove(element);
                _visibleElements.Sort(new Comparison<Element>(Parent.CompareDrawOrders));
            }
        }

        private void SubscribeToElementEvents(Element element)
        {
            element.LeftClick += OnElementLeftClicked;
            element.RightClick += OnElementRightClicked;
            element.DoubleClick += OnElementDoubleClicked;
            element.MouseOver += OnElementMousedOver;
            element.MouseGone += OnElementMouseGone;
            element.ScrollWheelChange += OnElementScrollWheelMoved;
        }

        private void UnsubscribeFromElementEvents(Element element)
        {
            element.LeftClick -= OnElementLeftClicked;
            element.RightClick -= OnElementRightClicked;
            element.DoubleClick -= OnElementDoubleClicked;
            element.MouseOver -= OnElementMousedOver;
            element.MouseGone -= OnElementMouseGone;
            element.ScrollWheelChange -= OnElementScrollWheelMoved;
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
        private void SubscribeToElementPropertyEvents()
        {
            SubscribeToElementEnabledEvents();
            SubscribeToElementVisibleEvents();
        }

        private void SubscribeToElementEnabledEvents()
        {
            Elements.Values.ToList().ForEach(e => e.EnabledChanged += OnElementEnabledChange);
        }

        private void SubscribeToElementVisibleEvents()
        {
            Elements.Values.ToList().ForEach(e => e.VisibleChanged += OnElementVisibleChange);
        }
        #endregion
    }
}
