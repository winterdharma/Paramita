using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Scenes;

namespace Paramita
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
        private float speed = 180f;
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

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
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
            base.Update(gameTime);
        }

        public override void Draw( GameTime gameTime )
        {
            base.Draw(gameTime);
        }
    }
}
