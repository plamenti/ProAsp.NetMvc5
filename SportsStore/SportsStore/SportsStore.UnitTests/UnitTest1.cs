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
            ProductsListViewModel result = (ProductsListViewModel)controller.List(2).Model;
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
            ProductsListViewModel result = (ProductsListViewModel) controller.List(2).Model;
            PagingInfo pagingInfo = result.PagingInfo;

            // Assert
            Assert.AreEqual(2, pagingInfo.CurrentPage);
            Assert.AreEqual(3, pagingInfo.ItemsPerPage);
            Assert.AreEqual(5, pagingInfo.TotalItems);
            Assert.AreEqual(2, pagingInfo.TotalPages);
        }
    }
}
