using eActForm.BusinessLayer;
using eActForm.Models;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers //update 21-04-2020
{
    //public class BudgetViewerController : Controller
    //{
    //    // GET: BudgetViewer
    //    public ActionResult Index()
    //    {
    //        return View();
    //    }

    //    public ActionResult activityPDFView(string budgetApproveId)
    //    {

    //        //var var_budgetActivityId = BudgetApproveListController.getApproveBudgetId(budgetActivityId);
    //        TempData["budgetApproveId"] = budgetApproveId;
    //        ViewBag.budgetApproveId = budgetApproveId;
    //        return PartialView();
    //    }

    //    public PartialViewResult regenBudgetApprovePdf(string budgetApproveId, string activityId)
    //    {
    //        Budget_Approve_Detail_Model Budget_Model = new Budget_Approve_Detail_Model();
    //        Budget_Model.Budget_Invoce_History_list = QueryGetBudgetApprove.getBudgetInvoiceHistory(null, budgetApproveId);
    //        Budget_Model.Budget_Activity = QueryGetBudgetActivity.getBudgetActivity(null, activityId, null, null, DateTime.Now.AddYears(-10), DateTime.Now.AddYears(2), null).FirstOrDefault();

    //        Budget_Model.Budget_Approve_detail_list = QueryGetBudgetApprove.getBudgetApproveId(budgetApproveId);
    //        return PartialView(Budget_Model);

    //        //budgetApproveId
    //        //activityId

    //    }

    //    public ActionResult getPdfBudget(string budgetApproveId)
    //    {
    //        string rootPath = "";
    //        FileStream fileStream = null;
    //        FileStreamResult fsResult = null;

    //        try
    //        {
    //            rootPath = ConfigurationManager.AppSettings["rootBudgetPdftURL"];
    //            if (!System.IO.File.Exists(Server.MapPath(string.Format(rootPath, budgetApproveId))))
    //            {
    //                budgetApproveId = "fileNotFound";
    //            }

    //            fileStream = new FileStream(Server.MapPath(string.Format(rootPath, budgetApproveId)),
    //                                     FileMode.Open,
    //                                     FileAccess.Read
    //                                   );

    //            fsResult = new FileStreamResult(fileStream, "application/pdf");

    //        }
    //        catch (Exception ex)
    //        {
    //            ExceptionManager.WriteError("getPdfBudget >>" + ex.Message);
    //        }


    //        return fsResult;
    //    }

    //    public ActionResult getInvoicePdfBudget(string fileName)
    //    {
    //        string rootPath = "";
    //        FileStream fileStream = null;
    //        FileStreamResult fsResult = null;

    //        try
    //        {
    //            rootPath = ConfigurationManager.AppSettings["rootUploadfilesBudget"];
    //            if (!System.IO.File.Exists(Server.MapPath(string.Format(rootPath, fileName))))
    //            {
    //                fileName = "fileNotFound.pdf";
    //            }

    //            fileStream = new FileStream(Server.MapPath(string.Format(rootPath, fileName)),
    //                                                 FileMode.Open,
    //                                                 FileAccess.Read
    //                                               );
    //            fsResult = new FileStreamResult(fileStream, "application/pdf");

    //        }
    //        catch (Exception ex)
    //        {
    //            ExceptionManager.WriteError("getInvoicePdfBudget >>" + ex.Message);
    //        }

    //        return fsResult;
    //    }

    //    //---------------------------------------------------------------------------------------------------

    //    public ActionResult invoicePDFView(string fileName)
    //    {

    //        TempData["fileName"] = fileName;
    //        ViewBag.fileName = fileName;
    //        return PartialView();
    //    }


    //}
}