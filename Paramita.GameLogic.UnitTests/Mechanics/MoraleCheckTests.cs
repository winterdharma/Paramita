using NUnit.Framework;
using Paramita.GameLogic.Mechanics;
using System;
using System.Collections.Generic;
using static Paramita.GameLogic.UnitTests.Mechanics.DiceTests;
using Paramita.GameLogic.Actors;

namespace Paramita.GameLogic.UnitTests.Mechanics
{
    [TestFixture]
    public class MoraleCheckTests
    {
        [Test]
        public void MoraleCheck_WhenMoraleCheckTypeNotImplemented_ThrowsException()
        {
            var player = new Player("Test player");
            var rat = ActorCreator.CreateGiantRat();
            Assert.Catch<NotImplementedException>(() => 
                new MoraleCheck(player.Combatant, MoraleCheckType.Test, rat.Combatant));
        }

        [Test]
        public void MoraleCheck_RepelAttackCheckTotalBeatsTarget_IsSuccessfulSetToTrue()
        {
            var fakeRNG = new FakeRandomNum(new List<int>() { 6, 5, 5, 5, 5 });

            var player = ActorCreator.CreateHumanPlayer();
            var rat = ActorCreator.CreateGiantRat();

            var check = new MoraleCheck(player.Combatant, MoraleCheckType.RepelAttack, rat.Combatant, 0, fakeRNG);
            Assert.True(check.IsSuccessful);
        }

        [Test]
        public void CheckMorale_RepelAttackCheckTotalLessThanTarget_IsSuccessfulSetToFalse()
        {
            var fakeRNG = new FakeRandomNum(new List<int>() { 1, 1, 5, 5 });

            var player = ActorCreator.CreateHumanPlayer();
            var rat = ActorCreator.CreateGiantRat();

            var check = new MoraleCheck(player.Combatant, MoraleCheckType.RepelAttack, rat.Combatant, 0, fakeRNG);
            Assert.False(check.IsSuccessful);
        }

        [Test]
        public void CheckMorale_GivenValidInputs_CreatesLogOfCheckVariables()
        {
            var fakeRNG = new FakeRandomNum(new List<int>() { 6, 5, 3, 5, 2 });

            var player = ActorCreator.CreateHumanPlayer();
            var rat = ActorCreator.CreateGiantRat();

            var check = new MoraleCheck(player.Combatant, MoraleCheckType.RepelAttack, rat.Combatant, 0, fakeRNG);

            var expected = new List<string>()
            {
                "Checker's total was 25 (Morale: 10, SizeDiff: 1)",
                "Target total was 17 (Target: 10, Modifier: 0)",
                "Checker passed the morale check!"
            };

            var actual = check.MoraleCheckReport;

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
