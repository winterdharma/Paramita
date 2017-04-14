using NUnit.Framework;
using Paramita.GameLogic.Actors;
using Paramita.GameLogic.Mechanics;
using System.Collections.Generic;
using System.Linq;
using static Paramita.GameLogic.UnitTests.Mechanics.DiceTests;

namespace Paramita.GameLogic.UnitTests.Mechanics
{
    public class AttackRollTests
    {
        #region Result Tests
        [Test]
        public void AttackRoll_GivenWinningRoll_CalculatesResultCorrectly()
        {
            var expected = 10;
            var attackRoll = MakeAttackRollThatHit();
            var actual = attackRoll.Result;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AttackRoll_GivenLosingRoll_CalculatesResultCorrectly()
        {
            var expected = -8;
            var attackRoll = MakeAttackRollThatMissed();
            var actual = attackRoll.Result;

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region Report Tests
        [Test]
        public void AttackRoll_GivenRoll_ReportsAttackScore()
        {
            var expected = "attack score was 25";
            var attackRoll = MakeAttackRollThatHit();
            var actual = attackRoll.AttackRollReport.First();

            StringAssert.Contains(expected, actual);
        }

        [Test]
        public void AttackRoll_GivenRoll_ReportsAttackSkill()
        {
            var expected = "Attack skill: 10";
            var attackRoll = MakeAttackRollThatHit();
            var actual = attackRoll.AttackRollReport.First();

            StringAssert.Contains(expected, actual);
        }

        [Test]
        public void AttackRoll_GivenRoll_ReportsAttackWeaponModifier()
        {
            var expected = "Weapon modifier: -1";
            var attackRoll = MakeAttackRollThatHit();
            var actual = attackRoll.AttackRollReport.First();

            StringAssert.Contains(expected, actual);
        }

        [Test]
        public void AttackRoll_GivenRoll_ReportsAttackersFatiguePenalty()
        {
            var expected = "Fatigue penalty: 0";
            var attackRoll = MakeAttackRollThatHit();
            var actual = attackRoll.AttackRollReport.First();

            StringAssert.Contains(expected, actual);
        }

        [Test]
        public void AttackRoll_GivenRoll_ReportsDefenseScore()
        {
            var expected = "defense score was 15";
            var attackRoll = MakeAttackRollThatHit();
            var actual = attackRoll.AttackRollReport[1];

            StringAssert.Contains(expected, actual);
        }

        [Test]
        public void AttackRoll_GivenRoll_ReportsDefendersTotalDefenseSkill()
        {
            var expected = "Total defense: 9";
            var attackRoll = MakeAttackRollThatHit();
            var actual = attackRoll.AttackRollReport[1];

            StringAssert.Contains(expected, actual);
        }

        [Test]
        public void AttackRoll_GivenRoll_ReportsDefendersFatiguePenalty()
        {
            var expected = "Fatigue penalty: 0";
            var attackRoll = MakeAttackRollThatHit();
            var actual = attackRoll.AttackRollReport[1];

            StringAssert.Contains(expected, actual);
        }

        [Test]
        public void AttackRoll_GivenRoll_ReportDefendersTimesAttackedPenalty()
        {
            var expected = "Times attacked penalty: 0";
            var attackRoll = MakeAttackRollThatHit();
            var actual = attackRoll.AttackRollReport[1];

            StringAssert.Contains(expected, actual);
        }
        #endregion

        #region Helper Methods
        private AttackRoll MakeAttackRollThatHit()
        {
            var fakeRNG = new FakeRandomNum(new List<int>() { 6, 5, 5, 3, 3 });

            var player = ActorCreator.CreateHumanPlayer();
            var weapon = player.Combatant.GetLongestWeapon();
            var rat = ActorCreator.CreateGiantRat();

            return new AttackRoll(player.Combatant, weapon, rat.Combatant, fakeRNG);
        }
        
        private AttackRoll MakeAttackRollThatMissed()
        {
            var fakeRNG = new FakeRandomNum(new List<int>() { 1, 1, 5, 5 });

            var player = ActorCreator.CreateHumanPlayer();
            var weapon = player.Combatant.GetLongestWeapon();
            var rat = ActorCreator.CreateGiantRat();

            return new AttackRoll(player.Combatant, weapon, rat.Combatant, fakeRNG);
        }
        #endregion

    }
}
