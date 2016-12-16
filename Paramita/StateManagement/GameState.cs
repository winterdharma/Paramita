using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace Paramita.StateManagement
{
    public interface IGameState
    {
        GameState Tag { get; }
        PlayerIndex? PlayerIndexInControl { get; set; }
    }

    public abstract partial class GameState : DrawableGameComponent, IGameState
    {
        protected GameState tag;
        protected readonly GameStateManager manager;
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

        public GameState Tag
        {
            get { return tag; }
        }



        public GameState(GameController game) : base(game)
        {
            tag = this;
            childComponents = new List<GameComponent>();
            content = Game.Content;
            manager = (GameStateManager)Game.Services.GetService(typeof(GameStateManager));
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
            if(manager.CurrentState == tag) { Show(); }
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
