using Paramita.GameLogic.Actors;
using Paramita.GameLogic.Items;
using System;
using System.Collections.Generic;

namespace Paramita.GameLogic.Data.Levels
{
    public class LevelData : DataFile
    {
        private string levelPath;
        private string unknownProperty;

        public TileMapData TileMap { get; set; }
        public List<Tuple<ActorType, int>> Actors { get; set; }
        public List<Tuple<ItemType, int>> Items { get; set; }

        public LevelData() { } 

        public LevelData(string levelTxtFile) : base(levelTxtFile)
        {
            levelPath = levelTxtFile;
            unknownProperty = "Unimplemented property encountered while processing " + levelPath + "!";

            TileMap = new TileMapData();
            Actors = new List<Tuple<ActorType, int>>();
            Items = new List<Tuple<ItemType, int>>();

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
                    TileMap.LevelWidth = int.Parse(value);
                    break;
                case "LevelHeight":
                    TileMap.LevelHeight = int.Parse(value);
                    break;
                case "MaxRooms":
                    TileMap.MaxRooms = int.Parse(value);
                    break;
                case "MaxRoomSize":
                    TileMap.MaxRoomSize = int.Parse(value);
                    break;
                case "MinRoomSize":
                    TileMap.MinRoomSize = int.Parse(value);
                    break;
                case "GiantRat":
                    Actors.Add(new Tuple<ActorType, int>(ActorType.GiantRat, int.Parse(value)));
                    break;
                case "ShortSword":
                    Items.Add(new Tuple<ItemType, int>(ItemType.ShortSword, int.Parse(value)));
                    break;
                case "Buckler":
                    Items.Add(new Tuple<ItemType, int>(ItemType.Shield, int.Parse(value)));
                    break;
                case "Meat":
                    Items.Add(new Tuple<ItemType, int>(ItemType.Meat, int.Parse(value)));
                    break;
                default:
                    Console.WriteLine(unknownProperty + " (" + property + ")");
                    break;
            }
        }
    }
}
