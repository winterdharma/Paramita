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

    public static class LevelManager
    {
        private static Dictionary<int, Level> levels = new Dictionary<int, Level>();
        private static Level currentLevel;
        private static int levelNumber;


        public static int LevelNumber
        {
            get { return levelNumber; }
            set { levelNumber = value; }
        }

        public static Level CurrentLevel
        {
            get { return currentLevel; }
            set { currentLevel = value; }
        }


        public static void ChangeLevel(int change, Player player)
        {
            levelNumber += change;

            MoveToLevel(levelNumber, player);
        }


        public static void MoveToLevel(int levelNumber, Player player)
        {
            if(!levels.ContainsKey(levelNumber))
            {
                Create(levelNumber);
            }

            SetLevel(levelNumber, player);
        }


        public static Level Create(int number)
        {
            levels[number] = LevelFactory.CreateLevel(number);
            return levels[number];
        }



        private static void SetLevel(int levelNumber, Player player)
        {
            CurrentLevel.Player = null;
            CurrentLevel = levels[levelNumber];
            CurrentLevel.Player = player;
        }

        public static Dictionary<int, Level> GetLevels()
        { return levels; }
    }
}
