using eActForm.BusinessLayer;
using eActForm.Models;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
namespace eActForm.Controllers
{
    [LoginExpire]
    public class BaseController : Controller
    {
        public JsonResult resendApprove(string actId)
        {
            var result = new AjaxResult();
            try
            {
                EmailAppCodes.resendHistory(actId);
                EmailAppCodes.sendApprove(actId, AppCode.ApproveType.Activity_Form, true,false);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = AppCode.StrMessFail + " Detail :" + ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult resendApproveBudget(string actId)
        {
            var result = new AjaxResult();
            try
            {
                EmailAppCodes.resendHistory(actId);
                EmailAppCodes.sendApproveBudget(actId, AppCode.ApproveType.Budget_form, true);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = AppCode.StrMessFail + " Detail :" + ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Image(string f)
        {
            string path = Server.MapPath(String.Format("~/Uploadfiles/" + f));

            string mime = MimeMapping.GetMimeMapping(path);
            FileInfo fi = new FileInfo(f);

            if (fi.Extension == ".xlsx" || fi.Extension == "xls")
            {
                return File(path, "application/vnd.ms-excel", f);
            }
            else
            {
                return File(path, mime);
            }

        }


    }
    public class LoginExpireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (bool.Parse(ConfigurationManager.AppSettings["isMaintainMode"]))
            {
                filterContext.Result = new RedirectResult("~/Login/Maintain");
                return;
            }
            else
            {// check  sessions here
                if (UtilsAppCode.Session.User == null)
                {
                    filterContext.Result = new RedirectResult("~/Login/Index?" + filterContext.HttpContext.Request.QueryString);
                    return;
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}