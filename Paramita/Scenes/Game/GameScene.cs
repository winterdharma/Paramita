using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Items;
using Paramita.Items.Weapons;
using Paramita.Levels;
using Paramita.Scenes.Game;
using Paramita.SentientBeings;
using Paramita.UI;
using System.Collections.Generic;

namespace Paramita.Scenes
{

    public class GameScene : Scene
    {
        private LevelManager levelManager;
        private Player player;
        
        private int levelNumber;
        private Level currentLevel;
        private static StatusMessages statuses;
        private Inventory inventoryPanel;

        private Texture2D tilesheet;
        private Texture2D inventory_background;

       

        public Player Player { get { return player; } }

        public int LevelNumber {
            get { return levelNumber; }
            private set { levelNumber = value; }
        }

        public TileMap Map { get { return CurrentLevel.TileMap; } }
        public Level CurrentLevel
        {
            get { return currentLevel; }
            set { currentLevel = value; }
        }

        public GameScene(GameController game) : base(game)
        {
        }



        public override void Initialize()
        {
            base.Initialize(); // This calls LoadContent()

            SentientBeingCreator.gameScene = this;
            player = SentientBeingCreator.CreateHumanPlayer();

            levelManager = new LevelManager(
                GameRef,
                new TileSet("tileset1", tilesheet, 8, 8, 32),
                GameController.random);
            
            statuses = new StatusMessages(GameController.ArialBold, 10, new Point(0,720));
            inventoryPanel = new Inventory(player, inventory_background, 10);
        }



        protected override void LoadContent()
        {
            tilesheet = content.Load<Texture2D>("tileset1");

            ItemCreator.Spritesheets.Add(ItemType.Coins, content.Load<Texture2D>("Images\\Items\\coins"));
            ItemCreator.Spritesheets.Add(ItemType.Meat, content.Load<Texture2D>("Images\\Items\\meat"));
            ItemCreator.Spritesheets.Add(ItemType.Shield, content.Load<Texture2D>("Images\\Items\\buckler"));
            ItemCreator.Spritesheets.Add(ItemType.ShortSword, content.Load<Texture2D>("Images\\Items\\short_sword"));

            SentientBeingCreator.Spritesheets.Add(BeingType.GiantRat, content.Load<Texture2D>("Images\\SentientBeings\\giant_rat"));
            SentientBeingCreator.Spritesheets.Add(BeingType.HumanPlayer, content.Load<Texture2D>("Images\\SentientBeings\\human_player"));

            inventory_background = content.Load<Texture2D>("black_background1");
        }



        public override void Update(GameTime gameTime)
        {
            // update the UI panels
            statuses.Update(gameTime);
            inventoryPanel.Update(gameTime);
            currentLevel.Update(gameTime);

            

            //move the camera to center on the player
            Camera.LockToSprite(Map, player.Sprite.Position, GameController.ScreenRectangle);

            base.Update(gameTime);
        }



        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            CurrentLevel.Draw(gameTime, GameRef.SpriteBatch);
            

            statuses.Draw(gameTime, GameRef.SpriteBatch);
            inventoryPanel.Draw(gameTime, GameRef.SpriteBatch);
        }



        // This is called from the MenuScene and creates the first level.
        // Currently, this is a simple test level. The item creation should
        // be moved to the LevelManager when the level generation fully
        // implemented.
        public void SetUpNewGame()
        {
            levelNumber = 1;
            levelManager.Create(levelNumber, player);
        }



        



        /*
         * Changes the current level given the TileType the player moved onto.
         * Calls methods that respond to the TileType, sets the new Level, places
         * the player on a starting tile, and sends a message to the status area.
         */
        public void ChangeLevel(int change)
        {
            levelNumber += change;

            levelManager.MoveToLevel(levelNumber);

            TileType startTile = TileType.StairsUp;

            if (change < 0)
                startTile = TileType.StairsDown;

            player.CurrentTile = Map.FindTileType(startTile);
            statuses.AddMessage("You are now on level " + levelNumber + ".");
        }



        // These are methods from tutorials that might be used in future
        public void LoadSavedGame()
        {
            // not yet implemented
        }



        public static void PostNewStatus(string message)
        {
            statuses.AddMessage(message);
        }
    }
}
