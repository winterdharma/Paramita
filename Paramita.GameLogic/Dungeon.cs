﻿using Paramita.GameLogic.Levels;
using Paramita.GameLogic.Actors;
using System;
using System.Collections.Generic;
using Paramita.GameLogic.Items;
using Paramita.GameLogic.Mechanics;
using Paramita.GameLogic.Utility;

namespace Paramita.GameLogic
{
    #region Custom EventArg Classes
    public class StatusMessageEventArgs : EventArgs
    {
        public List<string> Message { get; }
        public StatusMessageEventArgs(List<string> message)
        {
            Message = message;
        }
    }

    public class NewLevelEventArgs : EventArgs
    {
        public int LevelNumber { get; }
        public Tuple<TileType[,], ItemType[,], Tuple<ActorType, Compass, bool>[,]> Layers { get; }
        public NewLevelEventArgs(int levelNumber, 
            Tuple<TileType[,], ItemType[,], Tuple<ActorType, Compass, bool>[,]> layers)
        {
            LevelNumber = levelNumber;
            Layers = layers;
        }
    }
    #endregion

    public interface IRandom
    {
        int Next(int maxValue);
        int Next(int minValue, int maxValue);
    }

    public class Dungeon
    {
        internal static IRandom _random = new RandomNum();
        private Dictionary<int, Level> _levels;
        private Player _player;

        private static int _currentLevelNumber;
        private Level _currentLevel;

        public static int CurrentLevelNumber
        {
            get { return _currentLevelNumber; }
        }
        public Level CurrentLevel
        {
            get { return _currentLevel; }
        }
        public Player Player { get { return _player; } }

        public event EventHandler<MoveEventArgs> OnActorMoveUINotification;
        public event EventHandler<InventoryChangeEventArgs> OnInventoryChangeUINotification;
        public event EventHandler<ItemEventArgs> OnItemPickedUpUINotification;
        public event EventHandler<ItemEventArgs> OnItemDroppedUINotification;
        public static event EventHandler<StatusMessageEventArgs> OnStatusMsgUINotification;
        public event EventHandler<NewLevelEventArgs> OnLevelChangeUINotification;
        public event EventHandler<MoveEventArgs> OnActorRemovedUINotification;
        //public static event EventHandler<MoveEventArgs> OnPlayerDeathUINotification;

        public Dungeon()
        {
            _player = new Player("Wesley");
            _levels = new Dictionary<int, Level>();
            _currentLevelNumber = 1;
            _currentLevel = LevelFactory.CreateLevel(_currentLevelNumber, _player);
            _player.CurrentTile = _currentLevel.EntryFromAbove;
            _levels[_currentLevelNumber] = _currentLevel;
            SubscribeToEvents();
        }

        #region Public API Methods
        
        #region UI TileMap Initialization
        public Tuple<TileType[,], ItemType[,], Tuple<ActorType, Compass, bool>[,]> GetCurrentLevelLayers()
        { 
            return new Tuple<TileType[,], ItemType[,], Tuple<ActorType, Compass, bool>[,]>(
                    _currentLevel.TileTypeLayer, 
                    _currentLevel.ItemTypeLayer, 
                    _currentLevel.BeingTypeLayer
            );
        }
        #endregion

        #region Player Input Handlers
        public void MovePlayer(Compass direction)
        {
            _player.HandleInput(direction);
        }
        
        public void PlayerDropItem(string inventorySlot, ItemType item)
        {
            _player.DropItem(inventorySlot, item);
        }

        #endregion
        
        public void GetPlayerInventory()
        {
            _player.GetInventory();
        }


        #endregion


        #region Helper Methods
        private void SubscribeToEvents()
        {
            SubscribeToLevelEvents();
            SubscribeToActorEvents();
        }

        private void SubscribeToLevelEvents()
        {
            _currentLevel.OnActorWasMoved += HandleActorMovement;
            _currentLevel.OnActorDeath += HandleDeadActor;
            _currentLevel.TileMap.OnItemAdded += HandleItemAddedToTileMap;
            _currentLevel.TileMap.OnItemRemoved += HandleItemRemovedFromTileMap;
        }

        private void SubscribeToActorEvents()
        {
            _player.OnInventoryChange += HandleInventoryChange;
            _player.OnStatusMsgSent += HandleStatusMessage;
            _player.OnLevelChange += HandleLevelChange;
            SubscribeToNpcEvents(_currentLevel.Npcs);
        }

        private void SubscribeToNpcEvents(List<INpc> npcs)
        {
            foreach (INpc npc in npcs)
            {
                npc.OnStatusMsgSent += HandleStatusMessage;
            }
        }

        private void UnsubscribeFromLevelEvents()
        {
            _currentLevel.OnActorWasMoved -= HandleActorMovement;
            _currentLevel.OnActorDeath -= HandleDeadActor;
            _currentLevel.TileMap.OnItemAdded -= HandleItemAddedToTileMap;
            _currentLevel.TileMap.OnItemRemoved -= HandleItemRemovedFromTileMap;
        }

        private void GoUpOneLevel()
        {
            int levelNumber = _currentLevelNumber - 1;
            if (levelNumber > 0)
            {
                ChangeLevel(levelNumber);
            }
        }

        private void GoDownOneLevel()
        {
            int levelNumber = _currentLevelNumber + 1;
            if (!_levels.ContainsKey(levelNumber))
                CreateNextLevel(levelNumber);
            ChangeLevel(levelNumber);
        }

        private void ChangeLevel(int levelNumber)
        {
            // get the difference between current and new level number
            int upOrDown = levelNumber - _currentLevelNumber;
            // remove event handling for current level
            UnsubscribeFromLevelEvents();
            // change to the new level
            _currentLevelNumber = levelNumber;
            _currentLevel = _levels[_currentLevelNumber];
            // add event handling for new level
            SubscribeToLevelEvents();
            // place the player on new level
            PlacePlayerOnLevel(upOrDown);
            // notify the UI of new level data
            OnLevelChangeUINotification?.Invoke(null, new NewLevelEventArgs(_currentLevelNumber, GetCurrentLevelLayers()));
        }

        private void PlacePlayerOnLevel(int upOrDown)
        {
            _currentLevel.Player = _player;
            if (upOrDown == -1)
            {
                _player.CurrentTile = _currentLevel.EntryFromBelow;
            }
            else if (upOrDown == 1)
            {
                _player.CurrentTile = _currentLevel.EntryFromAbove;
            }
        }
        #endregion

        #region Event Handlers
        private void HandleLevelChange(object sender, LevelChangeEventArgs eventArgs)
        {
            int levelChange = eventArgs.LevelChange;
            MoveOneLevel(levelChange);
        }

        private void HandleActorMovement(object sender, MoveEventArgs eventArgs)
        {
            OnActorMoveUINotification?.Invoke(null, eventArgs);
        }

        private void HandleDeadActor(object sender, MoveEventArgs eventArgs)
        {
            OnActorRemovedUINotification?.Invoke(null, eventArgs);
        }

        private void HandleInventoryChange(object sender, InventoryChangeEventArgs eventArgs)
        {
            OnInventoryChangeUINotification?.Invoke(null, eventArgs);
        }

        private void HandleItemAddedToTileMap(object sender, ItemEventArgs eventArgs)
        {
            OnItemDroppedUINotification?.Invoke(null, eventArgs);
        }

        private void HandleItemRemovedFromTileMap(object sender, ItemEventArgs eventArgs)
        {
            OnItemPickedUpUINotification?.Invoke(null, eventArgs);
        }

        private void HandleStatusMessage(object sender, StatusMessageEventArgs eventArgs)
        {
            OnStatusMsgUINotification?.Invoke(null, eventArgs);
        }
        #endregion


        public void Update()
        {
            _currentLevel.Update();
        }

        public void CreateNextLevel(int levelNumber)
        {
            var level = LevelFactory.CreateLevel(levelNumber);
            _levels[levelNumber] = level;
            SubscribeToNpcEvents(level.Npcs);
        }


        public void MoveOneLevel(int levelChange)
        {
            if (levelChange == -1)
                GoUpOneLevel();
            else if (levelChange == 1)
                GoDownOneLevel();
            else
                throw new NotImplementedException("Moving more than 1 level at a time not implemented yet.");
        }
    }
}
