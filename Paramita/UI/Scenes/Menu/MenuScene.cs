using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.UI.Input;
using System;

namespace Paramita.UI.Scenes
{


    public class MenuScene : Scene
    {
        Texture2D background;
        SpriteFont spriteFont;
        MenuComponent menuComponent;
        Texture2D button;


        public MenuScene(GameController game) : base(game)
        {
           
        }



        public override void Initialize()
        {
            base.Initialize();

            string[] menuItems = { "NEW GAME", "CONTINUE", "OPTIONS", "EXIT" };
            Vector2 position = new Vector2();
            position.Y = 90;
            position.X = 1200 - button.Width;
            menuComponent = new MenuComponent(GameRef, spriteFont, button, menuItems, position);
        }


        protected override void LoadContent()
        {
            spriteFont = GameController.ArialBold;
            background = content.Load<Texture2D>("menuscreen");
            button = content.Load<Texture2D>("wooden-button");

            base.LoadContent();
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        private void HandleMouseClick(object sender, EventArgs e)
        {
            if (menuComponent.MouseOver)
            {
                if (menuComponent.SelectedIndex == 0)
                {
                    manager.PushScene(GameRef.GameScene, PlayerIndexInControl);
                    Hide();
                }
                
                else if (menuComponent.SelectedIndex == 1)
                {
                    // Loading saved games is not implemented yet
                    //GameRef.GameScene.LoadSavedGame();
                    //manager.PushScene(GameRef.GameScene, PlayerIndexInControl);
                }
                else if (menuComponent.SelectedIndex == 2)
                {
                    // Options screen is not implemented yet
                }
                else if (menuComponent.SelectedIndex == 3)
                {
                    Game.Exit();
                }
            }
        }


        public override void Hide()
        {
            base.Hide();
            InputDevices.OnLeftMouseButtonClicked -= HandleMouseClick;

        }

        public override void Show()
        {
            base.Show();
            InputDevices.OnLeftMouseButtonClicked += HandleMouseClick;
        }

        public override void Draw(GameTime gameTime)
        {
            GameRef.SpriteBatch.Begin();

            GameRef.SpriteBatch.Draw(background, Vector2.Zero, Color.White);
            menuComponent.Draw(gameTime, GameRef.SpriteBatch);

            GameRef.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
