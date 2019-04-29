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

		public ActionResult activityPDFView(string budgetActivityId)
		{
			//TempData["fileViewer"] = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rooPdftURL"], actId));
			//var var_budgetActivityId = BudgetApproveListController.getApproveBudgetId(budgetActivityId);
			TempData["budgetActivityId"] = budgetActivityId;
			return PartialView();
		}

		public ActionResult getPDF(string budgetActivityId)
		{
			var fileStream = new FileStream(Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootBudgetPdftURL"], budgetActivityId)),
											 FileMode.Open,
											 FileAccess.Read
										   );
			var fsResult = new FileStreamResult(fileStream, "application/pdf");
			return fsResult;
		}

	}
}