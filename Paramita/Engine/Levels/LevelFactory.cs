using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Paramita.Data;
using Paramita.SentientBeings;
using Paramita.Items;
using Paramita.Scenes;
using Microsoft.Xna.Framework.Graphics;

namespace Paramita.Levels
{
    public static class LevelFactory
    {
        private const string LEVEL_01 = "Paramita.GameLogic.Data.Levels.Level01.txt";
        private static Texture2D tilesheet;
        private static TileSet tileSet;
        
        public static Texture2D Tilesheet
        {
            get { return tilesheet; }
            set { tilesheet = value; }
        }

        public static TileSet TileSet
        {
            set { tileSet = value; }
        }




        public static Level CreateLevel(int levelNumber)
        {
            var levelData = GetLevelData(levelNumber);
            var newLevel = new Level();
            newLevel.TileMap = CreateTileMap(levelData);
            newLevel.Items = CreateItems(levelData);
            newLevel.Npcs = CreateNpcs(levelData, newLevel);
            return newLevel;
        }


        private static LevelData GetLevelData(int levelNumber)
        {
            string levelTxtFile = "";
            switch (levelNumber)
            {
                case 1:
                    levelTxtFile = LEVEL_01;
                    break;
                default:
                    levelTxtFile = LEVEL_01;
                    break;
            }

            return new LevelData(levelTxtFile);
        }


        private static TileMap CreateTileMap(LevelData data)
        {
            var mapCreator = new TileMapCreator(data.LevelWidth, data.LevelHeight, data.MaxRooms, 
                data.MaxRoomSize, data.MinRoomSize, GameController.random);
            return new TileMap(tileSet, mapCreator.CreateMap(), "level 1");
        }


        private static List<SentientBeing> CreateNpcs(LevelData data, Level level)
        {
            var npcs = new List<SentientBeing>();

            if (data.GiantRats > 0)
            {
                for(int i = 0; i < data.GiantRats; i++)
                {
                    var rat = SentientBeingCreator.CreateGiantRat(level);
                    rat.CurrentTile = level.GetEmptyWalkableTile();
                    npcs.Add(rat);
                }
            }

            return npcs;
        }


        private static List<Item> CreateItems(LevelData data)
        {
            var items = new List<Item>();

            if (data.ShortSwords > 0)
            {
                for (int i = 0; i < data.ShortSwords; i++)
                {
                    items.Add(ItemCreator.CreateShortSword());
                }
            }

            if (data.Bucklers > 0)
            {
                for (int i = 0; i < data.Bucklers; i++)
                {
                    items.Add(ItemCreator.CreateBuckler());
                }
            }

            if (data.Meat > 0)
            {
                for (int i = 0; i < data.Meat; i++)
                {
                    items.Add(ItemCreator.CreateMeat());
                }
            }

            return items;
        }
    }
}
