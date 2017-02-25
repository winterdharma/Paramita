using Paramita.GameLogic;
using Paramita.GameLogic.Mechanics;
using Paramita.UI.Scenes.Game;
using System;

namespace Paramita.UI.Input
{
    public static class InputResponder
    {
        public static void SubscribeToInputEvents()
        {
            SubscribeToPlayerMoveEvents();
            SubscribeToPlayerInventoryEvents();
        }

        private static void SubscribeToPlayerMoveEvents()
        {
            InputListener.OnLeftKeyWasPressed += PlayerMoveWestHandler;
            InputListener.OnRightKeyWasPressed += PlayerMoveEastHandler;
            InputListener.OnUpKeyWasPressed += PlayerMoveNorthHandler;
            InputListener.OnDownKeyWasPressed += PlayerMoveSouthHandler;
        }

        private static void SubscribeToPlayerInventoryEvents()
        {
            InventoryPanel.OnPlayerDroppedItem += PlayerDropItemEventHandler;
            InventoryPanel.OnPlayerEquippedItem += PlayerEquipItemEventHandler;
            InventoryPanel.OnPlayerUsedItem += PlayerUseItemEventHandler;
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


        private static void PlayerDropItemEventHandler(object sender, InventoryEventArgs e)
        {
            Dungeon.PlayerDropItem(e.InventorySlot, e.InventoryItem);
        }

        private static void PlayerEquipItemEventHandler(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void PlayerUseItemEventHandler(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
