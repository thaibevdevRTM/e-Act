using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using eActForm.BusinessLayer;
using eActForm.Models;
using iTextSharp.text;
using System.IO;
using System.Configuration;
using System.Web.Mvc;
using WebLibrary;
namespace eActForm.Controllers
{
	[LoginExpire]
	public class BudgetMyDocController : Controller
    {
        // GET: BudgetMyDoc
        
		public ActionResult Index()
		{
			SearchActivityModels models = SearchAppCode.getMasterDataForSearch();
			return View(models);
		}

		public ActionResult myDocBudgetList(string actId)
		{
			var result = new AjaxResult();
			ApproveModel.approveModels models = ApproveAppCode.getApproveByActFormId(actId);
			//models.approveStatusLists = ApproveAppCode.getApproveStatus();
			return PartialView(models);
		}

		
		public ActionResult myDocBudget() // กำลังแก้ ******
		{
			Budget_Approve_Detail_Model.budgetForms model = new Budget_Approve_Detail_Model.budgetForms();
			if (TempData["SearchDataModel"] != null)
			{
				//model = (Activity_Model.actForms)TempData["SearchDataModel"];
				model.budgetFormLists = (List<Budget_Approve_Detail_Model.budgetForm>)TempData["SearchDataModel"];
			}
			else
			{
				//model = new Activity_Model.actForms();
				//model.actLists = ActFormAppCode.getActFormByEmpId(UtilsAppCode.Session.User.empId);
				model = new Budget_Approve_Detail_Model.budgetForms();
				model.budgetFormLists = getBudgetListsByEmpId(null);
			}
			return PartialView(model);
		}

		public static List<Budget_Approve_Detail_Model.budgetForm> getBudgetListsByEmpId(string empId)
		{
			try
			{
				DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetFormByEmpId"
					, new SqlParameter[] { new SqlParameter("@empId", empId) });
				var lists = (from DataRow dr in ds.Tables[0].Rows
							 select new Budget_Approve_Detail_Model.budgetForm()
							 {
								 activityId = dr["ActivityFormId"].ToString(),
								 statusId = dr["statusId"].ToString(),
								 statusName = dr["statusName"].ToString(),
								 activityNo = dr["activityNo"].ToString(),

								 regApproveId = dr["regApproveId"].ToString(),
								 regApproveFlowId = dr["regApproveFlowId"].ToString(),
								 budgetApproveId = dr["budgetApproveId"].ToString(),
								 documentDate = dr["documentDate"] is DBNull ? null : (DateTime?)dr["documentDate"],


								 reference = dr["reference"].ToString(),
								 customerId = dr["customerId"].ToString(),
								 channelName = dr["channelName"].ToString(),
								 productTypeId = dr["productTypeId"].ToString(),
								 productTypeNameEN = dr["productTypeNameEN"].ToString(),

								 cusShortName = dr["cusShortName"].ToString(),
								 productCategory = dr["productCateText"].ToString(),
								 productGroup = dr["productGroupId"].ToString(),
								 productGroupName = dr["productGroupName"].ToString(),

								 activityPeriodSt = dr["activityPeriodSt"] is DBNull ? null : (DateTime?)dr["activityPeriodSt"],
								 activityPeriodEnd = dr["activityPeriodEnd"] is DBNull ? null : (DateTime?)dr["activityPeriodEnd"],
								 costPeriodSt = dr["costPeriodSt"] is DBNull ? null : (DateTime?)dr["costPeriodSt"],
								 costPeriodEnd = dr["costPeriodEnd"] is DBNull ? null : (DateTime?)dr["costPeriodEnd"],
								 activityName = dr["activityName"].ToString(),
								 theme = dr["theme"].ToString(),
								 objective = dr["objective"].ToString(),
								 trade = dr["trade"].ToString(),
								 activityDetail = dr["activityDetail"].ToString(),

								 budgetActivityId = dr["budgetActivityId"].ToString(),
								 //budgetApproveId = dr["budgetApproveId"].ToString(),
								 approveId = dr["approveId"].ToString(),
								 //approveDetailId = dr["approveDetailId"].ToString(),

								 //delFlag = (bool)dr["delFlag"],
								 createdDate = (DateTime?)dr["createdDate"],
								 createdByUserId = dr["createdByUserId"].ToString(),
								 updatedDate = (DateTime?)dr["updatedDate"],
								 updatedByUserId = dr["updatedByUserId"].ToString(),

								 normalCost = dr["normalCost"] is DBNull ? 0 : (decimal?)dr["normalCost"],
								 themeCost = dr["themeCost"] is DBNull ? 0 : (decimal?)dr["themeCost"],
								 totalCost = dr["totalCost"] is DBNull ? 0 : (decimal?)dr["totalCost"],
								 totalInvoiceApproveBath = dr["totalInvoiceApproveBath"] is DBNull ? 0 : (decimal?)dr["totalInvoiceApproveBath"]

							 }).ToList();
				return lists;
			}
			catch (Exception ex)
			{
				//throw new Exception("getApproveListsByStatusId >> " + ex.Message);
				ExceptionManager.WriteError("getApproveListsByStatusId >> " + ex.Message);
				return new List<Budget_Approve_Detail_Model.budgetForm>();
			}
		}


		public ActionResult searchBudgetForm()
		{
			string count = Request.Form.AllKeys.Count().ToString();
			Budget_Approve_Detail_Model.budgetForms model = new Budget_Approve_Detail_Model.budgetForms();
			model.budgetFormLists = QueryGetBudgetApprove.getApproveListsByEmpId(null);
			//model.budgetFormLists = QueryGetBudgetApprove.getApproveListsByEmpId(UtilsAppCode.Session.User.empId);

			if (Request.Form["txtActivityNo"] != "")
			{
				model.budgetFormLists = model.budgetFormLists.Where(r => r.activityNo == Request.Form["txtActivityNo"]).ToList();
			}

			if (Request.Form["ddlStatus"] != "")
			{
				model.budgetFormLists = model.budgetFormLists.Where(r => r.statusId == Request.Form["ddlStatus"]).ToList();
			}

			TempData["SearchDataModel"] = model.budgetFormLists;
			return RedirectToAction("myDocBudget");
		}
	}
}