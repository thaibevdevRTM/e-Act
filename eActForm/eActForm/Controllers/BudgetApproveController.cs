using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using eActForm.BusinessLayer;
using eActForm.Models;
using iTextSharp.text;

namespace eActForm.Controllers
{
	[LoginExpire]

	public class BudgetApproveController : Controller
    {
        // GET: BudgetApprove
        public ActionResult Index(string activityId)
        {
			if (activityId == null) return RedirectToAction("activityList", "Budget");
			else
			{
				ApproveModel.approveModels models = ApproveAppCode.getApproveByActFormId(activityId);
				return View(models);
			}
		}

		[HttpPost]
		[ValidateInput(false)]
		public JsonResult genPdfApprove(string GridHtml,  string activityId)
		{
			var resultAjax = new AjaxResult();
			try
			{
				AppCode.genPdfFile(GridHtml, new Document(PageSize.A4, 25, 25, 10, 10), activityId);

				//if (statusId == ConfigurationManager.AppSettings["statusReject"])
				//{
				//	EmailAppCodes.sendRejectActForm(activityId);
				//}
				//else if (statusId == ConfigurationManager.AppSettings["statusApprove"])
				//{
				//	AppCode.genPdfFile(GridHtml, new Document(PageSize.A4, 25, 25, 10, 10), activityId);
				//	EmailAppCodes.sendApproveActForm(activityId);
				//}
				resultAjax.Success = true;
			}
			catch (Exception ex)
			{
				resultAjax.Success = false;
				resultAjax.Message = ex.Message;
			}
			return Json(resultAjax, "text/plain");
		}


		public ActionResult budgetApproveList(string activityId)
		{
			//Session["activityId"] = activityId;
			Budget_Activity_Model budget_activity = new Budget_Activity_Model();
			try
			{
				budget_activity.Budget_Activity_Product_list = QueryBudgetBiz.getBudgetActivityProduct(activityId, null);
				budget_activity.Budget_Activity_Ststus_list = QueryBudgetBiz.getBudgetActivityStatus();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return PartialView(budget_activity);
		}

		public ActionResult budgetApprove(string activityId)
		{
			Budget_Activity_Model budget_activity = new Budget_Activity_Model();
			try
			{
				budget_activity.Budget_Activity_list = QueryBudgetBiz.getBudgetActivity("3", activityId, null).ToList();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return View(budget_activity);
		}

	}
}