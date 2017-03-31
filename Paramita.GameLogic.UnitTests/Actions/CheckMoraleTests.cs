using NUnit.Framework;
using Paramita.GameLogic.Mechanics;
using Paramita.GameLogic.Actions;
using System;
using System.Collections.Generic;
using static Paramita.GameLogic.UnitTests.Mechanics.DiceTests;
using Paramita.GameLogic.Actors;

namespace Paramita.GameLogic.UnitTests.Actions
{
    [TestFixture]
    public class CheckMoraleTests
    {
        [Test]
        public void CheckMorale_WhenMoraleCheckTypeNotImplemented_ThrowsException()
        {
            var player = new Player("Test player");
            var rat = ActorCreator.CreateGiantRat();
            Assert.Catch<NotImplementedException>(() => 
                new CheckMorale(player, MoraleCheckType.Test, rat));
        }

        [Test]
        public void CheckMorale_RepelAttackCheckTotalBeatsTarget_IsSuccessfulSetToTrue()
        {
            var fakeDice = new Dice(2);
            fakeDice.Random = new FakeRandomNum(new List<int>() { 6, 5, 5, 5, 5 });

            var player = ActorCreator.CreateHumanPlayer();
            var rat = ActorCreator.CreateGiantRat();

            var check = new CheckMorale(player, MoraleCheckType.RepelAttack, rat, 0, fakeDice);
            Assert.True(check.IsSuccessful);
        }

        [Test]
        public void CheckMorale_RepelAttackCheckTotalLessThanTarget_IsSuccessfulSetToFalse()
        {
            var fakeDice = new Dice(2);
            fakeDice.Random = new FakeRandomNum(new List<int>() { 1, 1, 5, 5 });

            var player = ActorCreator.CreateHumanPlayer();
            var rat = ActorCreator.CreateGiantRat();

            var check = new CheckMorale(player, MoraleCheckType.RepelAttack, rat, 0, fakeDice);
            Assert.False(check.IsSuccessful);
        }

        [Test]
        public void CheckMorale_GivenValidInputs_CreatesLogOfCheckVariables()
        {
            var fakeDice = new Dice(2);
            fakeDice.Random = new FakeRandomNum(new List<int>() { 6,5,3,5,2 });

            var player = ActorCreator.CreateHumanPlayer();
            var rat = ActorCreator.CreateGiantRat();

            var check = new CheckMorale(player, MoraleCheckType.RepelAttack, rat, 0, fakeDice);

            var expected = new List<string>()
            {
                "Roll d6: 6",
                "Bonus Roll!",
                "Roll d6: 5",
                "Roll d6: 3",
                "Total: 14",
                "Checker's morale was 10 and size difference was 1",
                "Checker's total was 25",
                "Roll d6: 5",
                "Roll d6: 2",
                "Total: 7",
                "Target value was 10 and modifier was 0",
                "Target total was 17",
                "Checker passed the morale check!"
            };

            var actual = check.Report;

            CollectionAssert.AreEqual(expected, actual);
        }

        #region Helper Methods
        private Dice MakeDiceWithFakeRandom(List<int> data)
        {
            var fakeRandom = new FakeRandomNum(data);
            var dice = new Dice(2);
            dice.Random = fakeRandom;
            return dice;
        }
        #endregion
    }
}
