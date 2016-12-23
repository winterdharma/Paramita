using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Items;
using Paramita.Items.Weapons;
using Paramita.SentientBeings;

namespace Paramita.Scenes
{

    public class GameScene : Scene
    {
        private LevelManager levelManager;
        private ItemCreator itemCreator;
        private Camera camera;
        private Player player;
        private int levelNumber = 0;

        private Texture2D tilesheet;
        private Texture2D player_sprite;
        private Texture2D item_sprites;

        public int LevelNumber {
            get { return levelNumber; }
            private set { levelNumber = value; }
        }

        public TileMap Map { get; private set; }

        public GameScene(GameController game) : base(game)
        {
            
        }



        public override void Initialize()
        {
            base.Initialize(); // This calls LoadContent()

            itemCreator = new ItemCreator(item_sprites);
            levelManager = new LevelManager(
                new TileSet("tileset1", tilesheet, 8, 8, 32),
                GameController.random);
        }


        protected override void LoadContent()
        {
            tilesheet = content.Load<Texture2D>("tileset1");
            item_sprites = content.Load<Texture2D>("item_sprites");
            player_sprite = content.Load<Texture2D>("maleplayer");
        }


        public override void Update(GameTime gameTime)
        {
            
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

            GameRef.SpriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,null,null,
                camera.Transformation);
            player.Sprite.Draw(gameTime, GameRef.SpriteBatch);
            GameRef.SpriteBatch.End();
        }

        public void SetUpNewGame()
        {
            Map = levelManager.CreateLevel(levelNumber);    
            player = new Player(GameRef, "Wesley", false, player_sprite);
            player.CurrentTile = GetEmptyWalkableTile();

            ShortSword sword1 = itemCreator.CreateShortSword();
            ShortSword sword2 = itemCreator.CreateShortSword();
            GetEmptyWalkableTile().AddItem(sword1);
            GetEmptyWalkableTile().AddItem(sword2);

            camera = new Camera();
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


        // These methods handle moving between levels, up and down
        public void GoUpOneLevel()
        {
            levelNumber--;
            Map = levelManager.GetLevel(levelNumber);
            player.CurrentTile = Map.FindStairsDownTile();
        }


        public void GoDownOneLevel()
        {
            TileMap nextLevel = null;
            levelNumber++;
            if (levelManager.GetLevels().Count == levelNumber)
            {
                nextLevel = levelManager.CreateLevel(levelNumber);
            }
            else
            {
                nextLevel = levelManager.GetLevel(levelNumber);
            }

            Map = nextLevel;
            player.CurrentTile = Map.FindStairsUpTile();
        }





        // These are methods from tutorials that might be used in future
        public void LoadSavedGame()
        {
            // not yet implemented
        }

        public void StartGame()
        {
            // not yet implemented
        }
    }
}
