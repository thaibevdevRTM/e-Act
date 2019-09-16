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
				if (!System.IO.File.Exists(Server.MapPath(string.Format(rootPath, budgetApproveId))))
				{
					budgetApproveId = "fileNotFound";
				}

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
		}

		//---------------------------------------------------------------------------------------------------


		public ActionResult invoicePDFView(string fileName)
		{

			TempData["fileName"] = fileName;
			ViewBag.fileName = fileName;
			return PartialView();
		}
		

		public ActionResult getInvoicePdfBudget(string fileName)
		{
			string rootPath = "", mapPath = "";

			try
			{
				rootPath = ConfigurationManager.AppSettings["rootUploadfilesBudget"];
				if (!System.IO.File.Exists(Server.MapPath(string.Format(rootPath, fileName))))
				{
					fileName = "fileNotFound";
				}

			}
			catch (Exception ex)
			{
				ExceptionManager.WriteError(ex.Message);
			}
			var fileStream = new FileStream(Server.MapPath(string.Format(rootPath, fileName)),
													 FileMode.Open,
													 FileAccess.Read
												   );
			var fsResult = new FileStreamResult(fileStream, "application/pdf");
			return fsResult;
		}
	}
}