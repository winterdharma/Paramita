using Paramita.GameLogic.Levels;
using Paramita.GameLogic.Actors;
using System;
using System.Collections.Generic;
using Paramita.GameLogic.Items;
using Paramita.GameLogic.Mechanics;

namespace Paramita.GameLogic
{
    #region Custom EventArg Classes
    public class InventoryChangeEventArgs : EventArgs
    {
        public Tuple<Dictionary<string, ItemType>, int> Inventory { get; }
        public InventoryChangeEventArgs(Tuple<Dictionary<string, ItemType>, int> inventory)
        {
            Inventory = inventory;
        }
    }

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
        public Tuple<TileType[,], ItemType[,], Tuple<BeingType, Compass, bool>[,]> Layers { get; }
        public NewLevelEventArgs(int levelNumber, 
            Tuple<TileType[,], ItemType[,], Tuple<BeingType, Compass, bool>[,]> layers)
        {
            LevelNumber = levelNumber;
            Layers = layers;
        }
    }
    #endregion


    public class Dungeon
    {
        internal static Random _random = new Random();
        private static Dictionary<int, Level> _levels;
        private static Player _player;

        private static int _currentLevelNumber;
        private static Level _currentLevel;

        public static int CurrentLevelNumber
        {
            get { return _currentLevelNumber; }
        }
        public static Level CurrentLevel
        {
            get { return _currentLevel; }
        }
        public static Player Player { get { return _player; } }

        public static event EventHandler<MoveEventArgs> OnActorMoveUINotification;
        public static event EventHandler<InventoryChangeEventArgs> OnInventoryChangeUINotification;
        public static event EventHandler<ItemEventArgs> OnItemPickedUpUINotification;
        public static event EventHandler<ItemEventArgs> OnItemDroppedUINotification;
        public static event EventHandler<StatusMessageEventArgs> OnStatusMsgUINotification;
        public static event EventHandler<NewLevelEventArgs> OnLevelChangeUINotification;
        public static event EventHandler<MoveEventArgs> OnActorRemovedUINotification;
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
        public static Tuple<TileType[,], ItemType[,], Tuple<BeingType, Compass, bool>[,]> GetCurrentLevelLayers()
        { 
            return new Tuple<TileType[,], ItemType[,], Tuple<BeingType, Compass, bool>[,]>(
                    _currentLevel.TileTypeLayer, 
                    _currentLevel.ItemTypeLayer, 
                    _currentLevel.BeingTypeLayer
            );
        }
        #endregion

        #region Player Input Handlers
        public static void MovePlayer(Compass direction)
        {
            _player.HandleInput(direction);
        }
        
        public static void PlayerDropItem(string inventorySlot, ItemType item)
        {
            _player.DropItem(inventorySlot, item);
        }

        #endregion
        
        public static Tuple<Dictionary<string, ItemType>, int> GetPlayerInventory()
        {
            return _player.GetInventory();
        }


        #endregion


        #region Helper Methods
        private void SubscribeToEvents()
        {
            SubscribeToLevelEvents();
            SubscribeToActorEvents();
        }

        private static void SubscribeToLevelEvents()
        {
            _currentLevel.OnActorWasMoved += HandleActorMovement;
            _currentLevel.TileMap.OnItemAdded += HandleItemAddedToTileMap;
            _currentLevel.TileMap.OnItemRemoved += HandleItemRemovedFromTileMap;
        }

        private void SubscribeToActorEvents()
        {
            _player.OnInventoryChange += HandleInventoryChange;
            _player.OnStatusMsgSent += HandleStatusMessage;
            _player.OnLevelChange += HandleLevelChange;
            //_player.OnActorDeath += HandlePlayerDeath;
            SubscribeToNpcEvents(_currentLevel.Npcs);
        }

        private static void SubscribeToNpcEvents(List<INpc> npcs)
        {
            foreach (INpc npc in npcs)
            {
                npc.OnStatusMsgSent += HandleStatusMessage;
                npc.OnActorDeath += HandleDeadActor;
            }
        }

        private static void UnsubscribeFromLevelEvents()
        {
            _currentLevel.OnActorWasMoved -= HandleActorMovement;
            _currentLevel.TileMap.OnItemAdded -= HandleItemAddedToTileMap;
            _currentLevel.TileMap.OnItemRemoved -= HandleItemRemovedFromTileMap;
        }

        private static void GoUpOneLevel()
        {
            int levelNumber = _currentLevelNumber - 1;
            if (levelNumber > 0)
            {
                ChangeLevel(levelNumber);
            }
        }

        private static void GoDownOneLevel()
        {
            int levelNumber = _currentLevelNumber + 1;
            if (!_levels.ContainsKey(levelNumber))
                CreateNextLevel(levelNumber);
            ChangeLevel(levelNumber);
        }

        private static void ChangeLevel(int levelNumber)
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

        private static void PlacePlayerOnLevel(int upOrDown)
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
        private static void HandleLevelChange(object sender, LevelChangeEventArgs eventArgs)
        {
            int levelChange = eventArgs.LevelChange;
            MoveOneLevel(levelChange);
        }

        private static void HandleActorMovement(object sender, MoveEventArgs eventArgs)
        {
            OnActorMoveUINotification?.Invoke(null, eventArgs);
        }

        private static void HandleDeadActor(object sender, MoveEventArgs eventArgs)
        {
            OnActorRemovedUINotification?.Invoke(null, eventArgs);
        }

        private static void HandleInventoryChange(object sender, InventoryChangeEventArgs eventArgs)
        {
            OnInventoryChangeUINotification?.Invoke(null, eventArgs);
        }

        private static void HandleItemAddedToTileMap(object sender, ItemEventArgs eventArgs)
        {
            OnItemDroppedUINotification?.Invoke(null, eventArgs);
        }

        private static void HandleItemRemovedFromTileMap(object sender, ItemEventArgs eventArgs)
        {
            OnItemPickedUpUINotification?.Invoke(null, eventArgs);
        }

        private static void HandleStatusMessage(object sender, StatusMessageEventArgs eventArgs)
        {
            OnStatusMsgUINotification?.Invoke(null, eventArgs);
        }
        #endregion


        public void Update()
        {
            _currentLevel.Update();
        }

        public static void CreateNextLevel(int levelNumber)
        {
            var level = LevelFactory.CreateLevel(levelNumber);
            _levels[levelNumber] = level;
            SubscribeToNpcEvents(level.Npcs);
        }


        public static void MoveOneLevel(int levelChange)
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
