using System.Collections.Generic;
using Paramita.GameLogic.Data;
using Paramita.GameLogic.Actors;
using Paramita.GameLogic.Items;
using System;
using Paramita.GameLogic.Data.Levels;

namespace Paramita.GameLogic.Levels
{
    public static class LevelFactory
    {
        public static ILevelDataCreator dataCreator = new LevelDataCreator();
        public static ITileMapCreator mapCreator = new TileMapCreator();

        public static Level CreateLevel(int levelNumber, Player player = null)
        {
            var levelData = dataCreator.CreateLevelData(levelNumber);
                       
            return new Level(
                CreateTileMap(levelData.TileMap),
                CreateNpcs(levelData.Actors),
                CreateItems(levelData.Items),
                player);
        }


        #region Helper Methods
        

        private static TileMap CreateTileMap(TileMapData data)
        {
            return new TileMap(mapCreator.CreateMap(data), "level 1");
        }

        private static List<INpc> CreateNpcs(List<Tuple<ActorType, int>> actors)
        {
            var npcs = new List<INpc>();
            
            foreach(var actorType in actors)
            {
                for(int i = 0; i < actorType.Item2; i++)
                {
                    npcs.Add(ActorCreator.CreateActor(actorType.Item1) as INpc);
                }
            }

            return npcs;
        }

        private static List<Item> CreateItems(List<Tuple<ItemType, int>> items)
        {
            var itemList = new List<Item>();

            foreach (var itemType in items)
            {
                for (int i = 0; i < itemType.Item2; i++)
                {
                    itemList.Add(ItemCreator.CreateItem(itemType.Item1));
                }
            }

            return itemList;
        }
        #endregion
    }
}
