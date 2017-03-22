using Paramita.GameLogic.Actors.Animals;

namespace Paramita.GameLogic.Actors
{


    public static class ActorCreator
    {
        public static Actor CreateActor(BeingType type)
        {
            if (type == BeingType.GiantRat)
                return CreateGiantRat();
            else
                return CreateHumanPlayer();
        }

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
