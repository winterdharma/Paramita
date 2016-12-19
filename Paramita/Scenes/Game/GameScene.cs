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
            tileset = new TileSet("tileset1", tileset1, 8, 8, 32);
        }


        public override void Update(GameTime gameTime)
        {
            Vector2 motion = Vector2.Zero;

            if(GameRef.InputDevices.IsUpperLeft())
            {
                motion.X = -1; motion.Y = -1;
                player.Sprite.CurrentAnimation = AnimationKey.WalkLeft;
            }
            else if(GameRef.InputDevices.IsUpperRight())
            {
                motion.X = 1; motion.Y = -1;
                player.Sprite.CurrentAnimation = AnimationKey.WalkRight;
            }
            else if(GameRef.InputDevices.IsLowerLeft())
            {
                motion.X = -1; motion.Y = 1;
                player.Sprite.CurrentAnimation = AnimationKey.WalkLeft;
            }
            else if (GameRef.InputDevices.IsLowerRight())
            {
                motion.X = 1; motion.Y = 1;
                player.Sprite.CurrentAnimation = AnimationKey.WalkRight;
            }
            else if (GameRef.InputDevices.IsUp())
            {
                motion.Y = -1;
                player.Sprite.CurrentAnimation = AnimationKey.WalkUp;
            }
            else if (GameRef.InputDevices.IsDown())
            {
                motion.Y = 1;
                player.Sprite.CurrentAnimation = AnimationKey.WalkDown;
            }
            else if (GameRef.InputDevices.IsRight())
            {
                motion.X = -1;
                player.Sprite.CurrentAnimation = AnimationKey.WalkLeft;
            }
            else if (GameRef.InputDevices.IsLeft())
            {
                motion.X = 1;
                player.Sprite.CurrentAnimation = AnimationKey.WalkRight;
            }

            if(motion != Vector2.Zero)
            {
                //motion.Normalize();
                motion *= tileset.TileSize;
                Vector2 newPosition = player.Sprite.Position + motion;
                player.Sprite.Position = newPosition;
                player.Sprite.IsAnimating = true;
                player.Sprite.LockToMap(new Point(map.WidthInPixels, map.HeightInPixels));
            }

            camera.LockToSprite(map, player.Sprite, GameController.ScreenRectangle);
            player.Sprite.Update(gameTime);

            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if(map != null && camera != null)
            {
                map.Draw(gameTime, GameRef.SpriteBatch, camera);
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
            map = new TileMap(tileset, mapCreator.CreateMap(), 80, 80, "test-map");

            
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
                int x = random.Next(map.MapWidth-1);
                int y = random.Next(map.MapHeight-1);
                if (map.IsTileWalkable(x, y))
                {
                    return map.GetTile(x, y);
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
