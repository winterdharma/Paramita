using NUnit.Framework;
using Paramita.GameLogic.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita.GameLogic.UnitTests.Items
{
    public class WeaponTests
    {
        #region Property Tests
        [Test]
        public void GetDamage_WeaponInstantiated_DamageIsAssigned()
        {
            var weapon = ItemCreator.CreateShortSword();
            var expected = 5;
            var actual = weapon.Damage;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetAttackModifier_WeaponInstantiated_AttackModifierIsAssigned()
        {
            var weapon = ItemCreator.CreateShortSword();
            var expected = 0;
            var actual = weapon.AttackModifier;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetDefenseModifier_WeaponInstantiated_DefenseModifierIsAssigned()
        {
            var weapon = ItemCreator.CreateShortSword();
            var expected = 2;
            var actual = weapon.DefenseModifier;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetLength_WeaponInstantiated_LengthIsAssigned()
        {
            var weapon = ItemCreator.CreateShortSword();
            var expected = 1;
            var actual = weapon.Length;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetEncumbrance_WeaponInstantiated_EncumbranceIsAssigned()
        {
            var weapon = ItemCreator.CreateShortSword();
            var expected = 2;
            var actual = weapon.Encumbrance;
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region Public Method Tests
        [Test]
        public void GetDescription_GivenWeapon_ReturnsNameAndStats()
        {
            var weapon = (Weapon)ItemCreator.CreateShortSword();
            var expected = "short sword (Damage: " + weapon.Damage + " plus strength, Att:" 
                + weapon.AttackModifier + ", Def:" + weapon.DefenseModifier + ", Length:" 
                + weapon.Length + ", Encumbrance: " + weapon.Encumbrance + ")";
            var actual = weapon.GetDescription();
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
