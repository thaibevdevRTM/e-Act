using eActForm.BusinessLayer;
using eActForm.Models;
using System;
using System.Configuration;
using System.Web;
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
            string user = "";
            string password = "";
            if (TempData["CustomerError"] != null)
            {
                ModelState.AddModelError("CustomerError", TempData["CustomerError"].ToString());
            }

            if (Request.Cookies["CL"] != null)
            {
                user = Request.Cookies["CL"]["n"];
                password = Request.Cookies["CL"]["p"];
                var chkRemember = Request.Cookies["CL"]["chkRemember"];
                return redirectResult(user, password);

            }

            return View();

        }

        [HttpPost]
        public ActionResult Login(string strUserName, string strPassword)
        {

            strUserName = !string.IsNullOrEmpty(strUserName) ? strUserName : EncrptHelper.MD5Encryp(Request.Form["txtUserName"].ToString().ToLower().Replace("i", "1"));
            strPassword = !string.IsNullOrEmpty(strPassword) ? strPassword : EncrptHelper.MD5Encryp(Request.Form["txtPassword"].ToString());
            return redirectResult(strUserName, strPassword);

        }

        public ActionResult redirectResult(string strUserName, string strPassword)
        {
            try
            {
                UtilsAppCode.Session.User = new ActUserModel.User();

                bool chkRemember = Request.Form["chkRemember"] == "true" ? true : false;
                if (chkRemember == true)
                {
                    HttpCookie newCookie = new HttpCookie("CL");
                    newCookie["n"] = strUserName;
                    newCookie["p"] = strPassword;
                    newCookie["chkRemember"] = chkRemember.ToString();
                    newCookie.Expires = DateTime.Today.AddDays(7);
                    Response.Cookies.Add(newCookie);
                }

                ActUserModel.ResponseUserAPI response = AuthenAppCode.doAuthen(strUserName, strPassword);
                if (response != null && response.userModel.Count > 0)
                {
                    UtilsAppCode.Session.User = response.userModel[0];
                    var getNewPosition = ApproveFlowAppCode.getNewPosition(UtilsAppCode.Session.User.empId, "");
                    if (getNewPosition.Count > 0)
                    {
                        UtilsAppCode.Session.User.empPositionTitleTH = getNewPosition[0].empPositionTitleTH;
                        UtilsAppCode.Session.User.empPositionTitleEN = getNewPosition[0].empPositionTitleEN;
                    }

                    int insertToken = eForms.Presenter.AppCode.pUserAppCode.insertTokenByEmpId(AppCode.StrCon, UtilsAppCode.Session.User.empId, UtilsAppCode.Session.User.tokenAccess, UtilsAppCode.Session.User.tokenType);
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
                    else if (Request.Form["txtParam"] == AppCode.ApproveEmailype.attachment.ToString())
                    {
                        return RedirectToAction("Image", "Base", new { f = Request.Form["txtFilename"] });
                    }
                    else
                    {
                        return RedirectToAction("index", "DashBoard");
                    }
                }
                else
                {
                    TempData["CustomerError"] = ConfigurationManager.AppSettings["messLoginFail"];

                    HttpCookie delCookie = new HttpCookie("CL");
                    delCookie.Expires = DateTime.Now.AddDays(-1D);
                    Response.Cookies.Add(delCookie);
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