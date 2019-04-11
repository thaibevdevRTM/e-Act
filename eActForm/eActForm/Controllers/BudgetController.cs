using eActForm.BusinessLayer;
using eActForm.Models;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebLibrary;

namespace eActForm.Controllers
{
	[LoginExpire]
	public class BudgetController : Controller
	{

		[HttpPost]
		[ValidateInput(false)]
		public JsonResult genPdfApprove(string GridHtml, string activityId)
		{
			var resultAjax = new AjaxResult();
			try
			{
				AppCode.genPdfFile(GridHtml, new Document(PageSize.A4, 25, 25, 10, 10), activityId);
				//EmailAppCodes.sendApproveActForm(activityId);
			
				resultAjax.Success = true;
			}
			catch (Exception ex)
			{
				resultAjax.Success = false;
				resultAjax.Message = ex.Message;
			}
			return Json(resultAjax, "text/plain");
		}


		public JsonResult submitInvoice(Budget_Activity_Model.Budget_Activity_Invoice_Att budgetInvoiceModel)
		{
			var resultAjax = new AjaxResult();
			try
			{

				if (budgetInvoiceModel.invoiceId == null)
				{
					// insert invoice
					int countSuccess = BudgetFormCommandHandler.insertInvoiceProduct(budgetInvoiceModel);
				}
				else
				{
					//update invoice
					int countSuccess = BudgetFormCommandHandler.updateInvoiceProduct(budgetInvoiceModel);
				}

				//resultAjax.ActivityId = Session["activityId"].ToString();
				resultAjax.Success = true;
			}
			catch (Exception ex)
			{
				resultAjax.Success = false;
				resultAjax.Message = ex.Message;
			}
			return Json(resultAjax, "text/plain");
		}
		
		public JsonResult delInvoiceDetail(string actId,  string estId, string invId)
		{
			var result = new AjaxResult();
			try
			{		
				int countSuccess = BudgetFormCommandHandler.deleteInvoiceProduct(actId, estId, invId);
				result.Success = true;
			}
			catch (Exception ex)
			{
				result.Message = ex.Message;
				result.Success = false;
			}
			return Json(result, JsonRequestBehavior.AllowGet);
		}


		//---------------------------------------------------------------------------------------
		public PartialViewResult activityProductInvoiceEdit(string activityId, string activityOfEstimateId, string invoiceId)
		{
			if (!string.IsNullOrEmpty(invoiceId))
			{// for edit invoice
				Budget_Activity_Model Budget_Activity = new Budget_Activity_Model();
				Budget_Activity.Budget_Activity_Product_list = QueryGetBudgetActivity.getBudgetActivityProduct(activityId, activityOfEstimateId);
				Budget_Activity.Budget_Activity_Invoice_list = QueryGetBudgetActivity.getBudgetActivityInvoice(activityId, activityOfEstimateId, invoiceId);
				Budget_Activity.Budget_Activity_Ststus_list = QueryGetBudgetActivity.getBudgetActivityStatus();
				return PartialView("activityProductInvoiceEdit", Budget_Activity);
			}
			else
			{// for insert invoice
				Budget_Activity_Model Budget_Activity = new Budget_Activity_Model();
				Budget_Activity.Budget_Activity_Product_list = QueryGetBudgetActivity.getBudgetActivityProduct(activityId, activityOfEstimateId);
				Budget_Activity.Budget_Activity_Ststus_list = QueryGetBudgetActivity.getBudgetActivityStatus();
				return PartialView("activityProductInvoiceEdit", Budget_Activity);
			}
		}
		
		public PartialViewResult activityInvoicePreviewList(string activityId)
		{
			Budget_Approve_Model Budget_Approve_Model = new Budget_Approve_Model();
			Budget_Approve_Model.Budget_Approve_list = QueryGetBudgetApprove.getBudgetActivityApprove(activityId);
			Budget_Approve_Model.Budget_Activity_list = QueryGetBudgetActivity.getBudgetActivity(null, activityId, null);
			return PartialView(Budget_Approve_Model);
		}
		
		public PartialViewResult activityProductInvoiceList(string activityId , string activityOfEstimateId)
		{
			Budget_Activity_Model budget_activity_model = new Budget_Activity_Model();
			budget_activity_model.Budget_Activity_Product_list = QueryGetBudgetActivity.getBudgetActivityProduct(activityId,  activityOfEstimateId);
			budget_activity_model.Budget_Activity_Invoice_list = QueryGetBudgetActivity.getBudgetActivityInvoice(activityId, activityOfEstimateId, null);
			budget_activity_model.Budget_Activity_Ststus_list = QueryGetBudgetActivity.getBudgetActivityStatus();
			return PartialView(budget_activity_model);
		}

		public ActionResult activityProductList(string activityId)
		{
			Session["activityId"] = activityId;
			Budget_Activity_Model budget_activity = new Budget_Activity_Model();
			try
			{
				budget_activity.Budget_Activity_Product_list = QueryGetBudgetActivity.getBudgetActivityProduct(activityId, null);
				budget_activity.Budget_Activity_Ststus_list = QueryGetBudgetActivity.getBudgetActivityStatus();	
			} catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return PartialView(budget_activity);
		}
		//----------------------------------------------------------------------------------------


		public ActionResult activityProduct(string activityId)
		{
			Budget_Activity_Model budget_activity = new Budget_Activity_Model();
			try
			{
				budget_activity.Budget_Activity_list = QueryGetBudgetActivity.getBudgetActivity("3", activityId, null).ToList();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return View(budget_activity);
		}
		
		public ActionResult activityList()
		{
			//Session["activityId"] = Guid.NewGuid().ToString();
			Budget_Activity_Model budget_activity = new Budget_Activity_Model();
			budget_activity.Budget_Activity_list = QueryGetBudgetActivity.getBudgetActivity("3", null,null).ToList();
			return View(budget_activity);
		}
	}



}