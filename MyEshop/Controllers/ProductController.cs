using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace MyEshop.Controllers
{
    public class ProductController : Controller
    {
        MyEshop_DBEntities db = new MyEshop_DBEntities();
        public ActionResult ShowGroups()
        {
            return PartialView(db.Product_Groups.ToList());
        }
    }
}