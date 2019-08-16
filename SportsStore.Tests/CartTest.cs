using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using SportsStore.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
