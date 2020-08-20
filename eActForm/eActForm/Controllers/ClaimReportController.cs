using eActForm.BusinessLayer;
using eActForm.Models;
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    public class ClaimReportController : Controller
    {
        // GET: ClaimReport
        public ActionResult claimReportIndex(string typeForm)
        {
            SearchBudgetActivityModels models = getMasterDataForSearch(typeForm);
            return View(models);
        }


        public static SearchBudgetActivityModels getMasterDataForSearch(string typeForm)
        {
            try
            {
                SearchBudgetActivityModels models = new SearchBudgetActivityModels();
                //models.productGroupList = QueryGetAllProductGroup.getAllProductGroup().ToList();
                //models.productTypelist = QuerygetAllProductCate.getAllProductType().ToList();
                models.approveStatusList = ApproveAppCode.getApproveStatus(AppCode.StatusType.app);
                models.activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup()
                .GroupBy(item => item.activitySales)
                .Select(grp => new TB_Act_ActivityGroup_Model { id = grp.First().id, activitySales = grp.First().activitySales }).ToList();

                if (typeForm == Activity_Model.activityType.MT.ToString())
                {
                    models.customerslist = QueryGetAllCustomers.getCustomersMT().ToList();
                }
                else if (typeForm == Activity_Model.activityType.OMT.ToString())
                {
                    models.customerslist = QueryGetAllCustomers.getCustomersOMT().ToList();
                }

                return models;
            }
            catch (Exception ex)
            {
                throw new Exception("getMasterDataForSearch >>" + ex.Message);
            }
        }


        public ActionResult claimReportList(string typeForm)
        {
            string count = Request.Form.AllKeys.Count().ToString();
            Claim_Report_Model models = new Claim_Report_Model();

            try
            {
                if (TempData["searchClaimActivity"] != null)
                {
                    models = (Claim_Report_Model)TempData["searchClaimActivity"];
                }
                else
                {
                    string act_companyEn = typeForm;
                    string act_activityNo = Request["txtActivityNo"] == "" ? null : Request["txtActivityNo"];
                    string act_createdEmpId = UtilsAppCode.Session.User.empId;
                    string act_claimStatus = null;


                    DateTime startDate = Request["startDate"] == null ? DateTime.Now.AddDays(-120) : DateTime.ParseExact(Request.Form["startDate"], "MM/dd/yyyy", null);
                    DateTime endDate = Request["endDate"] == null ? DateTime.Now : DateTime.ParseExact(Request.Form["endDate"], "MM/dd/yyyy", null);

                    if (UtilsAppCode.Session.User.isSuperAdmin || UtilsAppCode.Session.User.isAdmin || UtilsAppCode.Session.User.isAdminOMT)
                    {
                        act_createdEmpId = null;
                    }

                    if (Request.Form["chk_all"] != null && Request.Form["chk_all"] == "true")
                    {
                        startDate = DateTime.Now.AddYears(-30);
                        endDate = DateTime.Now.AddDays(1);
                    }

                    if (String.IsNullOrEmpty(Request.Form["ddlClaimStatus"]) != true)
                    {
                        act_claimStatus = Request["ddlClaimStatus"];
                        if (act_claimStatus == "0") { act_claimStatus = null; }
                        if (act_claimStatus == "1") { act_claimStatus = "Y"; }
                        if (act_claimStatus == "2") { act_claimStatus = "N"; }
                    }


                    models.Claim_Activity_List = QueryGetBudgetActivity.getClaimActivityList(act_companyEn, act_activityNo, act_createdEmpId, act_claimStatus, null, startDate, endDate).ToList();

                    if (String.IsNullOrEmpty(Request.Form["ddlCustomer"]) != true)
                    {
                        models.Claim_Activity_List = models.Claim_Activity_List.Where(r => r.cus_cusId == Request.Form["ddlCustomer"]).ToList();
                    }

                    if (String.IsNullOrEmpty(Request.Form["ddlTheme"]) != true)
                    {
                        models.Claim_Activity_List = models.Claim_Activity_List.Where(r => r.prd_themeId == Request.Form["ddlTheme"]).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("claimReportList --> " + ex.Message);
            }
            TempData["searchClaimActivity"] = null;
            return PartialView(models);
        }

        [HttpPost]
        [ValidateInput(false)]
        public FileResult claimExportExcel(string gridHtml)
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

            string createDate = DateTime.Today.ToString("yyyyMMdd");
            return File(Encoding.UTF8.GetBytes(gridHtml), "application/vnd.ms-excel", "ClaimReport_" + createDate + ".xls");
        }

    }
}