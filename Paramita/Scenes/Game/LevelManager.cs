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
        private Dictionary<int, TileMap> levels = new Dictionary<int, TileMap>();
        private TileMap currentMap;
        private TileMapCreator mapCreator;
        private TileSet tileset;

        public Dictionary<int, TileMap> Levels { get; private set; }

        public TileMap CurrentMap
        {
            get { return currentMap; }
            private set { currentMap = value; }
        }
        

        public LevelManager(TileSet tileset, Random random)
        {
            mapCreator = new TileMapCreator(80, 80, 10, 20, 10, random);
            this.tileset = tileset;
        }

       


        public TileMap CreateLevel(int levelNumber)
        {
            TileMap levelMap = new TileMap(tileset, mapCreator.CreateMap(), 80, 80, "test-map");
            levels.Add(levelNumber, levelMap);
            return levelMap;
        }
        

        public TileMap GetLevel(int levelNumber)
        {
            return levels[levelNumber];
        }
        


        public void Update(GameTime gameTime)
        {
            CurrentMap.Update(gameTime);
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }
    }
}
