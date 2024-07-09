using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using DataLayer.ViewModels;

namespace MyEshop.Controllers
{
    public class HomeController : Controller
    {
        private MyEshop_DBEntities db = new MyEshop_DBEntities();

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
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            return PartialView(db.Slider.Where(s => s.IsActive && s.StartDate <= dt && s.EndDate >= dt));
        }



        public ActionResult VisitSite()
        {
            DateTime dtNow = DateTime.Now.Date;
            DateTime yesterday = dtNow.AddDays(-1);
            VisitSiteViewModel visit = new VisitSiteViewModel();
            visit.VisitSum = db.SiteVisit.Count();
            visit.VisitToday = db.SiteVisit.Count(v => v.Date == dtNow);
            visit.VisitYesterday = db.SiteVisit.Count(v => v.Date == yesterday);
            visit.online = int.Parse(System.Web.HttpContext.Current.Application["Online"].ToString());

            return PartialView(visit);
        }
    }
}
