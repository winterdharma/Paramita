using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;
using Paramita.GameLogic;
using Paramita.GameLogic.Mechanics;
using Paramita.UI.Scenes.Game;
using System;

namespace Paramita.UI.Input
{
    public class InputResponder
    {
        #region Fields
        private KeyboardListener _keyboard;
        private MouseListener _mouse;
        #endregion



        public InputResponder(KeyboardListener keyboard, MouseListener mouse)
        {
            _keyboard = keyboard;
            _mouse = mouse;

            SubscribeToInputListenerEvents();
        }

        private void SubscribeToInputListenerEvents()
        {
            _keyboard.KeyPressed += OnKeyPressed;
            //_mouse.MouseClicked += OnMouseClick;
            //_mouse.MouseMoved += OnMouseMove;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnKeyPressed(object sender, KeyboardEventArgs e)
        {
            if (e.Key == Keys.Left) Dungeon.MovePlayer(Compass.West);
            if (e.Key == Keys.Right) Dungeon.MovePlayer(Compass.East);
            if (e.Key == Keys.Up) Dungeon.MovePlayer(Compass.North);
            if (e.Key == Keys.Down) Dungeon.MovePlayer(Compass.South);
        }

        public void SubscribeToInputEvents()
        {
            SubscribeToPlayerInventoryEvents();
        }

        private void SubscribeToPlayerInventoryEvents()
        {
            InventoryPanel.OnPlayerDroppedItem += PlayerDropItemEventHandler;
            InventoryPanel.OnPlayerEquippedItem += PlayerEquipItemEventHandler;
            InventoryPanel.OnPlayerUsedItem += PlayerUseItemEventHandler;
        }


        private void PlayerDropItemEventHandler(object sender, InventoryEventArgs e)
        {
            Dungeon.PlayerDropItem(e.InventorySlot, e.InventoryItem);
        }

        private void PlayerEquipItemEventHandler(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PlayerUseItemEventHandler(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
