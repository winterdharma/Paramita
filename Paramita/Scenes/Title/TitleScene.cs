using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.UI.Input;
using System;

namespace Paramita.Scenes
{


    public class TitleScene : Scene
    {
        Texture2D background;
        Rectangle backgroundDestination;
        SpriteFont font;
        TimeSpan elapsed;
        Vector2 position;
        string message = "PRESS ANY KEY TO CONTINUE";




        public TitleScene(GameController game) : base(game) {
            // see Initialize() and LoadContent() for instantiation tasks
            backgroundDestination = GameController.ScreenRectangle;
            InputDevices.OnAnyKeyWasPressed += HandleInput;
            InputDevices.OnLeftMouseButtonClicked += HandleInput;
        }



        /*
         * Called once by GameController.Initialize() 
         */
        public override void Initialize()
        {
            base.Initialize(); // calls LoadContent()

            //backgroundDestination = GameController.ScreenRectangle;

            // these variables are used to display the @message on the screen when Draw() is called
            font = GameController.ArialBold;
            elapsed = TimeSpan.Zero;
            Vector2 size = font.MeasureString(message);
            position = new Vector2((backgroundDestination.Width - size.X) / 2,
                backgroundDestination.Bottom - 50 - font.LineSpacing);
        }


        /*
         * Called once by base.Initialize()
         */
        protected override void LoadContent()
        {
            background = content.Load<Texture2D>("titlescreen");
        }


        public override void Update(GameTime gameTime)
        {
            elapsed += gameTime.ElapsedGameTime;

            base.Update(gameTime);
        }

        private void HandleInput(object sender, EventArgs e)
        {
            manager.ChangeScene(GameRef.MenuScene, null);
        }


        /* 
         * Called by GameController when this @Scene is the @CurrentScene in SceneManager
         *   Draws the @Scene to the screen.
         */
        public override void Draw(GameTime gameTime)
        {
            GameRef.SpriteBatch.Begin();

            GameRef.SpriteBatch.Draw(background, backgroundDestination, Color.White);

            // Causes the @message to slowly blink on and off by applying a Sine
            // function to the alpha channel
            Color color = new Color(1f, 1f, 1f) *
            (float)Math.Abs(Math.Sin(elapsed.TotalSeconds * 2));

            GameRef.SpriteBatch.DrawString(font, message, position, color);

            GameRef.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
