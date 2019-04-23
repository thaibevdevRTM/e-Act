using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eActForm.BusinessLayer;
using eActForm.Models;

namespace eActForm.Controllers
{
    public class BaseController : Controller
    {

        public JsonResult resendApprove(string actId)
        {
            var result = new AjaxResult();
            try
            {
                EmailAppCodes.resendHistory(actId);
                EmailAppCodes.sendApprove(actId, AppCode.ApproveType.Activity_Form);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = AppCode.StrMessFail + " Detail :" + ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
    public class LoginExpireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            // check  sessions here
            if (UtilsAppCode.Session.User == null)
            {
                filterContext.Result = new RedirectResult("~/Login/Index?" + filterContext.HttpContext.Request.QueryString);
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}