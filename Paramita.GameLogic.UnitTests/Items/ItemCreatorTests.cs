using NUnit.Framework;
using Paramita.GameLogic.Items;
using Paramita.GameLogic.Items.Armors;
using Paramita.GameLogic.Items.Consumables;
using Paramita.GameLogic.Items.Valuables;
using Paramita.GameLogic.Items.Weapons;
using System;

namespace Paramita.GameLogic.UnitTests.Items
{
    public class ItemCreatorTests
    {
        [Test]
        public void CreateItem_GivenUnimplementedItemType_ThrowsException()
        {
            Assert.Catch<NotImplementedException>(() => ItemCreator.CreateItem(ItemType.None));
        }

        [TestCase(ItemType.Bite, typeof(Bite))]
        [TestCase(ItemType.Coins, typeof(Coins), 10)]
        [TestCase(ItemType.Fist, typeof(Fist))]
        [TestCase(ItemType.Meat, typeof(Meat))]
        [TestCase(ItemType.ScaleMailCuirass, typeof(ScaleMailCuirass))]
        [TestCase(ItemType.Shield, typeof(Buckler))]
        [TestCase(ItemType.ShortSword, typeof(ShortSword))]
        public void CreateItem_GivenItemType_ReturnsCorrespondingObject(ItemType itemType, Type type, int number = 0)
        {
            var item = ItemCreator.CreateItem(itemType, number);
            var expected = type;
            var actual = item.GetType();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CreateScaleMailCuirass_WhenCalled_ReturnsScaleMailCuirass()
        {
            var item = ItemCreator.CreateScaleMailCuirass();
            var expected = typeof(ScaleMailCuirass);
            var actual = item.GetType();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CreateFist_WhenCalled_ReturnsFistObject()
        {
            var item = ItemCreator.CreateFist();
            var expected = typeof(Fist);
            var actual = item.GetType();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CreateBite_WhenCalled_ReturnsBiteObject()
        {
            var item = ItemCreator.CreateBite();
            var expected = typeof(Bite);
            var actual = item.GetType();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CreateShortSword_WhenCalled_ReturnsShortSwordObject()
        {
            var item = ItemCreator.CreateShortSword();
            var expected = typeof(ShortSword);
            var actual = item.GetType();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CreateCoins_WhenCalledWithNumber_ReturnsCoinsObject()
        {
            var expectedNum = 10;
            var expectedType = typeof(Coins);

            var item = ItemCreator.CreateCoins(expectedNum);

            var actualType = item.GetType();
            var actualNum = item.Number;

            Assert.AreEqual(expectedType, actualType);
            Assert.AreEqual(expectedNum, actualNum);
        }

        [Test]
        public void CreateMeat_WhenCalled_ReturnsMeatObject()
        {
            var item = ItemCreator.CreateMeat();
            var expected = typeof(Meat);
            var actual = item.GetType();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CreateBuckler_WhenCalled_ReturnsBucklerObject()
        {
            var item = ItemCreator.CreateBuckler();
            var expected = typeof(Buckler);
            var actual = item.GetType();

            Assert.AreEqual(expected, actual);
        }
    }
}
