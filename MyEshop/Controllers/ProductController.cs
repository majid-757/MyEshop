using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using MoreLinq;

namespace MyEshop.Controllers
{
    public class ProductController : Controller
    {
        MyEshop_DBEntities db = new MyEshop_DBEntities();
        public ActionResult ShowGroups()
        {
            return PartialView(db.Product_Groups.ToList());
        }



        public ActionResult LastProduct()
        {
            return PartialView(db.Products.OrderByDescending(p=>p.CreateDate).Take(12));
        }


        [Route("ShowProduct/{id}")]
        public ActionResult ShowProduct(int id)
        {
            var product = db.Products.Find(id);
            ViewBag.ProductFeatures =
                product.Product_Features.DistinctBy(f=>f.FeaturesID).Select(f=>new ShowProductFeatureViewModel()
                {
                    FeatureTitle = f.Features.FeaturesTitle,
                    Values = db.Product_Features.Where(fe=>fe.FeaturesID == f.FeaturesID).Select(fe=>fe.Value).ToList()
                }).ToList();
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }


        public ActionResult ShowComments(int id)
        {
            return PartialView(db.Product_Comments.Where(c=>c.ProductID == id));
        }



        public ActionResult CreateComment(int id)
        {
            return PartialView(new Product_Comments()
            {
                ProductID = id
            });
        }



        [HttpPost]
        public ActionResult CreateComment(Product_Comments productComments)
        {
            if (ModelState.IsValid)
            {
                productComments.CreateDate = DateTime.Now;
                db.Product_Comments.Add(productComments);
                db.SaveChanges();

                return PartialView("ShowComments", db.Product_Comments.Where(c=>c.ProductID == productComments.ProductID));
            }

            return PartialView(productComments);
        }
    }
}
