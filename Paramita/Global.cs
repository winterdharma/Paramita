using RogueSharp.Random;
using Paramita;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita
{
    public enum GameStates
    {
        None = 0,
        PlayerTurn = 1,
        EnemyTurn = 2,
        Debugging = 3
    }

    public class Global
    {
        public static readonly IRandom Random = new DotNetRandom();
        public static CombatManager CombatManager;
        public static GameStates GameState { get; set; }
        public static readonly Camera Camera = new Camera();
        public static readonly int MapWidth = 50;
        public static readonly int MapHeight = 30;
        public static readonly int SpriteWidth = 64;
        public static readonly int SpriteHeight = 64;
    }
}
