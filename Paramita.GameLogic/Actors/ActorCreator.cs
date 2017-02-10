using Paramita.GameLogic.Actors.Animals;

namespace Paramita.GameLogic.Actors
{


    public static class ActorCreator
    {
        public static Player CreateHumanPlayer()
        {
            return new Player("Wesley");
        }

        public static GiantRat CreateGiantRat()
        {
            return new GiantRat();
        }
    }
}
