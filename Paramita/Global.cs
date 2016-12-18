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

    public enum ResizeType
    {
        None,
        ResizePanels,
        ResizeScale
    }

    public enum WindowState
    {
        Normal = 0,
        Minimized,
        Maximized,
        Fullscreen
    }

    public enum WindowBorder
    {
        /// <summary>
        /// The window has a resizable border. A window with a resizable border can be resized by the user or programmatically.
        /// </summary>
        Resizable = 0,
        /// <summary>
        /// The window has a fixed border. A window with a fixed border can only be resized programmatically.
        /// </summary>
        Fixed,
        /// <summary>
        /// The window does not have a border. A window with a hidden border can only be resized programmatically.
        /// </summary>
        Hidden
    }

    public class Global
    {
        public static readonly IRandom Random = new DotNetRandom();
        public static CombatManager CombatManager;
        public static OldGameStates GameState { get; set; }
        public static readonly Camera Camera = new Camera();
        public static readonly int MapWidth = 50;
        public static readonly int MapHeight = 30;
        public static readonly int SpriteWidth = 64;
        public static readonly int SpriteHeight = 64;
    }
}
