using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Paramita.UI.Input;
using System;
using System.Collections.Generic;

namespace Paramita.UI.Base
{
    /*
     * This is the base class for Scenes of the App. 
     * The SceneManager handles transitions from one scene to another. 
     * The GameController instantiates scene objects.
     * 
     * This class provides a Random number generator, 
     * a reference to the GameController, 
     * a reference to the SceneManager, and
     * a reference to the ContentManager (inherited from DrawableGameComponent)
     * which allows scenes to load Texture2D assets, etc.
     * 
     * It provides some generic methods anticipating scenes with child components.
     */

    public abstract partial class Scene
    {
        #region Fields
        protected SpriteBatch _spriteBatch;
        protected readonly SceneManager _manager;
        protected ContentManager _content;
        private List<Component> _components;
        #endregion

        #region Constructors
        public Scene(GameController game)
        {
            Game = game;
            _spriteBatch = game.SpriteBatch;
            ScreenRectangle = game.ScreenRectangle;
            Input = game.InputResponder;
            _components = new List<Component>();
            _content = Game.Content;
            _manager = (SceneManager)Game.Services.GetService(typeof(SceneManager));
        }
        #endregion

        #region Properties
        public GameController Game { get; set; }
        public List<Component> Components { get { return _components; } }
        public InputResponder Input { get; set; }
        public int DrawOrder { get; set; }
        public Rectangle ScreenRectangle { get; set; }
        public bool Enabled { get; set; }
        public bool Visible { get; set; }
        #endregion

        #region Initialization
        public virtual void Initialize()
        {
            LoadContent();
            SubscribeToEvents();
        }
        #endregion

        #region Event Handling
        private void SubscribeToEvents()
        {
            SubscribeToComponentMouseEvents();
            SubscribeToKeyboardEvents();
        }

        protected virtual void SubscribeToComponentMouseEvents()
        {
            foreach(var component in Components)
            {
                component.ElementLeftClicked += OnElementLeftClicked;
                component.ElementRightClicked += OnElementRightClicked;
                component.ElementDoubleClicked += OnElementDoubleClicked;
                component.ElementMousedOver += OnElementMousedOver;
                component.ElementMouseGone += OnElementMouseGone;
                component.ElementScrollWheelMoved += OnElementScrollWheelMoved;
            }
        }

        protected abstract void SubscribeToKeyboardEvents();

        protected virtual void UnsubscribeFromComponentMouseEvents()
        {
            foreach (var component in Components)
            {
                component.ElementLeftClicked -= OnElementLeftClicked;
                component.ElementRightClicked -= OnElementRightClicked;
                component.ElementDoubleClicked -= OnElementDoubleClicked;
                component.ElementMousedOver -= OnElementMousedOver;
                component.ElementMouseGone -= OnElementMouseGone;
                component.ElementScrollWheelMoved -= OnElementScrollWheelMoved;
            }
        }

        private void OnElementLeftClicked(object sender, ElementEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnElementRightClicked(object sender, ElementEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnElementDoubleClicked(object sender, ElementEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnElementMousedOver(object sender, ElementEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnElementMouseGone(object sender, ElementEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnElementScrollWheelMoved(object sender, ElementEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected abstract void UnsubscribeFromKeyboardEvents();
        #endregion


        #region Public API
        public virtual void Show()
        {
            Enabled = true;
            Visible = true;

            foreach (var component in _components)
            {
                component.Show();
            }
        }

        public virtual void Hide()
        {
            Enabled = false;
            Visible = false;

            foreach (var component in _components)
            {
                component.Hide();
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var component in _components)
            { 
                if (component.Enabled)
                    component.Update(gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime)
        {
            Components.ForEach(c => c.Draw(gameTime, _spriteBatch));
        }

        /// <summary>
        /// Compares the DrawOrder values of two objects that implement the IDrawable interface.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int CompareDrawOrders(IDrawable x, IDrawable y)
        {
            if (x.DrawOrder > y.DrawOrder)
                return 1;
            else if (x.DrawOrder < y.DrawOrder)
                return -1;
            else
                return 0;
        }
        #endregion

        #region Protected Methods
        protected virtual void LoadContent()
        {
        }

        protected internal virtual void SceneChanged(object sender, EventArgs e)
        {
            if(_manager.CurrentScene == this) { Show(); }
            else { Hide(); }
        }

        /// <summary>
        /// Adds a Component object to the Scene's Components list and ensures the list is sorted
        /// by DrawOrder.
        /// </summary>
        /// <param name="component"></param>
        protected void AddComponent(Component component)
        {
            _components.Add(component);
            _components.Sort(new Comparison<Component>(CompareDrawOrders));
        }
        #endregion



    }
}
