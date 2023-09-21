using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class MainReportController : Controller
    {
        [HttpPost]
        public async Task<ActionResult> Index(string activityId)
        {
            Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
            ActivityFormTBMMKT activityFormTBMMKT = new ActivityFormTBMMKT();

            if (!string.IsNullOrEmpty(activityId))
            {
                activity_TBMMKT_Model = ReportAppCode.mainReport(activityId, UtilsAppCode.Session.User.empId);
            }

            #region set viewbag
            if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formTrvTbmId"]
                || activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formTrvHcmId"]
                || activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formExpTrvNumId"]
                // (AppCode.hcForm.Contains(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id))
                )//แบบฟอร์มเดินทางปฏิบัติงานนอกสถานที่
            {
                ViewBag.classFont = "fontDocSmall";
                ViewBag.padding = "paddingFormV2";
            }
            else if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPaymentVoucherTbmId"]
                || activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPurchaseTbm"]
                || activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formBgTbmId"])
            {
                ViewBag.classFont = "formBorderStyle2";
                ViewBag.padding = "paddingFormV3";
            }
            else if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formExpMedNumId"])
            {
                ViewBag.classFont = "fontDocSmall";
                ViewBag.padding = "paddingFormV2";
            }
            else if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formCR_IT_FRM_314"])
            {
                ViewBag.classFont = "formBorderStyle3";
                ViewBag.padding = "paddingFormV3";
            }
            else
            {
                ViewBag.classFont = "fontDocV1";
                ViewBag.padding = "paddingFormV1";
            }
            #endregion

            return PartialView(activity_TBMMKT_Model);
        }


        public ActionResult styleView()
        {

            return PartialView();
        }

        public ActionResult ReportPettyCashNum(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            try
            {
                activity_TBMMKT_Model = ReportAppCode.reportPettyCashNumAppCode(activity_TBMMKT_Model);
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("ReportPettyCashNum>>" + ex.Message);
            }
            return PartialView(activity_TBMMKT_Model);
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



        public ActionResult ReportIndex(string typeForm, string typeFormId)
        {
            SearchReportModels models = new SearchReportModels();
            models.formDetail = QueryGet_master_type_form_detail.get_master_type_form_detail(typeFormId, "rptExport");

            return PartialView(models);
        }

    }
}