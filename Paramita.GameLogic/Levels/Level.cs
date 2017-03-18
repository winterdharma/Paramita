using Microsoft.Xna.Framework;
using Paramita.GameLogic.Items;
using Paramita.GameLogic.Actors;
using System;
using System.Collections.Generic;
using Paramita.GameLogic.Mechanics;
using System.Linq;

namespace Paramita.GameLogic.Levels
{
    /*
     * This class represents a single level of a multi-level dungeon.
     */
    public class Level
    {
        #region Fields
        private TileMap _tileMap;
        private List<Actor> _npcs;
        private Player _player;
        private bool _isPlayersTurn = true;
        #endregion


        #region Events
        // these events are for the Dungeon class to pass onto the UI
        public event EventHandler<MoveEventArgs> OnActorWasMoved;
        public event EventHandler<StatusMessageEventArgs> OnStatusMessageSent;
        #endregion


        #region Constructors
        public Level() { }

        public Level(TileMap map, List<Actor> npcs, Player player = null)
        {
            TileMap = map;
            Npcs = npcs;
            Player = player;
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
            set { _tileMap = value; }
        }

        public ItemType[,] ItemTypeLayer
        {
            get { return _tileMap.ConvertMapToItemTypes(); }
        }

        public TileType[,] TileTypeLayer
        {
            get { return _tileMap.ConvertMapToTileTypes(); }
        }

        public Tuple<BeingType, Compass, bool>[,] BeingTypeLayer
        {
            get { return ConvertMapToBeingTypes(); }
        }

        public List<Actor> Npcs
        {
            get { return _npcs; }
            set
            {
                _npcs = value;
                if(_npcs != null)
                    SubscribeToNpcEvents();
            }
        }

        public Player Player
        {
            get { return _player; }
            set
            {
                _player = value;
                if (_player != null)
                    SubscribeToPlayerEvents();
            }
        }
        #endregion


        // The bool is an IsPlayer flag used by UI
        private Tuple<BeingType, Compass, bool>[,] ConvertMapToBeingTypes()
        {
            // this is to defeat any future attempt to pass nulls through
            if (_player == null)
            {
                throw new NullReferenceException("_player is null.");
            }
            if (_npcs == null)
            {
                throw new NullReferenceException("_npcs is null.");
            }

            var typeArray = new Tuple<BeingType, Compass, bool>[TileMap.TilesWide, TileMap.TilesHigh];

            var playerTile = _player.CurrentTile.TilePoint;
            typeArray[playerTile.X, playerTile.Y] = 
                new Tuple<BeingType, Compass, bool>(_player.BeingType, _player.Facing, true);

            for (int i = 0; i < _npcs.Count; i++)
            {
                var npc = (Actor)_npcs[i];
                var npcTile = npc.CurrentTile.TilePoint;
                typeArray[npcTile.X, npcTile.Y] = 
                    new Tuple<BeingType, Compass, bool>(npc.BeingType, npc.Facing, false);
            }

            return typeArray;
        }


        #region Event Handlers
        private void HandleNpcDeath(object sender, MoveEventArgs eventArgs)
        {
            Actor npc = sender as Actor;
            Npcs.Remove(npc);
            UnsubscribeFromOneNpcsEvents(npc);           
        }


        private void HandleActorMove(object sender, MoveEventArgs eventArgs)
        {
            Actor actor = sender as Actor;
            Compass direction = eventArgs.Direction;
            Tile newTile = TileMap.GetTile(actor.CurrentTile.TilePoint + Direction.GetPoint(direction));
            Point origin = actor.CurrentTile.TilePoint;

            if (actor is Player)
                HandlePlayerMove(actor as Player, origin, newTile);
            else
                HandleNpcMove(actor, origin, newTile);
        }

        
        private void HandlePlayerMove(Player player, Point origin, Tile destination)
        {
            Actor defender;
            if (GetNpcOnTile(destination, out defender))
            {
                player.Attack(defender);
                TogglePlayersTurn();
            }
            else if(destination.IsWalkable)
            {
                player.CurrentTile = destination;
                OnActorWasMoved?.Invoke(player, new MoveEventArgs(Compass.None, origin, destination.TilePoint));
                TogglePlayersTurn();
            }
        }

        private void HandleNpcMove(Actor npc, Point origin, Tile destination)
        {
            if(IsPlayerOnTile(destination))
            {
                npc.Attack(_player);
            }
            else if(destination.IsWalkable && !IsNpcOnTile(destination))
            {
                npc.CurrentTile = destination;
                OnActorWasMoved?.Invoke(npc, new MoveEventArgs(Compass.None, origin, destination.TilePoint));
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
            foreach (Actor npc in _npcs)
            {
                npc.OnMoveAttempt += HandleActorMove;
                npc.OnActorDeath += HandleNpcDeath;
            }
        }

        private void UnsubscribeFromOneNpcsEvents(Actor actor)
        {
            actor.OnMoveAttempt -= HandleActorMove;
            actor.OnActorDeath -= HandleNpcDeath;
        }
        #endregion


        #region Tile Getters
        // these are called by Dungeon when placing player on Level.
        // Will be changed in future reviews to delegate placement details
        // to Level class.
        public Tile GetStairsUpTile()
        {
            return _tileMap.FindTileType(TileType.StairsUp);
        }

        public Tile GetStairsDownTile()
        {
            return _tileMap.FindTileType(TileType.StairsDown);
        }

        // this is called by LevelFactory when placing npcs.
        // in future code review, LevelFactory will delegate placement
        // to Level class.
        public Tile GetRandomWalkableTile()
        {
            return _tileMap.GetRandomWalkableTile();
        }
        #endregion


        public void Update()
        {
            _npcs.RemoveAll(NpcIsDead);

            
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

        private bool GetNpcOnTile(Tile tile, out Actor actor)
        {
            var isNpcOnTile = IsNpcOnTile(tile);
            actor = isNpcOnTile ? _npcs.Find(n => n.CurrentTile == tile) : null;
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

        private bool NpcIsDead(Actor actor)
        {
            return actor.IsDead;
        }

        private void UpdateNpcs()
        {
            foreach(INpc npc in _npcs)
            {
                npc.Update(_player);
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
        #endregion
    }
}
