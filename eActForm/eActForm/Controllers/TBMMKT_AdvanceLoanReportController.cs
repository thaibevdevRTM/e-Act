using eActForm.BusinessLayer;
using eActForm.Models;
using eActForm.Models;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class TBMMKT_AdvanceLoanReportController : Controller
    {

        public ActionResult Index(string activityId)
        {
            //Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
            //if (!string.IsNullOrEmpty(activityId))
            //{
            //    activity_TBMMKT_Model = ActivityFormTBMMKTCommandHandler.getDataForEditActivity(activityId);
            //}

            //return PartialView(activity_TBMMKT_Model);
            //return PartialView(); //ไว้ใช้เวลาจะใส่กับ Modal
            return View();
        }

       

        [HttpPost]
        [ValidateInput(false)]
        public FileResult printDoc(string gridHtml)
        {
            List<Attachment> file = new List<Attachment>();
            try
            {
                var rootPathInsert = string.Format(ConfigurationManager.AppSettings["rooPdftURL"], "");
                gridHtml = gridHtml.Replace("<br>", "<br/>");
                file = AppCode.genPdfFile(gridHtml, new Document(PageSize.A4, 25, 25, 10, 10), "");

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }
            return File(file[0].ContentStream, "application/pdf", "reportPDF.pdf");

        }
    }
}