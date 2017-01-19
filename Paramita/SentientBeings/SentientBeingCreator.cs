using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Items;
using Paramita.Scenes;
using Paramita.SentientBeings.Animals;
using System.Collections.Generic;

namespace Paramita.SentientBeings
{
    public enum BeingType
    {
        GiantRat,
        HumanPlayer
    }

    public static class SentientBeingCreator
    {
        private static Dictionary<BeingType, Texture2D> spritesheets = new Dictionary<BeingType, Texture2D>();
       
        public static GameScene gameScene;


        public static Dictionary<BeingType, Texture2D> Spritesheets
        {
            get { return spritesheets; }
            set { spritesheets = value; }
        }


        public static Player CreateHumanPlayer()
        {
            return new Player(gameScene, "Wesley", spritesheets[BeingType.HumanPlayer]);
        }

        public static GiantRat CreateGiantRat()
        {
            return new GiantRat(gameScene, spritesheets[BeingType.GiantRat]);
        }
    }
}
