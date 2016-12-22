using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Items;
using Paramita.Items.Weapons;
using Paramita.SentientBeings;

namespace Paramita.Scenes
{

    public class GameScene : Scene
    {
        TileEngine engine = new TileEngine(GameController.ScreenRectangle, 32, 32);
        TileMapCreator mapCreator;
        ItemCreator itemCreator;
        Camera camera;
        Player player;

        Texture2D tilesheet;
        Texture2D player_sprite;
        Texture2D item_sprites;

        public TileMap Map { get; private set; }
        public TileSet TileSet { get; private set; }

        public GameScene(GameController game) : base(game)
        {
            
        }



        public override void Initialize()
        {
            base.Initialize();
            itemCreator = new ItemCreator(item_sprites);
            TileSet = new TileSet("tileset1", tilesheet, 8, 8, 32);
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
            mapCreator = new TileMapCreator(80, 80, 10, 20, 10, random);
            Map = new TileMap(TileSet, mapCreator.CreateMap(), 80, 80, "test-map");

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
                int x = random.Next(Map.TilesWide-1);
                int y = random.Next(Map.TilesHigh-1);
                if (Map.IsTileWalkable(x, y))
                {
                    return Map.GetTile(new Point(x, y));
                }
            }
        }

        public void LoadSavedGame()
        {

        }

        public void StartGame()
        {

        }
    }
}
