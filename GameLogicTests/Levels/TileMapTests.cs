using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Paramita.GameLogic.Items;
using Paramita.GameLogic.Items.Weapons;
using Paramita.GameLogic.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogicTests.Levels
{
    [TestClass]
    public class TileMapTests
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

        ShortSword sword = ItemCreator.CreateShortSword();
        
        ItemType[,] expectedItemTypeArray = new ItemType[3,3]
        {
            { ItemType.None, ItemType.None, ItemType.None },
            { ItemType.None, ItemType.None, ItemType.None },
            { ItemType.ShortSword, ItemType.None, ItemType.None }
        };

        public TileMap CreateTestMap()
        {
            return new TileMap(tiles, "3by3_testmap");
        }
        #endregion


        #region Tile and Item Layer Conversion API
        // these are actually Tile/TileMap integration tests
        [TestMethod]
        public void ConvertMapToTileTypes_Test_Result_ShouldPass()
        {
            var tileMap = CreateTestMap();
            var actual = tileMap.ConvertMapToTileTypes();

            Assert.IsNotNull(actual);
            CollectionAssert.AllItemsAreNotNull(actual);
            CollectionAssert.AllItemsAreInstancesOfType(actual, typeof(TileType));
            CollectionAssert.AreEqual(expectedTileTypeArray, actual);
        }

        [TestMethod]
        public void ConvertMapToItemTypes_Test_Result_ShouldPass()
        {
            var tileMap = CreateTestMap();
            tileMap.GetTile(new Point(2,0)).AddItem(sword);
            var actual = tileMap.ConvertMapToItemTypes();

            CollectionAssert.AreEqual(expectedItemTypeArray, actual);
        }
        #endregion


        #region Tile Getter and Setter API
        [TestMethod]
        public void GetTile_Test_ShouldGetCorrectTileAtPoint()
        {
            var tileMap = CreateTestMap();
            var point = new Point(2, 2);
            var expected = new Tile(2, 2, TileType.Floor);
            var actual = tileMap.GetTile(point);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetTile_Test_ShouldThrowExceptionOnPointOutOfBounds()
        {
            var tileMap = CreateTestMap();
            var point = new Point(10, 10);
            tileMap.GetTile(point);
        }

        [TestMethod]
        public void SetTile_Test_TileMapShouldGetTileEqualToArgument()
        {
            var tileMap = CreateTestMap();
            var expected = new Tile(2, 1, TileType.StairsUp);
            tileMap.SetTile(expected);

            var actual = tileMap.GetTile(new Point(2, 1));

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void SetTile_Test_ShouldThrowExceptionOnNullArgument()
        {
            var tileMap = CreateTestMap();
            tileMap.SetTile(null);
        }
        #endregion


        #region IsTileWalkable()
        [TestMethod]
        public void IsTileWalkable_Test_TrueFalseCases()
        {
            var tileMap = CreateTestMap();
            var pointIsWalkable = new Point(2, 0);
            var pointIsNotWalkable = new Point(0, 0);

            var isWalkable = tileMap.IsTileWalkable(pointIsWalkable);
            var isNotWalkable = tileMap.IsTileWalkable(pointIsNotWalkable);

            Assert.AreEqual(true, isWalkable);
            Assert.AreEqual(false, isNotWalkable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IsTileWalkable_Test_ShouldThrowExceptionOnPointOutOfBounds()
        {
            var tileMap = CreateTestMap();
            var pointOutsideTileMap = new Point(3, 3);
            tileMap.IsTileWalkable(pointOutsideTileMap);
        }
        #endregion


        #region FindTileType()
        [TestMethod]
        public void FindTileType_Test_FindsImplementedTypes()
        {
            var tileMap = CreateTestMap();

            var expectedStairsUp = new Tile(0, 2,TileType.StairsUp);
            var expectedStairsDown = new Tile(1, 0, TileType.StairsDown);

            var stairsUpTile = tileMap.FindTileType(TileType.StairsUp);
            var stairsDownTile = tileMap.FindTileType(TileType.StairsDown);

            Assert.AreEqual(expectedStairsUp, stairsUpTile);
            Assert.AreEqual(expectedStairsDown, stairsDownTile);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void FindTileType_Test_ThrowsExceptionIfNotFound()
        {
            var tileMap = CreateTestMap();
            tileMap.SetTile(new Tile(0, 2, TileType.Floor));
            tileMap.FindTileType(TileType.StairsUp);
        }

        [TestMethod]
        public void FindTileType_ThrowsExceptionUnimplementedTypes()
        {
            var tileMap = CreateTestMap();
            try
            {
                tileMap.FindTileType(TileType.Floor);
            }
            catch(NotImplementedException e)
            {
                StringAssert.Contains(e.Message, "TileType.Floor");
            }

            try
            {
                tileMap.FindTileType(TileType.Door);
            }
            catch (NotImplementedException e)
            {
                StringAssert.Contains(e.Message, "TileType.Door");
            }

            try
            {
                tileMap.FindTileType(TileType.None);
            }
            catch (NotImplementedException e)
            {
                StringAssert.Contains(e.Message, "TileType.None");
            }

            try
            {
                tileMap.FindTileType(TileType.Wall);
            }
            catch (NotImplementedException e)
            {
                StringAssert.Contains(e.Message, "TileType.Wall");
            }
        }
        #endregion
    }
}
