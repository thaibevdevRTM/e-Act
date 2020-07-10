using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                string startDate = "";
                string endDate = "";
                string companyId = "";
                string department = "";
                string empId = "";
                string reportType = "";

                reportType = Request.Form["ddlReportType"];
                companyId = Request.Form["ddlCompany"];
                department = Request.Form["ddlDepartment"];
                startDate = DocumentsAppCode.convertDateTHToShowCultureDateTH(BaseAppCodes.converStrToDatetimeWithFormat(Request.Form["startDate"], ConfigurationManager.AppSettings["formatDateUse"]), ConfigurationManager.AppSettings["formatLongDate"]);
                endDate = DocumentsAppCode.convertDateTHToShowCultureDateTH(BaseAppCodes.converStrToDatetimeWithFormat(Request.Form["endDate"], ConfigurationManager.AppSettings["formatDateUse"]), ConfigurationManager.AppSettings["formatLongDate"]);

                MedReportHeader medReportHeader = new MedReportHeader();
                medReportHeader.reportTypeId = reportType;
                medReportHeader.reportTypeName = QueryGetReport.getReportTypeByTypeFormId(typeFormId).Where(x => x.id == reportType).FirstOrDefault().reportName;
                medReportHeader.companyName = ReportAppCode.getCompanyByRole(typeFormId).Where(x => x.companyId == companyId).FirstOrDefault().companyNameTH;
                medReportHeader.period = "วันที่ " + startDate + " ถึงวันที่ " + endDate;
                models.medReportHeader = medReportHeader;

                //List<MedIndividualDetail> models = new List<MedIndividualDetail>();

                if (reportType == AppCode.ReportType.MedIndividual)
                {
                    empId = Request.Form["ddlEmp"];
                }
                else
                {
                    empId = Request.Form["ddlMutiEmp"];
                }


                //RequestEmpModel empModel = new RequestEmpModel();


                List<RequestEmpModel> empModel = new List<RequestEmpModel>();

                empModel = QueryGet_empDetailById.getEmpDetailById(empId);

                models.empList = empModel;

                models.medIndividualDetail = QueryGetReport.getReportMedIndividualDetail(empId, typeFormId, Request.Form["startDate"], Request.Form["endDate"]);

                List<MedAllDetail> medAllDetail = new List<MedAllDetail>();
                medAllDetail.Add(new MedAllDetail() { activityNo = "", documentDate = "" });
                models.medAllDetail = medAllDetail;



            }

            return PartialView(models);
        }
    }
}