﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DataLayer;
using DataLayer.ViewModels;

namespace MyEshop.Areas.UserPanel.Controllers
{
    public class AccountController : Controller
    {
        private MyEshop_DBEntities db = new MyEshop_DBEntities();
        // GET: UserPanel/Account
        public ActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel change)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Single(u => u.UserName == User.Identity.Name);
                string oldPasswordHash =
                    FormsAuthentication.HashPasswordForStoringInConfigFile(change.OldPassword, "SHA256");
                if (user.Password == oldPasswordHash)
                {
                    string hashNewPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(change.Password, "SHA256");
                    user.Password = hashNewPassword;
                    db.SaveChanges();
                    ViewBag.Success = true;

                }
                else
                {
                    ModelState.AddModelError("OldPassword", "کلمه عبور فعلی درست نمی باشد.");
                }
            }
            return View();
        }
    }
}
