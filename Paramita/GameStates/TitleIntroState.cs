using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.StateManagement;
using System;

namespace Paramita.GameStates
{
    public interface ITitleIntroState : IGameState
    {
    }


    public class TitleIntroState : BaseGameState, ITitleIntroState
    {
        Texture2D background;
        Rectangle backgroundDestination;
        SpriteFont font;
        TimeSpan elapsed;
        Vector2 position;
        string message;


        public TitleIntroState(GameController game) : base(game)
        {
            game.Services.AddService(typeof(ITitleIntroState), this);
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
            Paramita.GameController.ScreenRectangle.Bottom - 50 - font.LineSpacing);
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            PlayerIndex index = PlayerIndex.One;
            elapsed += gameTime.ElapsedGameTime;
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
