using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Paramita.GameLogic.Actors;
using Paramita.GameLogic.Actors.Animals;
using Paramita.GameLogic.Items;
using Paramita.GameLogic.Items.Armors;
using Paramita.GameLogic.Items.Weapons;
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

        TileType[,] expectedTileTypeArray = new TileType[3, 3]
        {
            { TileType.Wall, TileType.Wall, TileType.StairsUp },
            { TileType.StairsDown, TileType.Wall, TileType.Wall },
            { TileType.Floor, TileType.Floor, TileType.Floor }
        };

        ItemType[,] expectedItemTypeArray = new ItemType[3, 3]
        {
            { ItemType.None, ItemType.None, ItemType.None },
            { ItemType.None, ItemType.None, ItemType.None },
            { ItemType.ShortSword, ItemType.None, ItemType.None }
        };

        Tuple<BeingType, Compass, bool>[,] expectedBeingTypeLayer =
            new Tuple<BeingType, Compass, bool>[3, 3] {
                { null, null, null },
                { null, null, null },
                { new Tuple<BeingType, Compass, bool>(BeingType.GiantRat, Compass.East, false),
                  null,
                  new Tuple<BeingType, Compass, bool>(BeingType.HumanPlayer, Compass.East, true) }
            };

        ShortSword sword = ItemCreator.CreateShortSword();
        Buckler buckler = ItemCreator.CreateBuckler();

        Player player = new Player("Chuck");
        List<Actor> npcs = new List<Actor>() { new GiantRat() };

        public List<Item> CreateItems()
        {
            var items = new List<Item>();
            items.Add(sword);
            items.Add(buckler);
            return items;
        }
        private TileMap CreateTestMap()
        {
            return new TileMap(tiles, "3x3_testmap");
        }
        private Level CreateTestLevel()
        {
            var level = new Level();
            level.TileMap = CreateTestMap();
            level.Player = player;
            level.Player.CurrentTile = 
                level.TileMap.GetTile(new Point(2,2));
            npcs[0].CurrentTile = level.TileMap.GetTile(new Point(2,0));
            level.Npcs = npcs;
            return level;
        }
        #endregion

        #region Tile Getters and Setters
        #region GetStairsUpTile()
        [TestMethod]
        public void GetStairsUpTile_Test_ReturnsStairsUp()
        {
            var level = CreateTestLevel();
            var expected = new Tile(0, 2, TileType.StairsUp);

            var actual = level.GetStairsUpTile();

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetStairsUpTile_ThrowsExceptionIfNotFound()
        {
            var level = CreateTestLevel();
            level.TileMap.SetTile(new Tile(0,2,TileType.Floor));

            level.GetStairsUpTile();
        }
        #endregion

        #region GetStairsDownTile()
        [TestMethod]
        public void GetStairsDownTile_Test_ReturnsStairsDown()
        {
            var level = CreateTestLevel();
            var expected = new Tile(1, 0, TileType.StairsDown);

            var actual = level.GetStairsDownTile();

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetStairsDownTile_ThrowsExceptionIfNotFound()
        {
            var level = CreateTestLevel();
            level.TileMap.SetTile(new Tile(1, 0, TileType.Floor));

            level.GetStairsDownTile();
        }
        #endregion

        #region GetRandomWalkableTile()
        [TestMethod]
        public void GetRandomWalkableTile_Test_GetsTileIsWalkableTrue()
        {
            var level = CreateTestLevel();

            var actual = level.GetRandomWalkableTile();
            var actualWalkable = actual.IsWalkable;

            Assert.AreEqual(true, actualWalkable);
            Assert.IsInstanceOfType(actual, typeof(Tile));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetRandomWalkableTile_Test_ThrowsExceptionIfNoneFound()
        {
            var level = CreateTestLevel();
            for(int i = 0; i < level.TileMap.TilesWide; i++)
            {
                for(int j = 0; j < level.TileMap.TilesWide; j++)
                {
                    var tile = level.TileMap.GetTile(new Point(i, j));
                    if (tile.IsWalkable)
                    {
                        level.TileMap.SetTile(new Tile(tile.TilePoint.X, tile.TilePoint.Y, TileType.Wall));
                    }
                }
            }

            var actual = level.GetRandomWalkableTile();
        }
        #endregion
        #endregion


        #region Update()
        [TestMethod]
        public void Update_RemovesDeadNpc()
        {
            var level = CreateTestLevel();
            var expected = new List<Actor>();

            // enough damage to set Actor.IsDead to true
            level.Npcs[0].TakeDamage(10);
            level.Update();
            var actual = level.Npcs;

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Update_DoesNotRemoveLiveNpc()
        {
            var level = CreateTestLevel();
            var expected = level.Npcs;

            level.Update();
            var actual = level.Npcs;

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Update_UpdatesPlayerWhenPlayersTurnTrue()
        {
            // not implemented
        }

        [TestMethod]
        public void Update_UpdatesNpcsWhenPlayersTurnFalse()
        {
            // not implemented
        }

        [TestMethod]
        public void Update_ResetsTimesAttackedWhenPlayersTurnFalse()
        {
            var level = CreateTestLevel();
            level.Player.TimesAttacked = 1;
            level.Npcs[0].TimesAttacked = 1;
            // trigger a valid move command to toggle _isPlayersTurn
            level.Player.HandleInput(Compass.North);

            var expected = 0;

            level.Update();
            var actualPlayer = level.Player.TimesAttacked;
            var actualNpc = level.Npcs[0].TimesAttacked;

            Assert.AreEqual(expected, actualPlayer);
            Assert.AreEqual(expected, actualNpc);
        }

        [TestMethod]
        public void Update_TogglesIsPlayersTurnWhenFalse()
        {
            var level = CreateTestLevel();
            // valid move toggles IsPlayersTurn flag to false
            level.Player.HandleInput(Compass.North);

            var expected = true;

            level.Update();

            var actual = level.IsPlayersTurn;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Update_DoesNotToggleIsPlayersTurnWhenTrue()
        {
            var level = CreateTestLevel();

            var expected = true;

            level.Update();

            var actual = level.IsPlayersTurn;

            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
