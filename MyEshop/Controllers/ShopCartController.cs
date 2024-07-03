using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using DataLayer.ViewModels;

namespace MyEshop.Controllers
{
    public class ShopCartController : Controller
    {
        private MyEshop_DBEntities db = new MyEshop_DBEntities();

        // GET: ShopCart
        public ActionResult ShowCart()
        {
            List<ShopCartItemViewModel> list = new List<ShopCartItemViewModel>();

            if (Session["ShopCart"] != null)
            {
                List<ShopCartItem> listShop = (List<ShopCartItem>)Session["ShopCart"];

                foreach (var item in listShop)
                {
                    var product = db.Products.Where(p => p.ProductID == item.ProductID).Select(p => new
                    {
                        p.ImageName,
                        p.ProductTitle
                    }).Single();

                    list.Add(new ShopCartItemViewModel()
                    {
                        Count = item.Count,
                        ProductID = item.ProductID,
                        Title = product.ProductTitle,
                        ImageName = product.ImageName
                    });
                }
            }

            return PartialView(list);
        }



        public ActionResult Index()
        {
            return View();
        }


        List<ShopOrderViewModel> getListOrder()
        {
            List<ShopOrderViewModel> list = new List<ShopOrderViewModel>();

            if (Session["ShopCart"] != null)
            {
                List<ShopCartItem> listShop = Session["ShopCart"] as List<ShopCartItem>;

                foreach (var item in listShop)
                {
                    var product = db.Products.Where(p => p.ProductID == item.ProductID).Select(p => new
                    {
                        p.ImageName,
                        p.ProductTitle,
                        p.Price
                    }).Single();

                    list.Add(new ShopOrderViewModel()
                    {
                        Count = item.Count,
                        ProductID = item.ProductID,
                        Price = product.Price,
                        ImageName = product.ImageName,
                        Title = product.ProductTitle,
                        Sum = item.Count * product.Price
                    });
                }
            }

            return list;
        }


        public ActionResult Order()
        {
            return PartialView(getListOrder());
        }


        public ActionResult CommandOrder(int id, int count)
        {

            List<ShopCartItem> listShop = Session["ShopCart"] as List<ShopCartItem>;
            int index = listShop.FindIndex(p => p.ProductID == id);
            if (count == 0)
            {
                listShop.RemoveAt(index);
            }
            else
            {
                listShop[index].Count = count;
            }

            Session["ShopCart"] = listShop;

            return PartialView("Order", getListOrder());
        }



        [Authorize]
        public ActionResult Payment()
        {
            int userId = db.Users.Single(u => u.UserName == User.Identity.Name).UserID;
            Orders orders = new Orders()
            {
                UserID = userId,
                Date = DateTime.Now,
                IsFinaly = false
            };
            db.Orders.Add(orders);

            var listDetails = getListOrder();

            foreach (var item in listDetails)
            {
                db.OrderDetails.Add(new OrderDetails()
                {
                    Count = item.Count,
                    OrderID = orders.OrderID,
                    Price = item.Price,
                    ProductID = item.ProductID
                });
            }

            db.SaveChanges();

            //TODO : Online Payment

            return null;
        }
    }
}

