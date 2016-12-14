using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueSharp;
using RogueSharp.MapCreation;
using RogueSharp.Random;
using System;

namespace Paramita
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private InputState _inputState;
        private IMap _map;
        private Texture2D _floor;
        private Texture2D _wall;
        private Player _player;
        private Enemy _enemy;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _inputState = new InputState();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IMapCreationStrategy<Map> mapCreationStrategy =
                new RandomRoomsMapCreationStrategy<Map>(50, 30, 100, 7,3);
            _map = Map.Create(mapCreationStrategy);
            Console.WriteLine(_map.ToString());
            base.Initialize();
        }

        
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _floor = Content.Load<Texture2D>("floor");
            _wall = Content.Load<Texture2D>("wall");
            Cell startingCell = GetRandomEmptyCell();
            _player = new Player
            {
                X = startingCell.X,
                Y = startingCell.Y,
                Scale = 0.25f,
                Sprite = Content.Load<Texture2D>("player")
            };
            startingCell = GetRandomEmptyCell();

            PathToPlayer pathFromEnemy = new PathToPlayer(_player, _map, Content.Load<Texture2D>("white"));
            pathFromEnemy.CreateFrom(startingCell.X, startingCell.Y);

            _enemy = new Enemy(pathFromEnemy)
            {
                X = startingCell.X,
                Y = startingCell.Y,
                Scale = 0.25f,
                Sprite = Content.Load<Texture2D>("hound")
            };
            UpdatePlayerFieldOfView();
            Global.GameState = GameStates.PlayerTurn;
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
            if (_inputState.IsExitGame(PlayerIndex.One)) 
            {
                Exit();
            }
            else if(_inputState.IsSpace(PlayerIndex.One))
            {
                if( Global.GameState == GameStates.PlayerTurn)
                {
                    Global.GameState = GameStates.Debugging;
                }
                else if(Global.GameState == GameStates.Debugging)
                {
                    Global.GameState = GameStates.PlayerTurn;
                }
            }
            else
            {
                if (Global.GameState == GameStates.PlayerTurn
                    && _player.HandleInput(_inputState, _map))
                {
                    UpdatePlayerFieldOfView();
                    Global.GameState = GameStates.EnemyTurn;
                }
                if (Global.GameState == GameStates.EnemyTurn)
                {
                    _enemy.Update();
                    Global.GameState = GameStates.PlayerTurn;
                }
            }


            // TODO: Add your update logic here
            _inputState.Update();
            base.Update(gameTime);
        }


        /// This is called when the game should draw itself.
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            int sizeOfSprites = 64;
            float scale = 0.25f;

            foreach(Cell c in _map.GetAllCells() )
            {
                var position = new Vector2(c.X * sizeOfSprites * scale, c.Y * sizeOfSprites * scale);

                if(c.IsExplored == false && Global.GameState != GameStates.Debugging)
                {
                    continue;
                }

                Color tint = Color.White;

                if (c.IsInFov == false && Global.GameState != GameStates.Debugging)
                {
                    tint = Color.Gray;
                }

                Texture2D sprite;
                if(c.IsWalkable == true)
                {
                    sprite = _floor;
                }
                else
                {
                    sprite = _wall;
                }

                spriteBatch.Draw(sprite, position,
                        null, null, null, 0.0f, new Vector2(scale, scale),
                        tint, SpriteEffects.None, 0.8f);

            }
            _player.Draw(spriteBatch);

            if (Global.GameState == GameStates.Debugging || _map.IsInFov(_enemy.X, _enemy.Y))
            {
                _enemy.Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }



        private Cell GetRandomEmptyCell()
        {
            while(true)
            {
                int x = Global.Random.Next(49); int y = Global.Random.Next(29);
                if(_map.IsWalkable(x,y))
                {
                    return _map.GetCell(x, y);
                }
            }
        }

        private void UpdatePlayerFieldOfView()
        {
            _map.ComputeFov(_player.X, _player.Y, 30, true);
            foreach(Cell c in _map.GetAllCells())
            {
                if(_map.IsInFov(c.X, c.Y))
                {
                    _map.SetCellProperties(c.X, c.Y, c.IsTransparent, c.IsWalkable, true);
                }
            }
        }
    }
}
