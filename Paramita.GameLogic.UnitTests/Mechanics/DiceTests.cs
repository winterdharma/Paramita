using NUnit.Framework;
using Paramita.GameLogic.Mechanics;
using System.Collections.Generic;
using System.Linq;

namespace Paramita.GameLogic.UnitTests.Mechanics
{
    [TestFixture]
    public class DiceTests
    {
        #region OpenEndedRoll Tests
        [Test]
        public void OpenEndedRoll_WhenMaxRoll_AddsBonusDice()
        {
            var testData = new List<int>() { 1, 6, 2 };
            var fakeRandom = new FakeRandomNum(testData);
            var expected = testData[0] + testData[1] + testData[2];

            var dice2d6 = new Dice(2);
            dice2d6.Random = fakeRandom;
            var actual = dice2d6.OpenEndedRoll();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void OpenEndedRoll_WhenNoMaxRolls_DoesNotAddBonusDice()
        {
            var testData = new List<int>() { 1, 5, 2 };
            var fakeRandom = new FakeRandomNum(testData);
            var expected = testData[0] + testData[1];

            var dice2d6 = new Dice(2);
            dice2d6.Random = fakeRandom;
            var actual = dice2d6.OpenEndedRoll();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void OpenEndedRoll_GivenRollValues_ReturnsCorrectTotal()
        {
            var testData = new List<int>() { 6, 6, 6, 4, 3, 2, 4, 2};
            var fakeRandom = new FakeRandomNum(testData);
            var expected = testData[0] + testData[1] + testData[2] + testData[3] + testData[4]
                + testData[5] + testData[6] + testData[7];

            var dice2d6 = new Dice(5);
            dice2d6.Random = fakeRandom;
            var actual = dice2d6.OpenEndedRoll();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void OpenEndedRoll_GivenRollValues_RecordsRollLog()
        {
            var testData = new List<int>() { 6, 6, 6, 4, 3, 2, 4, 2 };
            var fakeRandom = new FakeRandomNum(testData);
            var expected = new List<string>()
            {
                "New Dice Roll",
                "Roll d6: 6",
                "Bonus Roll!",
                "Roll d6: 6",
                "Bonus Roll!",
                "Roll d6: 6",
                "Bonus Roll!",
                "Roll d6: 4",
                "Roll d6: 3",
                "Roll d6: 2",
                "Roll d6: 4",
                "Roll d6: 2",
                "Total: 33",
                " "
            };

            var dice5d6 = new Dice(5);
            dice5d6.Random = fakeRandom;
            dice5d6.OpenEndedRoll();
            var actual = dice5d6.Report;

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region ClosedEndedRoll Tests
        [Test]
        public void ClosedEndedRoll_GivenMaxValue_DoesNotAddBonusDice()
        {
            var testData = new List<int>() { 1, 6, 2 };
            var fakeRandom = new FakeRandomNum(testData);
            var expected = testData[0] + testData[1];

            var dice2d6 = new Dice(2);
            dice2d6.Random = fakeRandom;
            var actual = dice2d6.ClosedEndedRoll();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ClosedEndedRoll_GivenRollValues_ReturnsCorrectTotal()
        {
            var testData = new List<int>() { 6, 6, 6, 4, 3, 2, 4, 2 };
            var fakeRandom = new FakeRandomNum(testData);
            var expected = testData[0] + testData[1] + testData[2] + testData[3] + testData[4];

            var dice2d6 = new Dice(5);
            dice2d6.Random = fakeRandom;
            var actual = dice2d6.ClosedEndedRoll();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ClosedEndedRoll_GivenRollValues_RecordsRollLog()
        {
            var testData = new List<int>() { 6, 6, 6, 4, 3, 2, 4, 2 };
            var fakeRandom = new FakeRandomNum(testData);
            var expected = new List<string>()
            {
                "New Dice Roll",
                "Roll d6: 6",
                "Roll d6: 6",
                "Roll d6: 6",
                "Roll d6: 4",
                "Roll d6: 3",
                "Total: 25",
                " "
            };

            var dice5d6 = new Dice(5);
            dice5d6.Random = fakeRandom;
            dice5d6.ClosedEndedRoll();
            var actual = dice5d6.Report;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ClosedEndedRoll_GivenMultipleRolls_RecordsRollLogForAllRolls()
        {
            var testData = new List<int>() { 6, 6, 6, 4, 3, 2 };
            var fakeRandom = new FakeRandomNum(testData);
            var expected = new List<string>()
            {
                "New Dice Roll",
                "Roll d6: 6",
                "Roll d6: 6",
                "Total: 12",
                " ",
                "New Dice Roll",
                "Roll d6: 6",
                "Roll d6: 4",
                "Total: 10",
                " ",
                "New Dice Roll",
                "Roll d6: 3",
                "Roll d6: 2",
                "Total: 5",
                " "
            };

            var dice2d6 = new Dice(2);
            dice2d6.Random = fakeRandom;

            dice2d6.ClosedEndedRoll();
            dice2d6.ClosedEndedRoll();
            dice2d6.ClosedEndedRoll();

            var actual = dice2d6.Report;

            Assert.AreEqual(expected, actual);
        }
        #endregion

        internal class FakeRandomNum : IRandom
        {
            List<int> _numbers;

            public FakeRandomNum(params int[] fakedNumbers)
            {
                _numbers = fakedNumbers.ToList();
            }

            public FakeRandomNum(List<int> fakedNumbers)
            {
                _numbers = fakedNumbers;
            }

            public int Next(int maxValue)
            {
                return NextNumber();
            }

            public int Next(int minValue, int maxValue)
            {
                return NextNumber();
            }

            private int NextNumber()
            {
                int number = _numbers[0];
                _numbers.RemoveAt(0);
                return number;
            }
        }
    }
}
