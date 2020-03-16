using eActForm.BusinessLayer;
using eActForm.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    public class ApproveSummaryDetailController : Controller
    {
        // GET: ApproveSummary
        public ActionResult Index()
        {
            SearchActivityModels models = SearchAppCode.getMasterDataForSearch();
            return View(models);
        }

        public ActionResult ListViewApprove()
        {
            ReportSummaryModels model = new ReportSummaryModels();
            if (TempData["ApproveSearchResult"] == null)
            {
                model.summaryDetailLists = ReportSummaryAppCode.getApproveSummaryDetailListsByEmpId();
                TempData["ApproveFormLists"] = model.summaryDetailLists;
            }
            else
            {
                model.summaryDetailLists = (List<ReportSummaryModels.actApproveSummaryDetailModel>)TempData["ApproveSearchResult"];
            }
            return PartialView(model);
        }

        public ActionResult searchActForm()
        {
            ReportSummaryModels model = new ReportSummaryModels();
            model.summaryDetailLists = (List<ReportSummaryModels.actApproveSummaryDetailModel>)TempData["ApproveFormLists"];

            if (Request.Form["ddlStatus"] != "")
            {
                model.summaryDetailLists = ReportSummaryAppCode.getFilterFormByStatusId(model.summaryDetailLists, int.Parse(Request.Form["ddlStatus"]));
            }
            TempData["ApproveSearchResult"] = model.summaryDetailLists;
            return RedirectToAction("ListViewApprove");
        }
    }
}