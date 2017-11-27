using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameUI;
using Paramita.UI.Scenes;

namespace Paramita
{
    public class ParamitaController : GameController
    {
        #region Properties
        public TitleScene TitleScene { get; private set; }
        public MenuScene MenuScene { get; private set; }
        public GameScene GameScene { get; private set; }

        public static SpriteFont ArialBold { get; private set; }
        public static SpriteFont LucidaConsole { get; private set; }
        public static SpriteFont NotoSans { get; private set; }
        #endregion

        #region Constructors
        public ParamitaController()
        {
            _screenRectangle = new Rectangle(0, 0, 1280, 720);
            _graphics.PreferredBackBufferWidth = _screenRectangle.Width;
            _graphics.PreferredBackBufferHeight = _screenRectangle.Height;           
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
            ArialBold = Content.Load<SpriteFont>("Fonts\\InterfaceFont");
            LucidaConsole = Content.Load<SpriteFont>("Fonts\\lucida_console");
            NotoSans = Content.Load<SpriteFont>("Fonts\\noto_sans");
        }
        #endregion

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Update.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // check if this is required
        }



        /// <summary>This is called when the game should draw itself.</summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime); // check if this is required
        }
    }
}
