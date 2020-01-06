using eActForm.BusinessLayer;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    public class ApproveSummaryDetailController : Controller
    {
        // GET: ApproveSummary
        public ActionResult Index()
        {
            SearchActivityModels models = new SearchActivityModels();
            try
            {
                models = SearchAppCode.getMasterDataForSearch();
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("ApproveSummaryDetail => Index " + ex.Message);
            }
            return View(models);
        }

        public ActionResult ListViewApprove()
        {
            ReportSummaryModels model = new ReportSummaryModels();
            try
            {
                if (TempData["ApproveSearchResult"] == null)
                {
                    model.summaryDetailLists = ReportSummaryAppCode.getApproveSummaryDetailListsByEmpId();
                    TempData["ApproveFormLists"] = model.summaryDetailLists;
                }
                else
                {
                    model.summaryDetailLists = (List<ReportSummaryModels.actApproveSummaryDetailModel>)TempData["ApproveSearchResult"];
                }
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("ApproveSummaryDetail => ListViewApprove " + ex.Message);
            }
            return PartialView(model);
        }

        public ActionResult searchActForm()
        {
            ReportSummaryModels model = new ReportSummaryModels();
            try
            {
                model.summaryDetailLists = (List<ReportSummaryModels.actApproveSummaryDetailModel>)TempData["ApproveFormLists"];

                if (Request.Form["ddlStatus"] != "")
                {
                    model.summaryDetailLists = ReportSummaryAppCode.getFilterFormByStatusId(model.summaryDetailLists, int.Parse(Request.Form["ddlStatus"]));
                }
                TempData["ApproveSearchResult"] = model.summaryDetailLists;
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("ApproveSummaryDetail => searchActForm " + ex.Message);
            }
            return RedirectToAction("ListViewApprove");
        }
    }
}