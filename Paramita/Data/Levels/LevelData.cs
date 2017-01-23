using System;

namespace Paramita.Data
{
    public class LevelData : DataFile
    {
        private string levelPath;
        private int levelWidth;
        private int levelHeight;
        private int maxRooms;
        private int maxRoomSize;
        private int minRoomSize;
        private int giantRats;
        private int shortSwords;
        private int bucklers;
        private int meat;
        private string unknownProperty;

        public int LevelWidth { get { return levelWidth; } }
        public int LevelHeight { get { return levelHeight; } }
        public int MaxRooms { get { return maxRooms; } }
        public int MaxRoomSize { get { return maxRoomSize; } }
        public int MinRoomSize { get { return minRoomSize; } }

        public int GiantRats { get { return giantRats; } }

        public int ShortSwords { get { return shortSwords; } }
        public int Bucklers { get { return bucklers; } }
        public int Meat { get { return meat; } }

        public LevelData(string levelTxtFile) : base(levelTxtFile)
        {
            levelPath = levelTxtFile;
            unknownProperty = "Unimplemented property encountered while processing " + levelPath + "!";
            ParseLevelFile();
        }

        private void ParseLevelFile()
        {
            while(txtFileLines.Count > 0)
            {
                ParseNextLine(txtFileLines[0]);
                txtFileLines.RemoveAt(0);
            }
        }

        private void ParseNextLine(string line)
        {
            string[] lineSplit = line.Split(':');
            ParseProperty(lineSplit);

        }

        private void ParseProperty(string[] line)
        {
            string property = line[0];
            string value = line[1];

            switch (property)
            {
                case "LevelWidth":
                    levelWidth = int.Parse(value);
                    break;
                case "LevelHeight":
                    levelHeight = int.Parse(value);
                    break;
                case "MaxRooms":
                    maxRooms = int.Parse(value);
                    break;
                case "MaxRoomSize":
                    maxRoomSize = int.Parse(value);
                    break;
                case "MinRoomSize":
                    minRoomSize = int.Parse(value);
                    break;
                case "GiantRat":
                    giantRats = int.Parse(value);
                    break;
                case "ShortSword":
                    shortSwords = int.Parse(value);
                    break;
                case "Buckler":
                    bucklers = int.Parse(value);
                    break;
                case "Meat":
                    meat = int.Parse(value);
                    break;
                default:
                    Console.WriteLine(unknownProperty + " (" + property + ")");
                    break;
            }
        }
    }
}
