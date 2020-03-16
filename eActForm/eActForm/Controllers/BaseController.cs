﻿using eActForm.BusinessLayer;
using eActForm.Models;
using System;
using System.Configuration;
using System.Web.Mvc;
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
                EmailAppCodes.sendApprove(actId, AppCode.ApproveType.Activity_Form, true);
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