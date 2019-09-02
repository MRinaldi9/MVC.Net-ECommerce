using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
using Moq;
using SportsStore.Domain.Abstract;
using System.Linq;
using SportsStore.WebUI.Controllers;
using System.Web.Mvc;

namespace SportsStore.Tests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            Product prod = new Product
            {
                Name = "Test",
                ProductID = 2,
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };

            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {Name = "P1",ProductID = 1},
                prod,
                new Product{Name ="P2",ProductID = 3}
            }.AsQueryable());

            ProductController controller = new ProductController(mock.Object);

            ActionResult result = controller.GetImage(2);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(prod.ImageMimeType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{Name = "P1",ProductID = 1},
                new Product{Name = "P2",ProductID = 2},
            }.AsQueryable());

            ProductController controller = new ProductController(mock.Object);

            ActionResult result = controller.GetImage(100);

            Assert.IsNull(result);
        }
    }
}
