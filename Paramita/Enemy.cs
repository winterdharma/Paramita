using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Paramita
{
    class Enemy
    {
        private readonly PathToPlayer _path;
        public int X { get; set; }
        public int Y { get; set; }
        public Texture2D Sprite { get; set; }

        public Enemy(PathToPlayer path)
        {
            _path = path;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, new Vector2(X * Sprite.Width, Y * Sprite.Height),
                null, null, null, 0.0f, Vector2.One, Color.White, 
                SpriteEffects.None, LayerDepth.Sprites);
            _path.Draw(spriteBatch);
        }

        public void Update()
        {
            _path.CreateFrom(X, Y);
            X = _path.FirstCell.X;
            Y = _path.FirstCell.Y;
        }
    }
}
