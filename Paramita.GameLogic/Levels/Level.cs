using Microsoft.Xna.Framework;
using Paramita.GameLogic.Items;
using Paramita.GameLogic.Actors;
using System;
using System.Collections.Generic;
using Paramita.GameLogic.Mechanics;

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

        public Level(TileMap map, List<INpc> npcs, Player player = null)
        {
            _tileMap = map;
            _npcs = npcs;
            _player = player;
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

        public List<INpc> Npcs
        {
            get { return _npcs; }
            set
            {
                _npcs = value;
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
                {
                    SubscribeToPlayerEvents();
                }
            }
        }
        #endregion


        // The bool is an IsPlayer flag used by UI
        public Tuple<BeingType, Compass, bool>[,] ConvertMapToBeingTypes()
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
            INpc npc = sender as INpc;
            Npcs.Remove(npc);
            UnsubscribeFromOneNpcsEvents(npc as Actor);           
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
            if (IsNpcOnTile(destination))
            {
                player.Attack(GetNpcOnTile(destination));
                _isPlayersTurn = false;
            }
            else if(destination.IsWalkable)
            {
                player.CurrentTile = destination;
                OnActorWasMoved?.Invoke(player, new MoveEventArgs(Compass.None, origin, destination.TilePoint));
                _isPlayersTurn = false;
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


        #region Tile Getters and Setters
        public Tile GetStairsUpTile()
        {
            return _tileMap.FindTileType(TileType.StairsUp);
        }

        public Tile GetStairsDownTile()
        {
            return _tileMap.FindTileType(TileType.StairsDown);
        }

        // ?? why is this in this class? Why not LevelFactory?
        public void PlaceItemsOnTileMap(List<Item> items)
        {
            for(int i = 0; i < items.Count; i++)
            {
                var tile = GetRandomWalkableTile();
                tile.AddItem(items[i]);
            }
        }

        public Tile GetRandomWalkableTile()
        {
            int x, y;
            while (true)
            {
                x = Dungeon._random.Next(_tileMap.TilesWide - 1);
                y = Dungeon._random.Next(_tileMap.TilesHigh - 1);
                var point = new Point(x, y);
                
                if (_tileMap.IsTileWalkable(point))
                {
                    return _tileMap.GetTile(point);
                }
            }
        }
        #endregion


        #region Actors on Tiles API
        // Iterates over the @npcs list of active NPCs to find if one of them is
        // currently on @tile
        public bool IsNpcOnTile(Tile tile)
        {
            for (int i = 0; i < _npcs.Count; i++)
            {
                var npc = (Actor)_npcs[i];
                if (npc.CurrentTile == tile)
                {
                    return true;
                }
            }
            return false;
        }

        // Finds the NPC that is located on @tile. If none is there, null is returned
        // (this method should be called after IsNpcOnTile check)
        public Actor GetNpcOnTile(Tile tile)
        {
            for (int i = 0; i < _npcs.Count; i++)
            {
                var npc = (Actor)_npcs[i];
                if (npc.CurrentTile == tile)
                {
                    return npc;
                }
            }
            return null;
        }

        public bool IsPlayerOnTile(Tile tile)
        {
            if (_player.CurrentTile == tile)
                return true;
            return false;
        }
        #endregion


        public void Update()
        {
            // remove dead npcs before updating them
            for (int i = 0; i < _npcs.Count; i++)
            {
                var npc = (Actor)_npcs[i];
                if (npc.IsDead)
                {
                    _npcs.Remove(_npcs[i]);
                }
            }

            // check for player's input until he moves
            if (_isPlayersTurn)
            {
                _player.Update();
            }
            // give the npcs a turn after the player moves
            else
            {
                for (int i = 0; i < _npcs.Count; i++)
                {
                    var npc = (Actor)_npcs[i];
                    npc.TimesAttacked = 0;
                    _npcs[i].Update(_player);
                }
                _isPlayersTurn = true;
                _player.TimesAttacked = 0;
            }
        }
    }
}
