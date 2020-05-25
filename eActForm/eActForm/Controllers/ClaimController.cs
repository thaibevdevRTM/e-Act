using eActForm.BusinessLayer;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLibrary;
namespace eActForm.Controllers
{
    [LoginExpire]
    public class ClaimController : Controller
    {
        // GET: Claim
        public ActionResult claimIndex()
        {
            return View();
        }

		public ActionResult searchClaimActivityForm(string typeForm)
		{
			DateTime act_createdDateStart = DateTime.Now.AddYears(-10);
			DateTime act_createdDateEnd = DateTime.Now.AddYears(2);
			string act_budgetStatusIdin = null;

			Budget_Activity_Model budget_activity = new Budget_Activity_Model();

			try
			{
				#region filter
				if (Request.Form["chk_all"] != null && Request.Form["chk_all"] == "true")
				{
					act_createdDateStart = DateTime.Now.AddYears(-10);
					act_createdDateEnd = DateTime.Now.AddYears(2);
				}
				else
				{
					act_createdDateStart = DateTime.ParseExact(Request.Form["startDate"].Trim(), "MM/dd/yyyy", null);
					act_createdDateEnd = DateTime.ParseExact(Request.Form["endDate"].Trim(), "MM/dd/yyyy", null);
				}

				if (Request.Form["ddlFormType"] == "Select All")
				{
					typeForm = null;
				}
				else
				{
					typeForm = Request.Form["ddlFormType"];
				}		

				if (Request.Form["ddlBudgetStatusId"] != "" && Request.Form["ddlBudgetStatusId"] != "Select All")
				{
					act_budgetStatusIdin = Request.Form["ddlBudgetStatusId"];
				}
				#endregion

				budget_activity.Budget_Activity_list = QueryGetBudgetActivity.getBudgetActivityList("3", null, null, null, typeForm, act_createdDateStart, act_createdDateEnd, act_budgetStatusIdin).ToList();
				TempData["searchBudgetActivityForm"] = budget_activity;
			}
			catch (Exception ex)
			{
				ExceptionManager.WriteError("searchBudgetActivityForm --> " + ex.Message);
			}

			return RedirectToAction("claimList");
		}

		public ActionResult claimList(string typeForm)
		{
			Budget_Activity_Model models = new Budget_Activity_Model();

			try
			{
				if (TempData["searchBudgetActivityForm"] != null)
				{
					models = (Budget_Activity_Model)TempData["searchBudgetActivityForm"];
				}
				else
				{
					models.Budget_Activity_list = QueryGetBudgetActivity.getBudgetActivityList("3", null, null, null, typeForm, DateTime.Now.AddDays(-30), DateTime.Now, null).ToList();
				}
				TempData["searchBudgetActivityForm"] = null;
				return PartialView(models);
			}
			catch (Exception ex)
			{
				ExceptionManager.WriteError("claimList --> " + ex.Message);
			}
			return PartialView(models);
		}

		public ActionResult claimProduct(string activityId)
		{
			Budget_Activity_Model budget_activity = new Budget_Activity_Model();

			if (activityId == null) { activityId = Session["activityId"].ToString(); }

			if (activityId == null) return RedirectToAction("claimList", "Claim");
			else
			{
				try
				{
					budget_activity.Budget_Activity_list = QueryGetBudgetActivity.getBudgetActivityList("3", activityId, null, null, null, DateTime.Now.AddYears(-10), DateTime.Now.AddYears(2), null).ToList();
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
			return View(budget_activity);
		}

		public ActionResult claimProductList(string activityId)
		{

			Session["activityId"] = activityId;
			Budget_Activity_Model budget_activity = new Budget_Activity_Model();
			try
			{
				budget_activity.Budget_Activity = QueryGetBudgetActivity.getBudgetActivityList(null, activityId, null, null, null, DateTime.Now.AddYears(-10), DateTime.Now.AddYears(2), null).FirstOrDefault(); ;
				budget_activity.Budget_Activity_Product_list = QueryGetBudgetActivity.getBudgetActivityProduct(activityId, null);
				budget_activity.Budget_Activity_Ststus_list = QueryGetBudgetActivity.getBudgetActivityStatus();
				budget_activity.Budget_Activity_Last_Approve = QueryGetBudgetActivity.getBudgetActivityLastApprove(activityId).FirstOrDefault();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return PartialView(budget_activity);
		}

		public PartialViewResult claimProductInvoiceList(string activityId, string activityOfEstimateId)
		{
			Budget_Activity_Model budget_activity_model = new Budget_Activity_Model();
			budget_activity_model.Budget_Activity = QueryGetBudgetActivity.getBudgetActivityList(null, activityId, null, null, null, DateTime.Now.AddYears(-10), DateTime.Now.AddYears(2), null).FirstOrDefault(); ;
			budget_activity_model.Budget_Activity_Invoice_list = QueryGetBudgetActivity.getBudgetActivityInvoice(activityId, activityOfEstimateId, null);
			//budget_activity_model.Budget_Activity_Last_Approve = QueryGetBudgetActivity.getBudgetActivityLastApprove(activityId).FirstOrDefault();

			return PartialView(budget_activity_model);
		}

		public PartialViewResult claimProductInvoiceEdit(string activityId, string activityOfEstimateId, string invoiceId)
		{
			Budget_Activity_Model Budget_Activity = new Budget_Activity_Model();
			Budget_Activity.Budget_Activity_Product = QueryGetBudgetActivity.getBudgetActivityProduct(activityId, activityOfEstimateId).FirstOrDefault();
			Budget_Activity.Budget_Activity_Ststus_list = QueryGetBudgetActivity.getBudgetActivityStatus();
			//Budget_Activity.Budget_Count_Wait_Approve = QueryGetBudgetActivity.getBudgetActivityWaitApprove(activityId).FirstOrDefault();


			if (!string.IsNullOrEmpty(invoiceId))
			{// for get invoice history 
				Budget_Activity.Budget_Activity_Invoice = QueryGetBudgetActivity.getBudgetActivityInvoice(activityId, activityOfEstimateId, invoiceId).FirstOrDefault();
			}
			return PartialView(Budget_Activity);
		}


	}
}