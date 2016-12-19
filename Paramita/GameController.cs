using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Paramita.Components;
using Paramita.Scenes;
using System.Collections.Generic;

namespace Paramita
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameController : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Dictionary<AnimationKey, Animation> playerAnimations = new Dictionary<AnimationKey, Animation>();

        public SpriteBatch SpriteBatch { get { return spriteBatch; } }
        public static Rectangle ScreenRectangle { get; private set; }

        public SceneManager SceneManager { get; private set; }
        public TitleScene TitleScene { get; private set; }
        public MenuScene MenuScene { get; private set; }
        public GameScene GameScene { get; private set; }

        public Dictionary<AnimationKey, Animation> PlayerAnimations
        {
            get { return playerAnimations; }
        }




        public GameController()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            ScreenRectangle = new Rectangle(0, 0, 1280, 720);
            graphics.PreferredBackBufferWidth = ScreenRectangle.Width;
            graphics.PreferredBackBufferHeight = ScreenRectangle.Height;


            SceneManager = new SceneManager(this);
            Components.Add(SceneManager);
            this.IsMouseVisible = true;

            TitleScene = new TitleScene(this);
            MenuScene = new MenuScene(this);
            GameScene = new GameScene(this);

            SceneManager.ChangeScene((TitleScene)TitleScene, PlayerIndex.One);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Components.Add(new InputDevices(this));

            Animation animation = new Animation(3, 32, 32, 0, 0);
            playerAnimations.Add(AnimationKey.WalkDown, animation);
            animation = new Animation(3, 32, 32, 0, 32);
            playerAnimations.Add(AnimationKey.WalkLeft, animation);
            animation = new Animation(3, 32, 32, 0, 64);
            playerAnimations.Add(AnimationKey.WalkRight, animation);
            animation = new Animation(3, 32, 32, 0, 96);
            playerAnimations.Add(AnimationKey.WalkUp, animation);

            base.Initialize();
        }

        
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            
            //Cell startingCell = GetRandomEmptyCell();
            
            
            //startingCell = GetRandomEmptyCell();

            //PathToPlayer pathFromEnemy = new PathToPlayer(_player, _map, Content.Load<Texture2D>("white"));
            //pathFromEnemy.CreateFrom(startingCell.X, startingCell.Y);

            //AddEnemies(10);
            //Global.CombatManager = new CombatManager(_player, _enemies);
            //UpdatePlayerFieldOfView();
            //Global.GameState = OldGameStates.PlayerTurn;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            base.Update(gameTime);
        }


        /// This is called when the game should draw itself.
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            

            //foreach(Cell c in _map.GetAllCells() )
            //{
            //    var position = new Vector2(c.X * Global.SpriteWidth, c.Y * Global.SpriteHeight);

            //    if(c.IsExplored == false && Global.GameState != OldGameStates.Debugging)
            //    {
            //        continue;
            //    }

            //    Color tint = Color.White;

            //    if (c.IsInFov == false && Global.GameState != OldGameStates.Debugging)
            //    {
            //        tint = Color.Gray;
            //    }

            //    Texture2D sprite;
            //    if(c.IsWalkable == true)
            //    {
            //        sprite = _floor;
            //    }
            //    else
            //    {
            //        sprite = _wall;
            //    }

            //    spriteBatch.Draw(sprite, position,
            //            null, null, null, 0.0f, Vector2.One,
            //            tint, SpriteEffects.None, LayerDepth.Cells);
            //}

            //foreach (Enemy e in _enemies)
            //{
            //    if (Global.GameState == OldGameStates.Debugging || _map.IsInFov(e.X, e.Y))
            //    {
            //        e.Draw(spriteBatch);
            //    }
            //}

            
            base.Draw(gameTime);
        }

        //private void UpdatePlayerFieldOfView()
        //{
        //    _map.ComputeFov(_player.X, _player.Y, 30, true);
        //    foreach(Cell c in _map.GetAllCells())
        //    {
        //        if(_map.IsInFov(c.X, c.Y))
        //        {
        //            _map.SetCellProperties(c.X, c.Y, c.IsTransparent, c.IsWalkable, true);
        //        }
        //    }
        //}

        //private void AddEnemies(int numberOfEnemies)
        //{
        //    for (int i = 0; i < numberOfEnemies; i++)
        //    {
        //        // Find a new empty cell for each enemy
        //        Cell enemyCell = GetRandomEmptyCell();
        //        var pathFromEnemy =
        //          new PathToPlayer(_player, _map, Content.Load<Texture2D>("White"));
        //        pathFromEnemy.CreateFrom(enemyCell.X, enemyCell.Y);
        //        var enemy = new Enemy(this, _map, pathFromEnemy)
        //        {
        //            X = enemyCell.X,
        //            Y = enemyCell.Y,
        //            //Sprite = Content.Load<Texture2D>("hound"),
        //            ArmorClass = 10,
        //            AttackBonus = 0,
        //            Damage = Dice.Parse("d4"),
        //            Health = 10,
        //            Name = "Hell Hound"
        //        };
        //        // Add each enemy to list of enemies
        //        _enemies.Add(enemy);
        //    }
        //}
    }
}
