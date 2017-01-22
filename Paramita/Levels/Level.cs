using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Items;
using Paramita.SentientBeings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita.Levels
{
    /*
     * This class represents a single level of a multi-level dungeon.
     */
    public class Level
    {
        private TileMap tileMap;
        private List<SentientBeing> npcs;
        private List<Item> items;
        private List<StoryEvent> storyEvents;
        private Player player;
        private bool isPlayersTurn = true;

        public bool IsPlayersTurn
        {
            get { return isPlayersTurn; }
            set { isPlayersTurn = value; }
        }

        public TileMap TileMap
        {
            get { return tileMap; }
            set { tileMap = value; }
        }

        public List<SentientBeing> Npcs
        {
            get { return npcs; }
            set { npcs = value; }
        }

        public List<Item> Items
        {
            get { return items; }
            set { items = value; }
        }

        public List<StoryEvent> StoryEvents
        {
            get { return storyEvents; }
            set { storyEvents = value; }
        }

        public Player Player
        {
            get { return player; }
            set { player = value; }
        }

        public Level()
        {

        }

        public Level(TileMap map, List<Item> items, List<SentientBeing> npcs, List<StoryEvent> events = null, Player player = null)
        {
            tileMap = map;
            this.items = items;
            this.npcs = npcs;
            storyEvents = events;
            this.player = player;
        }



        public Tile GetStairsUpTile()
        {
            return tileMap.FindTileType(TileType.StairsUp);
        }

        // returns a suitable starting tile for the player or enemy
        // Does not check for empty state yet
        public Tile GetEmptyWalkableTile()
        {
            while (true)
            {
                int x = GameController.random.Next(tileMap.TilesWide - 1);
                int y = GameController.random.Next(tileMap.TilesHigh - 1);
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
            for (int i = 0; i < npcs.Count; i++)
            {
                if (npcs[i].CurrentTile == tile)
                {
                    return true;
                }
            }
            return false;
        }

        // Finds the NPC that is located on @tile. If none is there, null is returned
        // (this method should be called after IsNpcOnTile check)
        public SentientBeing GetNpcOnTile(Tile tile)
        {
            for (int x = 0; x < npcs.Count; x++)
            {
                if (npcs[x].CurrentTile == tile)
                {
                    return npcs[x];
                }
            }
            return null;
        }



        public void Update(GameTime gameTime)
        {
            // remove dead npcs before updating them
            for (int x = 0; x < npcs.Count; x++)
            {
                if (npcs[x].IsDead == true)
                {
                    npcs.Remove(npcs[x]);
                }
            }

            // check for player's input until he moves
            if (isPlayersTurn == true)
            {
                player.Update(gameTime);
            }
            // give the npcs a turn after the player moves
            else
            {
                for (int x = 0; x < npcs.Count; x++)
                {
                    npcs[x].TimesAttacked = 0;
                    npcs[x].Update(gameTime);
                }
                isPlayersTurn = true;
                player.TimesAttacked = 0;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            tileMap.Draw(gameTime, spriteBatch);

            player.Draw(gameTime, spriteBatch);

            for (int x = 0; x < npcs.Count; x++)
            {
                npcs[x].Draw(gameTime, spriteBatch);
            }
        }
    }
}
