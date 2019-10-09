using eActForm.BusinessLayer.Appcodes;
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


        public ActionResult insertUsers(AdminUserModel.AdminUserModels model)
        {
            //Insert Role User
            if (model.chkRole.Any())
            {
                foreach (var item in model.chkRole)
                {
                    AdminUserAppCode.insertRole(Request.Form["txtEmpCode"], item);
                }
            }

            if (model.chkProductType.Any())
            {
                foreach (var item in model.chkProductType)
                {
                    foreach (var itemCust in model.custLi)
                    {
                        AdminUserAppCode.insertAuthorized(Request.Form["txtEmpCode"], Request.Form["ddlCompany"], itemCust, item);
                    }
                }
            }


            return RedirectToAction("Index");
        }
    }
}