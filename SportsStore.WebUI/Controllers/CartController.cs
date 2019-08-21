using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository repository;
        private readonly IOrderProcessor orderProcessor;

        public CartController(IProductRepository repo, IOrderProcessor processor)
        {
            this.repository = repo;
            this.orderProcessor = processor;
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            //Pass to the view the model CartIndexViewModel with the object Cart and URL to return when continue the shopping
            return View(new CartIndexViewModel { _Cart = cart, ReturnUrl = returnUrl });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails details)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry your cart is empty");
            }

            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, details);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(details);
            }
        }

        //Get (and set) cart object through Session that use cookie
        //private Cart GetCart()
        //{
        //    Cart cart = (Cart)Session["Cart"];
        //    if (cart == null)
        //    {
        //        cart = new Cart();
        //        Session["Cart"] = cart;
        //    }
        //    return cart;
        //}

        //Using parameters that are equal to the input field allows the MVC framework to associate them with the variable of the form
        //and avoid to process manually the form
        public RedirectToRouteResult AddToCart(Cart cart, int productID, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productID);

            if (product != null)
            {
                cart.AddItem(product, 1);
            }
            //redirecttoaction has the effect of sending a HTTP redirect instruction for asking, at browser, a new URL.
            //In this case redirection pass through the Index action method 
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int productID, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productID);

            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }



    }
}