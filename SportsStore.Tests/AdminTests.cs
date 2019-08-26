using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SportsStore.Tests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contain_All_Products()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{Name = "P1",ProductID =1},
                new Product{Name = "P2",ProductID =2},
                new Product{Name = "P3",ProductID =3}
            });

            AdminController controller = new AdminController(mock.Object);

            Product[] result = ((IEnumerable<Product>)controller.Index().ViewData.Model).ToArray();

            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual("P1", result[0].Name);
            Assert.AreEqual("P2", result[1].Name);
            Assert.AreEqual("P3", result[2].Name);
        }

        [TestMethod]
        public void Can_Edit_Product()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID = 1,Name = "P1"},
                new Product{ProductID = 2,Name = "P2"},
                new Product{ProductID = 3,Name = "P3"}
            });

            AdminController controller = new AdminController(mock.Object);

            Product p1 = controller.Edit(1).ViewData.Model as Product;
            Product p2 = controller.Edit(2).ViewData.Model as Product;
            Product p3 = controller.Edit(3).ViewData.Model as Product;

            Assert.AreEqual(1, p1.ProductID);
            Assert.AreEqual(2, p2.ProductID);
            Assert.AreEqual(3, p3.ProductID);
        }

        [TestMethod]
        public void Canno_Edit_Nonexisten_Product()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
             {
                new Product{ProductID = 1,Name = "P1"},
                new Product{ProductID = 2,Name = "P2"},
                new Product{ProductID = 3,Name = "P3"}
             });

            AdminController controller = new AdminController(mock.Object);

            Product p4 = controller.Edit(4).ViewData.Model as Product;

            Assert.IsNull(p4);
        }
    }
}
