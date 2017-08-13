﻿using ControllersAndActions.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace ControllersAndActions.Tests
{
    [TestClass]
    public class ActionTests
    {
        [TestMethod]
        public void ControllerTest()
        {
            // Arrange
            // create a controller
            ExampleController target = new ExampleController();

            // Act
            // call the action method
            ViewResult result = target.Index();

            // Assert
            // check teh result
            Assert.AreEqual("Homepage", result.ViewName);
        }
    }
}
