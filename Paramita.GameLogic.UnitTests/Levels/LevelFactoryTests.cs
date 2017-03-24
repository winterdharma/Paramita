using System;
using System.Collections.Generic;
using NUnit.Framework;
using Paramita.GameLogic.Data.Levels;
using Paramita.GameLogic.Levels;
using Paramita.GameLogic.Actors;
using Paramita.GameLogic.Items;

namespace Paramita.GameLogic.UnitTests.Levels
{
    [TestFixture]
    public class LevelFactoryTests
    {

        #region CreateLevel Tests
        [Test]
        public void CreateLevel_GivenTileMapData_CreatesEquivalentMap()
        {
            var fakeLevelDataCreator = new FakeLevelDataCreator();
            LevelFactory.dataCreator = fakeLevelDataCreator;
            
            var fakeTileMapCreator = new FakeTileMapCreator();
            LevelFactory.mapCreator = fakeTileMapCreator;

            Tile[,] tiles = new Tile[3, 3]
            {
                {
                    new Tile(0,0, TileType.Wall),
                    new Tile(0,1, TileType.Wall),
                    new Tile(0,2, TileType.StairsUp)
                },
                {
                    new Tile(1,0, TileType.StairsDown),
                    new Tile(1,1, TileType.Wall),
                    new Tile(1,2, TileType.Wall)
                },
                {
                    new Tile(2,0, TileType.Floor),
                    new Tile(2,1, TileType.Floor),
                    new Tile(2,2, TileType.Floor)
                }
            };
            var expected = new TileMap(tiles, "level 1");

            var level = LevelFactory.CreateLevel(1);
            var actual = level.TileMap;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CreateLevel_GivenActorsData_CreatesActors()
        {
            var fakeLevelDataCreator = new FakeLevelDataCreator();
            LevelFactory.dataCreator = fakeLevelDataCreator;

            var fakeTileMapCreator = new FakeTileMapCreator();
            LevelFactory.mapCreator = fakeTileMapCreator;

            var expected = new List<INpc>()
            {
                ActorCreator.CreateGiantRat(),
                ActorCreator.CreateGiantRat()
            };

            var actual = LevelFactory.CreateLevel(1).Npcs;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CreateLevel_GivenItemsData_CreatesItems()
        {
            var fakeLevelDataCreator = new FakeLevelDataCreator();
            LevelFactory.dataCreator = fakeLevelDataCreator;

            var fakeTileMapCreator = new FakeTileMapCreator();
            LevelFactory.mapCreator = fakeTileMapCreator;

            var expected = new List<Item>()
            {
                ItemCreator.CreateItem(ItemType.Meat),
                ItemCreator.CreateItem(ItemType.ShortSword)
            };

            var level = LevelFactory.CreateLevel(1);
            var actual = TileMapTests.GetItemsOnMap(level.TileMap);

            CollectionAssert.AreEquivalent(expected, actual);
        }
        #endregion


        #region Helper Methods

        #endregion
    }

    internal class FakeLevelDataCreator : ILevelDataCreator
    {
        public LevelData CreateLevelData(int levelNumber)
        {
            var levelData = new LevelData();

            levelData.TileMap = new TileMapData();
            levelData.Actors = CreateActors();
            levelData.Items = CreateItems();

            return levelData;
        }

        private List<Tuple<BeingType, int>> CreateActors()
        {
            return new List<Tuple<BeingType, int>>()
            {
                new Tuple<BeingType, int>(BeingType.GiantRat, 2)
            };
        }

        private List<Tuple<ItemType, int>> CreateItems()
        {
            return new List<Tuple<ItemType, int>>()
            {
                new Tuple<ItemType, int>(ItemType.Meat, 1),
                new Tuple<ItemType, int>(ItemType.ShortSword, 1)
            };
        }
    }

    internal class FakeTileMapCreator : ITileMapCreator
    {
        public Tile[,] CreateMap(TileMapData data)
        {
            Tile[,] tiles = new Tile[3, 3]
            {
                {
                    new Tile(0,0, TileType.Wall),
                    new Tile(0,1, TileType.Wall),
                    new Tile(0,2, TileType.StairsUp)
                },
                {
                    new Tile(1,0, TileType.StairsDown),
                    new Tile(1,1, TileType.Wall),
                    new Tile(1,2, TileType.Wall)
                },
                {
                    new Tile(2,0, TileType.Floor),
                    new Tile(2,1, TileType.Floor),
                    new Tile(2,2, TileType.Floor)
                }
            };

            return tiles;
        }
    }
}
