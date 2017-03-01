using Paramita.GameLogic.Levels;
using Paramita.GameLogic.Actors;
using System;
using System.Collections.Generic;
using Paramita.GameLogic.Items;
using Paramita.GameLogic.Mechanics;

namespace Paramita.GameLogic
{

    public class InventoryChangeEventArgs : EventArgs
    {
        public Tuple<Dictionary<string, ItemType>, int> Inventory { get; }
        public InventoryChangeEventArgs(Tuple<Dictionary<string, ItemType>, int> inventory)
        {
            Inventory = inventory;
        }
    }

    public class Dungeon
    {
        internal static Random _random;
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

        public Dungeon()
        {
            _random = new Random();
            _player = new Player("Wesley");
            _levels = new Dictionary<int, Level>();
            _currentLevelNumber = 1;
            _currentLevel = LevelFactory.CreateLevel(_currentLevelNumber);
            _currentLevel.Player = _player;
            _player.CurrentTile = _currentLevel.GetStairsUpTile();
            _levels[_currentLevelNumber] = _currentLevel;
            SubscribeToLevelEvents();
        }

        #region Public API Methods
        
        #region UI TileMap Initialization API
        public TileType[,] GetCurrentLevelTiles()
        {
            var tileTypeArray = _currentLevel.TileMap.ConvertMapToTileTypes();
            return tileTypeArray;
        }

        public Tuple<ItemType>[,] GetCurrentLevelItems()
        {
            var itemTypeArray = _currentLevel.TileMap.ConvertMapToItemTypes();
            return itemTypeArray;
        }

        public Tuple<BeingType, Compass, bool>[,] GetCurrentLevelActors()
        {
            var beingTypeArray = _currentLevel.ConvertMapToBeingTypes();
            return beingTypeArray;
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



        private void SubscribeToLevelEvents()
        {
            _currentLevel.OnLevelChange += HandleLevelChange;
            _currentLevel.OnActorWasMoved += HandleActorMovement;
            _player.OnInventoryChange += HandleInventoryChange;
        }


        private void HandleLevelChange(object sender, LevelChangeEventArgs eventArgs)
        {
            int levelChange = eventArgs.LevelChange;
            ChangeLevel(levelChange);

            if (levelChange < 0)
                _player.CurrentTile = _currentLevel.GetStairsDownTile();
            else if (levelChange > 0)
                _player.CurrentTile = _currentLevel.GetStairsUpTile();
            _currentLevel.Player = _player;
        }


        private void HandleActorMovement(object sender, MoveEventArgs eventArgs)
        {
            OnActorMoveUINotification(null, eventArgs);
        }

        private void HandleInventoryChange(object sender, InventoryChangeEventArgs eventArgs)
        {
            OnInventoryChangeUINotification(null, eventArgs);
        }



        public void Update()
        {
            _currentLevel.Update();
        }

        public static void CreateNextLevel(int levelNumber)
        {
            _levels[levelNumber] = LevelFactory.CreateLevel(levelNumber);
        }


        public static void ChangeLevel(int levelChange)
        {
            if (levelChange == -1)
                GoUpOneLevel();
            else if (levelChange == 1)
                GoDownOneLevel();
            else
                throw new NotImplementedException("Moving more than 1 level at a time not implemented yet.");
        }


        private static void GoUpOneLevel()
        {
            int levelNumber = _currentLevelNumber - 1;
            if (levelNumber > 1)
            {
                _currentLevelNumber = levelNumber;
                _currentLevel = _levels[_currentLevelNumber];
            }   
        }

        private static void GoDownOneLevel()
        {
            int levelNumber = _currentLevelNumber + 1;
            if (!_levels.ContainsKey(levelNumber))
                CreateNextLevel(levelNumber);
            _currentLevelNumber = levelNumber;
            _currentLevel = _levels[_currentLevelNumber];
        }
    }
}
