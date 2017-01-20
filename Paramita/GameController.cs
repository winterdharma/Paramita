using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Paramita.Mechanics;
using Paramita.Scenes;
using System;

namespace Paramita
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameController : Game
    {
        public static Random random = new Random();
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameTime gameTime;
        public GameTime GameTime { get { return GameTime; } }
        public SpriteBatch SpriteBatch { get { return spriteBatch; } }
        public Rectangle ScreenRectangle { get; private set; }

        public SceneManager SceneManager { get; private set; }
        public TitleScene TitleScene { get; private set; }
        public MenuScene MenuScene { get; private set; }
        public GameScene GameScene { get; private set; }
        public InputDevices InputDevices { get; private set; }
        public static SpriteFont ArialBold { get; private set; }
        public static SpriteFont LucidaConsole { get; private set; }
        public static SpriteFont NotoSans { get; private set; }





        public GameController()
        {
            graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
            ScreenRectangle = new Rectangle(0, 0, 1280, 720);
            graphics.PreferredBackBufferWidth = ScreenRectangle.Width;
            graphics.PreferredBackBufferHeight = ScreenRectangle.Height;

            InputDevices = new InputDevices(this);

            SceneManager = new SceneManager(this);
            Components.Add(SceneManager);
            IsMouseVisible = true;

            TitleScene = new TitleScene(this);
            MenuScene = new MenuScene(this);
            GameScene = new GameScene(this);
            
            SceneManager.ChangeScene(TitleScene, PlayerIndex.One);
        }



        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            TitleScene.Initialize();
            MenuScene.Initialize();
            GameScene.Initialize(); // this also calls GameScene.LoadContent()
        }

        

        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ArialBold = Content.Load<SpriteFont>("Fonts\\InterfaceFont");
            LucidaConsole = Content.Load<SpriteFont>("Fonts\\lucida_console");
            NotoSans = Content.Load<SpriteFont>("Fonts\\noto_sans");
        }



        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            
        }



        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // check for user input
            InputDevices.Update(gameTime);
            // call Update() on the active @Scene object
            SceneManager.CurrentScene.Update(gameTime);
        }



        /// This is called when the game should draw itself.
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // clear the previous frame
            GraphicsDevice.Clear(Color.Black);
            // call Draw() on the active @Scene object
            SceneManager.CurrentScene.Draw(gameTime);
        }
    }
}
