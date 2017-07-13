using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.HtmlHelpers;
using SportsStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;
            Product[] prodArray = result.Products.ToArray();

            // Assert
            Assert.IsTrue(prodArray.Length == expectedProductsCount, string.Format("Products count in page {0} should be {1}. It is {2}.", page, expectedProductsCount, prodArray.Count()));
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            // Arrange - define an HTML helper - we need to do this in order to apply teh extension method
            HtmlHelper myHelper = null;

            // Create PagingInfo data
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Set up the delegate using a lambda expession
            Func<int, string> pageUrlDelegate = i => "Page" + i; // delegate (int i) { return "Page" + i; }; 

            // Act
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Assert
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a><a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a><a class=""btn btn-default"" href=""Page3"">3</a>", result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                new Product[]
                {
                    new Product {ProductID = 1, Name = "P1" },
                    new Product {ProductID = 2, Name = "P2" },
                    new Product {ProductID = 3, Name = "P3" },
                    new Product {ProductID = 4, Name = "P4" },
                    new Product {ProductID = 5, Name = "P5" }
                });

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // Act 
            ProductsListViewModel result = (ProductsListViewModel) controller.List(null, 2).Model;
            PagingInfo pagingInfo = result.PagingInfo;

            // Assert
            Assert.AreEqual(2, pagingInfo.CurrentPage);
            Assert.AreEqual(3, pagingInfo.ItemsPerPage);
            Assert.AreEqual(5, pagingInfo.TotalItems);
            Assert.AreEqual(2, pagingInfo.TotalPages);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            // Arrange
            // create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                new Product[] 
                {
                    new Product {ProductID = 1, Name = "P1", Category = "Cat1" },
                    new Product {ProductID = 2, Name = "P2", Category = "Cat2" },
                    new Product {ProductID = 3, Name = "P3", Category = "Cat1" },
                    new Product {ProductID = 4, Name = "P4", Category = "Cat2" },
                    new Product {ProductID = 5, Name = "P5", Category = "Cat3" },
                });

            // create a controller and make te page size 3 items
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;
            string category = "Cat2";

            // Action
            Product[] result = ((ProductsListViewModel)controller.List(category, 1).Model).Products.ToArray();

            // Assert
            Assert.AreEqual(2, result.Length, string.Format("Wrong count of products with category {0}", category));
            Assert.IsTrue(result[0].Name == "P2", string.Format("Product name should be P2, it is {0}", result[0].Name));
            Assert.IsTrue(result[0].Category == "Cat2", string.Format("Product name should be Cat2, it is {0}", result[0].Category));
            Assert.IsTrue(result[1].Name == "P4", string.Format("Product name should be P4, it is {0}", result[1].Name));
            Assert.IsTrue(result[1].Category == "Cat2", string.Format("Product name should be Cat2, it is {0}", result[1].Category));
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            // Arrange
            // creae the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                new Product[] 
                {
                    new Product {ProductID = 1, Name = "P1", Category = "Apples" },
                    new Product {ProductID = 2, Name = "P2", Category = "Apples" },
                    new Product {ProductID = 3, Name = "P3", Category = "Plums" },
                    new Product {ProductID = 4, Name = "P4", Category = "Oranges" }
                });

            // create a controller
            NavController target = new NavController(mock.Object);

            // Act
            // get the set f categories
            string[] results = ((IEnumerable<string>)target.Menu().Model).ToArray();

            // Assert
            Assert.AreEqual(3, results.Length, "Wrong count of categories");
            Assert.AreEqual("Apples", results[0], "Wrong first categorie");
            Assert.AreEqual("Oranges", results[1], "Wrong second categorie");
            Assert.AreEqual("Plums", results[2], "Wrong third categorie");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            // Arrange
            // create a mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                new Product[] 
            {
                new Product {ProductID = 1, Name = "P1", Category = "Apples" },
                new Product {ProductID = 4, Name = "P2", Category = "Oranges" }
            });

            // create the controller
            NavController target = new NavController(mock.Object);

            // define the cateory to selected
            string categoryToSelect = "Apples";

            // Act
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategry;

            // Assert
            Assert.AreEqual(categoryToSelect, result, "Wrong selected category");
        }
    }
}
