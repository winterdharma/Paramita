using NUnit.Framework;
using Paramita.GameLogic.Actors;
using Paramita.GameLogic.Items;
using Paramita.GameLogic.Mechanics;
using System.Collections.Generic;
using System.Linq;
using static Paramita.GameLogic.UnitTests.Mechanics.DiceTests;

namespace Paramita.GameLogic.UnitTests.Mechanics
{
    public class DamageRollTests
    {
        #region Damage Property Tests
        [Test]
        public void DamageRoll_GivenDamageRollWithDamage_DamageIsCalculatedCorrectly()
        {
            var expected = 17;
            var damageRoll = MakeDamageRollWithDamage();
            var actual = damageRoll.Damage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DamageRoll_GivenDamageRollWithNegativeDamage_DamageIsZero()
        {
            var expected = 0;
            var damageRoll = MakeShieldHitWithoutDamage();
            var actual = damageRoll.Damage;

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region Shield Hit Tests
        [Test]
        public void DamageRoll_WhenAttackResultEqualOrLessThanParry_ShieldProtectionAdded()
        {
            var expected = 0;
            var damageRoll = MakeShieldHitWithoutDamage();
            var actual = damageRoll.Damage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DamageRoll_WhenAttackResultGreaterThanParry_NoShieldProtectionAdded()
        {
            var expected = 13;
            var damageRoll = MakeShieldMiss();
            var actual = damageRoll.Damage;

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region Critical Hit Tests
        [Test]
        public void DamageRoll_WhenCriticalHitRollFails_ProtectionIsAppliedNormally()
        {
            var expected = 17;
            var damageRoll = MakeDamageRollWithDamage();
            var actual = damageRoll.Damage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DamageRoll_WhenCriticalHitRollPasses_ProtectionIsHalved()
        {
            var expected = 18; // rat's protection reduced to 2 / 1 = 1
            var damageRoll = MakeCriticalHit();
            var actual = damageRoll.Damage;

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region Report Tests
        [Test]
        public void DamageRoll_GivenValidRoll_ReportsTotalDamageScore()
        {
            var expected = "damage score was 22";
            var damageRoll = MakeDamageRollWithDamage();
            var actual = damageRoll.DamageRollReport.First();

            StringAssert.Contains(expected, actual);

        }

        [Test]
        public void DamageRoll_GivenValidRoll_ReportsAttackersStrength()
        {
            var expected = "Strength: 10";
            var damageRoll = MakeDamageRollWithDamage();
            var actual = damageRoll.DamageRollReport.First();

            StringAssert.Contains(expected, actual);

        }

        [Test]
        public void DamageRoll_GivenValidRoll_ReportsWeaponsDamage()
        {
            var expected = "Weapon damage: 2";
            var damageRoll = MakeDamageRollWithDamage();
            var actual = damageRoll.DamageRollReport.First();

            StringAssert.Contains(expected, actual);

        }

        [Test]
        public void DamageRoll_GivenValidRoll_ReportsTotalProtectionScore()
        {
            var expected = "protection score was 5";
            var damageRoll = MakeDamageRollWithDamage();
            var actual = damageRoll.DamageRollReport[1];

            StringAssert.Contains(expected, actual);

        }

        [Test]
        public void DamageRoll_GivenValidRoll_ReportsDefendersProtection()
        {
            var expected = "Protection: 2";
            var damageRoll = MakeDamageRollWithDamage();
            var actual = damageRoll.DamageRollReport[1];

            StringAssert.Contains(expected, actual);

        }

        [Test]
        public void DamageRoll_GivenShieldHitRoll_ReportsDefendersShieldWasHit()
        {
            var expected = "shield was hit";
            var damageRoll = MakeShieldHitWithoutDamage();
            var actual = damageRoll.DamageRollReport.First();

            StringAssert.Contains(expected, actual);

        }

        [Test]
        public void DamageRoll_GivenSuccessfulCriticalHitRoll_ReportsCriticalHit()
        {
            var expected = "critical hit";
            var damageRoll = MakeCriticalHit();
            var actual = damageRoll.DamageRollReport.First();

            StringAssert.Contains(expected, actual);

        }
        #endregion

        #region Helper Methods
        private DamageRoll MakeDamageRollWithDamage()
        {
            var fakeRNG = new FakeRandomNum(4, 5, 5, 5, 2, 1);

            var player = ActorCreator.CreateHumanPlayer();
            var rat = ActorCreator.CreateGiantRat();

            return new DamageRoll(5, 2, player.Combatant, rat.Combatant, fakeRNG);
        }

        private DamageRoll MakeShieldHitWithoutDamage()
        {
            var fakeRNG = new FakeRandomNum(4, 5, 5, 5, 1, 1);

            var player = ActorCreator.CreateHumanPlayer();
            player.Acquire(ItemCreator.CreateBuckler());
            var rat = ActorCreator.CreateGiantRat();

            return new DamageRoll(2, 2, rat.Combatant, player.Combatant, fakeRNG);
        }

        private DamageRoll MakeShieldMiss()
        {
            var fakeRNG = new FakeRandomNum(4, 5, 5, 5, 1, 1);

            var player = ActorCreator.CreateHumanPlayer();
            player.Acquire(ItemCreator.CreateBuckler());
            var rat = ActorCreator.CreateGiantRat();
            // buckler's parry is 2
            return new DamageRoll(3, 2, rat.Combatant, player.Combatant, fakeRNG);
        }

        private DamageRoll MakeCriticalHit()
        {
            var fakeRNG = new FakeRandomNum(new List<int>() { 1, 1, 5, 5, 2, 1 });

            var player = ActorCreator.CreateHumanPlayer();
            var rat = ActorCreator.CreateGiantRat();

            return new DamageRoll(5, 2, player.Combatant, rat.Combatant, fakeRNG);
        }
        #endregion
    }
}
