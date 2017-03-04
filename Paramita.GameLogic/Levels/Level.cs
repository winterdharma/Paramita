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
        private TileMap tileMap;
        private List<INpc> _npcs;
        private List<Item> items;
        private Player _player;
        private bool _isPlayersTurn = true;

        public bool IsPlayersTurn
        {
            get { return _isPlayersTurn; }
            set { _isPlayersTurn = value; }
        }

        public TileMap TileMap
        {
            get { return tileMap; }
            set { tileMap = value; }
        }

        public List<INpc> Npcs
        {
            get { return _npcs; }
            set
            {
                _npcs = value;
                foreach (Actor npc in _npcs)
                {
                    npc.OnMoveAttempt += HandleActorMove;
                    npc.OnStatusMsgSent += HandleStatusMessage;
                }
            }
        }

        public List<Item> Items
        {
            get { return items; }
            set { items = value; }
        }


        public Player Player
        {
            get { return _player; }
            set
            {
                _player = value;
                if (_player != null)
                {
                    SubscribeToPlayerEvents(_player);
                }
            }
        }

        private void SubscribeToPlayerEvents(Player player)
        {
            _player.OnMoveAttempt += HandleActorMove;
            _player.OnLevelChange += HandleLevelChange;
        }

        public event EventHandler<MoveEventArgs> OnActorWasMoved;
        public event EventHandler<StatusMessageEventArgs> OnStatusMessageSent;

        public Level()
        {

        }

        public Level(TileMap map, List<Item> items, List<INpc> npcs, Player player = null)
        {
            tileMap = map;
            this.items = items;
            _npcs = npcs;
            _player = player;
        }



        // The bool is an IsPlayer flag used by UI
        public Tuple<BeingType, Compass, bool>[,] ConvertMapToBeingTypes()
        {
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

        private void HandleActorMove(object sender, MoveEventArgs eventArgs)
        {
            Actor actor = sender as Actor;
            Compass direction = eventArgs.Direction;
            
            Tile newTile = TileMap.GetTile(actor.CurrentTile.TilePoint + Direction.GetPoint(direction));

            // GetTile() return null if the destination tile is outside the bounds of the TileMap
            if (newTile == null)
                return;

            if(actor is Player && IsNpcOnTile(newTile))
            {
                actor.Attack(GetNpcOnTile(newTile));
                _isPlayersTurn = false;
            }
            else if(actor is INpc && IsPlayerOnTile(newTile))
            {
                actor.Attack(_player);
            }
            else if(newTile.IsWalkable)
            {
                actor.CurrentTile = newTile;
                OnActorWasMoved?.Invoke(actor, new MoveEventArgs(direction, actor.CurrentTile.TilePoint));
                _isPlayersTurn = actor is Player ? false : _isPlayersTurn;
            }
        }

        private void HandleLevelChange(object sender, LevelChangeEventArgs eventArgs)
        {
            UnsubscribeFromPlayerEvents(_player);
            _player = null;
        }

        private void UnsubscribeFromPlayerEvents(Player player)
        {
            _player.OnMoveAttempt -= HandleActorMove;
            _player.OnStatusMsgSent -= HandleStatusMessage;
            _player.OnLevelChange -= HandleLevelChange;
        }

        private void HandleStatusMessage(object sender, StatusMessageEventArgs eventArgs)
        {
            OnStatusMessageSent?.Invoke(this, eventArgs);
        }

        public Tile GetStairsUpTile()
        {
            return tileMap.FindTileType(TileType.StairsUp);
        }

        public Tile GetStairsDownTile()
        {
            return tileMap.FindTileType(TileType.StairsDown);
        }


        public void PlaceItemsOnTileMap(List<Item> items)
        {
            for(int i = 0; i < items.Count; i++)
            {
                var tile = GetEmptyWalkableTile();
                tile.AddItem(items[i]);
            }
        }

        // returns a suitable starting tile for the player or enemy
        // Does not check for empty state yet
        public Tile GetEmptyWalkableTile()
        {
            while (true)
            {
                int x = Dungeon._random.Next(tileMap.TilesWide - 1);
                int y = Dungeon._random.Next(tileMap.TilesHigh - 1);
                if (tileMap.IsTileWalkable(x, y))
                {
                    return tileMap.GetTile(new Point(x, y));
                }
            }
        }


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
