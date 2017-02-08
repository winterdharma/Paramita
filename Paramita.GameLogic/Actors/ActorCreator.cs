using Paramita.GameLogic.Levels;
using Paramita.GameLogic.Actors.Animals;

namespace Paramita.GameLogic.Actors
{
    public enum BeingType
    {
        GiantRat,
        HumanPlayer
    }

    public static class ActorCreator
    {
        public static Player CreateHumanPlayer(Level level)
        {
            return new Player("Wesley");
        }

        public static GiantRat CreateGiantRat(Level level)
        {
            return new GiantRat();
        }
    }
}
