using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace Paramita.UI.Scenes
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
        protected GameController GameRef;

        protected Scene tag;
        protected readonly SceneManager manager;
        protected ContentManager content;
        protected readonly List<GameComponent> childComponents;
        protected PlayerIndex? indexInControl;
 


        public PlayerIndex? PlayerIndexInControl
        {
            get { return indexInControl; }
            set { indexInControl = value; }
        }

        public List<GameComponent> Components
        {
            get { return childComponents; }
        }

        public Scene Tag
        {
            get { return tag; }
        }



        public Scene(GameController game) : base(game)
        {
            GameRef = game;
            tag = this;
            childComponents = new List<GameComponent>();
            content = Game.Content;
            manager = (SceneManager)Game.Services.GetService(typeof(SceneManager));
        }



        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GameComponent component in childComponents)
            { 
                if (component.Enabled)
                    component.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            foreach(GameComponent component in childComponents)
            {
                if (component is DrawableGameComponent && ((DrawableGameComponent)component).Visible)
                    ((DrawableGameComponent)component).Draw(gameTime);
            }
        }

        protected internal virtual void StateChanged(object sender, EventArgs e)
        {
            if(manager.CurrentScene == tag) { Show(); }
            else { Hide(); }
        }

        public virtual void Show()
        {
            Enabled = true;
            Visible = true;

            foreach(GameComponent component in childComponents)
            {
                component.Enabled = true;
                if(component is DrawableGameComponent)
                {
                    ((DrawableGameComponent)component).Visible = true;
                }
            }
        }

        public virtual void Hide()
        {
            Enabled = false;
            Visible = false;

            foreach (GameComponent component in childComponents)
            {
                component.Enabled = false;
                if (component is DrawableGameComponent)
                {
                    ((DrawableGameComponent)component).Visible = false;
                }
            }
        }


    }
}
