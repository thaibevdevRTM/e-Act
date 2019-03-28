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
		// GET: Budget
		//public ActionResult activityList()
		//{
		//    return View();
		//}

		//[HttpPost]
		//[ValidateInput(false)]
		public JsonResult submitInvoice(Budget_Activity_Model.Budget_Activity_Invoice_Att budgetInvoiceModel)
		{
			var resultAjax = new AjaxResult();
			try
			{

				if (!string.IsNullOrEmpty(budgetInvoiceModel.id))
				{
					//update invoice
					int countSuccess = BudgetFormCommandHandler.updateInvoiceProduct(budgetInvoiceModel);
				}
				else
				{
					// insert invoice
					int countSuccess = BudgetFormCommandHandler.insertInvoiceProduct(budgetInvoiceModel);
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

		public JsonResult delInvoiceDetail(string actId, string prdId, string invNo)
		{
			var result = new AjaxResult();
			try
			{		
				int countSuccess = BudgetFormCommandHandler.deleteInvoiceProduct(actId, prdId, invNo);
				result.Success = true;
			}
			catch (Exception ex)
			{
				result.Message = ex.Message;
				result.Success = false;
			}

			return Json(result, JsonRequestBehavior.AllowGet);
		}


		public ActionResult EditBudgetInvoice(string invoiceId)
		{
			Budget_Activity_Model.Budget_Activity_Invoice_Att Budget_Activity_Invoice = new Budget_Activity_Model.Budget_Activity_Invoice_Att();
			return PartialView("EditBudgetInvoice", Budget_Activity_Invoice);
		}

		public ActionResult PreviewBudgetInvoice(string activityId, string productId, string invoiceId)
		{
						
			if (!string.IsNullOrEmpty(invoiceId))
			{
				Budget_Activity_Model Budget_Activity = new Budget_Activity_Model();
				Budget_Activity.Budget_Activity_Product_list = QueryBudgetBiz.getBudgetActivityProduct(activityId, productId, invoiceId);
				Budget_Activity.Budget_Activity_Ststus_list = QueryBudgetBiz.getBudgetActivityStatus();


				//var selectList = new SelectList(Budget_Activity.Budget_Activity_Ststus_list);
				//foreach (var item in selectList)
				//{
				//	if (item.Value == Budget_Activity.Budget_Activity_Product_list.ElementAt(0).invoiceActivityStatusId)
				//	{
				//		item.Selected = true;
				//	}
				//}
				
				return PartialView("PreviewBudgetInvoice", Budget_Activity);
			}
			else
			{
				Budget_Activity_Model Budget_Activity = new Budget_Activity_Model();
				Budget_Activity.Budget_Activity_Product_list = QueryBudgetBiz.getBudgetActivityProduct(activityId, productId,null);
				Budget_Activity.Budget_Activity_Ststus_list = QueryBudgetBiz.getBudgetActivityStatus();
				return PartialView("PreviewBudgetInvoice", Budget_Activity);
			}

		}


		public ActionResult EditForm(string activityId)
		{

			Session["activityId"] = activityId;
			//Session["activityNo"] = activityNo;
			Budget_Activity_Model budget_activity_model = new Budget_Activity_Model();
			budget_activity_model.Budget_Activity_list = QueryBudgetBiz.getBudgetActivity("2", null).ToList();
			budget_activity_model.Budget_Activity_Product_list = QueryBudgetBiz.getBudgetActivityProduct(activityId, null,null);
			budget_activity_model.Budget_Activity_Ststus_list = QueryBudgetBiz.getBudgetActivityStatus();

			return View(budget_activity_model);
			
		}

		public ActionResult activityList()
        {
            Session["activityId"] = Guid.NewGuid().ToString();
			Budget_Activity_Model budget_activity_model = new Budget_Activity_Model();
            budget_activity_model.Budget_Activity_list = QueryBudgetBiz.getBudgetActivity("2",null).ToList();
		
			return View(budget_activity_model);
        }
	}

  

}