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

    public abstract partial class Scene : DrawableGameComponent
    {
        protected GameController _game;
        protected SpriteBatch _spriteBatch;
        protected readonly SceneManager _manager;
        protected ContentManager _content;
        private List<Component> _components;
        
        public List<Component> Components { get { return _components; } }
        public InputResponder Input { get; set; }
        public Rectangle ScreenRectangle { get; set; }


        public Scene(GameController game) : base(game)
        {
            _game = game;
            _spriteBatch = game.SpriteBatch;
            ScreenRectangle = game.ScreenRectangle;
            Input = game.InputResponder;
            _components = new List<Component>();
            _content = Game.Content;
            _manager = (SceneManager)Game.Services.GetService(typeof(SceneManager));
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

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
            { 
                if (component.Enabled)
                    component.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            foreach(var component in _components)
            {
                component.Draw(gameTime, _spriteBatch);
            }
        }

        protected internal virtual void SceneChanged(object sender, EventArgs e)
        {
            if(_manager.CurrentScene == this) { Show(); }
            else { Hide(); }
        }

        public virtual void Show()
        {
            Enabled = true;
            Visible = true;

            foreach(var component in _components)
            {
                component.Enabled = true;
                component.Visible = true;
            }
        }

        public virtual void Hide()
        {
            Enabled = false;
            Visible = false;

            foreach (var component in _components)
            {
                component.Enabled = false;
                component.Visible = false;
            }
        }


    }
}
