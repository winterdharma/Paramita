using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Paramita.GameLogic.Items;
using Paramita.GameLogic.Items.Consumables;
using Paramita.GameLogic.Levels;
using Paramita.GameLogic.Mechanics;
using System;

namespace GameLogicTests.Levels
{
    [TestClass]
    public class TileTests
    {
        private string notSetCorrect = "was not set correctly.";
        private string tilePoint = "TilePoint ";
        private string tileType = "TileType property ";
        private string isWalkable = "IsWalkable flag ";
        private string isTransparent = "IsTransparatent flag ";
        Meat item = ItemCreator.CreateMeat();
        Tile tile = new Tile(10, 10, TileType.Floor);

        #region Constructor Tests
        [TestMethod]
        public void Create_Tile_CorrectTilePoint()
        {
            var expectedPoint = new Point(9, 13);

            var tile = new Tile(9, 13, TileType.Floor);

            var actualPoint = tile.TilePoint;

            Assert.AreEqual(expectedPoint, actualPoint, tilePoint + notSetCorrect);
        }

        [TestMethod]
        public void Create_Tile_Floor_CorrectTileTypeAndFlags()
        {
            var expectedType = TileType.Floor;
            var expectedWalkable = true;
            var expectedTransparant = true;

            var tile = new Tile(10, 10, TileType.Floor);

            var actualType = tile.TileType;
            var actualWalkable = tile.IsWalkable;
            var actualTransparant = tile.IsTransparent;
                    
            Assert.AreEqual(expectedType, actualType, tileType + notSetCorrect);
            Assert.AreEqual(expectedWalkable, actualWalkable, isWalkable + notSetCorrect);
            Assert.AreEqual(expectedTransparant, actualTransparant, isTransparent + notSetCorrect);
        }

        [TestMethod]
        public void Create_Tile_Wall_CorrectTileTypeAndFlags()
        {
            var expectedType = TileType.Wall;
            var expectedWalkable = false;
            var expectedTransparant = false;

            var tile = new Tile(10, 10, TileType.Wall);

            var actualType = tile.TileType;
            var actualWalkable = tile.IsWalkable;
            var actualTransparant = tile.IsTransparent;

            Assert.AreEqual(expectedType, actualType, tileType + notSetCorrect);
            Assert.AreEqual(expectedWalkable, actualWalkable, isWalkable + notSetCorrect);
            Assert.AreEqual(expectedTransparant, actualTransparant, isTransparent + notSetCorrect);
        }

        [TestMethod]
        public void Create_Tile_Door_CorrectTileTypeAndFlags()
        {
            var expectedType = TileType.Door;
            var expectedWalkable = false;
            var expectedTransparant = false;

            var tile = new Tile(10, 10, TileType.Door);

            var actualType = tile.TileType;
            var actualWalkable = tile.IsWalkable;
            var actualTransparant = tile.IsTransparent;

            Assert.AreEqual(expectedType, actualType, tileType + notSetCorrect);
            Assert.AreEqual(expectedWalkable, actualWalkable, isWalkable + notSetCorrect);
            Assert.AreEqual(expectedTransparant, actualTransparant, isTransparent + notSetCorrect);
        }

        [TestMethod]
        public void Create_Tile_StairsUp_CorrectTileTypeAndFlags()
        {
            var expectedType = TileType.StairsUp;
            var expectedWalkable = true;
            var expectedTransparant = true;

            var tile = new Tile(10, 10, TileType.StairsUp);

            var actualType = tile.TileType;
            var actualWalkable = tile.IsWalkable;
            var actualTransparant = tile.IsTransparent;

            Assert.AreEqual(expectedType, actualType, tileType + notSetCorrect);
            Assert.AreEqual(expectedWalkable, actualWalkable, isWalkable + notSetCorrect);
            Assert.AreEqual(expectedTransparant, actualTransparant, isTransparent + notSetCorrect);
        }

        [TestMethod]
        public void Create_Tile_StairsDown_CorrectTileTypeAndFlags()
        {
            var expectedType = TileType.StairsDown;
            var expectedWalkable = true;
            var expectedTransparant = true;

            var tile = new Tile(10, 10, TileType.StairsDown);

            var actualType = tile.TileType;
            var actualWalkable = tile.IsWalkable;
            var actualTransparant = tile.IsTransparent;

            Assert.AreEqual(expectedType, actualType, tileType + notSetCorrect);
            Assert.AreEqual(expectedWalkable, actualWalkable, isWalkable + notSetCorrect);
            Assert.AreEqual(expectedTransparant, actualTransparant, isTransparent + notSetCorrect);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Create_Tile_TypeNone_ShouldThrowException()
        {
            var tile = new Tile(10, 10, TileType.None);
        }
        #endregion


        #region AdjacentTo Tests
        [TestMethod]
        public void AdjacentTo_Test_AdjacentTile_NorthShouldPass()
        {
            var tile = new Tile(10, 10, TileType.Floor);
            var tileToNorth = new Tile(10,9, TileType.Floor);
            var expectedDirection = Compass.North;
            var expectedBool = true;

            var direction = Compass.None;
            var isAdjacent = tile.AdjacentTo(tileToNorth, out direction);

            Assert.AreEqual(expectedDirection, direction, "Direction output not correct.");
            Assert.AreEqual(expectedBool, isAdjacent, "Did not return true.");
        }

        [TestMethod]
        public void AdjacentTo_Test_AdjacentTile_SouthShouldPass()
        {
            var tile = new Tile(10, 10, TileType.Floor);
            var tileToSouth = new Tile(10, 11, TileType.Floor);
            var expectedDirection = Compass.South;
            var expectedBool = true;

            var direction = Compass.None;
            var isAdjacent = tile.AdjacentTo(tileToSouth, out direction);

            Assert.AreEqual(expectedDirection, direction, "Direction output not correct.");
            Assert.AreEqual(expectedBool, isAdjacent, "Did not return true.");
        }

        [TestMethod]
        public void AdjacentTo_Test_AdjacentTile_EastShouldPass()
        {
            var tile = new Tile(10, 10, TileType.Floor);
            var tileToEast = new Tile(11, 10, TileType.Floor);
            var expectedDirection = Compass.East;
            var expectedBool = true;

            var direction = Compass.None;
            var isAdjacent = tile.AdjacentTo(tileToEast, out direction);

            Assert.AreEqual(expectedDirection, direction, "Direction output not correct.");
            Assert.AreEqual(expectedBool, isAdjacent, "Did not return true.");
        }

        [TestMethod]
        public void AdjacentTo_Test_AdjacentTile_WestShouldPass()
        {
            var tile = new Tile(10, 10, TileType.Floor);
            var tileToWest = new Tile(9, 10, TileType.Floor);
            var expectedDirection = Compass.West;
            var expectedBool = true;

            var direction = Compass.None;
            var isAdjacent = tile.AdjacentTo(tileToWest, out direction);

            Assert.AreEqual(expectedDirection, direction, "Direction output not correct.");
            Assert.AreEqual(expectedBool, isAdjacent, "Did not return true.");
        }

        [TestMethod]
        public void AdjacentTo_Test_NotAdjacentTile_ShouldFail()
        {
            var tile = new Tile(10, 10, TileType.Floor);
            var tileNotAdj = new Tile(11, 11, TileType.Floor);
            var expectedDirection = Compass.None;
            var expectedBool = false;

            var direction = Compass.None;
            var isAdjacent = tile.AdjacentTo(tileNotAdj, out direction);

            Assert.AreEqual(expectedDirection, direction, "Direction output not correct.");
            Assert.AreEqual(expectedBool, isAdjacent, "Did not return false.");
        }
        #endregion


        #region IContainer Methods

        [TestMethod]
        public void AddItem_Test()
        {
            Tuple<object, ItemEventArgs> actualEvent = null;
            tile.OnItemAddedToTile += (object sender, ItemEventArgs e) =>
                actualEvent = new Tuple<object, ItemEventArgs>(sender, e);

            var expectedIsAdded = true;
            var expectedSender = tile as object;
            var expectedTilePoint = tile.TilePoint;
            var expectedItemType = item.ItemType; 

            var isAdded = tile.AddItem(item);

            Assert.AreEqual(expectedIsAdded, isAdded, "Added the item, but returned false.");
            Assert.AreEqual(expectedSender, actualEvent.Item1, "Sender object was not correct.");
            Assert.AreEqual(expectedTilePoint, actualEvent.Item2.Location, "TilePoint was incorrect.");
            Assert.AreEqual(expectedItemType, actualEvent.Item2.ItemType, "ItemType was incorrect.");
        }

        [TestMethod]
        public void RemoveItem_Test_Successful()
        {
            tile.AddItem(item);

            Tuple<object, ItemEventArgs> actualEvent = null;
            tile.OnItemRemovedFromTile += (object sender, ItemEventArgs e) =>
                actualEvent = new Tuple<object, ItemEventArgs>(sender, e);

            var expectedSender = tile as object;
            var expectedTilePoint = tile.TilePoint;
            var expectedItemType = item.ItemType;

            tile.RemoveItem(item);

            Assert.AreEqual(expectedSender, actualEvent.Item1, "Sender object was not correct.");
            Assert.AreEqual(expectedTilePoint, actualEvent.Item2.Location, "TilePoint was incorrect.");
            Assert.AreEqual(expectedItemType, actualEvent.Item2.ItemType, "ItemType was incorrect.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RemoveItem_Test_Unsuccessful()
        {
            Tuple<object, ItemEventArgs> actualEvent = null;
            tile.OnItemRemovedFromTile += (object sender, ItemEventArgs e) =>
                actualEvent = new Tuple<object, ItemEventArgs>(sender, e);

            var expectedSender = tile as object;
            Tuple<object, ItemEventArgs> expectedEvent = null;

            tile.RemoveItem(item);

            Assert.AreEqual(expectedSender, actualEvent.Item1, "Sender object was not correct.");
            Assert.AreEqual(expectedEvent, actualEvent, "There should not have been an event.");
        }

        [TestMethod]
        public void InspectItems_Test()
        {
            tile.AddItem(item);

            var expected = item;
            var actual = tile.InspectItems()[0];

            Assert.AreEqual(expected, actual, "The Item[] returned was not expected.");
        }
        #endregion
    }
}
