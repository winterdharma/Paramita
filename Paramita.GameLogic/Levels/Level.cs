using Microsoft.Xna.Framework;
using Paramita.GameLogic.Items;
using Paramita.GameLogic.Actors;
using System;
using System.Collections.Generic;
using Paramita.GameLogic.Mechanics;
using Paramita.GameLogic.Utility;

namespace Paramita.GameLogic.Levels
{
    /*
     * This class represents a single level of a multi-level dungeon.
     */
    public class Level
    {
        #region Fields
        private TileMap _tileMap;
        private List<INpc> _npcs;
        private IPlayer _player;
        private bool _isPlayersTurn = true;
        #endregion


        #region Events
        // these events are for the Dungeon class to pass onto the UI
        public event EventHandler<MoveEventArgs> OnActorWasMoved;
        public event EventHandler<StatusMessageEventArgs> OnStatusMessageSent;
        public event EventHandler<MoveEventArgs> OnActorDeath;
        #endregion


        #region Constructors
        public Level(TileMap map, List<INpc> npcs, List<Item> items = null, Player player = null)
        {
            TileMap = map;
            if(items != null)
                TileMap.PlaceItemsOnRandomTiles(items);
            Npcs = npcs;
            Player = player as IPlayer;
        }
        #endregion


        #region Properties
        public bool IsPlayersTurn
        {
            get { return _isPlayersTurn; }
            set { _isPlayersTurn = value; }
        }

        public TileMap TileMap
        {
            get { return _tileMap; }
            set
            {
                _tileMap = value;
                if(_tileMap != null)
                    SetEntryTiles();
            }
        }

        public ItemType[,] ItemTypeLayer
        {
            get { return _tileMap.ConvertMapToItemTypes(); }
        }

        public TileType[,] TileTypeLayer
        {
            get { return _tileMap.ConvertMapToTileTypes(); }
        }

        public Tuple<ActorType, Compass, bool>[,] BeingTypeLayer
        {
            get { return ConvertMapToBeingTypes(); }
        }

        public List<INpc> Npcs
        {
            get { return _npcs; }
            set
            {
                _npcs = value;
                if(_npcs != null)
                {
                    PlaceNpcsOnTileMap();
                    SubscribeToNpcEvents();
                }
            }
        }

        public IPlayer Player
        {
            get { return _player; }
            set
            {
                _player = value;
                if (_player != null)
                    SubscribeToPlayerEvents();
            }
        }

        public Tile EntryFromAbove { get; private set; }
        public Tile EntryFromBelow { get; private set; }
        #endregion


        // The bool is an IsPlayer flag used by UI
        private Tuple<ActorType, Compass, bool>[,] ConvertMapToBeingTypes()
        {
            // this is to defeat any future attempt to pass nulls through
            Utilities.ThrowExceptionIfNull(_player);
            Utilities.ThrowExceptionIfNull(_npcs);

            var typeArray = new Tuple<ActorType, Compass, bool>[TileMap.TilesWide, TileMap.TilesHigh];
            var playerTile = _player.CurrentTile.TilePoint;

            typeArray[playerTile.X, playerTile.Y] = 
                new Tuple<ActorType, Compass, bool>(_player.ActorType, _player.Facing, true);

            for (int i = 0; i < _npcs.Count; i++)
            {
                var npc = _npcs[i];
                var npcTile = npc.CurrentTile.TilePoint;
                typeArray[npcTile.X, npcTile.Y] = 
                    new Tuple<ActorType, Compass, bool>(npc.ActorType, npc.Facing, false);
            }

            return typeArray;
        }


        #region Event Handlers
        private void HandleNpcDeath(object sender, MoveEventArgs eventArgs)
        {
            var npc = sender as INpc;
            npc.IsDead = true;
            UnsubscribeFromOneNpcsEvents(npc);           
        }

        private void HandleActorMove(object sender, DirectionEventArgs eventArgs)
        {
            Actor actor = sender as Actor;
            Compass direction = eventArgs.Direction;
            Tile newTile = TileMap.GetTile(actor.CurrentTile.TilePoint + Direction.GetPoint(direction));

            if (actor is Player)
                HandlePlayerMove(actor as Player, newTile);
            else
                HandleNpcMove(actor, newTile);
        }
        
        private void HandlePlayerMove(Player player, Tile destination)
        {
            INpc npc;
            if (GetNpcOnTile(destination, out npc))
            {
                var defender = npc as Actor;
                player.Attack(defender.Combatant);
                TogglePlayersTurn();
            }
            else if(destination.IsWalkable)
            {
                var origin = player.CurrentTile.TilePoint;
                player.CurrentTile = destination;
                OnActorWasMoved?.Invoke(player, new MoveEventArgs(origin, destination.TilePoint));
                TogglePlayersTurn();
            }
        }

        private void HandleNpcMove(Actor npc, Tile destination)
        {
            var player = _player as Actor;
            if(IsPlayerOnTile(destination))
            {
                npc.Attack(player.Combatant);
            }
            else if(destination.IsWalkable && !IsNpcOnTile(destination))
            {
                var origin = npc.CurrentTile.TilePoint;
                npc.CurrentTile = destination;
                OnActorWasMoved?.Invoke(npc, new MoveEventArgs(origin, destination.TilePoint));
            }
        }

        private void HandleLevelChange(object sender, LevelChangeEventArgs eventArgs)
        {
            UnsubscribeFromPlayerEvents();
            _player = null;
        }

        private void HandleStatusMessage(object sender, StatusMessageEventArgs eventArgs)
        {
            OnStatusMessageSent?.Invoke(this, eventArgs);
        }
        #endregion


        #region Event Subscriptions
        private void SubscribeToPlayerEvents()
        {
            _player.OnMoveAttempt += HandleActorMove;
            _player.OnLevelChange += HandleLevelChange;
        }

        private void UnsubscribeFromPlayerEvents()
        {
            _player.OnMoveAttempt -= HandleActorMove;
            _player.OnLevelChange -= HandleLevelChange;
        }

        private void SubscribeToNpcEvents()
        {
            foreach (INpc npc in _npcs)
            {
                npc.OnMoveAttempt += HandleActorMove;
            }
        }

        private void UnsubscribeFromOneNpcsEvents(INpc npc)
        {
            npc.OnMoveAttempt -= HandleActorMove;
        }
        #endregion


        public void Update()
        {
            RemoveDeadNPCs(_npcs);
            
            if (_isPlayersTurn)
            {
                _player.Update();
            }
            else
            {
                UpdateNpcs();
                ResetActorsTimesAttacked();
                TogglePlayersTurn();
            }
        }


        #region Helper Methods
        private bool IsNpcOnTile(Tile tile)
        {
            return _npcs.Exists(npc => npc.CurrentTile == tile);
        }

        private bool GetNpcOnTile(Tile tile, out INpc npc)
        {
            var isNpcOnTile = IsNpcOnTile(tile);
            npc = isNpcOnTile ? _npcs.Find(n => n.CurrentTile == tile) : null;
            return isNpcOnTile;
        }

        private bool IsPlayerOnTile(Tile tile)
        {
            return _player.CurrentTile == tile;
        }

        private void TogglePlayersTurn()
        {
            _isPlayersTurn = !_isPlayersTurn;
        }

        private void RemoveDeadNPCs(List<INpc> npcs)
        {
            foreach(INpc npc in npcs)
            {
                if (NpcIsDead(npc))
                {
                    OnActorDeath?.Invoke(this, new MoveEventArgs(npc.CurrentTile.TilePoint, Point.Zero));
                }
            }
            npcs.RemoveAll(NpcIsDead);
        }

        private bool NpcIsDead(INpc npc)
        {
            return npc.IsDead;
        }

        private void UpdateNpcs()
        {
            foreach(INpc npc in _npcs)
            {
                npc.Update(_player as Player);
            }
        }

        private void ResetActorsTimesAttacked()
        {
            _player.TimesAttacked = 0;

            foreach(var npc in _npcs)
            {
                npc.TimesAttacked = 0;
            }
        }

        private void SetEntryTiles()
        {
            EntryFromAbove = _tileMap.FindTileType(TileType.StairsUp);
            EntryFromBelow = _tileMap.FindTileType(TileType.StairsDown);
        }

        private void PlaceNpcsOnTileMap()
        {
            var placedTiles = new bool[TileMap.TilesWide, TileMap.TilesHigh];

            // remove the entry tiles from possible placement tiles
            placedTiles[EntryFromAbove.TilePoint.X, EntryFromAbove.TilePoint.Y] = true;
            placedTiles[EntryFromBelow.TilePoint.X, EntryFromBelow.TilePoint.Y] = true;

            foreach (var npc in _npcs)
            {
                // fetches walkable tiles and checks if its available for npc to be placed
                var tile = _tileMap.GetRandomWalkableTile();
                while (placedTiles[tile.TilePoint.X, tile.TilePoint.Y])
                {
                    tile = _tileMap.GetRandomWalkableTile();
                }

                // places the rat on the level and notes where it is
                npc.CurrentTile = tile;
                placedTiles[tile.TilePoint.X, tile.TilePoint.Y] = true;
            }
        }
        #endregion
    }
}
