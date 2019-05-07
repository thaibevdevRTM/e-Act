using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eActForm.Models;
using eActForm.BusinessLayer;
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
				model.budgetFormLists = BudgetApproveListController.getApproveListsByEmpId(UtilsAppCode.Session.User.empId);
			}
			return PartialView(model);
		}

		public ActionResult searchBudgetForm()
		{
			string count = Request.Form.AllKeys.Count().ToString();
			Budget_Approve_Detail_Model.budgetForms model = new Budget_Approve_Detail_Model.budgetForms();
			model.budgetFormLists = BudgetApproveListController.getApproveListsByEmpId(UtilsAppCode.Session.User.empId);

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

		public ActionResult requestDeleteDoc(string actId, string statusId)
		{
			//return RedirectToAction("index");
			AjaxResult result = new AjaxResult();
			result.Success = false;
			//if (statusId == "1")
			//{
			// Draft
			if (ActFormAppCode.deleteActForm(actId, "request delete by user") > 0)
			{
				result.Success = true;
				TempData["SearchDataModel"] = null;
			}
			//}
			//else
			//{

			//}

			return RedirectToAction("myDoc");
		}


	}
}