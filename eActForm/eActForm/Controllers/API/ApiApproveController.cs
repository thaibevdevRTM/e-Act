using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using eForms.Models.MasterData;
using eForms.Presenter.AppCode;
using iTextSharp.tool.xml.html;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;
using WebLibrary;
using static iTextSharp.text.pdf.AcroFields;

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

            Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
            activity_TBMMKT_Model = ReportAppCode.mainReport(activityId, "");

            ApproveAppCode.setCountWatingApprove();
            var getHeader = GenPDFAppCode.getHeader(activity_TBMMKT_Model);
            HostingEnvironment.QueueBackgroundWorkItem(c => new ActivityController().doGenFile(gridHtml, getHeader, empId, statusId, activityId, "", activity_TBMMKT_Model));

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
        /// 



    }
}