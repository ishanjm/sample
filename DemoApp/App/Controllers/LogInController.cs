using Model.SystemModel;
using Services.SystemService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Controllers
{
    public class LogInController : Controller
    {
        // GET: LogIn
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            bool IsLogIn = SystemService.LogIn(Request.Form["UserNAme"], Request.Form["Password"]);
            if (IsLogIn)
            {
                string UserID = UserIdentity.UserID;
                var userCookie = new HttpCookie("UserID", UserID);
                userCookie.Expires.AddDays(365);
                HttpContext.Response.SetCookie(userCookie);
                return RedirectToAction("Index","Home");
            }
            return View();
        }
    }
}