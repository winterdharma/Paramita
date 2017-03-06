using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Paramita.GameLogic.Actors;
using Paramita.GameLogic.Actors.Animals;
using Paramita.GameLogic.Levels;
using Paramita.GameLogic.Mechanics;
using System;
using System.Collections.Generic;

namespace GameLogicTests.Levels
{
    [TestClass]
    public class LevelTests
    {
        #region Test Fixtures
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

        Tuple<BeingType, Compass, bool>[,] expectedBeingTypeLayer =
            new Tuple<BeingType, Compass, bool>[3, 3] {
                { null, null,
                  new Tuple<BeingType, Compass, bool>(BeingType.HumanPlayer, Compass.East, true) },
                { null, null, null },
                { new Tuple<BeingType, Compass, bool>(BeingType.GiantRat, Compass.East, false),
                  null, null }
            };

        Player player = new Player("Chuck");
        List<INpc> npcs = new List<INpc>() { new GiantRat() };

        private TileMap CreateTestMap()
        {
            return new TileMap(tiles, "3x3_testmap");
        }
        private Level CreateTestLevel()
        {
            var level = new Level(CreateTestMap(), npcs, player);
            level.Player.CurrentTile = 
                level.TileMap.GetTile(new Point(0,2));
            var npc = npcs[0] as Actor;
            npc.CurrentTile = level.TileMap.GetTile(new Point(2,0));
            level.Npcs = npcs;
            return level;
        }
        #endregion


        [TestMethod]
        public void ConvertMapToBeingTypes_Test()
        {
            var level = CreateTestLevel();
            var actual = level.ConvertMapToBeingTypes();

            CollectionAssert.AreEqual(expectedBeingTypeLayer, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ConvertMapToBeingTypes_ThrowsExceptionWhenPlayerNull()
        {
            var level = CreateTestLevel();
            level.Player = null;
            var actual = level.ConvertMapToBeingTypes();
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ConvertMapToBeingTypes_ThrowsExceptionWhenNpcsNull()
        {
            var level = CreateTestLevel();
            level.Npcs = null;
            var actual = level.ConvertMapToBeingTypes();
        }
    }
}
