using Paramita.Items;
using Paramita.Items.Weapons;
using Paramita.SentientBeings;
using System;
using System.Collections.Generic;

namespace Paramita.Levels
{
    /*
     * This class handles the creation and storage of TileMaps
     * and provides access to them for level transitions.
     */

    public class LevelManager
    {
        public Dictionary<int, Level> levels;
        private Level currentLevel;
        private TileMapCreator mapCreator;
        private TileSet tileset;
        private GameController game;

        public Level CurrentLevel
        {
            get { return currentLevel; }
            private set { currentLevel = value; }
        }
        


        public LevelManager(GameController game, TileSet tileset, Random random)
        {
            this.game = game;
            mapCreator = new TileMapCreator(40, 25, 10, 8, 3, random);
            this.tileset = tileset;
            levels = new Dictionary<int, Level>();
        }

       

        public void MoveToLevel(int levelNumber)
        {
            if(levels.ContainsKey(levelNumber) == false)
            {
                Create(levelNumber);
            }

            SetLevel(levelNumber);
        }

        public void Create(int number, Player player = null)
        {
            Level newLevel = LevelFactory.CreateLevel(number);
        }

        

        private void SetLevel(int levelNumber)
        {
            CurrentLevel = levels[levelNumber];
        }

        public Dictionary<int, Level> GetLevels()
        { return levels; }
    }
}
