using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;
using Paramita.UI.Input;
using Paramita.UI.Base;
using Paramita.UI.Scenes;

namespace Paramita
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameController : Game
    {
        #region Fields
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Rectangle _screenRectangle;
        private Scene _currentScene;
        #endregion

        #region Properties
        public SpriteBatch SpriteBatch { get { return _spriteBatch; } }
        public Rectangle ScreenRectangle { get { return _screenRectangle; } }
        public InputListenerComponent InputListener { get; private set; }
        public InputResponder InputResponder { get; private set; }

        public Scene CurrentScene
        {
            get => _currentScene;
            set
            {
                if(_currentScene != null)
                    _currentScene.Hide();
                _currentScene = value;
                _currentScene.Show();
            }
        }
        public TitleScene TitleScene { get; private set; }
        public MenuScene MenuScene { get; private set; }
        public GameScene GameScene { get; private set; }

        public static SpriteFont ArialBold { get; private set; }
        public static SpriteFont LucidaConsole { get; private set; }
        public static SpriteFont NotoSans { get; private set; }
        #endregion




        #region Constructors
        public GameController()
        {
            _graphics = new GraphicsDeviceManager(this);
            _screenRectangle = new Rectangle(0, 0, 1280, 720);
            _graphics.PreferredBackBufferWidth = _screenRectangle.Width;
            _graphics.PreferredBackBufferHeight = _screenRectangle.Height;

            Content.RootDirectory = "Content";

            IsMouseVisible = true;

            var keyboard = new KeyboardListener();
            var mouseSettings = new MouseListenerSettings
            {
                DoubleClickMilliseconds = 250
            };
            var mouse = new MouseListener(mouseSettings);
            InputListener = new InputListenerComponent(this, keyboard, mouse);
            InputResponder = new InputResponder(keyboard, mouse);
            keyboard.KeyPressed += CheckForExit;            
        }
        #endregion

        #region Event Handling
        private void CheckForExit(object sender, KeyboardEventArgs e)
        {
            if (e.Key == Keys.Escape) Exit();
        }
        #endregion

        #region Initialization
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

            CurrentScene = TitleScene;
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
        #endregion

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
        /// <param name="gameTime">Time passed since the last call to Update.</param>
        protected override void Update(GameTime gameTime)
        {
            InputListener.Update(gameTime);
            CurrentScene.Update(gameTime);
        }



        /// <summary>This is called when the game should draw itself.</summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // clear the previous frame
            GraphicsDevice.Clear(Color.Black);
            CurrentScene.Draw(gameTime);
        }
    }
}
