using System;
using System.Text;
using System.Web.Mvc;
using System.Linq;
using SportsStore.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;
using SportsStore.Domain.Abstract;

namespace SportsStore.Tests
{
    [TestClass]
    public class CartTest
    {

        [TestMethod]
        public void Can_Add_New_Lines()
        {
            Product product1 = new Product { ProductID = 1, Name = "P1" };
            Product product2 = new Product { ProductID = 2, Name = "P2" };

            Cart cart = new Cart();

            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);

            CartLine[] results = cart.Lines.ToArray();

            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0]._Product, product1);
            Assert.AreEqual(results[1]._Product, product2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            Cart cart = new Cart();

            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            cart.AddItem(p1, 10);

            CartLine[] results = cart.Lines.OrderBy(r => r._Product.ProductID).ToArray();

            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Quantity, 11);
            Assert.AreEqual(results[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Product p3 = new Product { ProductID = 3, Name = "P3" };

            Cart cart = new Cart();

            cart.AddItem(p1, 1);
            cart.AddItem(p2, 3);
            cart.AddItem(p3, 5);
            cart.AddItem(p2, 2);

            cart.RemoveLine(p2);

            Assert.AreEqual(cart.Lines.Where(p => p._Product == p2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);
        }

        [TestMethod]
        public void Calculate_Total_Cart()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

            Cart cart = new Cart();

            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            cart.AddItem(p1, 3);

            decimal result = cart.ComputeTotalValue();

            Assert.AreEqual(result, 450);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

            Cart cart = new Cart();

            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);

            cart.Clear();

            Assert.AreEqual(cart.Lines.Count(), 0);
        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product
                {
                    ProductID = 1,
                    Name = "P1",
                    Category = "Apples"
                }
            }.AsQueryable());

            Cart cart = new Cart();
            CartController controller = new CartController(mock.Object, null);

            controller.AddToCart(cart, 1, null);

            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0]._Product.Name, "P1");
        }

        [TestMethod]
        public void Adding_Product_To_Cart_Goes_To_Cart_Screen()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product
                {
                    ProductID = 1,
                    Name = "P1",
                    Category = "Apples"
                }
            }.AsQueryable());

            Cart cart = new Cart();
            CartController controller = new CartController(mock.Object, null);

            var route = controller.AddToCart(cart, 1, "myUrl");

            //TODO: understanding this Assert
            Assert.AreEqual(route.RouteValues["action"], "Index");
            Assert.AreEqual(route.RouteValues["returnUrl"], "myUrl");
        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            Cart cart = new Cart();

            CartController controller = new CartController(null, null);

            CartIndexViewModel result = (CartIndexViewModel)controller.Index(cart, "myUrl").ViewData.Model;

            Assert.AreSame(result._Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            ShippingDetails details = new ShippingDetails();
            CartController controller = new CartController(null, mock.Object);

            ViewResult result = controller.Checkout(cart, details);

            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());

            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            CartController controller = new CartController(null, mock.Object);
            controller.ModelState.AddModelError("error", "Error");

            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_and_Submit_Order()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            CartController controller = new CartController(null, mock.Object);

            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Once());

            Assert.AreEqual("Completed", result.ViewName);
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}
