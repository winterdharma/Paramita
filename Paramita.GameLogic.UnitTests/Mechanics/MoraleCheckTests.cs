using NUnit.Framework;
using Paramita.GameLogic.Mechanics;
using System;
using System.Collections.Generic;
using static Paramita.GameLogic.UnitTests.Mechanics.DiceTests;
using Paramita.GameLogic.Actors;
using System.Linq;

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
        public void CheckMorale_RepelAttackCheckTotalBeatsTarget_IsSuccessfulSetToTrue()
        {
            var check = MakePassingCheck();
            Assert.True(check.IsSuccessful);
        }

        [Test]
        public void MoraleCheck_RepelAttackCheckTotalLessThanTarget_IsSuccessfulSetToFalse()
        {
            var check = MakeFailingCheck();
            Assert.False(check.IsSuccessful);
        }


        #region Reporting Tests
        [Test]
        public void MoraleCheck_WhenCheckPassed_ReportsTheCheckPassed()
        {
            var expected = "passed";
            var check = MakePassingCheck();
            var actual = check.MoraleCheckReport.Last();
            StringAssert.Contains(expected, actual);
        }

        [Test]
        public void MoraleCheck_WhenCheckFailed_ReportsTheCheckFailed()
        {
            var expected = "failed";
            var check = MakeFailingCheck();
            var actual = check.MoraleCheckReport.Last();
            StringAssert.Contains(expected, actual);
        }

        [Test]
        public void MoraleCheck_WhenChecked_ReportsCheckersTotalScore()
        {
            var check = MakePassingCheck();
            var expected = "Checker total: 27";
            var actual = check.MoraleCheckReport.First();

            StringAssert.Contains(expected, actual);
        }

        [Test]
        public void MoraleCheck_WhenChecked_ReportsCheckersMorale()
        {
            var check = MakePassingCheck();
            var expected = "Morale: 10";
            var actual = check.MoraleCheckReport.First();

            StringAssert.Contains(expected, actual);
        }

        [Test]
        public void MoraleCheck_WhenChecked_ReportsCheckersSizeDifference()
        {
            var check = MakePassingCheck();
            var expected = "Size difference: 1";
            var actual = check.MoraleCheckReport.First();

            StringAssert.Contains(expected, actual);
        }

        [Test]
        public void MoraleCheck_WhenChecked_ReportsTotalTargetScore()
        {
            var check = MakePassingCheck();
            var expected = "Target total: 20";
            var actual = check.MoraleCheckReport[1];

            StringAssert.Contains(expected, actual);
        }

        [Test]
        public void MoraleCheck_WhenChecked_ReportsTargetValue()
        {
            var check = MakePassingCheck();
            var expected = "Target: 10";
            var actual = check.MoraleCheckReport[1];

            StringAssert.Contains(expected, actual);
        }

        [Test]
        public void MoraleCheck_WhenChecked_ReportsModifierValue()
        {
            var check = MakePassingCheck();
            var expected = "Modifier: 0";
            var actual = check.MoraleCheckReport[1];

            StringAssert.Contains(expected, actual);
        }
        #endregion


        #region Helper Methods
        private MoraleCheck MakePassingCheck()
        {
            var fakeRNG = new FakeRandomNum(new List<int>() { 6, 5, 5, 5, 5 });

            var player = ActorCreator.CreateHumanPlayer();
            var rat = ActorCreator.CreateGiantRat();

            return new MoraleCheck(player.Combatant, MoraleCheckType.RepelAttack, rat.Combatant, 0, fakeRNG);
        }

        private MoraleCheck MakeFailingCheck()
        {
            var fakeRNG = new FakeRandomNum(new List<int>() { 4, 5, 5, 5 });

            var player = ActorCreator.CreateHumanPlayer();
            var rat = ActorCreator.CreateGiantRat();

            return new MoraleCheck(player.Combatant, MoraleCheckType.RepelAttack, rat.Combatant, 0, fakeRNG);
        }
        #endregion
    }
}
