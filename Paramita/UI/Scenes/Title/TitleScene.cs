using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Paramita.UI.Base
{
    public class TitleScene : Scene
    {
        #region Fields
        private Texture2D _background;
        private SpriteFont _fontArialBold;
        private TimeSpan _elapsedTime;
        private Color _messageColor;
        private Color WHITE = new Color(1f, 1f, 1f);
        private Vector2 _messagePosition;
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
        /*
         * Called once by GameController.Initialize() 
         */
        public override void Initialize()
        {
            LoadContent();
            _fontArialBold = GameController.ArialBold;
            SetMessagePosition();
        }

        protected override List<UserAction> InitializeUserActions(List<Component> components)
        {
            var actionsList = new List<UserAction>();
            return actionsList;
        }

        protected override void LoadContent()
        {
            _background = _content.Load<Texture2D>("Images\\Scenes\\titlescreen");
        }
        #endregion

        #region Event Handling
        private void HandleInput(object sender, EventArgs e)
        {
            Game.CurrentScene = Game.MenuScene;
        }

        protected override void SubscribeToKeyboardEvents()
        {
        }

        protected override void UnsubscribeFromKeyboardEvents()
        {
        }
        #endregion

        #region Public API
        public override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime;
            _messageColor = WHITE * (float)Math.Abs(Math.Sin(_elapsedTime.TotalSeconds * 2));
            base.Update(gameTime);
        }

        /* 
         * Called by GameController when this @Scene is the @CurrentScene in SceneManager
         *   Draws the @Scene to the screen.
         */
        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            _spriteBatch.Draw(_background, ScreenRectangle, Color.White);

            _spriteBatch.DrawString(_fontArialBold, MESSAGE, _messagePosition, _messageColor);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Hide()
        {
            base.Hide();
            Input.KeyPressed -= HandleInput;
            Input.MouseClick -= HandleInput;
        }

        public override void Show()
        {
            base.Show();
            Input.KeyPressed += HandleInput;
            Input.MouseClick += HandleInput;
        }
        #endregion

        #region Helper Methods
        private void SetMessagePosition()
        {
            Vector2 size = _fontArialBold.MeasureString(MESSAGE);
            _messagePosition = new Vector2((ScreenRectangle.Width - size.X) / 2,
                ScreenRectangle.Bottom - 50 - _fontArialBold.LineSpacing);
        }
        #endregion
    }
}
