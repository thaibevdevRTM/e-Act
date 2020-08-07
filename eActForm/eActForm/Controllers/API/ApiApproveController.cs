using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.Models;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    public class ApiApproveController : Controller
    {
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult doApprove(string gridHtml, string statusId, string activityId)
        {
            var resultAjax = new AjaxResult();

            string empId = UtilsAppCode.Session.User.empId;
            ApproveAppCode.setCountWatingApprove();
            HostingEnvironment.QueueBackgroundWorkItem(c => doGenFile(gridHtml, empId, statusId, activityId));

            return Json(resultAjax, "text/plain");
        }

        /// <summary>
        /// ******* BackGround Service can't  use session ************
        /// </summary>
        /// <param name="gridHtml"></param>
        /// <param name="empId"></param>
        /// <param name="statusId"></param>
        /// <param name="activityId"></param>
        /// <returns></returns>
        private async Task<AjaxResult> doGenFile(string gridHtml, string empId, string statusId, string activityId)
        {
            var resultAjax = new AjaxResult();
            try
            {

                if (statusId == ConfigurationManager.AppSettings["statusReject"])
                {
                    EmailAppCodes.sendReject(activityId, AppCode.ApproveType.Activity_Form, empId);
                }
                else if (statusId == ConfigurationManager.AppSettings["statusApprove"])
                {

                    GenPDFAppCode.doGen(gridHtml, activityId);

                    EmailAppCodes.sendApprove(activityId, AppCode.ApproveType.Activity_Form, false);
                }
                resultAjax.Success = true;
            }
            catch (Exception ex)
            {
                resultAjax.Success = false;
                resultAjax.Message = ex.Message;
                //throw new Exception("apiApprove genPdfApprove >> " + ex.Message);
                EmailAppCodes.sendEmailWithActId("activityId"
                    , ConfigurationManager.AppSettings["emailForDevelopSite"]
                    , ""
                    , "eAct ApiApprove Error"
                    , activityId + " " + ex.Message
                    , null);
            }

            return resultAjax;
        }


        // GET: ApiApprove
        public ActionResult Index()
        {
            return View();
        }
    }
}