using Paramita.StateManagement;
using System;
using Microsoft.Xna.Framework;

namespace Paramita.GameStates
{

    // This class is a superclass to allow polymophism 
    // when processing specific GameStates that are children
    // It provides for a ref back to the Game class 
    // and a Random number generator
    public class BaseGameState : GameState
    {
        protected static Random random = new Random();
        protected GameController GameRef;



        public BaseGameState(GameController game) : base(game)
        {
            GameRef = game;
        }




        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
