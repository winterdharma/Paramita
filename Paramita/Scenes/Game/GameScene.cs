using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Paramita.Components;

namespace Paramita.Scenes
{

    public class GameScene : Scene
    {
        TileEngine engine = new TileEngine(GameController.ScreenRectangle, 32, 32);
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
            Texture2D spriteSheet = content.Load<Texture2D>("maleplayer");
            player = new Player(GameRef, "Wesley", false, spriteSheet);
            Texture2D tileset1 = content.Load<Texture2D>("tileset1");
            tileset = new TileSet("tileset1", tileset1, 8, 8, 32);
        }


        public override void Update(GameTime gameTime)
        {
            Vector2 motion = Vector2.Zero;

            if(InputDevices.KeyboardState.IsKeyDown(Keys.W) && InputDevices.KeyboardState.IsKeyDown(Keys.A))
            {
                motion.X = -1; motion.Y = -1;
                player.Sprite.CurrentAnimation = AnimationKey.WalkLeft;
            }
            else if(InputDevices.KeyboardState.IsKeyDown(Keys.W) && InputDevices.KeyboardState.IsKeyDown(Keys.D))
            {
                motion.X = 1; motion.Y = -1;
                player.Sprite.CurrentAnimation = AnimationKey.WalkRight;
            }
            else if(InputDevices.KeyboardState.IsKeyDown(Keys.S) && InputDevices.KeyboardState.IsKeyDown(Keys.A))
            {
                motion.X = -1; motion.Y = 1;
                player.Sprite.CurrentAnimation = AnimationKey.WalkLeft;
            }
            else if (InputDevices.KeyboardState.IsKeyDown(Keys.S) && InputDevices.KeyboardState.IsKeyDown(Keys.D))
            {
                motion.X = 1; motion.Y = 1;
                player.Sprite.CurrentAnimation = AnimationKey.WalkRight;
            }
            else if (InputDevices.KeyboardState.IsKeyDown(Keys.W))
            {
                motion.Y = -1;
                player.Sprite.CurrentAnimation = AnimationKey.WalkUp;
            }
            else if (InputDevices.KeyboardState.IsKeyDown(Keys.S))
            {
                motion.Y = 1;
                player.Sprite.CurrentAnimation = AnimationKey.WalkDown;
            }
            else if (InputDevices.KeyboardState.IsKeyDown(Keys.A))
            {
                motion.X = -1;
                player.Sprite.CurrentAnimation = AnimationKey.WalkLeft;
            }
            else if (InputDevices.KeyboardState.IsKeyDown(Keys.D))
            {
                motion.X = 1;
                player.Sprite.CurrentAnimation = AnimationKey.WalkRight;
            }

            if(motion != Vector2.Zero)
            {
                motion.Normalize();
                motion *= player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
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

            map = new TileMap(tileset, 50, 50, "test-map");

            camera = new Camera();
        }

        public void LoadSavedGame()
        {

        }

        public void StartGame()
        {

        }
    }
}
