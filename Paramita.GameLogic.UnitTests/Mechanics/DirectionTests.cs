using Microsoft.Xna.Framework;
using NUnit.Framework;
using Paramita.GameLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita.GameLogic.UnitTests.Mechanics
{
    [TestFixture]
    public class DirectionTests
    {
        [TestCase(Compass.North, 0, -1)]
        [TestCase(Compass.Northeast, 1, -1)]
        [TestCase(Compass.East, 1, 0)]
        [TestCase(Compass.Southeast, 1, 1)]
        [TestCase(Compass.South, 0, 1)]
        [TestCase(Compass.Southwest, -1, 1)]
        [TestCase(Compass.West, -1, 0)]
        [TestCase(Compass.Northwest, -1, -1)]
        [TestCase(Compass.None, 0, 0)]
        public void GetPoint_GivenCompassValue_ReturnsCorrectPoint(Compass compass, int x, int y)
        {
            var expected = new Point(x, y);
            var actual = Direction.GetPoint(compass);

            Assert.AreEqual(expected, actual);
        }

        [TestCase(Compass.North, 0, -1)]
        [TestCase(Compass.Northeast, 1, -1)]
        [TestCase(Compass.East, 1, 0)]
        [TestCase(Compass.Southeast, 1, 1)]
        [TestCase(Compass.South, 0, 1)]
        [TestCase(Compass.Southwest, -1, 1)]
        [TestCase(Compass.West, -1, 0)]
        [TestCase(Compass.Northwest, -1, -1)]
        [TestCase(Compass.None, 0, 0)]
        [TestCase(Compass.None, 5, 3)]
        public void GetDirection_GivenPointValue_ReturnsCorrectDirection(Compass expected, int x, int y)
        {
            var point = new Point(x, y);
            var actual = Direction.GetDirection(point);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void EightCompassPoints_ReturnsListOfPointsForAllEightDirections()
        {
            var expected = new List<Point>()
            {
                new Point(0, -1),
                new Point(1, -1),
                new Point(1, 0),
                new Point(1, 1),
                new Point(0, 1),
                new Point(-1, 1),
                new Point(-1, 0),
                new Point(-1, -1)
            };

            var actual = Direction.EightCompassPoints;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CardinalPoints_ReturnsListOfPointsForNorthSouthEastWest()
        {
            var expected = new List<Point>()
            {
                new Point(0, -1),
                new Point(1, 0),
                new Point(0, 1),
                new Point(-1, 0)
            };

            var actual = Direction.CardinalPoints;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void North_ReturnsPointForNorth()
        {
            var expected = new Point(0, -1);
            var actual = Direction.North;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Northeast_ReturnsPointForNortheast()
        {
            var expected = new Point(1, -1);
            var actual = Direction.Northeast;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void East_ReturnsPointForEast()
        {
            var expected = new Point(1, 0);
            var actual = Direction.East;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Southeast_ReturnsPointForSoutheast()
        {
            var expected = new Point(1, 1);
            var actual = Direction.Southeast;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void South_ReturnsPointForSouth()
        {
            var expected = new Point(0, 1);
            var actual = Direction.South;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Southwest_ReturnsPointForSouthwest()
        {
            var expected = new Point(-1, 1);
            var actual = Direction.Southwest;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void West_ReturnsPointForWest()
        {
            var expected = new Point(-1, 0);
            var actual = Direction.West;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Northwest_ReturnsPointForNorthwest()
        {
            var expected = new Point(-1, -1);
            var actual = Direction.Northwest;

            Assert.AreEqual(expected, actual);
        }
    }
}
