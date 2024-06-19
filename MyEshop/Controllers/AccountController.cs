using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using DataLayer.ViewModels;
using System.Web.Security;


namespace MyEshop.Controllers
{
    public class AccountController : Controller
    {
        private MyEshop_DBEntities db = new MyEshop_DBEntities();
        // GET: Account
        public ActionResult Register()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel register)
        {
            if (ModelState.IsValid)
            {
                if (!db.Users.Any(u=>u.Email == register.Email.Trim().ToLower()))
                {
                    Users user = new Users()
                    {
                        Email = register.Email.Trim().ToLower(),
                        Password = FormsAuthentication.HashPasswordForStoringInConfigFile(register.Password, "SHA256"),
                        ActiveCode = Guid.NewGuid().ToString(),
                        IsActive = false,
                        RegisterDate = DateTime.Now,
                        RoleID = 1,
                        UserName = register.UserName,
                    };
                    db.Users.Add(user);
                    db.SaveChanges();
                    return View("SuccessRegister", user);
                }
                else
                {
                    ModelState.AddModelError("Email", "ایمیل وارد شده تکراری است");
                }
            }

            return View(register);
        }



        public ActionResult Login()
        {
            return View();
        }
    }
}
