using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using WebLibrary;
using eActForm.BusinessLayer;
using eActForm.Models;

namespace eActForm.Controllers
{
    public class LoginController : Controller
    {

        public ActionResult Index()
        {
            if (TempData["CustomerError"] != null)
            {
                ModelState.AddModelError("CustomerError", TempData["CustomerError"].ToString());
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login()
        {
            try
            {
                string strUserName = EncrptHelper.MD5Encryp(Request.Form["txtUserName"].ToString());
                string strPassword = EncrptHelper.MD5Encryp(Request.Form["txtPassword"].ToString());
                ActUserModel.ResponseUserAPI response = AuthenAppCode.doAuthen(strUserName, strPassword);
                if (response != null && response.userModel.Count > 0)
                {
                    UtilsAppCode.Session.User = response.userModel[0];
                    UtilsAppCode.Session.User.empId = "11005737";
                    return RedirectToAction("index", "DashBoard");
                }
                else
                {
                    TempData["CustomerError"] = ConfigurationManager.AppSettings["messLoginFail"];
                }
            }
            catch (Exception ex)
            {
                TempData["CustomerError"] = ex.Message;
                
            }
            return RedirectToAction("Index");

        }
    }
}