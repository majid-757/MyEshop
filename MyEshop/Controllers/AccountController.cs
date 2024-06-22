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

        [Route("Register")]
        public ActionResult Register()
        {
            return View();
        }



        [HttpPost]
        [Route("Register")]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel register, string gRecaptchaResponse)
        {


            //var recaptchaHelper = new RecaptchaHelper();
            //var captchaResponse = recaptchaHelper.ValidateRecaptcha(gRecaptchaResponse);

            //System.Diagnostics.Debug.WriteLine("reCAPTCHA Success: " + captchaResponse.Success);

            //if (!captchaResponse.Success)
            //{
            //    ModelState.AddModelError("reCaptcha", "لطفا reCAPTCHA را تایید کنید.");
            //    return View();
            //}


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

                    // send active email
                    string body = PartialToStringClass.RenderPartialView("ManageEmails", "ActivationEmail", user);
                    SendEmail.Send(user.Email, "ایمیل فعال سازی", body);
                    // end active email
                    return View("SuccessRegister", user);
                }
                else
                {
                    ModelState.AddModelError("Email", "ایمیل وارد شده تکراری است");
                }
            }

            return View(register);
        }



        [Route("Login")]
        public ActionResult Login()
        {

            return View();
        }


        [HttpPost]
        [Route("Login")]
        public ActionResult Login(LoginViewModel login, string ReturnUrl="/")
        {
            if (ModelState.IsValid)
            {
                string hashPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(login.Password, "SHA256");
                var user = db.Users.SingleOrDefault(u => u.Email == login.Email && u.Password == hashPassword);
                if (user!=null)
                {
                    if (user.IsActive)
                    {
                        FormsAuthentication.SetAuthCookie(user.UserName, login.RememberMe);
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "حساب کاربری شما فعال نشده است");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "کاربری با اطلاعات  وارد شده یافت نشد");
                }
            }

            return View(login);
        }



        public ActionResult ActiveUser(string id)
        {
            var user = db.Users.SingleOrDefault(u => u.ActiveCode == id);
            if (user == null)
            {
                return HttpNotFound();
            }

            user.IsActive = true;
            user.ActiveCode = Guid.NewGuid().ToString();
            db.SaveChanges();
            ViewBag.UserName = user.UserName;
            return View();
        }



        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }


        [Route("ForgotPassword")]
        public ActionResult ForgotPassword()
        {
            return View();
        }



        [Route("ForgotPassword")]
        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordViewModel forgot)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.SingleOrDefault(u => u.Email == forgot.Email);
                if (user !=null)
                {
                    if (user.IsActive)
                    {
                        string bodyEmail =
                            PartialToStringClass.RenderPartialView("ManageEmails", "RecoveryPassword", user);
                        SendEmail.Send(user.Email, "بازیابی کلمه عبور ", bodyEmail);
                        return View("SuccessForgotPassword", user);
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "حساب کاربری شما فعال نیست");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "کاربری یافت نشد.");
                }
            }
            return View();
        }



        public ActionResult RecoveryPassword(string id)
        {
            return View();
        }



        [HttpPost]
        public ActionResult RecoveryPassword(string id, RecoveryPasswordViewModel recovery)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.SingleOrDefault(u => u.ActiveCode == id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(recovery.Password, "SHA256");
                user.ActiveCode = Guid.NewGuid().ToString();
                db.SaveChanges();
                return Redirect("/Login?recovery=true");
            }
            return View();
        }
    }
}
