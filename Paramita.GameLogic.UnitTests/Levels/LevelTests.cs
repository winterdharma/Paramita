using Microsoft.Xna.Framework;
using NUnit.Framework;
using Paramita.GameLogic.Actors;
using Paramita.GameLogic.Items;
using Paramita.GameLogic.Levels;
using Paramita.GameLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var expected = new Tuple<BeingType, Compass, bool>[3, 3] {
                { null, null, null },
                { null, null, null },
                { new Tuple<BeingType, Compass, bool>(BeingType.GiantRat, Compass.East, false),
                  null, new Tuple<BeingType, Compass, bool>(BeingType.HumanPlayer, Compass.East, true) }
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
            Tuple<BeingType, Compass, bool>[,] actual = null;
            Assert.Catch<NullReferenceException>(() => actual = level.BeingTypeLayer);
        }

        [Test]
        public void GetBeingTypeLayer_WhenNpcsIsNull_ThrowsExceptions()
        {
            var level = MakeLevel();
            level.Npcs = null;
            Tuple<BeingType, Compass, bool>[,] actual = null;
            Assert.Catch<NullReferenceException>(() => actual = level.BeingTypeLayer);
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
            var level = new Level();
            level.TileMap = MakeTileMap();
            level.Player = new Player("Test_player");
            level.Player.CurrentTile =
                level.TileMap.GetTile(new Point(2, 2));
            level.Npcs = new List<Actor>() { ActorCreator.CreateGiantRat() };
            level.Npcs[0].CurrentTile = level.TileMap.GetTile(new Point(2, 0));
            return level;
        }
        #endregion
    }
}
