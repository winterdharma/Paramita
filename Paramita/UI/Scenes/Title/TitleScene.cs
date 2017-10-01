using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.UI.Base;
using Paramita.UI.Components;
using Paramita.UI.Elements;
using System;
using System.Collections.Generic;

namespace Paramita.UI.Scenes
{
    public class TitleScene : Scene
    {
        #region Fields
        private Texture2D _background;
        private TimeSpan _elapsedTime;
        private Color WHITE = new Color(1f, 1f, 1f);
        private const string MESSAGE = "PRESS ANY KEY TO CONTINUE";
        #endregion

        #region Constructor
        public TitleScene(GameController game)
            : base(game)
        {
            _elapsedTime = TimeSpan.Zero;
        }
        #endregion

        #region Initialization
        public override void Initialize()
        {
            base.Initialize();

            var screen = new Screen(this, 0);
            screen.Elements = new Dictionary<string, Element>()
            {
                { "background", new Background("background", screen, ScreenRectangle.Location, 
                    _background, WHITE, WHITE, ScreenRectangle.Size, 0) },
                { "continue_text", new LineOfText("continue_text", screen, SetMessagePosition(), 
                    MESSAGE, GameController.ArialBold, WHITE, WHITE, 1) }
            };
            Components = InitializeComponents(screen);
            UserActions = InitializeUserActions(Components);
        }

        protected override List<UserAction> InitializeUserActions(List<Component> components)
        {
            var actionsList = new List<UserAction>()
            {
                new UserAction(this, ContinueToMenu, CanContinueToMenu)
            };
            return actionsList;
        }

        private bool CanContinueToMenu(Tuple<Scene,UserInputEventArgs> context)
        {
            if(context.Item2.EventType == EventType.LeftClick ||
                context.Item2.EventType == EventType.Keyboard)
                return true;
            return false;
        }

        private void ContinueToMenu(Scene parent, UserInputEventArgs eventArgs)
        {
            Game.CurrentScene = Game.MenuScene;
        }

        protected override void LoadContent()
        {
            _background = _content.Load<Texture2D>("Images\\Scenes\\titlescreen");
        }
        #endregion

        #region Public API
        public override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime;
            Components[0].Elements["continue_text"].Color = 
                WHITE * (float)Math.Abs(Math.Sin(_elapsedTime.TotalSeconds * 2));
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
        #endregion

        #region Helper Methods
        private Vector2 SetMessagePosition()
        {
            SpriteFont font = GameController.ArialBold;
            Vector2 size = font.MeasureString(MESSAGE);
            return new Vector2((ScreenRectangle.Width - size.X) / 2,
                ScreenRectangle.Bottom - 50 - font.LineSpacing);
        }
        #endregion
    }
}
