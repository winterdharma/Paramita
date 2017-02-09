using Microsoft.Xna.Framework.Graphics;
using Paramita.Levels;
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
       


        public static Dictionary<BeingType, Texture2D> Spritesheets
        {
            get { return spritesheets; }
            set { spritesheets = value; }
        }


        public static Player CreateHumanPlayer(Level level)
        {
            return new Player(level, "Wesley", spritesheets[BeingType.HumanPlayer]);
        }

        public static GiantRat CreateGiantRat(Level level)
        {
            return new GiantRat(level, spritesheets[BeingType.GiantRat]);
        }
    }
}
