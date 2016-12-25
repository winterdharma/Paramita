using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Items;
using Paramita.Items.Weapons;
using Paramita.Scenes.Game;
using Paramita.SentientBeings;

namespace Paramita.Scenes
{

    public class GameScene : Scene
    {
        private LevelManager levelManager;
        private Camera camera;
        private Player player;
        private int levelNumber;

        private StatusMessages statuses;

        private Texture2D tilesheet;
        private Texture2D player_sprite;
        private Texture2D item_sprites;

        public int LevelNumber {
            get { return levelNumber; }
            private set { levelNumber = value; }
        }

        public StatusMessages Statuses { get { return statuses; } }

        public TileMap Map { get { return levelManager.CurrentMap; } }



        public GameScene(GameController game) : base(game)
        {
            camera = new Camera();
        }



        public override void Initialize()
        {
            base.Initialize(); // This calls LoadContent()

            player = new Player(GameRef, "Wesley", false, player_sprite);
            player.Initialize();

            levelManager = new LevelManager(
                new TileSet("tileset1", tilesheet, 8, 8, 32),
                new ItemCreator(item_sprites),
                GameController.random);

            statuses = new StatusMessages(GameRef.Font, 10, new Point(0,720));
        }



        protected override void LoadContent()
        {
            tilesheet = content.Load<Texture2D>("tileset1");
            item_sprites = content.Load<Texture2D>("item_sprites");
            player_sprite = content.Load<Texture2D>("maleplayer");
        }



        public override void Update(GameTime gameTime)
        {
            statuses.Update(gameTime);
            player.Update(gameTime);
            camera.LockToSprite(Map, player.Sprite, GameController.ScreenRectangle);

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
            statuses.Draw(gameTime, GameRef.SpriteBatch);
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

            ShortSword sword = levelManager.ItemCreator.CreateShortSword();
            GetEmptyWalkableTile().AddItem(sword);

            var coins = levelManager.ItemCreator.CreateCoins(1);
            GetEmptyWalkableTile().AddItem(coins);
            coins = levelManager.ItemCreator.CreateCoins(100);
            GetEmptyWalkableTile().AddItem(coins);
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
    }
}
