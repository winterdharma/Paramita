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
        private int levelNumber = 0;

        private StatusMessages statuses;

        private Texture2D tilesheet;
        private Texture2D player_sprite;
        private Texture2D item_sprites;

        public int LevelNumber {
            get { return levelNumber; }
            private set { levelNumber = value; }
        }

        public StatusMessages Statuses { get { return statuses; } }

        // This is simply an alias for LevelManager.CurrentMap 
        public TileMap Map
        {
            get { return levelManager.CurrentMap; }
        }

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
            levelManager.CreateLevel(levelNumber);    
            
            player.CurrentTile = GetEmptyWalkableTile();

            ShortSword sword1 = levelManager.ItemCreator.CreateShortSword();
            ShortSword sword2 = levelManager.ItemCreator.CreateShortSword();
            GetEmptyWalkableTile().AddItem(sword1);
            GetEmptyWalkableTile().AddItem(sword2);
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


        // Sets LevelManager.CurrentMap to one level up and places the
        // the player on a StairsDown tile (the first found with a linear search)
        public void GoUpOneLevel()
        {
            levelNumber--;
            levelManager.SetLevel(levelNumber);
            player.CurrentTile = Map.FindStairsDownTile();
        }



        /*
         * If the next level down has already been created:
         *   Sets LevelManager.CurrentMap to one level up and places the
         *   the player on a StairsDown tile (the first found with a linear search)
         * Otherwise, creates a new level, sets it as LevelManager.CurrentMap
         *   and places the player on the first StairsUp tile found
         */
        public void GoDownOneLevel()
        {
            levelNumber++;

            if (levelManager.GetLevels().Count == levelNumber)
                levelManager.CreateLevel(levelNumber);
            else
                levelManager.SetLevel(levelNumber);

            player.CurrentTile = Map.FindStairsUpTile();
        }





        // These are methods from tutorials that might be used in future
        public void LoadSavedGame()
        {
            // not yet implemented
        }
    }
}
