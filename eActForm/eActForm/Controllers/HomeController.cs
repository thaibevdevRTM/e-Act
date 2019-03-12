using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eActForm.Models;
using eActForm.BusinessLayer;
namespace eActForm.Controllers
{
    [LoginExpire]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult myDoc()
        {
            Activity_Model.actForms model = new Activity_Model.actForms();
            model.actLists = ActFormAppCode.getActFormByEmpId(UtilsAppCode.Session.User.empId);
            return PartialView(model);
        }

        public ActionResult logOut()
        {
            UtilsAppCode.Session.User = null;
            return RedirectToAction("index", "home");
        }

        public ActionResult contact()
        {
            return View();
        }
    }
}