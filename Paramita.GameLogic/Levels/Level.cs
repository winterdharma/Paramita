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
            set { _npcs = value; }
        }

        public List<Item> Items
        {
            get { return items; }
            set { items = value; }
        }


        public Player Player
        {
            get { return _player; }
            set { _player = value; }
        }

        public event EventHandler<LevelChangeEventArgs> OnLevelChange;

        public Level()
        {

        }

        public Level(TileMap map, List<Item> items, List<INpc> npcs, Player player = null)
        {
            tileMap = map;
            this.items = items;
            _npcs = npcs;
            SubscribeToActorEvents();
            _player = player;
        }


        private void SubscribeToActorEvents()
        {
            _player.OnMove += HandleActorMove;
            _player.OnLevelChange += HandleLevelChange;
            foreach(Actor npc in _npcs)
            {
                npc.OnMove += HandleActorMove;
            }
        }

        private void HandleActorMove(object sender, MoveEventArgs eventArgs)
        {
            Actor actor = sender as Actor;
            Compass direction = eventArgs.Direction;
            //sprite.Facing = direction;
            Tile newTile = TileMap.GetTile(actor.CurrentTile.TilePoint + Direction.GetPoint(direction));


            if(actor is Player && IsNpcOnTile(newTile))
            {
                actor.Attack(GetNpcOnTile(newTile));
                _isPlayersTurn = false;
            }
            else if(actor is INpc && IsPlayerOnTile(newTile))
            {
                actor.Attack(_player);
            }
            else if(newTile != null && newTile.IsWalkable)
            {
                actor.CurrentTile = newTile;
                _isPlayersTurn = actor is Player ? false : _isPlayersTurn;
            }
        }

        private void HandleLevelChange(object sender, LevelChangeEventArgs eventArgs)
        {
            _player = null;
            OnLevelChange?.Invoke(this, eventArgs);
        }


        public Tile GetStairsUpTile()
        {
            return tileMap.FindTileType(TileType.StairsUp);
        }

        public Tile GetStairsDownTile()
        {
            return tileMap.FindTileType(TileType.StairsDown);
        }

        // returns a suitable starting tile for the player or enemy
        // Does not check for empty state yet
        public Tile GetEmptyWalkableTile()
        {
            while (true)
            {
                int x = Game._random.Next(tileMap.TilesWide - 1);
                int y = Game._random.Next(tileMap.TilesHigh - 1);
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
