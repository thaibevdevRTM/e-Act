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
    public class MainReportController : Controller
    {

        public ActionResult Index(string activityId)
        {



            Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
            ActivityFormTBMMKT activityFormTBMMKT = new ActivityFormTBMMKT();
            //=========for=====test=================
            /*ActivityFormTBMMKT dummy_activityFormTBMMKT = new ActivityFormTBMMKT();
            activity_TBMMKT_Model.activityFormTBMMKT = dummy_activityFormTBMMKT;
            activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id = ConfigurationManager.AppSettings["formPosTbmId"];
            activity_TBMMKT_Model.master_Type_Form_Detail_Models = QueryGet_master_type_form_detail.get_master_type_form_detail(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id, "report");*/
            //====END====for======test=================

            if (!string.IsNullOrEmpty(activityId))
            {
                activity_TBMMKT_Model = ActivityFormTBMMKTCommandHandler.getDataForEditActivity(activityId);
                activity_TBMMKT_Model.activityFormTBMMKT.companyName = QueryGet_master_company.get_master_company(activity_TBMMKT_Model.activityFormTBMMKT.companyId).FirstOrDefault().companyNameTH;
                activity_TBMMKT_Model.activityFormTBMMKT.formName = QueryGet_master_type_form.get_master_type_form(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id).FirstOrDefault().nameForm;
                activity_TBMMKT_Model.activityFormTBMMKT.formNameEn = QueryGet_master_type_form.get_master_type_form(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id).FirstOrDefault().nameForm_EN;
                activity_TBMMKT_Model.master_Type_Form_Detail_Models = QueryGet_master_type_form_detail.get_master_type_form_detail(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id, "report");
                activity_TBMMKT_Model.activityFormTBMMKT.formCompanyId = QueryGet_master_type_form.get_master_type_form(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id).FirstOrDefault().companyId;
                activity_TBMMKT_Model.activityFormTBMMKT.chkUseEng = (activity_TBMMKT_Model.activityFormTBMMKT.languageDoc == ConfigurationManager.AppSettings["cultureEng"]);
              



                if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formTrvTbmId"] || activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formTrvHcmId"])//แบบฟอร์มเดินทางปฏิบัติงานนอกสถานที่
                {
                    ViewBag.classFont = "fontDocSmall";
                    ViewBag.padding = "paddingFormV2";
                }
                else
                {
                    ViewBag.classFont = "fontDocV1";
                    ViewBag.padding = "paddingFormV1";
                }

                //===ดึงผู้อนุมัติทั้งหมด=เพือเอาไปใช้แสดงในรายงาน===
                activity_TBMMKT_Model.approveFlowDetail = ActivityFormTBMMKTCommandHandler.get_flowApproveDetail(activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.SubjectId, activityId);
              //  activity_TBMMKT_Model.
                //=END==ดึงผู้อนุมัติทั้งหมด=เพือเอาไปใช้แสดงในรายงาน===
            }
            //return PartialView(activity_TBMMKT_Model);  //ไว้ใช้เวลาจะใส่กับ Modal            
            //return View(activity_TBMMKT_Model); // test
            return PartialView(activity_TBMMKT_Model);// production
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