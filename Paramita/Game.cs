using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueSharp;
using RogueSharp.DiceNotation;
using RogueSharp.MapCreation;
using RogueSharp.Random;
using System;
using System.Collections.Generic;

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
        private List<Enemy> _enemies = new List<Enemy>();

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
                new RandomRoomsMapCreationStrategy<Map>(Global.MapWidth, Global.MapHeight, 100, 7,3);
            _map = Map.Create(mapCreationStrategy);
            Console.WriteLine(_map.ToString());

            Global.Camera.ViewportWidth = graphics.GraphicsDevice.Viewport.Width;
            Global.Camera.ViewportHeight = graphics.GraphicsDevice.Viewport.Height;

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
            Global.Camera.CenterOn(startingCell);
            _player = new Player
            {
                X = startingCell.X,
                Y = startingCell.Y,
                Sprite = Content.Load<Texture2D>("player"),
                ArmorClass = 15,
                AttackBonus = 1,
                Damage = Dice.Parse("2d4"),
                Health = 50,
                Name = "Mr. Rogue"
            };
            startingCell = GetRandomEmptyCell();

            PathToPlayer pathFromEnemy = new PathToPlayer(_player, _map, Content.Load<Texture2D>("white"));
            pathFromEnemy.CreateFrom(startingCell.X, startingCell.Y);

            AddEnemies(10);
            Global.CombatManager = new CombatManager(_player, _enemies);
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
                    Global.Camera.CenterOn(_map.GetCell(_player.X, _player.Y));
                    Global.GameState = GameStates.EnemyTurn;
                }
                if (Global.GameState == GameStates.EnemyTurn)
                {
                    foreach (Enemy e in _enemies)
                    {
                        e.Update();
                    }
                    Global.GameState = GameStates.PlayerTurn;
                }
            }


            // TODO: Add your update logic here
            _inputState.Update();
            Global.Camera.HandleInput(_inputState, PlayerIndex.One);
            base.Update(gameTime);
        }


        /// This is called when the game should draw itself.
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend,
                null,null,null,null,Global.Camera.TranslationMatrix);

            foreach(Cell c in _map.GetAllCells() )
            {
                var position = new Vector2(c.X * Global.SpriteWidth, c.Y * Global.SpriteHeight);

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
                        null, null, null, 0.0f, Vector2.One,
                        tint, SpriteEffects.None, LayerDepth.Cells);
            }
            _player.Draw(spriteBatch);

            foreach (Enemy e in _enemies)
            {
                if (Global.GameState == GameStates.Debugging || _map.IsInFov(e.X, e.Y))
                {
                    e.Draw(spriteBatch);
                }
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

        private void AddEnemies(int numberOfEnemies)
        {
            for (int i = 0; i < numberOfEnemies; i++)
            {
                // Find a new empty cell for each enemy
                Cell enemyCell = GetRandomEmptyCell();
                var pathFromEnemy =
                  new PathToPlayer(_player, _map, Content.Load<Texture2D>("White"));
                pathFromEnemy.CreateFrom(enemyCell.X, enemyCell.Y);
                var enemy = new Enemy(_map, pathFromEnemy)
                {
                    X = enemyCell.X,
                    Y = enemyCell.Y,
                    Sprite = Content.Load<Texture2D>("hound"),
                    ArmorClass = 10,
                    AttackBonus = 0,
                    Damage = Dice.Parse("d4"),
                    Health = 10,
                    Name = "Hell Hound"
                };
                // Add each enemy to list of enemies
                _enemies.Add(enemy);
            }
        }
    }
}
