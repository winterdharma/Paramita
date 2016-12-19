using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Paramita.Components;
using Paramita.SentientBeings;

namespace Paramita.Scenes
{

    public class GameScene : Scene
    {
        TileEngine engine = new TileEngine(GameController.ScreenRectangle, 32, 32);
        TileMapCreator mapCreator;
        TileMap map;
        TileSet tileset;
        Camera camera;
        Player player;

        public TileMap Map { get; private set; }
        public TileSet TileSet { get; private set; }

        public GameScene(GameController game) : base(game)
        {
            
        }



        public override void Initialize()
        {
            base.Initialize();
        }


        protected override void LoadContent()
        {
            
            
            Texture2D tileset1 = content.Load<Texture2D>("tileset1");
            TileSet = new TileSet("tileset1", tileset1, 8, 8, 32);
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
            Texture2D tileTextures = GameRef.Content.Load<Texture2D>("tileset1");
            tileset = new TileSet("tileset1", tileTextures, 8, 8, 32);
            mapCreator = new TileMapCreator(80, 80, 10, 20, 10, random);
            Map = new TileMap(tileset, mapCreator.CreateMap(), 80, 80, "test-map");

            
            Texture2D spriteSheet = content.Load<Texture2D>("maleplayer");
            player = new Player(GameRef, "Wesley", false, spriteSheet);
            Tile startTile = GetEmptyWalkableTile();
            player.Position = new Vector2(
                startTile.X * tileset.TileSize,
                startTile.Y * tileset.TileSize);

            camera = new Camera();
        }

        // returns a suitable starting tile for the player or enemy
        // Does not check for empty state yet
        private Tile GetEmptyWalkableTile()
        {
            while (true)
            {
                int x = random.Next(Map.MapWidth-1);
                int y = random.Next(Map.MapHeight-1);
                if (Map.IsTileWalkable(x, y))
                {
                    return Map.GetTile(x, y);
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
