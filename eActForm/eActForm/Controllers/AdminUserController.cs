using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class AdminUserController : Controller
    {
        // GET: AdminUser
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult insertUsers(List<AdminUserModel.AdminUserModels> model)
        {
            return View();
        }
    }
}