using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;
using System.Linq;
using System.Web.Mvc;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // Arrange
            // create some tests products
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            // create a new cart
            Cart target = new Cart();

            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            CartLine[] results = target.Lines.ToArray();

            // Assert
            Assert.AreEqual(2, results.Length);
            Assert.AreEqual(p1, results[0].Product);
            Assert.AreEqual(p2, results[1].Product);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // Arrange
            // create some test products
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            // Arrange
            // create a new cart
            Cart target = new Cart();

            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);
            CartLine[] results = target.Lines.OrderBy(x => x.Product.ProductID).ToArray();

            // Assert
            Assert.AreEqual(2, results.Length);
            Assert.AreEqual(11, results[0].Quantity);
            Assert.AreEqual(1, results[1].Quantity);
        }

        [TestMethod]
        public void Can_Remove_Lines()
        {
            // Arrange
            // create some tests proucts
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Product p3 = new Product { ProductID = 3, Name = "P3" };

            // create a new cart
            Cart target = new Cart();

            // add some products to the cart
            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);

            // Act
            target.RemoveLine(p2);

            // Assert
            Assert.AreEqual(0, target.Lines.Where(x => x.Product == p2).Count());
            Assert.AreEqual(2, target.Lines.Count());
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            // Arrange
            // create some tests proucts
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

            // create a new cart
            Cart target = new Cart();

            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);
            decimal result = target.ComputeTotalValue();

            // Assert
            Assert.AreEqual(450M, result);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            // Arrange
            // create some tests proucts
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

            // create a new cart
            Cart target = new Cart();
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            // Act
            target.Clear();

            // Assert
            Assert.AreEqual(0, target.Lines.Count());
        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            // Arrange 
            // create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                new Product[]
                {
                    new Product
                    {
                        ProductID = 1,
                        Name = "P1",
                        Category = "Apples"
                    }
                }.AsQueryable());
            
            // create a cart
            Cart cart = new Cart();

            // create the controller
            CartController target = new CartController(mock.Object, null);

            // Act
            // add a product to the cart
            target.AddToCart(cart, 1, null);

            // Assert
            Assert.AreEqual(1, cart.Lines.Count());
            Assert.AreEqual(1, cart.Lines.ToArray()[0].Product.ProductID);
        }

        [TestMethod]
        public void Adding_Product_To_Cart_Goes_To_Cart_Screen()
        {
            // Arrange 
            // create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                new Product[]
                {
                    new Product
                    {
                        ProductID = 1,
                        Name = "P1",
                        Category = "Apples"
                    }
                }.AsQueryable());

            // create a cart
            Cart cart = new Cart();

            // create the controller
            CartController target = new CartController(mock.Object, null);

            //Act
            // add a product to the cart
            RedirectToRouteResult result = target.AddToCart(cart, 2, "myUrl");

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("myUrl", result.RouteValues["returnUrl"]);
        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            // Arrange
            // create a cart
            Cart cart = new Cart();

            // create a controller
            CartController target = new CartController(null, null);

            // Act
            // call the Index action method
            CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            // Assert
            Assert.AreSame(cart, result.Cart);
            Assert.AreEqual("myUrl", result.ReturnUrl);
        }
    }
}
