using Microsoft.Xna.Framework;
using NSubstitute;
using NUnit.Framework;
using Paramita.GameLogic.Actors;
using Paramita.GameLogic.Items;
using Paramita.GameLogic.Levels;
using Paramita.GameLogic.Mechanics;
using System;
using System.Collections.Generic;

namespace Paramita.GameLogic.UnitTests.Levels
{
    [TestFixture]
    public class LevelTests
    {
        #region TileMap Layer Property Tests
        [Test]
        public void GetTileTypeLayer_ShouldReturnTileTypeArray()
        {
            var level = MakeLevel();
            var expected = new TileType[3, 3]
            {
                { TileType.Wall, TileType.Wall, TileType.StairsUp },
                { TileType.StairsDown, TileType.Wall, TileType.Wall },
                { TileType.Floor, TileType.Floor, TileType.Floor }
            };

            var actual = level.TileTypeLayer;

            Assert.IsNotNull(actual);
            CollectionAssert.AllItemsAreNotNull(actual);
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void GetItemTypeLayer_ShouldReturnItemTypeArray()
        {
            var level = MakeLevel();

            level.TileMap
                .GetTile(new Point(2, 0))
                .AddItem(ItemCreator.CreateShortSword());

            var expected = new ItemType[3, 3]
            {
                { ItemType.None, ItemType.None, ItemType.None },
                { ItemType.None, ItemType.None, ItemType.None },
                { ItemType.ShortSword, ItemType.None, ItemType.None }
            };

            var actual = level.ItemTypeLayer;

            Assert.IsNotNull(actual);
            CollectionAssert.AllItemsAreNotNull(actual);
            CollectionAssert.AreEqual(expected, actual);
        }
        
        [Test]
        public void GetBeingTypeLayer_ShouldReturnBeingTypeArray()
        {
            var level = MakeLevel();
            var expected = new Tuple<ActorType, Compass, bool>[3, 3] {
                { null, null, null },
                { null, null, null },
                { new Tuple<ActorType, Compass, bool>(ActorType.GiantRat, Compass.East, false),
                  new Tuple<ActorType, Compass, bool>(ActorType.GiantRat, Compass.East, false),
                  new Tuple<ActorType, Compass, bool>(ActorType.HumanPlayer, Compass.East, true) }
            };
            var actual = level.BeingTypeLayer;

            Assert.IsNotNull(actual);
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void GetBeingTypeLayer_WhenPlayerIsNull_ThrowsException()
        {
            var level = MakeLevel();
            level.Player = null;
            Tuple<ActorType, Compass, bool>[,] actual = null;
            Assert.Catch<NullReferenceException>(() => actual = level.BeingTypeLayer);
        }

        [Test]
        public void GetBeingTypeLayer_WhenNpcsIsNull_ThrowsExceptions()
        {
            var level = MakeLevel();
            level.Npcs = null;
            Tuple<ActorType, Compass, bool>[,] actual = null;
            Assert.Catch<NullReferenceException>(() => actual = level.BeingTypeLayer);
        }
        #endregion


        #region Entry Tile Property Tests
        [Test]
        public void GetEntryFromAbove_ReturnsStairsUpTile()
        {
            var level = MakeLevel();
            var expected = TileType.StairsUp;

            var actual = level.EntryFromAbove.TileType;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetEntryFromBelow_ReturnsStairsDownTile()
        {
            var level = MakeLevel();
            var expected = TileType.StairsDown;

            var actual = level.EntryFromBelow.TileType;

            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Npcs Property Tests
        [Test]
        public void SetNpcs_NonNullNpcs_PlacesNpcsOnWalkableTile()
        {
            var level = MakeLevel();
            var npcs = new List<INpc>()
            {
                ActorCreator.CreateGiantRat()
            };

            level.Npcs = npcs;

            Assert.IsNotNull(level.Npcs[0].CurrentTile);
            Assert.True(level.Npcs[0].CurrentTile.IsWalkable);
        }
        #endregion


        #region Update Method Tests
        [Test]
        public void Update_WhenUpdatingNpcs_RemovesDeadNpcs()
        {
            var level = MakeLevel();
            var expected = level.Npcs;
            expected.RemoveAt(0);

            // enough damage to set Actor.IsDead to true
            level.Npcs[0].IsDead = true;
            level.Update();

            var actual = level.Npcs;

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void Update_WhenIsPlayersTurnTrue_UpdatesPlayer()
        {
            var level = MakeLevel();
            level.Player = Substitute.For<IPlayer>();
            level.IsPlayersTurn = true;

            level.Update();

            level.Player.Received().Update();
        }

        [Test]
        public void Update_WhenPlayersTurnFalse_UpdatesNpcs()
        {
            var level = MakeLevel();
            level.Npcs = new List<INpc>()
            {
                Substitute.For<INpc>(),
                Substitute.For<INpc>()
            };
            level.IsPlayersTurn = false;

            level.Update();

            foreach(INpc npc in level.Npcs)
            {
                npc.Received().Update(level.Player as Player);
            }
        }

        [TestCase(true, 1)]
        [TestCase(false, 0)]
        public void Update_ResetsTimesAttackedWhenPlayersTurnFalse(
            bool testCase, int expected)
        {
            var level = MakeLevel();
            level.Player.TimesAttacked = 1;
            level.Npcs[0].TimesAttacked = 1;

            level.IsPlayersTurn = testCase;

            level.Update();

            var actualPlayer = level.Player.TimesAttacked;
            var actualNpc = level.Npcs[0].TimesAttacked;

            Assert.AreEqual(expected, actualPlayer);
            Assert.AreEqual(expected, actualNpc);
        }

        // This test assures that game waits for player to take his turn before
        // updating opponents.
        [TestCase(true, true)]
        [TestCase(false, true)]
        public void Update_IsPlayersTurn_ChangesOnlyWhenStartsFalse(
            bool testCase, bool expected)
        {
            var level = MakeLevel();
            level.IsPlayersTurn = testCase;

            level.Update();

            var actual = level.IsPlayersTurn;

            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Helpers
        public List<Item> MakeItems()
        {
            return new List<Item>()
            {
                ItemCreator.CreateShortSword(),
                ItemCreator.CreateBuckler()
            };
        }

        public TileMap MakeTileMap()
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

            return new TileMap(tiles, "3x3_testmap");
        }

        public Level MakeLevel()
        {
            var tileMap = MakeTileMap();

            var npcs = new List<INpc>()
            {
                ActorCreator.CreateGiantRat(),
                ActorCreator.CreateGiantRat()
            };

            var player = new Player("Test_player");
            player.CurrentTile = tileMap.GetTile(new Point(2, 2));
            
            var level = new Level(MakeTileMap(), npcs, null, player);

            level.Npcs[0].CurrentTile = tileMap.GetTile(new Point(2, 0));
            level.Npcs[1].CurrentTile = tileMap.GetTile(new Point(2, 1));

            return level;
        }
        #endregion
    }
}
