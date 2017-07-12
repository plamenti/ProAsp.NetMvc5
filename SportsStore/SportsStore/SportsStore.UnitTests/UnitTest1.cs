using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]{
                new Product {ProductID =1, Name="P1" },
                new Product {ProductID =2, Name="P2" },
                new Product {ProductID =3, Name="P3" },
                new Product {ProductID =4, Name="P4" },
                new Product {ProductID =5, Name="P5" },
            });

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;
            int page = 2;
            int expectedProductsCount = 2;

            // Act
            IEnumerable<Product> result = (IEnumerable<Product>)controller.List(page).Model;
            Product[] prodArray = result.ToArray();

            // Assert
            Assert.IsTrue(prodArray.Length == expectedProductsCount, string.Format("Products count in page {0} should be {1}. It is {2}.", page, expectedProductsCount, prodArray.Count()));
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }
    }
}
