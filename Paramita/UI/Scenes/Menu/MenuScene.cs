using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;
using System;

namespace Paramita.UI.Scenes
{


    public class MenuScene : Scene
    {
        private Texture2D _background;
        private SpriteFont _fontArialBold;
        private MenuComponent _menuButtons;
        private Texture2D _buttonTexture;
        private KeyboardListener _keyboard;
        private MouseListener _mouse;

        public MenuScene(GameController game, KeyboardListener keyboard, MouseListener mouse) 
            : base(game)
        {
            _keyboard = keyboard;
            _mouse = mouse;
        }



        public override void Initialize()
        {
            base.Initialize();
            InitializeMenuComponent();
        }


        private void InitializeMenuComponent()
        {
            string[] menuItems = { "NEW GAME", "CONTINUE", "OPTIONS", "EXIT" };
            Vector2 position = new Vector2( (1200 - _buttonTexture.Width), 90);
            _menuButtons = new MenuComponent(_fontArialBold, _buttonTexture, menuItems, position, 
                _keyboard, _mouse);
        }


        protected override void LoadContent()
        {
            _fontArialBold = GameController.ArialBold;
            _background = _content.Load<Texture2D>("Images\\Scenes\\menuscreen");
            _buttonTexture = _content.Load<Texture2D>("Images\\Scenes\\wooden-button");
        }


        private void HandleMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButton.Left) return;

            if (_menuButtons.MouseOver)
            {
                SelectMenuItem();
            }
        }


        private void HandleMenuItemSelected(object sender, KeyboardEventArgs e)
        {
            if (e.Key != Keys.Enter) return;

            SelectMenuItem();
        }

        private void SelectMenuItem()
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
            _mouse.MouseClicked -= HandleMouseClick;
            _keyboard.KeyPressed -= HandleMenuItemSelected;


        }

        public override void Show()
        {
            base.Show();
            _mouse.MouseClicked += HandleMouseClick;
            _keyboard.KeyPressed += HandleMenuItemSelected;
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
