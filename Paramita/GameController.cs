using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using MonoGame.Extended.Input.InputListeners;
using Paramita.UI.Input;
using Paramita.UI.Scenes;
using System;

namespace Paramita
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameController : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Rectangle _screenRectangle;

        public SpriteBatch SpriteBatch { get { return _spriteBatch; } }
        public Rectangle ScreenRectangle { get { return _screenRectangle; } }
        public SceneManager SceneManager { get; private set; }
        public InputListenerComponent InputListener { get; private set; }
        public InputResponder InputResponder { get; private set; }

        public TitleScene TitleScene { get; private set; }
        public MenuScene MenuScene { get; private set; }
        public GameScene GameScene { get; private set; }

        public static SpriteFont ArialBold { get; private set; }
        public static SpriteFont LucidaConsole { get; private set; }
        public static SpriteFont NotoSans { get; private set; }





        public GameController()
        {
            _graphics = new GraphicsDeviceManager(this);
            _screenRectangle = new Rectangle(0, 0, 1280, 720);
            _graphics.PreferredBackBufferWidth = _screenRectangle.Width;
            _graphics.PreferredBackBufferHeight = _screenRectangle.Height;

            Content.RootDirectory = "Content";

            SceneManager = new SceneManager(this);
            Components.Add(SceneManager);
            IsMouseVisible = true;

            var keyboard = new KeyboardListener();
            var mouse = new MouseListener();
            InputListener = new InputListenerComponent(this, keyboard, mouse);
            InputResponder = new InputResponder(keyboard, mouse);
            keyboard.KeyPressed += CheckForExit;            
        }

        private void CheckForExit(object sender, KeyboardEventArgs e)
        {
            if (e.Key == Keys.Escape) Exit();
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

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            TitleScene = new TitleScene(this);
            MenuScene = new MenuScene(this);
            GameScene = new GameScene(this);

            TitleScene.Initialize();
            MenuScene.Initialize();
            GameScene.Initialize(); // this also calls GameScene.LoadContent()

            SceneManager.ChangeScene(TitleScene);
        }



        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            //_spriteBatch = new SpriteBatch(GraphicsDevice);
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
            base.Update(gameTime);
            InputListener.Update(gameTime);
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
