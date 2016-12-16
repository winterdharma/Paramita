﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Paramita.Components;
using Paramita.GameStates;
using Paramita.StateManagement;
using Paramita.UI;
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
    public class GameController : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameStateManager gStateManager;
        ITitleIntroState titleIntroState;
        IMainMenuState startMenuState;

        static Microsoft.Xna.Framework.Rectangle screenRectangle;

        public SpriteBatch SpriteBatch { get { return spriteBatch; } }
        public static Microsoft.Xna.Framework.Rectangle ScreenRectangle
        {
            get { return screenRectangle; }
        }
        public GameStateManager GameStateManager
        {
             get { return gStateManager; }
        }
        public ITitleIntroState TitleIntroState
        {
            get { return titleIntroState; }
        }
        public IMainMenuState StartMenuState
        {
            get { return startMenuState; }
        }

        private InputState _inputState;
        private IMap _map;
        private Texture2D _floor;
        private Texture2D _wall;
        private Player _player;
        private List<Enemy> _enemies = new List<Enemy>();

        //UI Values
        // The screen height and width are in number of tiles
        private static readonly int _screenWidth = 100;
        private static readonly int _screenHeight = 70;
        //private static RootConsole _rootConsole;

        // The map console takes up most of the screen and is where the map will be drawn
        private static readonly int _mapWidth = 80;
        private static readonly int _mapHeight = 48;
        private static GameConsole _mapConsole;

        // Below the map console is the message console which displays attack rolls and other information
        private static readonly int _messageWidth = 80;
        private static readonly int _messageHeight = 11;
        private static GameConsole _messageConsole;

        // The stat console is to the right of the map and display player and monster stats
        private static readonly int _statWidth = 20;
        private static readonly int _statHeight = 70;
        private static GameConsole _statConsole;

        // Above the map is the inventory console which shows the players equipment, abilities, and items
        private static readonly int _inventoryWidth = 80;
        private static readonly int _inventoryHeight = 11;
        private static GameConsole _inventoryConsole;




        public GameController()
        {
            graphics = new GraphicsDeviceManager(this);
            screenRectangle = new Microsoft.Xna.Framework.Rectangle(0, 0, 1280, 720);
            graphics.PreferredBackBufferWidth = ScreenRectangle.Width;
            graphics.PreferredBackBufferHeight = ScreenRectangle.Height;

            Content.RootDirectory = "Content";
            _inputState = new InputState();

            gStateManager = new GameStateManager(this);
            Components.Add(gStateManager);
            this.IsMouseVisible = true;

            titleIntroState = new TitleIntroState(this);
            startMenuState = new MainMenuState(this);

            gStateManager.ChangeState((TitleIntroState)titleIntroState, PlayerIndex.One);
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

            IMapCreationStrategy<Map> mapCreationStrategy =
                new RandomRoomsMapCreationStrategy<Map>(Global.MapWidth, Global.MapHeight, 100, 7,3);
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
            Global.GameState = OldGameStates.PlayerTurn;
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
                if( Global.GameState == OldGameStates.PlayerTurn)
                {
                    Global.GameState = OldGameStates.Debugging;
                }
                else if(Global.GameState == OldGameStates.Debugging)
                {
                    Global.GameState = OldGameStates.PlayerTurn;
                }
            }
            else
            {
                if (Global.GameState == OldGameStates.PlayerTurn
                    && _player.HandleInput(_inputState, _map))
                {
                    UpdatePlayerFieldOfView();
                   
                    Global.GameState = OldGameStates.EnemyTurn;
                }
                if (Global.GameState == OldGameStates.EnemyTurn)
                {
                    foreach (Enemy e in _enemies)
                    {
                        e.Update();
                    }
                    Global.GameState = OldGameStates.PlayerTurn;
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
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend,
                null,null,null,null,Global.Camera.Transformation);

            foreach(Cell c in _map.GetAllCells() )
            {
                var position = new Vector2(c.X * Global.SpriteWidth, c.Y * Global.SpriteHeight);

                if(c.IsExplored == false && Global.GameState != OldGameStates.Debugging)
                {
                    continue;
                }

                Color tint = Color.White;

                if (c.IsInFov == false && Global.GameState != OldGameStates.Debugging)
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
                if (Global.GameState == OldGameStates.Debugging || _map.IsInFov(e.X, e.Y))
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
