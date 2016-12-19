using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Paramita.Components;
using Paramita.Scenes;

namespace Paramita.Scenes
{


    public class MenuScene : Scene
    {
        Texture2D background;
        SpriteFont spriteFont;
        MenuComponent menuComponent;



        public MenuScene(GameController game) : base(game)
        {
            
        }



        public override void Initialize()
        {
            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteFont = Game.Content.Load<SpriteFont>("InterfaceFont");

            background = Game.Content.Load<Texture2D>("menuscreen");
            Texture2D texture = Game.Content.Load<Texture2D>("wooden-button");
            string[] menuItems = { "NEW GAME", "CONTINUE", "OPTIONS", "EXIT" };
            menuComponent = new MenuComponent(GameRef, spriteFont, texture, menuItems);
            Vector2 position = new Vector2();
            position.Y = 90;
            position.X = 1200 - menuComponent.Width;
            menuComponent.Postion = position;

            base.LoadContent();
        }


        public override void Update(GameTime gameTime)
        {
            menuComponent.Update(gameTime);

            if (GameRef.InputDevices.CheckKeyReleased(Keys.Space) 
                || GameRef.InputDevices.CheckKeyReleased(Keys.Enter) 
                || (menuComponent.MouseOver && GameRef.InputDevices.CheckMouseReleased(MouseButtons.Left)))
            {
                if (menuComponent.SelectedIndex == 0)
                {
                    GameRef.InputDevices.FlushInput();
                    GameRef.GameScene.SetUpNewGame();
                    GameRef.GameScene.StartGame();
                    manager.PushScene((GameScene)GameRef.GameScene, PlayerIndexInControl);
                }
                else if (menuComponent.SelectedIndex == 1)
                {
                    GameRef.InputDevices.FlushInput();
                    GameRef.GameScene.LoadSavedGame();
                    GameRef.GameScene.StartGame();
                    manager.PushScene((GameScene)GameRef.GameScene, PlayerIndexInControl);
                }
                else if (menuComponent.SelectedIndex == 2)
                {
                    GameRef.InputDevices.FlushInput();
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
            GameRef.SpriteBatch.End();
            base.Draw(gameTime);
            GameRef.SpriteBatch.Begin();
            menuComponent.Draw(gameTime, GameRef.SpriteBatch);
            GameRef.SpriteBatch.End();
        }
    }
}
