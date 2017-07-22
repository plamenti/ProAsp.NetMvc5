using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Products()
        {
            // Arrange
            // create a mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name="P1" },
                new Product {ProductID = 2, Name="P2" },
                new Product {ProductID = 3, Name="P3" }
            });

            // create a controller
            AdminController target = new AdminController(mock.Object);

            // Act
            Product[] result = ((IEnumerable<Product>)target.Index().ViewData.Model).ToArray();

            // Assert
            Assert.AreEqual(3, result.Length);
            Assert.AreEqual("P1", result[0].Name);
            Assert.AreEqual("P2", result[1].Name);
            Assert.AreEqual("P3", result[2].Name);
        }

        [TestMethod]
        public void Can_Edit_Product()
        {
            // Arrange
            // create a mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name="P1" },
                new Product {ProductID = 2, Name="P2" },
                new Product {ProductID = 3, Name="P3" }
            });

            // create a controller
            AdminController target = new AdminController(mock.Object);

            // Act
            Product p1 = target.Edit(1).ViewData.Model as Product;
            Product p2 = target.Edit(2).ViewData.Model as Product;
            Product p3 = target.Edit(3).ViewData.Model as Product;

            // Assert
            Assert.AreEqual(1, p1.ProductID);
            Assert.AreEqual(2, p2.ProductID);
            Assert.AreEqual(3, p3.ProductID);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexisting_Prodct()
        {
            // Arrange
            // create a mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name="P1" },
                new Product {ProductID = 2, Name="P2" },
                new Product {ProductID = 3, Name="P3" }
            });

            // create a controller
            AdminController target = new AdminController(mock.Object);

            // Act
            Product result = target.Edit(4).ViewData.Model as Product;

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            // Arrange
            // create mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            // create the controler
            AdminController target = new AdminController(mock.Object);

            // create a product
            Product product = new Product
            {
                Name = "Test"
            };

            // Act
            // try to save the product
            ActionResult result = target.Edit(product);

            // Assert
            // check that the repository was caleld
            mock.Verify(m => m.SaveProduct(product));

            // check the mehod result type
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            // Arrange
            // create a mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            // create the controller
            AdminController target = new AdminController(mock.Object);

            // create a product
            Product product = new Product { Name = "Test" };

            // add a error to the model state
            target.ModelState.AddModelError("Error", "Error");

            // Act
            // try to save the product
            ActionResult result = target.Edit(product);

            // Assert
            // check that the repository was nto called
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());

            // check teh method result type
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_Valid_Producs()
        {
            // Arrange
            // create a product
            Product prod = new Product { ProductID = 2, Name = "P2" };

            // create a mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1" },
                prod,
                new Product {ProductID = 3, Name = "P3" },
            });

            // create a controller
            AdminController target = new AdminController(mock.Object);

            // Act
            // delete the product
            target.Delete(prod.ProductID);

            //Assert
            // ensure taht the repository delete method was called with the correct Product
            mock.Verify(m => m.DeleteProduct(prod.ProductID));
        }
    }
}
