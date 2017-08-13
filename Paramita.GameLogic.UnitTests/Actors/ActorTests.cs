using NUnit.Framework;
using Paramita.GameLogic.Actors;
using Paramita.GameLogic.Levels;
using Paramita.GameLogic.Mechanics;
using System;

namespace Paramita.GameLogic.UnitTests.Actors
{
    public class ActorTests
    {
        #region Properties
        [Test]
        public void GetActorType_ActorInstantiated_BeingTypeIsAssigned()
        {
            var expected = ActorType.GiantRat;
            var actor = ActorCreator.CreateGiantRat();
            var actual = actor.ActorType;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetAndSetCurrentTile_ActorInstantiated_CurrentTileIsAssignedAndReturned()
        {
            var expected = new Tile(0, 1, TileType.Floor);
            var actor = ActorCreator.CreateGiantRat();
            actor.CurrentTile = expected;
            var actual = actor.CurrentTile;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetFacing_ActorInstantiated_FacingIsAssigned()
        {
            var expected = Compass.East;
            var actor = ActorCreator.CreateGiantRat();
            var actual = actor.Facing;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetInventory_ActorInstantiated_InventoryIsAssigned()
        {
            var expected = typeof(Inventory);
            var actor = ActorCreator.CreateGiantRat();
            var actual = actor.Inventory.GetType();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetCombatant_ActorInstantiated_CombatantIsAssigned()
        {
            var expected = typeof(Combatant);
            var actor = ActorCreator.CreateGiantRat();
            var actual = actor.Combatant.GetType();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetAndSetTimesAttacked_ActorInstantiated_TimesAttackedIsAssignedAndReturned()
        {
            var expected = 1;
            var actor = ActorCreator.CreateGiantRat();
            actor.TimesAttacked = expected;
            var actual = actor.TimesAttacked;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetAndSetIsDead_ActorInstantiated_IsDeadIsAssignedAndReturned()
        {
            var expected = true;
            var actor = ActorCreator.CreateGiantRat();
            actor.IsDead = expected;
            var actual = actor.IsDead;

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region Attack Method Tests
        public void Attack_AttackResolved_RaisesStatusMsgEvent()
        {

        }

        public void Attack_WhenActorKilled_SetsIsDeadToTrue()
        {

        }

        public void Attack_WhenActorSurvives_DoesNotSetIsDeadToTrue()
        {

        }
        #endregion



    }
}
