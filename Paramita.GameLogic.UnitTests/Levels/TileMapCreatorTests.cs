using NUnit.Framework;
using Paramita.GameLogic.Data.Levels;
using Paramita.GameLogic.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita.GameLogic.UnitTests.Levels
{
    [TestFixture]
    public class TileMapCreatorTests
    {
        [Test]
        public void CreateMap_GivenValidData_ReturnsArrayOfCorrectSize()
        {
            int expectedX = 10; int expectedY = 15;
            var data = new TileMapData(expectedX, expectedY, 5, 8, 2);
            var mapCreator = new TileMapCreator();

            
            var map = mapCreator.CreateMap(data);
            var actualX = map.GetLength(0);
            var actualY = map.GetLength(1);

            Assert.AreEqual(expectedX, actualX);
            Assert.AreEqual(expectedY, actualY);
        }

        [TestCase(TileType.StairsUp, 1)]
        [TestCase(TileType.StairsDown, 1)]
        public void CreateMap_GivenValidData_ReturnsArrayWithTiles(TileType type, int expected)
        {
            var data = new TileMapData(20, 20, 5, 8, 2);
            var mapCreator = new TileMapCreator();

            var map = mapCreator.CreateMap(data);

            int actual = 0;
            foreach(var tile in map)
            {
                if (tile.TileType == type)
                    actual++;
            }

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CreateMap_GivenValidData_ReturnsArrayWithoutNulls()
        {
            var data = new TileMapData(20, 20, 5, 8, 2);
            var mapCreator = new TileMapCreator();

            var map = mapCreator.CreateMap(data);

            CollectionAssert.AllItemsAreNotNull(map);
        }

        [Test]
        public void CreateMap_InvalidDataMaxRoomsLessThanOne_ThrowsException()
        {
            var data = new TileMapData(20, 20, 0, 8, 2);
            var mapCreator = new TileMapCreator();

            Assert.Catch<ArgumentOutOfRangeException>( () => mapCreator.CreateMap(data));
        }

        [Test]
        public void CreateMap_InvalidDataMinRoomSizeLargerThanMaxSize_ThrowsException()
        {
            var data = new TileMapData(20, 20, 5, 2, 8);
            var mapCreator = new TileMapCreator();

            Assert.Catch<ArgumentOutOfRangeException>(() => mapCreator.CreateMap(data));
        }

        [Test]
        public void CreateMap_InvalidDataMapSizeDimensionsLessThanTen_ThrowsException()
        {
            var data = new TileMapData(9, 9, 5, 8, 2);
            var mapCreator = new TileMapCreator();

            Assert.Catch<ArgumentOutOfRangeException>(() => mapCreator.CreateMap(data));
        }
    }
}
