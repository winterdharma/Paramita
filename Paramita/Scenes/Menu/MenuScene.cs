using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Paramita.Mechanics;

namespace Paramita.Scenes
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
            menuComponent.Update(gameTime);

            if (menuComponent.MouseOver 
                && InputDevices.CheckMouseReleased(MouseButtons.Left))
            {
                if (menuComponent.SelectedIndex == 0)
                {
                    InputDevices.FlushInput();
                    GameRef.GameScene.SetUpNewGame();
                    manager.PushScene(GameRef.GameScene, PlayerIndexInControl);
                }
                // Loading saved games is not implemented yet
                else if (menuComponent.SelectedIndex == 1)
                {
                    InputDevices.FlushInput();
                    GameRef.GameScene.LoadSavedGame();
                    manager.PushScene(GameRef.GameScene, PlayerIndexInControl);
                }
                // Options screen is not implemented yet
                else if (menuComponent.SelectedIndex == 2)
                {
                    InputDevices.FlushInput();
                }
                else if (menuComponent.SelectedIndex == 3)
                {
                    Game.Exit();
                }
            }
            base.Update(gameTime);
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
