using eActForm.BusinessLayer;
using eActForm.Models;
using System;
using System.Configuration;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    public class LoginController : Controller
    {

        public ActionResult Maintain()
        {
            if (!bool.Parse(ConfigurationManager.AppSettings["isMaintainMode"]))
            {
                return RedirectToAction("index");
            }
            else
            {
                return View();
            }
        }
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
                UtilsAppCode.Session.User = new ActUserModel.User();
                string strUserName = EncrptHelper.MD5Encryp(Request.Form["txtUserName"].ToString().ToLower().Replace("i", "1"));
                string strPassword = EncrptHelper.MD5Encryp(Request.Form["txtPassword"].ToString());
                ActUserModel.ResponseUserAPI response = AuthenAppCode.doAuthen(strUserName, strPassword);
                if (response != null && response.userModel.Count > 0)
                {
                    UtilsAppCode.Session.User = response.userModel[0];
                    //UtilsAppCode.Session.User.empId = "11028011";
                    UserAppCode.setRoleUser(UtilsAppCode.Session.User.empId);
                    ApproveAppCode.setCountWatingApprove();
                    BudgetApproveController.setCountWatingApproveBudget();

                    if (Request.Form["txtParam"] != null && Request.Form["txtParam"] == AppCode.ApproveEmailype.approve.ToString())
                    {
                        return RedirectToAction("index", "ApproveLists");
                    }
                    else if (Request.Form["txtParam"] == AppCode.ApproveEmailype.document.ToString())
                    {
                        return RedirectToAction("index", "Home", new
                        {
                            actId = Request.Form["txtActId"],
                            typeForm = UtilsAppCode.Session.User.empCompanyId == ConfigurationManager.AppSettings["companyId_OMT"] ? Activity_Model.activityType.OMT.ToString() : Activity_Model.activityType.MT.ToString()
                        });
                    }
                    else
                    {
                        return RedirectToAction("index", "DashBoard");
                    }
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