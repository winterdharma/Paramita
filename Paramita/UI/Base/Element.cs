using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.GameLogic.Utility;

namespace Paramita.UI.Base
{
    /// <summary>
    /// An Element handles the logic and content for Update() and Draw() on a discrete part of a 
    /// UI Component.
    /// </summary>
    public abstract class Element : IDrawable
    {
        #region Fields
        protected Rectangle _rectangle;
        private bool _enabled;
        #endregion

        #region Events
        public event EventHandler LeftClick;
        public event EventHandler RightClick;
        public event EventHandler DoubleClick;
        public event EventHandler MouseOver;
        public event EventHandler MouseGone;
        public event EventHandler<IntegerEventArgs> ScrollWheelChange;
        #endregion

        #region Constructors
        public Element(string id, Component parent, Vector2 position, Color unhighlighted, 
            Color highlighted, int drawOrder)
        {
            Id = id;
            Parent = parent;
            Position = position;
            Color = unhighlighted;
            UnhighlightedColor = unhighlighted;
            HighlightedColor = highlighted;
            Visible = false;
            Enabled = false;
            IsMouseOver = false;
        }
        #endregion

        #region Properties
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
        public int DrawOrder { get; set; }
        public bool IsMouseOver { get; private set; }
        public Vector2 Position { get; set; }
        public Color Color { get; set; }
        public Color HighlightedColor { get; set; }
        public Color UnhighlightedColor { get; set; }
        public Rectangle Rectangle { get => _rectangle; set => _rectangle = value; }
        public Component Parent { get; set; }
        #endregion

        #region Initialization
        protected abstract Rectangle CreateRectangle();

        protected virtual void SubscribeToEvents()
        {
            Parent.Input.LeftMouseClick += OnLeftClick;
            Parent.Input.NewMousePosition += OnMouseMoved;
            Parent.Input.RightMouseClick += OnRightClick;
            Parent.Input.DoubleLeftMouseClick += OnDoubleClick;
            Parent.Input.ScrollWheelMoved += OnScrollWheelMove;
        }

        protected virtual void UnsubscribeFromEvents()
        {
            Parent.Input.NewMousePosition -= OnMouseMoved;
            Parent.Input.LeftMouseClick -= OnLeftClick;
            Parent.Input.RightMouseClick -= OnRightClick;
            Parent.Input.DoubleLeftMouseClick -= OnDoubleClick;
            Parent.Input.ScrollWheelMoved -= OnScrollWheelMove;
        }
        #endregion

        #region Event Handling
        private void OnMouseMoved(object sender, PointEventArgs e)
        {
            var mousePosition = e.Point;

            if (!IsMouseOver && Rectangle.Contains(mousePosition))
            {
                IsMouseOver = true;
                MouseOver?.Invoke(this, new EventArgs());
            }
            else if (IsMouseOver && !Rectangle.Contains(mousePosition))
            {
                IsMouseOver = false;
                MouseGone?.Invoke(this, new EventArgs());
            }
        }

        private void OnLeftClick(object sender, EventArgs e)
        {
            if (IsMouseOver)
                LeftClick?.Invoke(this, e);
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            if (IsMouseOver)
                DoubleClick?.Invoke(this, e);
        }

        private void OnRightClick(object sender, EventArgs e)
        {
            if (IsMouseOver)
                RightClick?.Invoke(this, e);
        }

        private void OnScrollWheelMove(object sender, IntegerEventArgs e)
        {
            if (IsMouseOver)
                ScrollWheelChange?.Invoke(this, e);
        }
        #endregion

        public virtual void Update(GameTime gameTime)
        {

        }

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

        public virtual void Highlight()
        {
            Color = HighlightedColor;
        }

        public virtual void Unhighlight()
        {
            Color = UnhighlightedColor;
        }
    }
}
