﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEshop.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult aboutus()
        {
            return View();
        }


        public ActionResult Slider()
        {
            return PartialView();
        }
    }
}
