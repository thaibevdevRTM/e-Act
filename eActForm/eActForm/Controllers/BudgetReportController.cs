
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using eActForm.BusinessLayer;
using eActForm.Models;
using iTextSharp.text;
using WebLibrary;
using eActForm.BusinessLayer.QueryHandler;

namespace eActForm.Controllers
{
	[LoginExpire]
	public class BudgetReportController : Controller
    {

		public static SearchBudgetActivityModels getMasterDataForSearch(string TypeForm)
		{
			try
			{
				SearchBudgetActivityModels models = new SearchBudgetActivityModels();
				models.approveStatusList = ApproveAppCode.getApproveStatus(AppCode.StatusType.app);
				models.productGroupList = QueryGetAllProductGroup.getAllProductGroup().ToList();
				models.productTypelist = QuerygetAllProductCate.getAllProductType().ToList();
				models.budgetStstuslist = QueryGetBudgetActivity.getBudgetActivityStatus().ToList();
				models.budgetStstuslist = models.budgetStstuslist.Where(r => r.id != "4" && r.id != "5").ToList();

				models.activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup()
				.GroupBy(item => item.activitySales)
				.Select(grp => new TB_Act_ActivityGroup_Model { id = grp.First().id, activitySales = grp.First().activitySales }).ToList();

				if (TypeForm == Activity_Model.activityType.MT.ToString())
				{
					models.customerslist = QueryGetAllCustomers.getCustomersMT().ToList();
				}
				else if (TypeForm == Activity_Model.activityType.OMT.ToString())
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

		public ActionResult Index()
		{
			return View();
		}
		
		//------------- report account SE ---------------------------------------------------------|
		public ActionResult reportBudgetAccSeIndex(string TypeForm)
		{
			SearchBudgetActivityModels models = getMasterDataForSearch(TypeForm);
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

				#region filter

				if (Request.Form["chk_all"] != null && Request.Form["chk_all"] == "true")
				{
					startDate = Request.Form["startDate"];
					endDate = Request.Form["endDate"];
				}

				actNo = Request["txtActivityNo"] == null ? null : Request["txtActivityNo"];
				actStatus = Request["ddlStatus"] == null ? null : Request["ddlStatus"];


				model.Report_Budget_Activity_List = QueryGetBudgetReport.getReportBudgetActivity(actStatus, actNo, typeForm, startDate, endDate);

				//----------------------------------------------

				if (String.IsNullOrEmpty(Request.Form["ddlCustomer"]) != true)
				{
					model.Report_Budget_Activity_List = model.Report_Budget_Activity_List.Where(r => r.cus_id == Request.Form["ddlCustomer"]).ToList();
				}

				if (String.IsNullOrEmpty(Request.Form["ddlTheme"]) != true)
				{
					model.Report_Budget_Activity_List = model.Report_Budget_Activity_List.Where(r => r.Theme == Request.Form["ddlTheme"]).ToList();
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

		//------------- report budget acctivity ----------------------------------------------------|
		public ActionResult reportBudgetActivityIndex(string TypeForm)
        {
			SearchBudgetActivityModels models = getMasterDataForSearch(TypeForm);
			return View(models);
        }

		public ActionResult reportBudgetActivityListView(string typeForm)
		{
			Budget_Report_Model.Report_Budget_Activity model = new Budget_Report_Model.Report_Budget_Activity();
			try
			{
				string startDate = null ;
				string endDate = null;
				string actNo = null;
				string actStatus = null;
				string actProductType = null;

				#region filter

				if (Request.Form["chk_all"] == null || Request.Form["chk_all"] == "false")
				{
					startDate = Request.Form["startDate"];
					endDate = Request.Form["endDate"];

					if (Request.Form["startDate"] == null) { startDate = DateTime.Today.AddDays(-45).ToString("MM/dd/yyyy"); }
					if (Request.Form["endDate"] == null) { endDate = DateTime.Today.ToString("MM/dd/yyyy"); }

				}

				actNo = Request["txtActivityNo"] == null ? null : Request["txtActivityNo"];
				actStatus = Request["ddlStatus"] == null ? null : Request["ddlStatus"];
				

				model.Report_Budget_Activity_List = QueryGetBudgetReport.getReportBudgetActivity(actStatus, actNo, typeForm, startDate, endDate);

				//----------------------------------------------

				if (String.IsNullOrEmpty(Request.Form["ddlCustomer"]) != true)
				{
					model.Report_Budget_Activity_List = model.Report_Budget_Activity_List.Where(r => r.cus_id == Request.Form["ddlCustomer"]).ToList();
				}

				if (String.IsNullOrEmpty(Request.Form["ddlTheme"]) != true)
				{
					model.Report_Budget_Activity_List = model.Report_Budget_Activity_List.Where(r => r.themeId == Request.Form["ddlTheme"]).ToList();
				}

				if (String.IsNullOrEmpty( Request.Form["ddlProductType"]) != true && Request.Form["ddlProductType"] != "," )
				{
					actProductType = Request["ddlProductType"];
					actProductType = actProductType.Replace(",", "");
					model.Report_Budget_Activity_List = model.Report_Budget_Activity_List.Where(r => r.prd_typeId == actProductType).ToList();
				}

				if (string.IsNullOrEmpty(Request.Form["ddlProductGrp"]) != true )
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