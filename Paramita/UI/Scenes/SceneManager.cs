using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Paramita.Scenes
{
    public interface ISceneManager
    {
        Scene CurrentScene { get; }
        event EventHandler SceneChanged;

        void PushScene(Scene scene, PlayerIndex? index);
        void PopScene();
        bool ContainsScene(Scene scene);
    }


    /*
     * The Scene Manager handles transitions from one game scene 
     * to the next. It registers itself as a service.
     */
    public class SceneManager : GameComponent, ISceneManager
    {
        private readonly Stack<Scene> scenesStack = new Stack<Scene>();

        private const int startDrawOrder = 5000;
        private const int drawOrderInc = 50;
        private int drawOrder;

        public event EventHandler SceneChanged;

        public Scene CurrentScene
        {
            get { return scenesStack.Peek(); }
        }


        public SceneManager(GameController game) : base(game)
        {
            Game.Services.AddService(typeof(SceneManager), this);
        }


        public void PushScene(Scene scene, PlayerIndex? index)
        {
            drawOrder += drawOrderInc;
            AddScene(scene, index);
            OnSceneChanged();
        }

        public void AddScene(Scene state, PlayerIndex? index)
        {
            scenesStack.Push(state);
            state.PlayerIndexInControl = index;
            //Game.Components.Add(state);
            SceneChanged += state.StateChanged;
        }

        public void PopScene()
        {
            if(scenesStack.Count != 0)
            {
                RemoveScene();
                drawOrder -= drawOrderInc;
                OnSceneChanged();
            }
        }

        public void RemoveScene()
        {
            Scene scene = scenesStack.Peek();
            SceneChanged -= scene.StateChanged;
            Game.Components.Remove(scene);
            scenesStack.Pop();
        }

        public void ChangeScene(Scene scene, PlayerIndex? index)
        {
            while(scenesStack.Count > 0)
            {
                RemoveScene();
            }

            drawOrder = startDrawOrder;
            scene.DrawOrder = drawOrder;
            drawOrder += drawOrderInc;

            AddScene(scene, index);
            OnSceneChanged();
        }

        public bool ContainsScene(Scene scene)
        {
            return scenesStack.Contains(scene);
        }

        protected internal virtual void OnSceneChanged()
        {
            SceneChanged?.Invoke(this, null);
        }
    }
}