using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.IO;

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
			return PartialView();
		}

		public ActionResult getPdfBudget(string budgetApproveId)
		{
			var fileStream = new FileStream(Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootBudgetPdftURL"], budgetApproveId)),
											 FileMode.Open,
											 FileAccess.Read
										   );
			var fsResult = new FileStreamResult(fileStream, "application/pdf");
			return fsResult;
		}

	}
}