using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.WebUI.Infrastructure.Abstract;
using Moq;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;
using System.Web.Mvc;

namespace SportsStore.Tests
{
    [TestClass]
    public class AdminSecurityTest
    {
        [TestMethod]
        public void Can_Login_With_Valid_Credentials()
        {
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "admin")).Returns(true);

            LoginViewModel model = new LoginViewModel
            {
                Name = "admin",
                Password = "admin"
            };

            AccountController controller = new AccountController(mock.Object);

            ActionResult result = controller.Login(model, "/MyURL");

            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("/MyURL", ((RedirectResult)result).Url);
        }

        [TestMethod]
        public void Cannot_Login_With_Invalid_Credentials()
        {
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("Nope", "nope")).Returns(false);

            LoginViewModel model = new LoginViewModel
            {
                Name = "nope",
                Password = "nope"
            };

            AccountController controller = new AccountController(mock.Object);

            ActionResult result = controller.Login(model, "/MyURL");

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }
    }
}
