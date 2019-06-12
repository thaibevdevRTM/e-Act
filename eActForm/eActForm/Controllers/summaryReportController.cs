using eActForm.BusinessLayer;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    public class summaryReportController : Controller
    {
        // GET: summaryReport
        public ActionResult Index()
        {
            SearchActivityModels models = SearchAppCode.getMasterDataForSearch();
            return View(models);
        }

        public ActionResult viewReportActivityBudget()
        {
            ReportSummaryModels model = new ReportSummaryModels();
            model.activitySummaryList = ReportSummaryAppCode.getReportSummary();

            return PartialView(model);
        }

        public ActionResult searchActForm()
        {
            try
            {
                ReportSummaryModels model = new ReportSummaryModels();
                model.activitySummaryList = ReportSummaryAppCode.getSummaryDetailReportByDate(Request.Form["startDate"], Request.Form["endDate"]);
                if (Request.Form["txtRepNo"] != "")
                {
                    model.activitySummaryList = model.activitySummaryList.Where(x => x.activityId == Request.Form["txtRepNo"]).ToList();
                }
                else
                {
                    #region filter
                    if (Request.Form["ddlCustomer"] != "")
                    {
                        model.activitySummaryList = ReportSummaryAppCode.getFilterSummaryDetailByCustomer(model.activitySummaryList, Request.Form["ddlCustomer"]);
                    }
                    
                    //if (Request.Form["ddlCustomer"] != "" && Request.Form["ddlProductType"] != "")
                    //{
                    //    model.flowList = ApproveFlowAppCode.getFlowForReportDetail(
                    //                    ConfigurationManager.AppSettings["subjectReportDetailId"]
                    //                    , Request.Form["ddlCustomer"]
                    //                    , Request.Form["ddlProductType"]);
                    //}
                    #endregion
                }
                Session["SummaryDetailModel"] = model;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }

            return RedirectToAction("repChooseView", new { startDate = Request.Form["startDate"] });
        }

        public ActionResult repSummaryView(string startDate)
        {
            ReportSummaryModels model = null;
            try
            {
                ViewBag.startDate = startDate;
                model = (ReportSummaryModels)Session["SummaryDetailModel"] ?? new ReportSummaryModels();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }

            return PartialView(model);
        }


      
    }
}