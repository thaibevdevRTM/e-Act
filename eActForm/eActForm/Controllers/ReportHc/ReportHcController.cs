using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Controllers;
using eActForm.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers.ReportHc
{
    public class ReportHcController : Controller
    {
        // GET: HcReport
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult mainSearchRpt(string typeForm, string typeFormId)
        {

            SearchReportModels models = new SearchReportModels();

            //models.formDetail = QueryGet_master_type_form_detail.get_master_type_form_detail(typeFormId, "rptExport");

            models.reportTypeList = QueryGetReport.getReportTypeByTypeFormId(typeFormId);
            models.companyList = ReportAppCode.getCompanyByRole(typeFormId);

            List<eForms.Models.MasterData.departmentMasterModel> departList = new List<eForms.Models.MasterData.departmentMasterModel>();
            departList.Add(new eForms.Models.MasterData.departmentMasterModel() { id = "", name = "", companyId = "" });
            models.departmentList = departList;

            List<RequestEmpModel> empList = new List<RequestEmpModel>();
            empList.Add(new RequestEmpModel() { empId = "", empName = "", departmentEN = "" });
            models.empList = empList;
            return PartialView(models);
        }
        public ActionResult listMedRpt(string typeForm, string typeFormId)
        {

            MedReportDetail models = new MedReportDetail();

            if (Request.Form["ddlReportType"] != null)
            {
                DateTime? startDate;
                DateTime? endDate;
                string companyId = "";
                string department = "";
                string empId = "";
                string reportType = "";

                reportType = Request.Form["ddlReportType"];
                companyId = Request.Form["ddlCompany"];
                department = Request.Form["ddlDepartment"];

                startDate = BaseAppCodes.converStrToDatetimeWithFormat(Request.Form["startDate"], ConfigurationManager.AppSettings["formatDateUse"]);
                endDate = BaseAppCodes.converStrToDatetimeWithFormat(Request.Form["endDate"], ConfigurationManager.AppSettings["formatDateUse"]);

                //startDate = DocumentsAppCode.convertDateTHToShowCultureDateTH(BaseAppCodes.converStrToDatetimeWithFormat(Request.Form["startDate"], ConfigurationManager.AppSettings["formatDateUse"]), ConfigurationManager.AppSettings["formatLongDate"]);
                //endDate = DocumentsAppCode.convertDateTHToShowCultureDateTH(BaseAppCodes.converStrToDatetimeWithFormat(Request.Form["endDate"], ConfigurationManager.AppSettings["formatDateUse"]), ConfigurationManager.AppSettings["formatLongDate"]);

                MedReportHeader medReportHeader = new MedReportHeader();
                medReportHeader.reportTypeId = reportType;
                medReportHeader.reportTypeName = QueryGetReport.getReportTypeByTypeFormId(typeFormId).Where(x => x.id == reportType).FirstOrDefault().reportName;
                medReportHeader.companyName = ReportAppCode.getCompanyByRole(typeFormId).Where(x => x.companyId == companyId).FirstOrDefault().companyNameTH;
                medReportHeader.period = "วันที่ " + DocumentsAppCode.convertDateTHToShowCultureDateTH(startDate, ConfigurationManager.AppSettings["formatLongDate"])
                + " ถึงวันที่ " + DocumentsAppCode.convertDateTHToShowCultureDateTH(endDate, ConfigurationManager.AppSettings["formatLongDate"]);

                models.medReportHeader = medReportHeader;

                //List<MedIndividualDetail> models = new List<MedIndividualDetail>();
                List<RequestEmpModel> empModel = new List<RequestEmpModel>();
                if (reportType == AppCode.ReportType.MedIndividual)
                {
                    empId = Request.Form["ddlEmp"];

                    empModel = QueryGet_empDetailById.getEmpDetailById(empId);
                    models.empList = empModel;
                    models.medIndividualDetail = QueryGetReport.getReportMedIndividualDetail(empId, typeFormId, startDate, endDate);
                    if (models.medIndividualDetail.Where(x => x.amountByDetail == 0).Count() > 0)
                    {
                        //ลบกรณีไม่เคยเบิกนอกระบบ
                        var itemToRemove = models.medIndividualDetail.Remove(models.medIndividualDetail.Single(x => x.amountByDetail == 0));
                    }
                    if (models.medIndividualDetail.Where(x => x.activityNo == "").Count() > 0 && models.medIndividualDetail.Count() == 1)
                    {
                        decimal? cashPerDay = 0;
                        //กรณีมีนอกระบบ อย่างเดียว
                        List<CashEmpModel> cashEmpList = new List<CashEmpModel>();
                        cashEmpList = QueryGetBenefit.getCashLimitByTypeId(@AppCode.Expenses.Medical, BaseAppCodes.getEmpFromApi(empId).empProbationEndDate, empModel[0].level).ToList();
                        if (cashEmpList.Count > 0)
                        { cashPerDay = cashEmpList[0].cashPerDay; }

                        models.medIndividualDetail.Where(x => x.activityNo == "").Select(c =>
                        {
                            c.treatmentDate = "-";
                            c.amountLimit = cashPerDay;
                            c.unitPrice = c.amountByDetail;
                            c.typeName = "-";
                            c.hospNameTH = "-";
                            c.detail = "วงเงินที่ใช้ไปก่อนเข้าระบบ"; return c;
                        }).ToList();
                    }
                    else
                    {
                        //มีนอกและมีในระบบ
                        models.medIndividualDetail.Where(x => x.activityNo == "").Select(c =>
                        {
                            c.treatmentDate = "-";
                            c.amountLimit = models.medIndividualDetail[1].amountLimit;
                            c.unitPrice = c.amountByDetail;
                            c.typeName = "-";
                            c.hospNameTH = "-";
                            c.detail = "วงเงินที่ใช้ไปก่อนเข้าระบบ"; return c;
                        }).ToList();
                    }
                }
                else
                {
                    // empId = Request.Form["ddlMutiEmp"];
                    //  List<MedAllDetail> medAllDetail = new List<MedAllDetail>();
                    //medAllDetail.Add(new MedAllDetail() { activityNo = "", documentDate = "" });
                    //models.medAllDetail = medAllDetail;
                    models.medAllDetail = QueryGetReport.getReportMedAllDetail(companyId, department, typeFormId, startDate, endDate);
                }
            }

            return PartialView(models);
        }

        [HttpPost]
        [ValidateInput(false)]
        public FileResult repExportExcel(string gridHtml)
        {
            try
            {
                //RepDetailModel.actFormRepDetails model = (RepDetailModel.actFormRepDetails)Session["ActFormRepDetail"] ?? new RepDetailModel.actFormRepDetails();
                //gridHtml = gridHtml.Replace("\n", "<br>");
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }

            return File(Encoding.UTF8.GetBytes(gridHtml), "application/vnd.ms-excel", "DetailReport.xls");
        }
    }
}