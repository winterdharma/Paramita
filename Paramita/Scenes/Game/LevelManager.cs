using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Paramita.Scenes
{
    /*
     * This class will become a LevelManager that handles the saving of TileMaps
     * as they are created and handles level transitions.
     */

    public class LevelManager
    {
        public Dictionary<int, TileMap> levels;
        private TileMap currentMap;
        private TileMapCreator mapCreator;
        private TileSet tileset;


        public TileMap CurrentMap
        {
            get { return currentMap; }
            private set { currentMap = value; }
        }
        

        public LevelManager(TileSet tileset, Random random)
        {
            mapCreator = new TileMapCreator(40, 25, 10, 20, 10, random);
            this.tileset = tileset;
            levels = new Dictionary<int, TileMap>();
        }

       


        public TileMap CreateLevel(int levelNumber)
        {
            TileMap levelMap = new TileMap(tileset, mapCreator.CreateMap(), 40, 25, "test-map");
            currentMap = levelMap;
            levels.Add(levelNumber, levelMap);
            return levelMap;
        }
        

        public TileMap GetLevel(int levelNumber)
        {
            return levels[levelNumber];
        }

        public Dictionary<int, TileMap> GetLevels()
        { return levels; }
    }
}
