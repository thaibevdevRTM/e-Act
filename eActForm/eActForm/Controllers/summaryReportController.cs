using eActForm.BusinessLayer;
using eActForm.Models;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    public class SummaryReportController : Controller
    {
        // GET: summaryReport
        public ActionResult Index()
        {
            SearchActivityModels models = SearchAppCode.getMasterDataForSearch();
            return View(models);
        }

        public ActionResult viewReportActivityBudget(string startDate)
        {
            string repDetail = "";
            ReportSummaryModels model = new ReportSummaryModels();
           
            model = (ReportSummaryModels)Session["SummaryDetailModel"] ?? new ReportSummaryModels();
            model.activitySummaryList = model.activitySummaryList.Where(r => r.delFlag == false).ToList();
            if(model.activitySummaryList.Any())
            {
                repDetail = string.Join(",", model.activitySummaryList.Select(x => x.repDetailId));

                model.activitySummaryList = ReportSummaryAppCode.getReportSummary(repDetail);
            }

            ViewBag.MouthText = DateTime.ParseExact(startDate, "MM/dd/yyyy", null).ToString("MMM yyyy");
            return PartialView(model);
        }



        public ActionResult searchActForm()
        {
            try
            {
                ReportSummaryModels model = new ReportSummaryModels();
                model.activitySummaryList = ReportSummaryAppCode.getSummaryDetailReportByDate(Request.Form["startDate"], Request.Form["endDate"]);

                #region filter
                if (Request.Form["ddlCustomer"] != "")
                {
                    model.activitySummaryList = ReportSummaryAppCode.getFilterSummaryDetailByCustomer(model.activitySummaryList, Request.Form["ddlCustomer"]);
                }
                if (Request.Form["ddlProductType"] != "")
                {
                    model.activitySummaryList = ReportSummaryAppCode.getFilterSummaryDetailByProductType(model.activitySummaryList, Request.Form["ddlProductType"]);
                }

                if (Request.Form["ddlCustomer"] != "" && Request.Form["ddlProductType"] != "")
                {
                    model.flowList = ApproveFlowAppCode.getFlowForReportDetail(
                                    ConfigurationManager.AppSettings["subjectSummaryId"]
                                    , Request.Form["ddlCustomer"]
                                    , Request.Form["ddlProductType"]);
                }
                #endregion

                Session["SummaryDetailModel"] = model;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }

            return RedirectToAction("repSummaryView", new { startDate = Request.Form["startDate"] });
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

        public JsonResult repSetDelFlagRecodeSummaryDetail(string repId, bool delFlag)
        {
            var result = new AjaxResult();
            try
            {
                ReportSummaryModels model = (ReportSummaryModels)Session["SummaryDetailModel"];
                model.activitySummaryList
                    .Where(r => r.repDetailId == repId)
                    .Select(r => r.delFlag = !delFlag
                    ).ToList();
                Session["SummaryDetailModel"] = model;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                ExceptionManager.WriteError(ex.Message);
            }

            return Json(result);
        }


        [HttpPost]
        [ValidateInput(false)]
        public JsonResult repReportSummaryApprove(string gridHtml, string customerId, string productTypeId, string startDate, string endDate)
        {
            var result = new AjaxResult();
            try
            {
                ReportSummaryModels model = (ReportSummaryModels)Session["SummaryDetailModel"];
                model.activitySummaryList = model.activitySummaryList.Where(r => r.delFlag == false).ToList();
                string summaryId = ReportSummaryAppCode.insertActivitySummaryDetail(customerId, productTypeId, startDate, endDate, model);
                if (ReportSummaryAppCode.insertApproveForReportSummaryDetail(customerId, productTypeId, summaryId) > 0)
                {
                    var rootPath = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootRepDetailPdftURL"], summaryId));
                    List<Attachment> file = AppCode.genPdfFile(gridHtml, new Document(PageSize.A4.Rotate(), 2, 2, 10, 10), rootPath);
                    EmailAppCodes.sendApprove(summaryId, AppCode.ApproveType.Report_Detail, false);
                    result.Success = true;
                }
                else
                {
                    result.Success = false;
                    result.Message = AppCode.StrMessFail;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                ExceptionManager.WriteError(ex.Message);
            }

            return Json(result);
        }

    }
}