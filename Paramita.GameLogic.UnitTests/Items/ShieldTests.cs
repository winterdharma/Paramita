using NUnit.Framework;
using Paramita.GameLogic.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita.GameLogic.UnitTests.Items
{
    public class ShieldTests
    {
        #region Property Tests
        [Test]
        public void GetProtection_ShieldInstantiated_ProtectionIsAssigned()
        {
            var shield = ItemCreator.CreateBuckler();
            var expected = 14;
            var actual = shield.Protection;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetEncumbrance_ShieldInstantiated_EncumbranceIsAssigned()
        {
            var shield = ItemCreator.CreateBuckler();
            var expected = 2;
            var actual = shield.Encumbrance;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetDefenseModifier_ShieldInstantiated_DefenseModifierIsAssigned()
        {
            var shield = ItemCreator.CreateBuckler();
            var expected = 0;
            var actual = shield.DefenseModifier;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetParry_ShieldInstantiated_ParryIsAssigned()
        {
            var shield = ItemCreator.CreateBuckler();
            var expected = 2;
            var actual = shield.Parry;
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region Public Method Tests
        [Test]
        public void GetDescription_ShieldInstance_ReportsDescription()
        {
            var shield = ItemCreator.CreateBuckler();
            var expected = "buckler (Protection: " + shield.Protection + ", Defense Modifier: " 
                + shield.DefenseModifier + ", Parry: " + shield.Parry + ", Encumbrance: " 
                + shield.Encumbrance + ")";
            var actual = shield.GetDescription();
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
