using Model;
using ServiceProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentRegister.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<Member> MemberList = MemberServiceProxy.GetMemberList();
            return View(MemberList);
        }
        [HttpGet]
        public ActionResult ManageMember()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageMember(Member model)
        {
            if (ModelState.IsValid)
            {
                string ID = MemberServiceProxy.CreateMember(model);
            }
            else
            {
                return View();
            }
            return RedirectToAction("Index");
        }
    }
}