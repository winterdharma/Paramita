using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Items;
using Paramita.Items.Weapons;
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
        private List<SentientBeing> npcs;
        private int levelNumber;

        private static StatusMessages statuses;
        private Inventory inventoryPanel;

        private Texture2D tilesheet;
        private Texture2D inventory_background;

        private bool isPlayersTurn = true;

        public bool IsPlayersTurn
        {
            get { return isPlayersTurn; }
            set { isPlayersTurn = value; }
        }

        public Player Player { get { return player; } }

        public int LevelNumber {
            get { return levelNumber; }
            private set { levelNumber = value; }
        }

        public TileMap Map { get { return levelManager.CurrentMap; } }


        public GameScene(GameController game) : base(game)
        {
        }



        public override void Initialize()
        {
            base.Initialize(); // This calls LoadContent()

            npcs = new List<SentientBeing>();

            SentientBeingCreator.gameScene = this;
            player = SentientBeingCreator.CreateHumanPlayer();

            levelManager = new LevelManager(
                GameRef,
                new TileSet("tileset1", tilesheet, 8, 8, 32),
                GameController.random);
            
            statuses = new StatusMessages(GameController.ArialBold, 10, new Point(0,720));
            inventoryPanel = new Inventory(GameRef.ScreenRectangle, player, inventory_background, 10);
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

            // remove dead npcs before updating them
            for (int x = 0; x < npcs.Count; x++)
            {
                if (npcs[x].IsDead == true)
                {
                    npcs.Remove(npcs[x]);
                }
            }

            // check for player's input until he moves
            if (isPlayersTurn == true)
            {
                player.Update(gameTime);
            }
            // give the npcs a turn after the player moves
            else
            {
                for (int x = 0; x < npcs.Count; x++)
                {
                    npcs[x].TimesAttacked = 0;
                    npcs[x].Update(gameTime);
                }
                isPlayersTurn = true;
                player.TimesAttacked = 0;
            }

            //move the camera to center on the player
            Camera.LockToSprite(Map, player.Sprite.Position, GameRef.ScreenRectangle);

            base.Update(gameTime);
        }



        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if(Map != null)
            {
                Map.Draw(gameTime, GameRef.SpriteBatch);
            }

            player.Draw(gameTime, GameRef.SpriteBatch);

            for(int x = 0; x < npcs.Count; x++)
            {
                npcs[x].Draw(gameTime, GameRef.SpriteBatch);
            }

            statuses.Draw(gameTime, GameRef.SpriteBatch);
            inventoryPanel.Draw(gameTime, GameRef.SpriteBatch);
        }



        // This is called from the MenuScene and creates the first level.
        // Currently, this is a simple test level. The item creation should
        // be moved to the LevelManager when the level generation fully
        // implemented.
        public void SetUpNewGame()
        {
            LevelNumber = 1;
            levelManager.MoveToLevel(levelNumber);
            player.CurrentTile = Map.FindTileType(TileType.StairsUp);

            var giantRat = SentientBeingCreator.CreateGiantRat();
            npcs.Add(giantRat);
            giantRat.CurrentTile = GetEmptyWalkableTile();

            ShortSword sword = ItemCreator.CreateShortSword();
            GetEmptyWalkableTile().AddItem(sword);

            var coins = ItemCreator.CreateCoins(1);
            GetEmptyWalkableTile().AddItem(coins);
            var meat = ItemCreator.CreateMeat();
            GetEmptyWalkableTile().AddItem(meat);
            var shield = ItemCreator.CreateBuckler();
            GetEmptyWalkableTile().AddItem(shield);
        }



        // returns a suitable starting tile for the player or enemy
        // Does not check for empty state yet
        private Tile GetEmptyWalkableTile()
        {
            while (true)
            {
                int x = GameController.random.Next(Map.TilesWide-1);
                int y = GameController.random.Next(Map.TilesHigh-1);
                if (Map.IsTileWalkable(x, y))
                {
                    return Map.GetTile(new Point(x, y));
                }
            }
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



        // Iterates over the @npcs list of active NPCs to find if one of them is
        // currently on @tile
        public bool IsNpcOnTile(Tile tile)
        {
            for(int x = 0; x < npcs.Count; x++)
            {
                if(npcs[x].CurrentTile == tile)
                {
                    return true;
                }
            }
            return false;
        }



        // Finds the NPC that is located on @tile. If none is there, null is returned
        // (this method should be called after IsNpcOnTile check)
        public SentientBeing GetNpcOnTile(Tile tile)
        {
            for (int x = 0; x < npcs.Count; x++)
            {
                if (npcs[x].CurrentTile == tile)
                {
                    return npcs[x];
                }
            }
            return null;
        }
    }
}
