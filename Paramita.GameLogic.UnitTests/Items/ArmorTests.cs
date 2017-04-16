using NUnit.Framework;
using Paramita.GameLogic.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita.GameLogic.UnitTests.Items
{
    public class ArmorTests
    {
        #region Property Tests
        [Test]
        public void GetProtection_ArmorInstantiated_ProtectionIsAssigned()
        {
            var armor = (Armor)ItemCreator.CreateScaleMailCuirass();
            var expected = 10;
            var actual = armor.Protection;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetDefenseModifier_ArmorInstantiated_DefenseModifierIsAssigned()
        {
            var armor = (Armor)ItemCreator.CreateScaleMailCuirass();
            var expected = -1;
            var actual = armor.DefenseModifier;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetEncumbrance_ArmorInstantiated_EncumbranceIsAssigned()
        {
            var armor = (Armor)ItemCreator.CreateScaleMailCuirass();
            var expected = 3;
            var actual = armor.Encumbrance;
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region Public Method Tests
        [Test]
        public void GetDescription_GivenArmor_ReturnsNameAndStats()
        {
            var armor = (Armor)ItemCreator.CreateScaleMailCuirass();
            var expected = "scalemail cuirass (Protection: " + armor.Protection + ", Defense modifier: "
                + armor.DefenseModifier + ", Encumbrance: " + armor.Encumbrance + ")";
            var actual = armor.GetDescription();
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
