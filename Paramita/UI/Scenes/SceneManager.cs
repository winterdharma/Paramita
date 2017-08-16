using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Paramita.UI.Scenes
{
    /*
     * The Scene Manager handles transitions from one game scene 
     * to the next. It registers itself as a service.
     */
    public class SceneManager : GameComponent
    {
        private readonly Stack<Scene> _scenesStack = new Stack<Scene>();

        private const int START_DRAW_ORDER = 5000;
        private int _drawOrder;

        public event EventHandler SceneChanged;

        public Scene CurrentScene
        {
            get { return _scenesStack.Peek(); }
        }


        public SceneManager(GameController game) : base(game)
        {
            Game.Services.AddService(typeof(SceneManager), this);
        }


        public override void Update(GameTime gameTime)
        {
            CurrentScene.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            CurrentScene.Draw(gameTime);
        }

        public void PushScene(Scene scene)
        {
            AddScene(scene);
            OnSceneChanged();
        }

        public void AddScene(Scene scene)
        {
            _scenesStack.Push(scene);
            SceneChanged += scene.SceneChanged;
        }

        public void PopScene()
        {
            if(_scenesStack.Count != 0)
            {
                RemoveScene();
                OnSceneChanged();
            }
        }

        public void RemoveScene()
        {
            Scene scene = _scenesStack.Peek();
            SceneChanged -= scene.SceneChanged;
            _scenesStack.Pop();
        }

        public void ChangeScene(Scene scene)
        {
            while(_scenesStack.Count > 0)
            {
                RemoveScene();
            }

            _drawOrder = START_DRAW_ORDER;
            scene.DrawOrder = _drawOrder;

            AddScene(scene);
            OnSceneChanged();
        }

        public bool ContainsScene(Scene scene)
        {
            return _scenesStack.Contains(scene);
        }

        protected internal virtual void OnSceneChanged()
        {
            SceneChanged?.Invoke(this, null);
        }
    }
}