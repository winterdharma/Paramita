using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Items;
using Paramita.Items.Weapons;
using Paramita.Scenes.Game;
using Paramita.SentientBeings;
using System.Collections.Generic;

namespace Paramita.Scenes
{

    public class GameScene : Scene
    {
        private LevelManager levelManager;
        private CombatManager combatManager;
        private Camera camera;
        private Player player;
        private List<SentientBeing> npcs;
        private int levelNumber;

        private StatusMessages statuses;
        private Inventory inventoryPanel;

        private Texture2D tilesheet;
        private Texture2D player_sprite;
        private Texture2D item_sprites;
        private Texture2D sentientbeing_sprites;
        private Texture2D inventory_background;

        public Player Player { get { return player; } }

        public int LevelNumber {
            get { return levelNumber; }
            private set { levelNumber = value; }
        }

        public TileMap Map { get { return levelManager.CurrentMap; } }



        public GameScene(GameController game) : base(game)
        {
            camera = new Camera();
        }



        public override void Initialize()
        {
            base.Initialize(); // This calls LoadContent()

            player = new Player(GameRef, "Wesley", sentientbeing_sprites,
                new Rectangle(0,32,32,32), new Rectangle(32,32,32,32));
            player.Initialize();
            npcs = new List<SentientBeing>();

            SentientBeingCreator.Sprites = sentientbeing_sprites;
            SentientBeingCreator.gameScene = this;
            ItemCreator.Sprites = item_sprites;

            levelManager = new LevelManager(
                GameRef,
                new TileSet("tileset1", tilesheet, 8, 8, 32),
                GameController.random);
            combatManager = new CombatManager(GameController.random);
            statuses = new StatusMessages(GameRef.Font, 10, new Point(0,720));
            inventoryPanel = new Inventory(GameRef.Font, GameRef.ScreenRectangle, GameRef.InputDevices, player, inventory_background, player.Items.Length);
        }



        protected override void LoadContent()
        {
            tilesheet = content.Load<Texture2D>("tileset1");
            item_sprites = content.Load<Texture2D>("item_sprites");
            sentientbeing_sprites = content.Load<Texture2D>("sentientbeing_sprites");
            player_sprite = content.Load<Texture2D>("maleplayer");
            inventory_background = content.Load<Texture2D>("black_background1");
        }



        public override void Update(GameTime gameTime)
        {
            statuses.Update(gameTime);
            inventoryPanel.Update(gameTime);
            player.Update(gameTime);

            for(int x = 0; x < npcs.Count; x++)
            {
                npcs[x].Update(gameTime);
            }

            camera.LockToSprite(Map, player.Position, GameRef.ScreenRectangle);

            base.Update(gameTime);
        }



        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if(Map != null && camera != null)
            {
                Map.Draw(gameTime, GameRef.SpriteBatch, camera);
            }

            player.Draw(gameTime, GameRef.SpriteBatch, camera);

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
            var shield = ItemCreator.CreateShield();
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



        public void PostNewStatus(string message)
        {
            statuses.AddMessage(message);
        }
    }
}
