﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Web.Mvc;
using SportsStore.WebUI.HtmlHelpers;
using SportsStore.WebUI.Models;

namespace SportsStore.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID = 1,Name = "P1"},
                new Product{ProductID = 2,Name = "P2"},
                new Product{ProductID = 3,Name = "P3"},
                new Product{ProductID = 4,Name = "P4"},
                new Product{ProductID = 5,Name = "P5"},
            });

            ProductController controller = new ProductController(mock.Object)
            {
                PageSize = 3
            };

            //Act
            ProductListVIewModel result = (ProductListVIewModel)controller.List(null, 2).Model;

            //Assert
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");

        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper myHelper = null;

            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            Func<int, string> pageUrlDelegate = i => "Page" + i;

            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            Assert.AreEqual(@"<a class=""btn btn-secondary"" href=""Page1"">1</a>" + @"<a class=""btn btn-primary selected"" href=""Page2"">2</a>" +
                            @"<a class=""btn btn-secondary"" href=""Page3"">3</a>", result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID = 1,Name = "P1"},
                new Product{ProductID = 2,Name = "P2"},
                new Product{ProductID = 3,Name = "P3"},
                new Product{ProductID = 4,Name = "P4"},
                new Product{ProductID = 5,Name = "P5"},
            });

            ProductController controller = new ProductController(mock.Object)
            {
                PageSize = 3
            };

            ProductListVIewModel result = (ProductListVIewModel)controller.List(null, 2).Model;

            PagingInfo pageInfo = result._PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID = 1,Name = "P1",Category = "Cat1"},
                new Product{ProductID = 2,Name = "P2",Category = "Cat2"},
                new Product{ProductID = 3,Name = "P3",Category = "Cat1"},
                new Product{ProductID = 4,Name = "P4",Category = "Cat2"},
                new Product{ProductID = 5,Name = "P5",Category = "Cat3"},
            });

            ProductController controller = new ProductController(mock.Object)
            {
                PageSize = 3
            };

            Product[] products = ((ProductListVIewModel)controller.List("Cat2", 1).Model).Products.ToArray();

            Assert.AreEqual(products.Length, 2);
            Assert.IsTrue(products[0].Name == "P2" && products[0].ProductID == 2);
            Assert.IsTrue(products[1].Name == "P4" && products[1].ProductID == 4);

        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID = 1,Name = "P1",Category = "Apples"},
                new Product{ProductID = 2,Name = "P2",Category = "Oranges"},
                new Product{ProductID = 3,Name = "P3",Category = "Apples"},
                new Product{ProductID = 4,Name = "P4",Category = "Plums"}
            });

            NavController controller = new NavController(mock.Object);

            string[] result = ((IEnumerable<string>)controller.Menu().Model).ToArray();

            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual(result[0], "Apples");
            Assert.AreEqual(result[1], "Oranges");
            Assert.AreEqual(result[2], "Plums");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID = 1,Name = "P1",Category = "Apples"},
                new Product{ProductID = 4,Name = "P2",Category = "Oranges"},
            });

            NavController controller = new NavController(mock.Object);

            string selectedCategory = "Apples";

            string result = controller.Menu(selectedCategory).ViewBag.SelectedCategory;

            Assert.AreEqual(selectedCategory, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Product_Count()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID = 1,Name = "P1",Category = "Cat1"},
                new Product{ProductID = 2,Name = "P2",Category = "Cat2"},
                new Product{ProductID = 3,Name = "P3",Category = "Cat1"},
                new Product{ProductID = 4,Name = "P4",Category = "Cat2"},
                new Product{ProductID = 5,Name = "P5",Category = "Cat3"},
            });

            ProductController controller = new ProductController(mock.Object)
            {
                PageSize = 3
            };

            int res1 = ((ProductListVIewModel)controller.List("Cat1").Model)._PagingInfo.TotalItems;
            int res2 = ((ProductListVIewModel)controller.List("Cat2").Model)._PagingInfo.TotalItems;
            int res3 = ((ProductListVIewModel)controller.List("Cat3").Model)._PagingInfo.TotalItems;
            int resAll = ((ProductListVIewModel)controller.List(null).Model)._PagingInfo.TotalItems;

            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
}
