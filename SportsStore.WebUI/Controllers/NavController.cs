using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SportsStore.Domain.Abstract;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private readonly IProductRepository repository;

        public NavController(IProductRepository repo)
        {
            this.repository = repo;
        }

        public PartialViewResult Menu()
        {
            IEnumerable<string> categories = repository.Products
                                             .Select(x => x.Category)
                                             .Distinct()
                                             .OrderBy(x => x);
            return PartialView(categories);
        }
    }
}