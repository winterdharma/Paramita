using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Paramita.StateManagement
{
    public interface IGameStateManager
    {
        GameState CurrentState { get; }
        event EventHandler StateChanged;

        void PushState(GameState state, PlayerIndex? index);
        void PopState();
        bool ContainsState(GameState state);
    }



    public class GameStateManager : GameComponent, IGameStateManager
    {
        private readonly Stack<GameState> gStates = new Stack<GameState>();

        private const int startDrawOrder = 5000;
        private const int drawOrderInc = 50;
        private int drawOrder;

        public event EventHandler StateChanged;

        public GameState CurrentState
        {
            get { return gStates.Peek(); }
        }


        public GameStateManager(GameController game) : base(game)
        {
            Game.Services.AddService(typeof(GameStateManager), this);
        }


        public void PushState(GameState state, PlayerIndex? index)
        {
            drawOrder += drawOrderInc;
            AddState(state, index);
            OnStateChanged();
        }

        public void AddState(GameState state, PlayerIndex? index)
        {
            gStates.Push(state);
            state.PlayerIndexInControl = index;
            Game.Components.Add(state);
            StateChanged += state.StateChanged;
        }

        public void PopState()
        {
            if(gStates.Count != 0)
            {
                RemoveState();
                drawOrder -= drawOrderInc;
                OnStateChanged();
            }
        }

        public void RemoveState()
        {
            GameState state = gStates.Peek();
            StateChanged -= state.StateChanged;
            Game.Components.Remove(state);
            gStates.Pop();
        }

        public void ChangeState(GameState state, PlayerIndex? index)
        {
            while(gStates.Count > 0)
            {
                RemoveState();
            }

            drawOrder = startDrawOrder;
            state.DrawOrder = drawOrder;
            drawOrder += drawOrderInc;

            AddState(state, index);
            OnStateChanged();
        }

        public bool ContainsState(GameState state)
        {
            return gStates.Contains(state);
        }

        protected internal virtual void OnStateChanged()
        {
            if(StateChanged != null)
            {
                StateChanged(this, null);
            }
        }
    }
}