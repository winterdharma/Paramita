using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita
{
    public class Player
    {
        // coordinates of the map cell the player is located. 
        // (0,0) = top left corner
        public int X { get; set; }
        public int Y { get; set; }
        // used to resize the original sprite image
        public float Scale { get; set; }
        public Texture2D Sprite { get; set; }

        public void Draw( SpriteBatch spriteBatch )
        {
            float multiplier = Scale * Sprite.Width;
            spriteBatch.Draw(Sprite, new Vector2(X * multiplier, Y * multiplier),
                null, null, null, 0.0f, new Vector2(Scale, Scale),
                Color.White, SpriteEffects.None, 0.5f); 
            // 0.5f = layer depth, places it on top of map tiles
        } 
    }
}
