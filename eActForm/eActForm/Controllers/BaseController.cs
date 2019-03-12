using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eActForm.BusinessLayer;
namespace eActForm.Controllers
{
    public class BaseController : Controller
    {
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