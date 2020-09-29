using eActForm.BusinessLayer;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.BusinessLayer.Appcodes;
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

            if (!string.IsNullOrEmpty(activityId))
            {
                activity_TBMMKT_Model = ActivityFormTBMMKTCommandHandler.getDataForEditActivity(activityId);
                activity_TBMMKT_Model.activityFormTBMMKT.companyName = QueryGet_master_company.get_master_company(activity_TBMMKT_Model.activityFormTBMMKT.companyId).FirstOrDefault().companyNameTH;
                activity_TBMMKT_Model.activityFormTBMMKT.formName = QueryGet_master_type_form.get_master_type_form(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id).FirstOrDefault().nameForm;
                activity_TBMMKT_Model.activityFormTBMMKT.formNameEn = QueryGet_master_type_form.get_master_type_form(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id).FirstOrDefault().nameForm_EN;

                activity_TBMMKT_Model.master_Type_Form_Detail_Models = QueryGet_master_type_form_detail.get_master_type_form_detail(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id, "report");
                activity_TBMMKT_Model.activityFormTBMMKT.formCompanyId = QueryGet_master_type_form.get_master_type_form(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id).FirstOrDefault().companyId;
                activity_TBMMKT_Model.activityFormTBMMKT.chkUseEng = (activity_TBMMKT_Model.activityFormTBMMKT.languageDoc == ConfigurationManager.AppSettings["cultureEng"]);

                if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formTrvTbmId"]
                    || activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formTrvHcmId"]
                    || activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formExpTrvNumId"]
                    // (AppCode.hcForm.Contains(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id))
                    )//แบบฟอร์มเดินทางปฏิบัติงานนอกสถานที่
                {
                    ViewBag.classFont = "fontDocSmall";
                    ViewBag.padding = "paddingFormV2";
                }
                else if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formBgTbmId"])//แบบฟอร์มอนุมัติจัดกิจกรรม dev date 20200313
                {
                    ViewBag.classFont = "formBorderStyle1";
                    ViewBag.padding = "paddingFormV3";
                }
                else if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPaymentVoucherTbmId"])//ใบสั่งจ่ายTBM dev date 20200420 peerapop
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


                //===ดึงผู้อนุมัติทั้งหมด=เพือเอาไปใช้แสดงในรายงาน===
                activity_TBMMKT_Model.approveFlowDetail = ActivityFormTBMMKTCommandHandler.get_flowApproveDetail(activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.SubjectId, activityId);
                if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formCR_IT_FRM_314"])
                {
                    activity_TBMMKT_Model.approveModels = ApproveAppCode.getApproveByActFormId(activityId);
                    //BaseAppCodes.WriteSignatureToDisk(activity_TBMMKT_Model.approveModels, activityId);
                }
                //=END==ดึงผู้อนุมัติทั้งหมด=เพือเอาไปใช้แสดงในรายงาน===
            }

            //===========Set Language By Document Dev date 20200310 Peerapop=====================
            //ไม่ต้องไปกังวลว่าภาษาหลักของWebที่Userใช้งานอยู่จะมีปัญหาเพราะ _ViewStart จะเปลี่ยนภาษาปัจจุบันที่Userใช้เว็บปรับCultureกลับให้เอง
            DocumentsAppCode.setCulture(activity_TBMMKT_Model.activityFormModel.languageDoc);
            //====END=======Set Language By Document Dev date 20200310 Peerapop==================

            return PartialView(activity_TBMMKT_Model);// production
        }

        public ActionResult ReportPettyCashNum(string activityId, Activity_TBMMKT_Model activity_TBMMKT_Model)
        {

            //=============for test=====================
            //activityId = "d57d1303-ded1-4927-bd31-4d9f85dfabe4";
            //activityId = "0a8517fb-0bc1-4c63-a545-718af4b9095c";
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


            CostDetailOfGroupPriceTBMMKT modelResult = new CostDetailOfGroupPriceTBMMKT
            {
                costDetailLists = new List<CostThemeDetailOfGroupByPriceTBMMKT>()
            };

            CostDetailOfGroupPriceTBMMKT model2 = new CostDetailOfGroupPriceTBMMKT
            {
                costDetailLists = new List<CostThemeDetailOfGroupByPriceTBMMKT>()
            };

            #region "ดึงข้อมูล GL "
            //ฟอร์มที่ใช้เป็นของ saleSupport
            List<GetDataGL> lstGL = new List<GetDataGL>();
            lstGL = QueryGetGL.getGLMasterByDivisionId(AppCode.Division.salesSupport);
            #endregion

            if (activity_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formExpTrvNumId"])
            {
                decimal? vat = 0,vatsum=0;
                #region "ค่าเดินทางของ NUM"
                
                model2.costDetailLists = QueryGetActivityEstimateByActivityId.getWithListChoice(activity_TBMMKT_Model.activityFormModel.id, activity_TBMMKT_Model.activityFormModel.master_type_form_id, AppCode.GLType.GLSaleSupport);
                for (int i = 0; i < model2.costDetailLists.Count; i++)
                {
                    if (model2.costDetailLists[i].total != 0 && model2.costDetailLists[i].listChoiceId != AppCode.Expenses.Allowance)
                    {
                        //vat แสดงรวมค่าใช้จ่ายอื่นๆแสดงแยก

                        if (model2.costDetailLists[i].listChoiceId == AppCode.Expenses.hotelExpense && model2.costDetailLists[i].unitPrice == 0)
                        {
                            vat = model2.costDetailLists[i].vat;
                          //  vatsum += model2.costDetailLists[i].vat;

                        }
                        else
                        {
                            vat = (model2.costDetailLists[i].vat * model2.costDetailLists[i].unit);
                          //  vatsum += (model2.costDetailLists[i].vat * model2.costDetailLists[i].unit);
                        }
                        vatsum += vat;

                        modelResult.costDetailLists.Add(new CostThemeDetailOfGroupByPriceTBMMKT()
                        {
                            listChoiceId = model2.costDetailLists[i].listChoiceId,
                            listChoiceName = model2.costDetailLists[i].listChoiceName,
                            productDetail = model2.costDetailLists[i].listChoiceName + " " +
                           (model2.costDetailLists[i].displayType == AppCode.CodeHtml.LabelHtml
                           ? model2.costDetailLists[i].unit + "วัน (สิทธิเบิก " + model2.costDetailLists[i].productDetail + " บาท/วัน)"
                           : model2.costDetailLists[i].productDetail),
                            total = model2.costDetailLists[i].total - (vat),
                            //glCode = lstGL.Where(x => x.groupGL.Contains(model2.costDetailLists[i].listChoiceName) ).FirstOrDefault()?.GL,
                            glCode =  QueryGetGL.getGL(lstGL, model2.costDetailLists[i].glCodeId, activity_TBMMKT_Model.activityFormModel.empId) //lstGL.Where(x => x.id == model2.costDetailLists[i].glCodeId).FirstOrDefault()?.GL,
                        });
                    }
                }
                if (vatsum > 0)
                {
                    modelResult.costDetailLists.Add(new CostThemeDetailOfGroupByPriceTBMMKT()
                    {
                        listChoiceId = "",
                        listChoiceName = "vat",
                        productDetail = lstGL.Where(x => x.GL == AppCode.GLVat.gl).FirstOrDefault()?.groupGL,
                        total = vatsum,
                        glCode = lstGL.Where(x => x.GL == AppCode.GLVat.gl).FirstOrDefault()?.GL,
                    });
                }

                activity_Model.totalCostThisActivity -= model2.costDetailLists.Where(X => X.listChoiceId == AppCode.Expenses.Allowance).FirstOrDefault().total;
                #endregion
            }
            else if (activity_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formExpMedNumId"])
            {
                modelResult.costDetailLists.Add(new CostThemeDetailOfGroupByPriceTBMMKT()
                {
                    listChoiceId = "",
                    listChoiceName = "",
                    productDetail = "ค่ารักษาพยาบาล",
                    total = activity_Model.tB_Act_ActivityForm_DetailOther.amountReceived,
                    displayType = "",
                    glCode = QueryGetGL.getGL(lstGL, AppCode.SSGLId.medical, activity_TBMMKT_Model.activityFormModel.empId)//lstGL.Where(x => AppCode.SSGLId.medical.Contains(x.id)).FirstOrDefault()?.GL,//glCode = lstGL.Where(x => x.id == ).FirstOrDefault()?.GL,
                });
                activity_Model.totalCostThisActivity = activity_Model.tB_Act_ActivityForm_DetailOther.amountReceived;

            }
            
            int rowAdd = 8 - modelResult.costDetailLists.Count;
            for (int i = 0; i < rowAdd; i++)
            {
                modelResult.costDetailLists.Add(new CostThemeDetailOfGroupByPriceTBMMKT()
                {
                    listChoiceId = "",
                    listChoiceName = "",
                    productDetail = "",
                    total = 0,
                    displayType = "",
                    glCode = "",
                });
            }

            modelResult.costDetailLists = modelResult.costDetailLists.ToList();
            activity_Model.expensesDetailModel = modelResult;



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

        public ActionResult ReportIndex(string typeForm, string typeFormId)
        {


            SearchReportModels models = new SearchReportModels();
            models.formDetail = QueryGet_master_type_form_detail.get_master_type_form_detail(typeFormId, "rptExport");

            //models.reportTypeList = QueryGetReport.getReportTypeByTypeFormId(typeFormId);
            //models.companyList = ReportAppCode.getCompanyByRole(typeFormId);

            //List<eForms.Models.MasterData.departmentMasterModel> departList = new List<eForms.Models.MasterData.departmentMasterModel>();
            //departList.Add(new eForms.Models.MasterData.departmentMasterModel() { id = "", name = "", companyId = "" });
            //models.departmentList = departList;

            //List<RequestEmpModel> empList = new List<RequestEmpModel>();
            //empList.Add(new RequestEmpModel() { empId = "", empName = "", departmentEN = "" });
            //models.empList = empList;
            return PartialView(models);
        }

    }
}