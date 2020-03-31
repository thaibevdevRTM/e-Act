using eActForm.BusinessLayer;
using eActForm.Models;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
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
            ActivityFormTBMMKT dummy_activityFormTBMMKT = new ActivityFormTBMMKT();
            activity_TBMMKT_Model.activityFormTBMMKT = dummy_activityFormTBMMKT;
            activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id = ConfigurationManager.AppSettings["formPaymentVoucherTbmId"];
            activity_TBMMKT_Model.master_Type_Form_Detail_Models = QueryGet_master_type_form_detail.get_master_type_form_detail(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id, "report");
            ViewBag.classFont = "formBorderStyle2";
            ViewBag.padding = "paddingFormV3";
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
                else if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formBgTbmId"])//แบบฟอร์มอนุมัติจัดกิจกรรม dev date 20200313
                {
                    ViewBag.classFont = "formBorderStyle1";
                    ViewBag.padding = "paddingFormV3";
                }
                else
                {
                    ViewBag.classFont = "fontDocV1";
                    ViewBag.padding = "paddingFormV1";
                }


                //===ดึงผู้อนุมัติทั้งหมด=เพือเอาไปใช้แสดงในรายงาน===
                activity_TBMMKT_Model.approveFlowDetail = ActivityFormTBMMKTCommandHandler.get_flowApproveDetail(activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.SubjectId, activityId);
                //=END==ดึงผู้อนุมัติทั้งหมด=เพือเอาไปใช้แสดงในรายงาน===
            }

            //===========Set Language By Document Dev date 20200310 Peerapop=====================
            //ไม่ต้องไปกังวลว่าภาษาหลักของWebที่Userใช้งานอยู่จะมีปัญหาเพราะ _ViewStart จะเปลี่ยนภาษาปัจจุบันที่Userใช้เว็บปรับCultureกลับให้เอง
            DocumentsAppCode.setCulture(activity_TBMMKT_Model.activityFormModel.languageDoc);
            //====END=======Set Language By Document Dev date 20200310 Peerapop==================

            //return PartialView(activity_TBMMKT_Model);  //ไว้ใช้เวลาจะใส่กับ Modal            
            //return View(activity_TBMMKT_Model); // test
            return PartialView(activity_TBMMKT_Model);// production
        }

        public ActionResult ReportPettyCashNum(string activityId, Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            //=============for test=====================
            activityId = "d57d1303-ded1-4927-bd31-4d9f85dfabe4";
            //==END===========for test=====================
            Activity_TBMMKT_Model activity_Model = new Activity_TBMMKT_Model();
            ActivityFormTBMMKT activityFormTBMMKT = new ActivityFormTBMMKT();
            if (activity_TBMMKT_Model.activityFormTBMMKT.id != null)
            {
                activity_Model = activity_TBMMKT_Model;
                activityId = activity_Model.activityFormModel.id;
            }
            else
            {
                activity_Model = ActivityFormTBMMKTCommandHandler.getDataForEditActivity(activityId);
                activity_Model.activityFormTBMMKT.companyName = QueryGet_master_company.get_master_company(activity_Model.activityFormTBMMKT.companyId).FirstOrDefault().companyNameTH;
                activity_Model.activityFormTBMMKT.chkUseEng = (activity_Model.activityFormTBMMKT.languageDoc == ConfigurationManager.AppSettings["cultureEng"]);
                //===ดึงผู้อนุมัติทั้งหมด=เพือเอาไปใช้แสดงในรายงาน===
                activity_Model.approveFlowDetail = ActivityFormTBMMKTCommandHandler.get_flowApproveDetail(activity_Model.tB_Act_ActivityForm_DetailOther.SubjectId, activityId);
                //=END==ดึงผู้อนุมัติทั้งหมด=เพือเอาไปใช้แสดงในรายงาน===
            }

            activity_Model.approveModels = ApproveAppCode.getApproveByActFormId(activityId);
            activity_Model.activityFormTBMMKT.formName = QueryGet_master_type_form.get_master_type_form(ConfigurationManager.AppSettings["formReportPettyCashNum"]).FirstOrDefault().nameForm;
            activity_Model.activityFormTBMMKT.formNameEn = QueryGet_master_type_form.get_master_type_form(ConfigurationManager.AppSettings["formReportPettyCashNum"]).FirstOrDefault().nameForm_EN;

            //===========Set Language By Document Dev date 20200310 Peerapop=====================
            //ไม่ต้องไปกังวลว่าภาษาหลักของWebที่Userใช้งานอยู่จะมีปัญหาเพราะ _ViewStart จะเปลี่ยนภาษาปัจจุบันที่Userใช้เว็บปรับCultureกลับให้เอง
            DocumentsAppCode.setCulture(activity_Model.activityFormModel.languageDoc);
            //====END=======Set Language By Document Dev date 20200310 Peerapop==================

            //return View(activity_Model); // test
            return PartialView(activity_Model);// production
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