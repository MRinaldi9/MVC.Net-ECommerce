﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsStore.WebUI.Models
{
    //view model class that contain info about the page
    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }

    }
}