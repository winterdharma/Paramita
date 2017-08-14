using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input.InputListeners;
using System;

namespace Paramita.UI.Scenes
{


    public class TitleScene : Scene
    {
        private Texture2D _background;
        private Rectangle _screenRectangle;
        private SpriteFont _fontArialBold;
        private TimeSpan _elapsedTime;
        private Color _messageColor;
        private Color WHITE = new Color(1f, 1f, 1f);
        private Vector2 _messagePosition;
        private const string MESSAGE = "PRESS ANY KEY TO CONTINUE";
        private KeyboardListener _keyboard;
        private MouseListener _mouse;

        public TitleScene(GameController game, KeyboardListener keyboard, MouseListener mouse)
            : base(game)
        {
            _screenRectangle = GameController.ScreenRectangle;
            _elapsedTime = TimeSpan.Zero;
            _keyboard = keyboard;
            _mouse = mouse;
        }



        /*
         * Called once by GameController.Initialize() 
         */
        public override void Initialize()
        {
            //base.Initialize(); // calls LoadContent()
            LoadContent();
            _fontArialBold = GameController.ArialBold;
            SetMessagePosition();
        }

        private void SetMessagePosition()
        {
            Vector2 size = _fontArialBold.MeasureString(MESSAGE);
            _messagePosition = new Vector2((_screenRectangle.Width - size.X) / 2,
                _screenRectangle.Bottom - 50 - _fontArialBold.LineSpacing);
        }

        /*
         * Called once by base.Initialize()
         */
        protected override void LoadContent()
        {
            _background = _content.Load<Texture2D>("Images\\Scenes\\titlescreen");
        }


        public override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime;
            _messageColor = WHITE * (float)Math.Abs(Math.Sin(_elapsedTime.TotalSeconds * 2));
            base.Update(gameTime);
        }

        private void HandleInput(object sender, EventArgs e)
        {
            _manager.ChangeScene(_game.MenuScene);
            Hide();
        }


        /* 
         * Called by GameController when this @Scene is the @CurrentScene in SceneManager
         *   Draws the @Scene to the screen.
         */
        public override void Draw(GameTime gameTime)
        {
            GameController.SpriteBatch.Begin();

            GameController.SpriteBatch.Draw(_background, _screenRectangle, Color.White);

            GameController.SpriteBatch.DrawString(_fontArialBold, MESSAGE, _messagePosition, _messageColor);

            GameController.SpriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Hide()
        {
            base.Hide();
            _keyboard.KeyPressed -= HandleInput;
            _mouse.MouseClicked -= HandleInput;
        }

        public override void Show()
        {
            base.Show();
            _keyboard.KeyPressed += HandleInput;
            _mouse.MouseClicked += HandleInput;
        }
    }
}
