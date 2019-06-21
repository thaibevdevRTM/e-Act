using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.IO;
using eActForm.Models;
using WebLibrary;

namespace eActForm.Controllers
{
    public class BudgetViewerController : Controller
    {
        // GET: BudgetViewer
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult activityPDFView(string budgetApproveId)
		{
	
			//var var_budgetActivityId = BudgetApproveListController.getApproveBudgetId(budgetActivityId);
			TempData["budgetApproveId"] = budgetApproveId;
			ViewBag.budgetApproveId = budgetApproveId;
			return PartialView();
		}

		public ActionResult getPdfBudget(string budgetApproveId)
		{
			string rootPath = "", mapPath = "";

			try
			{

	
					rootPath = ConfigurationManager.AppSettings["rootBudgetPdftURL"];



			}
			catch (Exception ex)
			{
				ExceptionManager.WriteError(ex.Message);
			}
			var fileStream = new FileStream(Server.MapPath(string.Format(rootPath, budgetApproveId)),
													 FileMode.Open,
													 FileAccess.Read
												   );
			var fsResult = new FileStreamResult(fileStream, "application/pdf");
			return fsResult;

			//var fileStream = new FileStream(Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootBudgetPdftURL"], budgetApproveId)),
			//								 FileMode.Open,
			//								 FileAccess.Read
			//							   );
			//var fsResult = new FileStreamResult(fileStream, "application/pdf");
			//return fsResult;
		}

		//---------------------------------------------------------------------------------------------------





	}
}