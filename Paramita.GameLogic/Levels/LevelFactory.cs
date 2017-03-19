using System.Collections.Generic;
using Paramita.GameLogic.Data;
using Paramita.GameLogic.Actors;
using Paramita.GameLogic.Items;

namespace Paramita.GameLogic.Levels
{
    public static class LevelFactory
    {
        private const string LEVEL_01 = "Paramita.GameLogic.Data.Levels.Level01.txt";


        public static Level CreateLevel(int levelNumber)
        {
            var levelData = GetLevelData(levelNumber);
            var newLevel = new Level();
            newLevel.TileMap = CreateTileMap(levelData);
            var items = CreateItems(levelData);
            newLevel.TileMap.PlaceItemsOnRandomTiles(items);
            
            newLevel.Npcs = CreateNpcs(levelData);
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
                data.MaxRoomSize, data.MinRoomSize, Dungeon._random);
            return new TileMap(mapCreator.CreateMap(), "level 1");
        }


        private static List<Actor> CreateNpcs(LevelData data)
        {
            var npcs = new List<Actor>();

            if (data.GiantRats > 0)
            {
                for(int i = 0; i < data.GiantRats; i++)
                {
                    npcs.Add(ActorCreator.CreateGiantRat());
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
