using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Paramita.UI.Input;
using System;
using System.Collections.Generic;
using System.Linq;

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
        protected ContentManager _content;
        private List<Component> _components;
        #endregion

        #region Events
        public event EventHandler<UserInputEventArgs> UserInputEvent; 
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
        }
        #endregion

        #region Properties
        public GameController Game { get; set; }
        public List<Component> Components
        {
            get { return _components; }
            protected set { _components = value; }
        }
        public List<UserAction> UserActions { get; protected set; }
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
        }

        protected abstract List<UserAction> InitializeUserActions(List<Component> components);
        #endregion

        #region Event Handling
        protected virtual void SubscribeToEvents()
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
            if (IsDrawableOnTopDrawLayer((Component)sender, Components.ToList<IDrawable>()))
                UserInputEvent?.Invoke(this, new UserInputEventArgs(
                    new InputSource(e.Element), 
                    EventType.LeftClick)
                    );

        }

        private void OnElementRightClicked(object sender, ElementEventArgs e)
        {
        }

        private void OnElementDoubleClicked(object sender, ElementEventArgs e)
        {
        }

        private void OnElementMousedOver(object sender, ElementEventArgs e)
        {
        }

        private void OnElementMouseGone(object sender, ElementEventArgs e)
        {
        }

        private void OnElementScrollWheelMoved(object sender, ElementEventArgs e)
        {
        }

        protected abstract void UnsubscribeFromKeyboardEvents();

        public bool IsDrawableOnTopDrawLayer(IDrawable drawable, List<IDrawable> visibleDrawables)
        {
            Point mousePosition;
            // if IDrawable has raised MouseGone event, use the previous Input.MousePosition
            if (drawable.IsMouseOver)
                mousePosition = Input.CurrentMousePosition;
            else
                mousePosition = Input.PreviousMousePosition;

            var drawablesMousedOver = visibleDrawables.FindAll(d => d.Rectangle.Contains(mousePosition));

            if (drawablesMousedOver.Count == 0)
                throw new Exception("No elements were under the mouse.");
            else if (drawablesMousedOver.Count == 1)
                return true;
            else
            {
                var max = drawablesMousedOver.Max(e => e.DrawOrder);
                return drawable.DrawOrder == max;
            }
        }
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

        /// <summary>
        /// Adds supplied Component objects to the Scene's List of Components and sorts the list by DrawOrder.
        /// </summary>
        /// <param name="components"></param>
        protected List<Component> InitializeComponents(params Component[] components)
        {
            var componentList = new List<Component>();
            componentList.AddRange(components);
            componentList.Sort(new Comparison<Component>(CompareDrawOrders));
            return componentList;
        }
        #endregion



    }
}
