using Paramita.Items;
using System;
using System.Collections.Generic;

namespace Paramita.Scenes
{
    /*
     * This class handles the creation and storage of TileMaps
     * and provides access to them for level transitions.
     */

    public class LevelManager
    {
        public Dictionary<int, TileMap> levels; // (Level number, level tilemap)
        private TileMap currentMap;
        private TileMapCreator mapCreator;
        private ItemCreator itemCreator;
        private TileSet tileset;
        private GameController game;

        public TileMap CurrentMap
        {
            get { return currentMap; }
            private set { currentMap = value; }
        }
        
        public ItemCreator ItemCreator { get { return itemCreator; } }

        public LevelManager(GameController game, TileSet tileset, ItemCreator itemCreator, Random random)
        {
            this.game = game;
            mapCreator = new TileMapCreator(40, 25, 10, 8, 3, random);
            this.tileset = tileset;
            this.itemCreator = itemCreator;
            levels = new Dictionary<int, TileMap>();
        }

       

        public void MoveToLevel(int levelNumber)
        {
            if(levels.ContainsKey(levelNumber) == false)
            {
                CreateLevel(levelNumber);
            }

            SetLevel(levelNumber);
        }

        private void CreateLevel(int levelNumber)
        {
            TileMap levelMap = new TileMap(game.ScreenRectangle, tileset, mapCreator.CreateMap(), 40, 25, "test-map");
            levels.Add(levelNumber, levelMap);
        }

        

        private void SetLevel(int levelNumber)
        {
            CurrentMap = levels[levelNumber];
        }

        public Dictionary<int, TileMap> GetLevels()
        { return levels; }
    }
}
