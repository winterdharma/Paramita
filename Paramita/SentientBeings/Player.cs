using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Components;
using Paramita.Scenes;

namespace Paramita.SentientBeings
{
    public class Player : SentientBeing
    {
        private GameController gameRef;
        private string name;
        private bool gender;
        private string mapName;
        private Point tile;
        private AnimatedSprite sprite;
        private Texture2D texture;
        private Vector2 position;

        public Vector2 Position
        {
            get { return sprite.Position; }
            set { sprite.Position = value; }
        }

        public AnimatedSprite Sprite
        {
            get { return sprite; }
        }




        private Player(GameController game) : base(game) { }

        public Player(GameController game, string name, bool gender, Texture2D texture) : base(game)
        {
            gameRef = game;
            this.name = name;
            this.gender = gender;
            this.texture = texture;
            sprite = new AnimatedSprite(texture, gameRef.PlayerAnimations);
            sprite.CurrentAnimation = AnimationKey.WalkDown;
        }




        public void SavePlayer() { }

        public static Player Load(GameController game)
        {
            Player player = new Player(game);
            return player;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            HandleInput();
            Sprite.Update(gameTime);
            base.Update(gameTime);
        }

        private void HandleInput()
        {
            Vector2 motion = Vector2.Zero;

            if (gameRef.InputDevices.IsUpperLeft())
            {
                motion.X = -1; motion.Y = -1;
                Sprite.CurrentAnimation = AnimationKey.WalkLeft;
            }
            else if (gameRef.InputDevices.IsUpperRight())
            {
                motion.X = 1; motion.Y = -1;
                Sprite.CurrentAnimation = AnimationKey.WalkRight;
            }
            else if (gameRef.InputDevices.IsLowerLeft())
            {
                motion.X = -1; motion.Y = 1;
                Sprite.CurrentAnimation = AnimationKey.WalkLeft;
            }
            else if (gameRef.InputDevices.IsLowerRight())
            {
                motion.X = 1; motion.Y = 1;
                Sprite.CurrentAnimation = AnimationKey.WalkRight;
            }
            else if (gameRef.InputDevices.IsUp())
            {
                motion.Y = -1;
                Sprite.CurrentAnimation = AnimationKey.WalkUp;
            }
            else if (gameRef.InputDevices.IsDown())
            {
                motion.Y = 1;
                Sprite.CurrentAnimation = AnimationKey.WalkDown;
            }
            else if (gameRef.InputDevices.IsRight())
            {
                motion.X = -1;
                Sprite.CurrentAnimation = AnimationKey.WalkLeft;
            }
            else if (gameRef.InputDevices.IsLeft())
            {
                motion.X = 1;
                Sprite.CurrentAnimation = AnimationKey.WalkRight;
            }

            if (motion != Vector2.Zero)
            {
                //motion.Normalize();
                motion *= gameRef.GameScene.TileSet.TileSize;
                Vector2 newPosition = Sprite.Position + motion;
                Sprite.Position = newPosition;
                Sprite.IsAnimating = true;
                Sprite.LockToMap(new Point(gameRef.GameScene.Map.WidthInPixels, 
                    gameRef.GameScene.Map.HeightInPixels));
            }
        }


        public override void Draw( GameTime gameTime )
        {
            base.Draw(gameTime);
        }

    }
}
