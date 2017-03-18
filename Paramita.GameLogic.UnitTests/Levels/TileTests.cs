using Microsoft.Xna.Framework;
using NUnit.Framework;
using NUnit.Framework.Constraints;
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
    public class TileTests
    {
        #region TileType.Set() Tests (via Constructor)
        [TestCase(TileType.Floor, true)]
        [TestCase(TileType.StairsUp, true)]
        [TestCase(TileType.StairsDown, true)]
        [TestCase(TileType.Wall, false)]
        [TestCase(TileType.Door, false)]
        public void SetTileType_ValidTileTypes_SetsWalkableFlagCorrectly(TileType tileType, bool expected)
        {
            // constructor calls TileType.Set()
            var tile = MakeTile(tileType);

            Assert.AreEqual(expected, tile.IsWalkable);
        }

        [TestCase(TileType.Floor, true)]
        [TestCase(TileType.StairsUp, true)]
        [TestCase(TileType.StairsDown, true)]
        [TestCase(TileType.Door, false)]
        [TestCase(TileType.Wall, false)]
        public void SetTileType_ValidTileTypes_SetsTransparentFlagCorrectly(
            TileType tileType, bool expected)
        {
            var tile = MakeTile(tileType);

            Assert.AreEqual(expected, tile.IsTransparent);
        }

        [TestCase(TileType.None)]
        public void SetTileType_InvalidTileTypes_ThrowNotImplementedException(
            TileType tileType)
        {
            var exception = Assert.Catch<NotImplementedException>(() => MakeTile(tileType));
            StringAssert.Contains("Tried to set flags for unimplemented " + tileType + ".", 
                exception.Message);
        }
        #endregion


        #region AdjacentTo() Tests

        [TestCase(Compass.North, true, Compass.North)]
        [TestCase(Compass.South, true, Compass.South)]
        [TestCase(Compass.East, true, Compass.East)]
        [TestCase(Compass.West, true, Compass.West)]
        [TestCase(Compass.Northeast, false, Compass.None)]
        [TestCase(Compass.Northwest, false, Compass.None)]
        [TestCase(Compass.Southeast, false, Compass.None)]
        [TestCase(Compass.Southwest, false, Compass.None)]
        public void AdjacentTo_AdjacentTiles_OnlyNorthSouthEastWestTrueAndDirection(
            Compass direction, bool expectedBool, Compass expectedDirection)
        {
            var refpoint = new Point(10, 10);
            var testpoint = refpoint + Direction.GetPoint(direction);
            var refTile = MakeTile(refpoint.X, refpoint.Y);
            var testTile = MakeTile(testpoint.X, testpoint.Y);

            Compass actualDirection;
            var actualBool = refTile.AdjacentTo(testTile, out actualDirection);

            Assert.AreEqual(expectedDirection, actualDirection);
            Assert.AreEqual(expectedBool, actualBool);
        }

        [TestCase(11, 23, false, Compass.None)]
        [TestCase(31, 9, false, Compass.None)]
        [TestCase(0, 0, false, Compass.None)]
        public void AdjacentTo_NonadjacentTile_ReturnFalseAndNoDirection(
            int x, int y, bool expectedBool, Compass expectedDirection)
        {
            var refpoint = new Point(10, 10);
            var testpoint = new Point(x, y);
            var refTile = MakeTile(refpoint.X, refpoint.Y);
            var testTile = MakeTile(testpoint.X, testpoint.Y);

            Compass actualDirection;
            var actualBool = refTile.AdjacentTo(testTile, out actualDirection);

            Assert.AreEqual(expectedDirection, actualDirection);
            Assert.AreEqual(expectedBool, actualBool);
        }
        #endregion

        #region AddItem() Tests
        [Test]
        public void AddItem_AddingNullItem_ThrowsNullReferenceException()
        {
            var tile = MakeTile();
            Assert.Catch<NullReferenceException>(() => tile.AddItem(null));
        }

        [Test]
        public void AddItem_AddingItem_AddsItemRaisesEventAndReturnsTrue()
        {
            var item = ItemCreator.CreateMeat();
            var tile = MakeTile();

            var expectedItems = new List<Item>() { item };
            Tuple<Tile, ItemEventArgs> actualEvent = null;
            tile.OnItemAddedToTile += (object sender, ItemEventArgs e) =>
                actualEvent = new Tuple<Tile, ItemEventArgs>(sender as Tile, e);

            var actualBool = tile.AddItem(item);

            Assert.True(actualBool);
            Assert.AreEqual(expectedItems, tile.Items);
            Assert.AreEqual(tile.TilePoint, actualEvent.Item2.Location);
            Assert.AreEqual(ItemType.Meat, actualEvent.Item2.ItemType);
        }
        #endregion

        #region RemoveItem() Tests
        [Test]
        public void RemoveItem_RemovingNullItem_ThrowsNullReferenceException()
        {
            var tile = MakeTile();
            Assert.Catch<NullReferenceException>(() => tile.RemoveItem(null));
        }

        [Test]
        public void RemoveItem_RemovingItemPresent_RemovesItemAndRaisesItemRemovedEvent()
        {
            var item = ItemCreator.CreateMeat();
            var tile = MakeTile();
            tile.AddItem(item);

            var expectedItems = new List<Item>();
            Tuple<Tile, ItemEventArgs> actualEvent = null;
            tile.OnItemRemovedFromTile += (object sender, ItemEventArgs e) =>
                actualEvent = new Tuple<Tile, ItemEventArgs>(sender as Tile, e);

            tile.RemoveItem(item);

            Assert.AreEqual(expectedItems, tile.Items);
            Assert.AreEqual(tile.TilePoint, actualEvent.Item2.Location);
            Assert.AreEqual(ItemType.Meat, actualEvent.Item2.ItemType);
        }

        [Test]
        public void RemoveItem_RemovingItemNotPresent_ThrowsArgumentException()
        {
            var item = ItemCreator.CreateMeat();
            var tile = MakeTile();

            Assert.Catch<ArgumentException>(() => tile.RemoveItem(item));
        }
        #endregion

        #region InspectItems() Test
        [Test]
        public void InspectItems_ItemsNotEmpty_ReturnsArrayOfItems()
        {
            var itemA = ItemCreator.CreateBuckler();
            var itemB = ItemCreator.CreateMeat();
            var tile = MakeTile();

            tile.AddItem(itemA);
            tile.AddItem(itemB);

            var expectedArray = new Item[] { itemA, itemB };
            Item[] actualArray = tile.InspectItems();

            CollectionAssert.AreEqual(expectedArray, actualArray);
        }

        #endregion

        #region IEquatable Methods
        [Test]
        public void Equals_ComparedToSelf_ReturnsTrue()
        {
            var tile = MakeTile();
            var otherTile = tile;

            var actual = tile.Equals(otherTile);

            Assert.True(actual);
        }

        [Test]
        public void Equals_ComparedToNull_ThrowsException()
        {
            var tile = MakeTile();
            Tile otherTile = null;
            Assert.Catch<NullReferenceException>( () => tile.Equals(otherTile));
        }

        [Test]
        public void Equals_ComparedToDifferentType_ReturnsFalse()
        {
            var tile = MakeTile();
            var otherTilePoint = new Point(0, 1);

            var actual = tile.Equals(otherTilePoint);

            Assert.False(actual);
        }

        [TestCase(0,0,TileType.Floor, 0,0, TileType.Floor, true)]
        [TestCase(0, 0, TileType.Floor, 0, 0, TileType.Wall, false)]
        [TestCase(0, 1, TileType.Floor, 0, 0, TileType.Floor, false)]
        public void Equals_ComparedProperties_ReturnsExpected(
            int tileAx, int tileAy, TileType tileAType, int tileBx, int tileBy, 
            TileType tileBType, bool expected)
        {
            var tile = MakeTile(tileAx, tileAy, tileAType);
            var otherTile = MakeTile(tileBx, tileBy, tileBType);
            var actual = tile.Equals(otherTile);

            Assert.AreEqual(expected, actual);
        }


        [TestCase(0, 0, TileType.Floor, 0, 0, TileType.Floor, true)]
        [TestCase(1, 1, TileType.Wall, 0, 0, TileType.Floor, false)]
        public void EqualOperator_ComparedTiles_ReturnsExpected(
            int tileAx, int tileAy, TileType tileAType, int tileBx, int tileBy, 
            TileType tileBType, bool expected)
        {
            var tile = MakeTile(tileAx, tileAy, tileAType);
            var otherTile = MakeTile(tileBx, tileBy, tileBType);

            var actual = tile == otherTile;

            Assert.AreEqual(expected, actual);
        }


        [TestCase(0, 0, TileType.Floor, 0, 0, TileType.Floor, false)]
        [TestCase(1, 1, TileType.Wall, 0, 0, TileType.Floor, true)]
        public void NotEqualOperator_ComparedTiles_ReturnsExpected(
            int tileAx, int tileAy, TileType tileAType, int tileBx, int tileBy,
            TileType tileBType, bool expected)
        {
            var tile = MakeTile(tileAx, tileAy, tileAType);
            var otherTile = MakeTile(tileBx, tileBy, tileBType);

            var actual = tile != otherTile;

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region IComparable
        [Test]
        public void CompareTo_ListOfTiles_SortsByYThenX()
        {
            var tileList = new List<Tile>()
            {
                MakeTile(3,1), MakeTile(3,2), MakeTile(3,0),
                MakeTile(2,0), MakeTile(2,2), MakeTile(2,1),
                MakeTile(1,2), MakeTile(1,1), MakeTile(1,0)
            };

            var expectedSort = new List<Tile>()
            {
                MakeTile(1,0), MakeTile(2,0), MakeTile(3,0),
                MakeTile(1,1), MakeTile(2,1), MakeTile(3,1),
                MakeTile(1,2), MakeTile(2,2), MakeTile(3,2)
            };

            tileList.Sort();

            CollectionAssert.AreEqual(expectedSort, tileList);
        }

        #endregion

        #region Helper Methods
        #region Tile Factory
        // for cases in which only a generic tile is needed
        private Tile MakeTile()
        {
            return new Tile(0, 0, TileType.Wall);
        }
        // for cases in which TilePoint is not relevant
        private Tile MakeTile(TileType tileType)
        {
            return new Tile(0, 0, tileType);
        }

        // for cases in which TileType is not relevant
        private Tile MakeTile(int x, int y)
        {
            return new Tile(x, y, TileType.Wall);
        }

        // for cases in which both TilePoint and TileType are relevant
        private Tile MakeTile(int x, int y, TileType tileType)
        {
            return new Tile(x, y, tileType);
        }
        #endregion
        #endregion
    }
}
