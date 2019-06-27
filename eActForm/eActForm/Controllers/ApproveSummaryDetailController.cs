using eActForm.BusinessLayer;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public ActionResult ListView()
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
            model.activitySummaryList = (List<ReportSummaryModels.ReportSummaryModel>)TempData["ApproveFormLists"];

            if (Request.Form["ddlStatus"] != "")
            {
                model.activitySummaryList = ReportSummaryAppCode.getFilterFormByStatusId(model.activitySummaryList, int.Parse(Request.Form["ddlStatus"]));
            }
            TempData["ApproveSearchResult"] = model.activitySummaryList;
            return RedirectToAction("ListView");
        }
    }
}