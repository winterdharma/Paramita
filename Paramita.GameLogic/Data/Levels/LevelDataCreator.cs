using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita.GameLogic.Data.Levels
{
    public interface ILevelDataCreator
    {
        LevelData CreateLevelData(int levelNumber);
    }

    public class LevelDataCreator : ILevelDataCreator
    {
        private static readonly List<string> LEVEL_FILES = new List<string>()
        {
            "",
            "Paramita.GameLogic.Data.Levels.Level01.txt"
        };

        public LevelData CreateLevelData(int levelNumber)
        {
            if (levelNumber < 1)
                levelNumber = 1;
            else if (levelNumber > LEVEL_FILES.Count - 1)
                levelNumber = LEVEL_FILES.Count - 1;

            string levelTxtFile = LEVEL_FILES[levelNumber];
            return new LevelData(levelTxtFile);
        }
    }
}
