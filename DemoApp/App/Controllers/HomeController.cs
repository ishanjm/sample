using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Controllers
{
    public class HomeController : Controller
    {
        string XMLPath = ConfigurationManager.AppSettings["XMLPath"];
        public ActionResult Index()
        {
            int length = 5;
            string HTML = string.Empty;
            for (int i = 0; i < length; i++)
            {
                HTML += "<tr><td style='display:none'>"+i+"</td><td> Item "+i+ "</td><td><input type='button' onclick='Add('"+i+ "')' class='btn btn-primary btn - labeled fa fa-file - text - o' value='Click me' /></td></tr>";
            }
            ViewBag.ItemTable = HTML;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}