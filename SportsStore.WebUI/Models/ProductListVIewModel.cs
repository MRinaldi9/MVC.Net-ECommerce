using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Models
{
    //class for sending datas to the view
    public class ProductListVIewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public PagingInfo _PagingInfo { get; set; }
    }
}