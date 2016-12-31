using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Items;
using Paramita.Scenes;
using Paramita.SentientBeings.Animals;
using System.Collections.Generic;

namespace Paramita.SentientBeings
{
    public enum SentientBeingType
    {
        GiantRat
    }

    public static class SentientBeingCreator
    {
        private static Texture2D sprites;
        private static Rectangle[] spriteMap = new Rectangle[2] {
                new Rectangle(0,0,32,32),
                new Rectangle(31,0,32,32)
            };
        public static GameScene gameScene;


        public static Texture2D Sprites
        {
            get { return sprites; }
            set { sprites = value; }
        }



        public static GiantRat CreateGiantRat()
        {
            return new GiantRat(gameScene, sprites, spriteMap[(int)SentientBeingType.GiantRat],
                spriteMap[(int)SentientBeingType.GiantRat + 1]);
        }
    }
}
