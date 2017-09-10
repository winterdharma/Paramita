﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        protected Rectangle _rectangle;
        private Point _mousePosition;
        private bool _mouseOver;
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
        public int DrawOrder { get; set; }
        public Vector2 Position { get; set; }
        public Color Color { get; set; }
        public Color HighlightedColor { get; set; }
        public Color UnhighlightedColor { get; set; }
        public Rectangle Rectangle { get => _rectangle; set => _rectangle = value; }
        public Component Parent { get; set; }

        public event EventHandler<EventArgs> LeftClick;
        public event EventHandler<EventArgs> RightClick;
        public event EventHandler<EventArgs> DoubleClick;
        public event EventHandler<EventArgs> MouseOver;
        public event EventHandler<EventArgs> MouseGone;
        public event EventHandler<EventArgs> ScrollWheelChange;


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

        public void Highlight()
        {
            Color = HighlightedColor;
        }

        public void Unhighlight()
        {
            Color = UnhighlightedColor;
        }

        public virtual void SubscribeToEvents()
        {
            Parent.Input.LeftMouseClick += OnLeftClick;
            Parent.Input.NewMousePosition += OnMouseMoved;
            Parent.Input.RightMouseClick += OnRightClick;
            Parent.Input.DoubleLeftMouseClick += OnDoubleClick;
            Parent.Input.ScrollWheelMoved += OnScrollWheelMove;
        }

        public virtual void UnsubscribeFromEvents()
        {
            Parent.Input.NewMousePosition -= OnMouseMoved;
            Parent.Input.LeftMouseClick -= OnLeftClick;
            Parent.Input.RightMouseClick -= OnRightClick;
            Parent.Input.DoubleLeftMouseClick -= OnDoubleClick;
            Parent.Input.ScrollWheelMoved -= OnScrollWheelMove;
        }

        private void OnMouseMoved(object sender, PointEventArgs e)
        {
            _mousePosition = e.Point;

            if (!_mouseOver && Rectangle.Contains(_mousePosition))
            {
                _mouseOver = true;
                MouseOver?.Invoke(this, new EventArgs());

            }
            else if (_mouseOver && !Rectangle.Contains(_mousePosition))
            {
                _mouseOver = false;
                MouseGone?.Invoke(this, new EventArgs());
            }
        }

        private void OnLeftClick(object sender, EventArgs e)
        {
            if (_mouseOver)
                LeftClick?.Invoke(this, e);
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            if (_mouseOver)
                DoubleClick?.Invoke(this, e);
        }

        private void OnRightClick(object sender, EventArgs e)
        {
            if (_mouseOver)
                RightClick?.Invoke(this, e);
        }

        private void OnScrollWheelMove(object sender, IntegerEventArgs e)
        {
            if (_mouseOver)
                ScrollWheelChange?.Invoke(this, e);
        }
    }
}
