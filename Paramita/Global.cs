using Paramita.Scenes;
using Paramita.SentientBeings;
using RogueSharp.Random;

namespace Paramita
{
    public enum OldGameStates
    {
        None = 0,
        PlayerTurn = 1,
        EnemyTurn = 2,
        Debugging = 3
    }


    public class Global
    {
        public static CombatManager CombatManager;
        public static OldGameStates GameState { get; set; }
    }
}
