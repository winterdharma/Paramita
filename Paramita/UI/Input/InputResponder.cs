using Paramita.GameLogic;
using Paramita.GameLogic.Mechanics;
using System;

namespace Paramita.UI.Input
{
    public static class InputResponder
    {
        public static void SubscribeToInputEvents()
        {
            SubscribeToPlayerMoveEvents();
        }

        public static void SubscribeToPlayerMoveEvents()
        {
            InputListener.OnLeftKeyWasPressed += PlayerMoveWestHandler;
            InputListener.OnRightKeyWasPressed += PlayerMoveEastHandler;
            InputListener.OnUpKeyWasPressed += PlayerMoveNorthHandler;
            InputListener.OnDownKeyWasPressed += PlayerMoveSouthHandler;
        }

        private static void PlayerMoveWestHandler(object sender, EventArgs e)
        {
            Dungeon.MovePlayer(Compass.West);
        }

        private static void PlayerMoveEastHandler(object sender, EventArgs e)
        {
            Dungeon.MovePlayer(Compass.East);
        }

        private static void PlayerMoveNorthHandler(object sender, EventArgs e)
        {
            Dungeon.MovePlayer(Compass.North);
        }

        private static void PlayerMoveSouthHandler(object sender, EventArgs e)
        {
            Dungeon.MovePlayer(Compass.South);
        }
    }
}
