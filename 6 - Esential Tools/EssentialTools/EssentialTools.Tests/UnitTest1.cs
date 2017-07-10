using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EssentialTools.Models;

namespace EssentialTools.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Discount_Above_100()
        {
            // arrange
            IDiscountHelper target = GetTestObject();
            decimal total = 200;

            // act
            var discountedTotal = target.ApplyDiscount(total);

            // assert
            Assert.AreEqual(total * 0.9M, discountedTotal);
        }

        [TestMethod]
        public void Discount_Between_10_And_100()
        {
            // arrange
            IDiscountHelper target = GetTestObject();

            // act
            decimal TenDollarDiscount = target.ApplyDiscount(10);
            decimal HundredDollarDiscount = target.ApplyDiscount(100);
            decimal FiftyDollarDiscount = target.ApplyDiscount(50);

            //assert
            Assert.AreEqual(5, TenDollarDiscount, "$10 discount is wrong");
            Assert.AreEqual(95, HundredDollarDiscount, "$100 discount is wrong");
            Assert.AreEqual(45, FiftyDollarDiscount, "$50 discount is worng");
        }

        [TestMethod]
        public void Discount_Less_Than_10()
        {
            // arrange
            IDiscountHelper target = GetTestObject();

            // act
            decimal FiveDollarDiscount = target.ApplyDiscount(5);
            decimal ZeroDollarDiscount = target.ApplyDiscount(0);

            // assert
            Assert.AreEqual(5, FiveDollarDiscount, "$5 discount is wrong");
            Assert.AreEqual(0, ZeroDollarDiscount, "$0 discount is wrong");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Discount_Negative_Total()
        {
            // arrange
            IDiscountHelper target = GetTestObject();

            // act
            target.ApplyDiscount(-1);
        }

        private IDiscountHelper GetTestObject()
        {
            return new MinimumDiscountHelper();
        }
    }
}
