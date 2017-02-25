using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.UI.Input;
using System;

namespace Paramita.UI.Scenes
{


    public class MenuScene : Scene
    {
        private Texture2D _background;
        private SpriteFont _fontArialBold;
        private MenuComponent _menuButtons;
        private Texture2D _buttonTexture;



        public MenuScene(GameController game) : base(game) { }



        public override void Initialize()
        {
            base.Initialize();
            InitializeMenuComponent();
        }


        private void InitializeMenuComponent()
        {
            string[] menuItems = { "NEW GAME", "CONTINUE", "OPTIONS", "EXIT" };
            Vector2 position = new Vector2( (1200 - _buttonTexture.Width), 90);
            _menuButtons = new MenuComponent(_fontArialBold, _buttonTexture, menuItems, position);
        }


        protected override void LoadContent()
        {
            _fontArialBold = GameController.ArialBold;
            _background = _content.Load<Texture2D>("Images\\Scenes\\menuscreen");
            _buttonTexture = _content.Load<Texture2D>("Images\\Scenes\\wooden-button");
        }


        private void HandleMouseClick(object sender, EventArgs e)
        {
            if (_menuButtons.MouseOver)
            {
                HandleMenuItemSelected(sender, e);
            }
        }

        private void HandleMenuItemSelected(object sender, EventArgs e)
        {
            if (_menuButtons.SelectedIndex == 0)
            {
                _manager.PushScene(_game.GameScene);
                Hide();
            }

            else if (_menuButtons.SelectedIndex == 1)
            {
                // Loading saved games is not implemented yet
                //GameRef.GameScene.LoadSavedGame();
                //manager.PushScene(GameRef.GameScene, PlayerIndexInControl);
            }
            else if (_menuButtons.SelectedIndex == 2)
            {
                // Options screen is not implemented yet
            }
            else if (_menuButtons.SelectedIndex == 3)
            {
                Game.Exit();
            }
        }

        public override void Hide()
        {
            base.Hide();
            InputListener.OnLeftMouseButtonClicked -= HandleMouseClick;
            InputListener.OnEnterKeyWasPressed -= HandleMenuItemSelected;


        }

        public override void Show()
        {
            base.Show();
            InputListener.OnLeftMouseButtonClicked += HandleMouseClick;
            InputListener.OnEnterKeyWasPressed += HandleMenuItemSelected;
        }

        public override void Draw(GameTime gameTime)
        {
            GameController.SpriteBatch.Begin();

            GameController.SpriteBatch.Draw(_background, Vector2.Zero, Color.White);
            _menuButtons.Draw(gameTime, GameController.SpriteBatch);

            GameController.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
