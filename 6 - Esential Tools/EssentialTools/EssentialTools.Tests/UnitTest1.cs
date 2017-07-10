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

        private IDiscountHelper GetTestObject()
        {
            return new MinimumDiscountHelper();
        }
    }
}
