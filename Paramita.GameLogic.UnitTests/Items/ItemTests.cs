using NUnit.Framework;
using Paramita.GameLogic.Items;
using System.Collections.Generic;

namespace Paramita.GameLogic.UnitTests.Items
{
    public class ItemTests
    {
        #region Property Tests
        [Test]
        public void GetId_ItemsInstantiated_ClassCounterIncrementsAndAssignsId()
        {
            var firstItem = (Item)ItemCreator.CreateMeat();
            var secondItem = (Item)ItemCreator.CreateShortSword();
            var thirdItem = (Item)ItemCreator.CreateBuckler();
            int firstId = firstItem.Id;

            var expected = new List<int>() { firstId, firstId + 1, firstId + 2 };
            var actual = new List<int>() { firstItem.Id, secondItem.Id, thirdItem.Id };

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void GetEquipType_ItemInstantiated_IsAssignedEquipType()
        {
            var expected = EquipType.Hand;
            var handItem = (Item)ItemCreator.CreateShortSword();
            var actual = handItem.EquipType;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetItemType_ItemInstantiated_IsAssignedItemType()
        {
            var expected = ItemType.ShortSword;
            var handItem = (Item)ItemCreator.CreateShortSword();
            var actual = handItem.ItemType;

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region IEquatable Tests
        [Test]
        public void Equals_ItemsOfSameType_ReturnsTrue()
        {
            var firstItem = ItemCreator.CreateBuckler();
            var secondItem = ItemCreator.CreateBuckler();
            var actual = firstItem.Equals(secondItem);

            Assert.True(actual);
        }

        [Test]
        public void Equals_ItemsOfDifferentTypes_ReturnsFalse()
        {
            var firstItem = ItemCreator.CreateBuckler();
            var secondItem = ItemCreator.CreateMeat();
            var actual = firstItem.Equals(secondItem);

            Assert.False(actual);
        }

        [Test]
        public void EqualOperator_ItemsWithSameId_ReturnsTrue()
        {
            var firstItem = ItemCreator.CreateBuckler();
            var secondItem = firstItem;
            var actual = firstItem == secondItem;

            Assert.True(actual);
        }

        [Test]
        public void EqualOpertor_ItemsWithDifferentIds_ReturnFalse()
        {
            var firstItem = ItemCreator.CreateBuckler();
            var secondItem = ItemCreator.CreateBuckler();
            var actual = firstItem == secondItem;

            Assert.False(actual);
        }

        [Test]
        public void NotEqualOperator_ItemsWithSameId_ReturnsFalse()
        {
            var firstItem = ItemCreator.CreateBuckler();
            var secondItem = firstItem;
            var actual = firstItem != secondItem;

            Assert.False(actual);
        }

        [Test]
        public void NotEqualOperator_ItemsWithDifferentIds_ReturnsTrue()
        {
            var firstItem = ItemCreator.CreateBuckler();
            var secondItem = ItemCreator.CreateBuckler();
            var actual = firstItem != secondItem;

            Assert.True(actual);
        }
        #endregion

        #region ToString Tests
        [Test]
        public void ToString_GivenItem_ReturnsIdAndName()
        {
            var item = ItemCreator.CreateBuckler();
            var expected = item.Id + " : buckler";
            var actual = item.ToString();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DisplayText_GivenItem_ReturnsName()
        {
            var item = ItemCreator.CreateBuckler();
            var expected = "buckler";
            var actual = item.DisplayText();
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
