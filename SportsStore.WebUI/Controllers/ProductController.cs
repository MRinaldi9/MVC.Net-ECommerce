﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int PageSize = 4;

        public ProductController(IProductRepository productRepository)
        {
            this.repository = productRepository;
        }

        public ViewResult List(string category, int page = 1)
        {
            ProductListVIewModel model = new ProductListVIewModel
            {
                Products = repository.Products
                           .Where(p => category == null || p.Category == category)
                           .OrderBy(product => product.ProductID)
                           .Skip((page - 1) * PageSize)
                           .Take(PageSize),

                _PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ? repository.Products.Count() : repository.Products.Where(f => f.Category == category).Count()
                },
                CurrentCategory = category
            };
            return View(model);
        }
    }
}