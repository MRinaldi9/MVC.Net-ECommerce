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

        public CartController(IProductRepository repo)
        {
            this.repository = repo;
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