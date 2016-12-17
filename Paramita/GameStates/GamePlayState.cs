using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Paramita.Components;
using Paramita.StateManagement;
using Paramita.TileMapEngine;

namespace Paramita.GameStates
{
    public interface IGamePlayState : IGameState
    {
        void SetUpNewGame();
        void LoadSavedGame();
        void StartGame();
    }

    public class GamePlayState : BaseGameState, IGamePlayState
    {
        TileEngine engine = new TileEngine(GameController.ScreenRectangle, 32, 32);
        TileMap map;
        Camera camera;
        Player player;



        public GamePlayState(GameController game) : base(game)
        {
            game.Services.AddService(typeof(IGamePlayState), this);
        }



        public override void Initialize()
        {
            base.Initialize();
        }


        protected override void LoadContent()
        {
            Texture2D spriteSheet = content.Load<Texture2D>("maleplayer");
            player = new Player(GameRef, "Wesley", false, spriteSheet);
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
            Texture2D tiles = GameRef.Content.Load<Texture2D>("tileset1");
            TileSet set = new TileSet(8, 8, 32, 32);
            set.Texture = tiles;

            TileLayer background = new TileLayer(200, 200);
            TileLayer edge = new TileLayer(200, 200);
            TileLayer building = new TileLayer(200, 200);
            TileLayer decor = new TileLayer(200, 200);

            map = new TileMap(set, background, edge, building, decor, "test-map");

            map.FillEdges();
            map.FillBuilding();
            map.FillDecoration();
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
