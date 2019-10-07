using System.Collections.Generic;
using System.Linq;
using System.Data;
using eActForm.BusinessLayer;
using eActForm.Models;
using System.Web.Mvc;


namespace eActForm.Controllers
{
	[LoginExpire]

	public class BudgetApproveListController : Controller
    {
		public ActionResult Index()
		{
			SearchActivityModels models = SearchAppCode.getMasterDataForSearch();
			return View(models);
		}

		public ActionResult searchActForm()
		{
			string count = Request.Form.AllKeys.Count().ToString();
			Budget_Approve_Detail_Model.budgetForms model = new Budget_Approve_Detail_Model.budgetForms();
			model.budgetFormLists = (List<Budget_Approve_Detail_Model.budgetForm>)TempData["ApproveFormLists"];

			if (Request.Form["txtActivityNo"] != "")
			{
				model.budgetFormLists = model.budgetFormLists.Where(r => r.activityNo == Request.Form["txtActivityNo"]).ToList();
			}
			else if (Request.Form["ddlStatus"] != "")
			{
				model.budgetFormLists = BudgetApproveController.getFilterFormByStatusId(model.budgetFormLists, int.Parse(Request.Form["ddlStatus"]));
			}
			TempData["ApproveSearchResult"] = model.budgetFormLists;
			return RedirectToAction("budgetApproveList");

		}

		public ActionResult budgetApproveList()
		{

			Budget_Approve_Detail_Model.budgetForms  model = new Budget_Approve_Detail_Model.budgetForms();
			if (TempData["ApproveSearchResult"] == null)
			{
				model = new Budget_Approve_Detail_Model.budgetForms();
				model.budgetFormLists = QueryGetBudgetApprove.getApproveListsByEmpId(UtilsAppCode.Session.User.empId);
				TempData["ApproveFormLists"] = model.budgetFormLists;
				model.budgetFormLists = BudgetApproveController.getFilterFormByStatusId(model.budgetFormLists, (int)AppCode.ApproveStatus.รออนุมัติ);
			}
			else
			{
				model.budgetFormLists = (List<Budget_Approve_Detail_Model.budgetForm>)TempData["ApproveSearchResult"];
			}
			return PartialView(model);
		}	
	}
}