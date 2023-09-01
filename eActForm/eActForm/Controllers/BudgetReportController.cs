
using eActForm.BusinessLayer;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class BudgetReportController : Controller
    {

        public static SearchBudgetActivityModels getMasterDataForSearchBudgetActivityReportMTM(string TypeForm)
        {
            try
            {
                SearchBudgetActivityModels models = new SearchBudgetActivityModels();
                models.approveStatusList = ApproveAppCode.getApproveStatus(AppCode.StatusType.app);
                models.productGroupList = QueryGetAllProductGroup.getAllProductGroup().ToList();
                models.productTypelist = QuerygetAllProductCate.getAllProductType().ToList();
                models.budgetStstuslist = QueryGetBudgetActivity.getBudgetActivityStatus().ToList();
                models.budgetStstuslist = models.budgetStstuslist.Where(r => r.id != "4" && r.id != "5").ToList();
                models.activityYearlist = QueryGetBudgetActivity.getYearActivity().ToList();

                models.activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.activityCondition.Contains(Activity_Model.activityType.MT.ToString()))
                    .GroupBy(item => item.activitySales)
                    .Select(grp => new TB_Act_ActivityGroup_Model { id = grp.First().id, activitySales = grp.First().activitySales }).ToList();


                if (TypeForm == Activity_Model.activityType.MT.ToString())
                    { models.customerslist = QueryGetAllCustomers.getCustomersMT().ToList(); }
                else if (TypeForm == Activity_Model.activityType.OMT.ToString())
                    { models.customerslist = QueryGetAllCustomers.getCustomersOMT().ToList(); }



                return models;
            }
            catch (Exception ex)
            {
                throw new Exception("getMasterDataForSearch >>" + ex.Message);
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        //------------- report Pending Approve ----------------------------------------------------|
        public ActionResult reportBudgetWaitApproveIndex(string TypeForm)
        {
            SearchBudgetActivityModels models = getMasterDataForSearchBudgetActivityReportMTM(TypeForm);
            return View(models);
        }

        public ActionResult reportBudgetWaitApproveListView(string typeForm)
        {
            Budget_Report_WaitApprove_Model.Report_Budget_WaitApprove model = new Budget_Report_WaitApprove_Model.Report_Budget_WaitApprove();
            try
            {
                string customerId = null;
                string empId = null;
                string keyword = null;

                customerId = Request.Form["ddlCustomer"];
                empId = Request.Form["txtEmpId"];
                keyword = Request.Form["txtKeyword"];

                model.Report_Budget_WaitApprove_List = QueryGetBudgetReport.getReportBudgetWaitApprove(typeForm, customerId, empId, keyword);

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("reportBudgetWaitApproveListView => " + ex.Message);
            }
            return PartialView(model);
        }



        //------------- report budget invoice ----------------------------------------------------|
        public ActionResult reportBudgetInvoiceIndex(string TypeForm)
        {
            SearchBudgetActivityModels models = getMasterDataForSearchBudgetActivityReportMTM(TypeForm);
            return View(models);
        }

        public ActionResult reportBudgetInvoiceListView(string typeForm)
        {
            Budget_Report_Invoice_Model.Report_Budget_Invoice model = new Budget_Report_Invoice_Model.Report_Budget_Invoice();
            try
            {
                string company = null;
                string keyword = null;

                company = Request.Form["ddlFormType"];
                keyword = Request.Form["txtKeyword"];

                if (company != null)
                {

                DateTime dateStartDate = DateTime.ParseExact(Request.Form["startDate"].Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string startDate = dateStartDate.ToString("yyyyMMdd");

                DateTime dateEndDate = DateTime.ParseExact(Request.Form["endDate"].Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string endDate = dateEndDate.ToString("yyyyMMdd");

                model.Report_Budget_Invoice_List = QueryGetBudgetReport.getReportBudgetInvoice(company, startDate.ToString(), endDate.ToString(), keyword);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("reportBudgetInvoiceListView => " + ex.Message);
            }
            return PartialView(model);
        }


        //------------- report budget acctivity ----------------------------------------------------|
        public ActionResult reportBudgetActivityIndex(string TypeForm)
        {
            SearchBudgetActivityModels models = getMasterDataForSearchBudgetActivityReportMTM(TypeForm);
            return View(models);
        }
        public ActionResult reportBudgetActivityListView(string typeForm)
        {
            Budget_Report_Model.Report_Budget_Activity model = new Budget_Report_Model.Report_Budget_Activity();
            try
            {
                string startDate = null;
                string endDate = null;
                string actNo = null;
                string actStatus = null;
                string actProductType = null;
                string actYear = null;

                #region filter

                actYear = Request.Form["ddlActYear"];
                actNo = Request["txtActivityNo"] == null ? null : Request["txtActivityNo"];
                actStatus = Request["ddlStatus"] == null ? null : Request["ddlStatus"];

                if (String.IsNullOrEmpty(actYear) != true)
                {
                    model.Report_Budget_Activity_List = QueryGetBudgetReport.getReportBudgetActivity(actStatus, actNo, typeForm, actYear);
                }

                //----------------------------------------------

                if (String.IsNullOrEmpty(Request.Form["ddlCustomer"]) != true)
                {
                    model.Report_Budget_Activity_List = model.Report_Budget_Activity_List.Where(r => r.cus_id == Request.Form["ddlCustomer"]).ToList();
                }

                if (String.IsNullOrEmpty(Request.Form["ddlTheme"]) != true)
                {
                    model.Report_Budget_Activity_List = model.Report_Budget_Activity_List.Where(r => r.themeId == Request.Form["ddlTheme"]).ToList();
                }

                if (String.IsNullOrEmpty(Request.Form["ddlProductType"]) != true && Request.Form["ddlProductType"] != ",")
                {
                    actProductType = Request["ddlProductType"];
                    actProductType = actProductType.Replace(",", "");
                    model.Report_Budget_Activity_List = model.Report_Budget_Activity_List.Where(r => r.prd_typeId == actProductType).ToList();
                }

                if (string.IsNullOrEmpty(Request.Form["ddlProductGrp"]) != true)
                {
                    model.Report_Budget_Activity_List = model.Report_Budget_Activity_List.Where(r => r.prd_groupId == Request.Form["ddlProductGrp"]).ToList();
                }

                if (string.IsNullOrEmpty(Request.Form["ddlBudgetStatus"]) != true)
                {
                    model.Report_Budget_Activity_List = model.Report_Budget_Activity_List.Where(r => r.productBudgetStatusGroupId == Request.Form["ddlBudgetStatus"]).ToList();
                }


                #endregion
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("searchRptBudgetActivity => " + ex.Message);
            }
            return PartialView(model);
        }

        
        //------------- report budget product price ------------------------------------------------|
        public ActionResult reportBudgetProductPriceIndex(string TypeForm)
        {
            SearchBudgetActivityModels models = getMasterDataForSearchBudgetActivityReportMTM(TypeForm);
            return View(models);
        }
        public ActionResult reportBudgetProductPriceListView(string typeForm)
        {
            Budget_Report_Product_Price_Model.Report_Budget_Product_Price model = new Budget_Report_Product_Price_Model.Report_Budget_Product_Price();
            try
            {
                string customer_id = null;
                string product_type_id = null;
                string keyword = null;

                customer_id = Request.Form["ddlCustomer"];
                product_type_id = Request.Form["ddlProductType"];
                keyword = Request.Form["txtKeyword"];

                if (customer_id == "") { customer_id = null; }
                if (product_type_id == "") { product_type_id = null; }
                if (keyword == "") { keyword = null; }

                model.Report_Budget_Product_Price_List = QueryGetBudgetReport.getReportBudgetProductPrice(typeForm, customer_id, product_type_id, keyword);
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("reportBudgetProductPriceListView => " + ex.Message);
            }
            return PartialView(model);
        }


        //------------- report account SE ---------------------------------------------------------|
        public ActionResult reportBudgetAccSeIndex(string TypeForm)
        {
            SearchBudgetActivityModels models = getMasterDataForSearchBudgetActivityReportMTM(TypeForm);
            return View(models);
        }
        public ActionResult reportBudgetAccSeListView(string typeForm)
        {
            Budget_Report_Model.Report_Budget_Activity model = new Budget_Report_Model.Report_Budget_Activity();
            try
            {
                string startDate = null;
                string endDate = null;
                string actNo = null;
                string actStatus = null;
                string actProductType = null;
                string actYear = null;
                #region filter

                actYear = Request.Form["ddlActYear"];
                actNo = Request["txtActivityNo"] == null ? null : Request["txtActivityNo"];
                actStatus = Request["ddlStatus"] == null ? null : Request["ddlStatus"];

                if (String.IsNullOrEmpty(actYear) != true)
                {
                    model.Report_Budget_Activity_List = QueryGetBudgetReport.getReportBudgetActivity(actStatus, actNo, typeForm, actYear);
                }

                //----------------------------------------------

                if (String.IsNullOrEmpty(Request.Form["ddlCustomer"]) != true)
                {
                    model.Report_Budget_Activity_List = model.Report_Budget_Activity_List.Where(r => r.cus_id == Request.Form["ddlCustomer"]).ToList();
                }

                if (String.IsNullOrEmpty(Request.Form["ddlTheme"]) != true)
                {
                    model.Report_Budget_Activity_List = model.Report_Budget_Activity_List.Where(r => r.act_theme == Request.Form["ddlTheme"]).ToList();
                }

                if (String.IsNullOrEmpty(Request.Form["ddlProductType"]) != true && Request.Form["ddlProductType"] != ",")
                {
                    actProductType = Request["ddlProductType"];
                    actProductType = actProductType.Replace(",", "");
                    model.Report_Budget_Activity_List = model.Report_Budget_Activity_List.Where(r => r.prd_typeId == actProductType).ToList();
                }

                if (string.IsNullOrEmpty(Request.Form["ddlProductGrp"]) != true)
                {
                    model.Report_Budget_Activity_List = model.Report_Budget_Activity_List.Where(r => r.prd_groupId == Request.Form["ddlProductGrp"]).ToList();
                }

                if (string.IsNullOrEmpty(Request.Form["ddlBudgetStatus"]) != true)
                {
                    model.Report_Budget_Activity_List = model.Report_Budget_Activity_List.Where(r => r.productBudgetStatusGroupId == Request.Form["ddlBudgetStatus"]).ToList();
                }

                if (UtilsAppCode.Session.User.isAdminOMT == false && UtilsAppCode.Session.User.isAdmin == false && UtilsAppCode.Session.User.isSuperAdmin == false)
                {
                    model.Report_Budget_Activity_List = model.Report_Budget_Activity_List.Where(r => r.actForm_CreatedByUserId == UtilsAppCode.Session.User.empId).ToList();
                }
                #endregion
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("searchRptBudgetActivity => " + ex.Message);
            }
            return PartialView(model);
        }


        //------------- export to excel ------------------------------------------------------------|
        [HttpPost]
        [ValidateInput(false)]
        public FileResult viewExportExcel(string gridHtml)
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

            string createDate = "";
            createDate = DateTime.Today.ToString("yyyyMMdd");
            return File(Encoding.UTF8.GetBytes(gridHtml), "application/vnd.ms-excel", "BudgetReport_" + createDate + ".xls");
        }

    } //end public class

} // end namespace