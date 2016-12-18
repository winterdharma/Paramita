using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Paramita.Components;
using System;

namespace Paramita.Scenes
{
    public interface ITitleScene : IScene
    {
    }


    public class TitleScene : Scene, ITitleScene
    {
        Texture2D background;
        Rectangle backgroundDestination;
        SpriteFont font;
        TimeSpan elapsed;
        Vector2 position;
        string message;




        public TitleScene(GameController game) : base(game)
        {
            game.Services.AddService(typeof(ITitleScene), this);
        }




        public override void Initialize()
        {
            backgroundDestination = GameController.ScreenRectangle;
            elapsed = TimeSpan.Zero;
            message = "PRESS SPACE TO CONTINUE";

            base.Initialize();
        }

        protected override void LoadContent()
        {
            background = content.Load<Texture2D>("titlescreen");
            font = content.Load<SpriteFont>("InterfaceFont");
            Vector2 size = font.MeasureString(message);
            position = new Vector2((GameController.ScreenRectangle.Width - size.X) / 2,
            GameController.ScreenRectangle.Bottom - 50 - font.LineSpacing);

            base.LoadContent();
        }


        public override void Update(GameTime gameTime)
        {
            PlayerIndex? index = null;
            elapsed += gameTime.ElapsedGameTime;

            if(InputDevices.CheckKeyReleased(Keys.Space) ||
                InputDevices.CheckKeyReleased(Keys.Enter) ||
                InputDevices.CheckMouseReleased(MouseButtons.Left))
            {
                manager.ChangeScene((MenuScene)GameRef.MenuScene, index);
            }

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            GameRef.SpriteBatch.Begin();
            GameRef.SpriteBatch.Draw(background, backgroundDestination, Color.White);
            Color color = new Color(1f, 1f, 1f) *
            (float)Math.Abs(Math.Sin(elapsed.TotalSeconds * 2));
            GameRef.SpriteBatch.DrawString(font, message, position, color);
            GameRef.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
