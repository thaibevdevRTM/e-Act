using eActForm.BusinessLayer;
using eActForm.Models;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class SummaryReportController : Controller
    {
        // GET: summaryReport
        public ActionResult Index()
        {
            SearchActivityModels models = SearchAppCode.getMasterDataForSearch();
            return View(models);
        }



        public ActionResult getPreviewSummary(string startDate)
        {
            string redirect = "";
            try
            {
                string repDetail = "";
                ReportSummaryModels model = new ReportSummaryModels();
                ReportSummaryModels modelResult = new ReportSummaryModels();

                ViewBag.MouthText = DateTime.ParseExact(startDate, "dd/MM/yyyy", null).ToString("MMM yyyy");

                model = (ReportSummaryModels)Session["SummaryDetailModel"] ?? new ReportSummaryModels();
                model.activitySummaryList = model.activitySummaryList.Where(r => r.delFlag == false).ToList();
                repDetail = string.Join(",", model.activitySummaryList.Select(x => x.repDetailId));

                if (model.activitySummaryList.Any())
                {
                    if (model.activitySummaryList.FirstOrDefault().productTypeId == AppCode.nonAL 
                      || model.activitySummaryList.FirstOrDefault().productTypeId == AppCode.Food)
                    {
                        modelResult = ReportSummaryAppCode.getReportSummary(repDetail, startDate);
                        modelResult.producttype_id = AppCode.nonAL;
                        redirect = "viewReportSummary";
                    }
                    else
                    {
                        modelResult = ReportSummaryAppCode.getReportSummaryAlcohol(repDetail, startDate);
                        modelResult.producttype_id = AppCode.AL;
                        redirect = "viewReportSummaryAlcohol";
                    }
                }
                modelResult.subId = model.subId;
                modelResult.cusId = model.cusId;
                modelResult.flowList = model.flowList;

                Session["SummaryDetailModel"] = modelResult;


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }


            return RedirectToAction(redirect, new { startDate = startDate });
        }


        public ActionResult viewReportSummary(string startDate)
        {
            ReportSummaryModels model = null;
            try
            {

                ViewBag.startDate = startDate;
                ViewBag.MouthText = DateTime.ParseExact(startDate, "dd/MM/yyyy", null).ToString("MMM yyyy");
                model = (ReportSummaryModels)Session["SummaryDetailModel"] ?? new ReportSummaryModels();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }

            return PartialView(model);
        }


        public ActionResult viewReportSummaryAlcohol(string startDate)
        {
            ReportSummaryModels model = null;
            try
            {

                ViewBag.startDate = startDate;
                ViewBag.MouthText = DateTime.ParseExact(startDate, "MM/dd/yyyy", null).ToString("MMM yyyy");
                model = (ReportSummaryModels)Session["SummaryDetailModel"] ?? new ReportSummaryModels();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }

            return PartialView(model);
        }


        [HttpPost]
        [ValidateInput(false)]
        public FileResult summaryListViewExportExcel(string gridHtml)
        {
            try
            {
                //RepDetailModel.actFormRepDetails model = (RepDetailModel.actFormRepDetails)Session["ActFormRepDetail"] ?? new RepDetailModel.actFormRepDetails();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }

            return File(Encoding.UTF8.GetBytes(gridHtml), "application/vnd.ms-excel", "summaryReport.xls");
        }


        public ActionResult searchActForm()
        {
            try
            {
                ReportSummaryModels model = new ReportSummaryModels();
                DateTime startDate = Request["startDate"] == null ? DateTime.Now.AddDays(-7) : DateTime.ParseExact(Request.Form["startDate"], "dd/MM/yyyy", null);
                DateTime endDate = Request["endDate"] == null ? DateTime.Now : DateTime.ParseExact(Request.Form["endDate"], "dd/MM/yyyy", null);
                model.activitySummaryList = ReportSummaryAppCode.getSummaryDetailReportByDate(startDate, endDate);
                string chk = Request.Form["chk_Approve"];
                #region filter

                if (model.activitySummaryList.Any())
                {
                    if (Request.Form["txtRepDetailNo"] != "")
                    {
                        model.activitySummaryList = ReportSummaryAppCode.getFilterSummaryDetailByRepDetailNo(model.activitySummaryList, Request.Form["txtRepDetailNo"]);
                    }
                    if (Request.Form["ddlProductType"] != "")
                    {
                        model.activitySummaryList = ReportSummaryAppCode.getFilterSummaryDetailByProductType(model.activitySummaryList, Request.Form["ddlProductType"]);
                    }
                }

                if (chk == "true")
                {
                    if (Request.Form["ddlProductType"] == AppCode.nonAL)
                    {
                        model.subId = "8D527314-6D2D-45AF-995F-54144A705BBD";
                        model.producttype_id = AppCode.nonAL;
                        model.flowList = ApproveFlowAppCode.getFlowForReportDetail(
                                      model.subId
                                    , ""
                                    , AppCode.nonAL);
                    }
                    else
                    {
                        model.subId = "8D527314-6D2D-45AF-995F-54144A705BBD";
                        model.producttype_id = AppCode.AL;
                        model.flowList = ApproveFlowAppCode.getFlowForReportDetail(
                                    model.subId
                                    , ""
                                    , AppCode.AL);
                    }
                }
                else
                {
                    if (Request.Form["ddlProductType"] == AppCode.nonAL)
                    {
                        model.subId = "8D527314-6D2D-45AF-995F-54B44A705BBD";
                        model.producttype_id = AppCode.nonAL;
                        model.flowList = ApproveFlowAppCode.getFlowForReportDetail(
                                    model.subId
                                    , ""
                                    , AppCode.nonAL);
                    }
                    else
                    {
                        model.subId = "8D527314-6D2D-45AF-995F-54B44A705BBD";
                        model.producttype_id = AppCode.AL;
                        model.flowList = ApproveFlowAppCode.getFlowForReportDetail(
                                    model.subId
                                    , ""
                                    , AppCode.AL);
                    }
                }
                model.cusId = "";
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
                ViewBag.MouthText = DateTime.ParseExact(startDate, "dd/MM/yyyy", null).ToString("MMM yyyy");
                model = (ReportSummaryModels)Session["SummaryDetailModel"] ?? new ReportSummaryModels();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
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
                foreach (var item in model.activitySummaryList.Where(r => r.repDetailId == repId))
                {
                    item.delFlag = !delFlag;
                }


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
                DateTime p_startDate = Request["startDate"] == null ? DateTime.Now.AddDays(-7) : DateTime.ParseExact(startDate, "dd/MM/yyyy", null);
                DateTime p_endDate = Request["endDate"] == null ? DateTime.Now : DateTime.ParseExact(endDate, "dd/MM/yyyy", null);

                string summaryId = ReportSummaryAppCode.insertActivitySummaryDetail(model.cusId, model.producttype_id, p_startDate, p_endDate, model);
                if (ReportSummaryAppCode.insertApproveForReportSummaryDetail(model.subId, model.cusId, model.producttype_id, summaryId) > 0)
                {
                    var rootPath = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootSummaryDetailPdftURL"], summaryId));
                    List<Attachment> file = AppCode.genPdfFile(gridHtml, new Document(PageSize.A4.Rotate(), 2, 2, 10, 10), rootPath);
                    EmailAppCodes.sendApprove(summaryId, AppCode.ApproveType.Report_Summary, false);
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


        public ActionResult summaryApproveListView(string summaryId)
        {
            ReportSummaryModels modelResult = new ReportSummaryModels();
            try
            {

                modelResult = ReportSummaryAppCode.getReportSummaryApprove(summaryId);
                modelResult.flowList = ApproveFlowAppCode.getFlowByActFormId(summaryId);
                Session["SummaryDetailModel"] = modelResult;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }

            return RedirectToAction("viewReportSummary", new { startDate = modelResult.activitySummaryList.Count > 0 ? modelResult.activitySummaryList[0].createdDate.Value.ToString("MM/dd/yyyy") : DateTime.Now.ToString("MM/dd/yyyy") });
        }


        [HttpPost]
        [ValidateInput(false)]
        public JsonResult summaryDetailGenPDF(string gridHtml, string summaryId)
        {
            var result = new AjaxResult();
            try
            {
                result.Success = true;
                if ((int)AppCode.ApproveStatus.ไม่อนุมัติ == (int)ReportSummaryAppCode.getsummaryDetailStatus(summaryId))
                {
                    EmailAppCodes.sendRejectRepDetail();
                }
                else
                {
                    var rootPath = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootSummaryDetailPdftURL"], summaryId));
                    List<Attachment> file = AppCode.genPdfFile(gridHtml, new Document(PageSize.A4.Rotate(), 2, 2, 10, 10), rootPath);
                    EmailAppCodes.sendApprove(summaryId, AppCode.ApproveType.Report_Summary, false);
                    Session["SummaryDetailModel"] = null;
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




        //==========================VIEW DOCUMENT====================================

        public ActionResult IndexDoc()
        {


            SearchActivityModels models = SearchAppCode.getMasterDataForSearchForDetailReport("");
            return View(models);

        }

        public ActionResult searchActFormSummary(string activityType)
        {

            DateTime startDate = Request["startDate"] == null ? DateTime.Now.AddDays(-15) : DateTime.ParseExact(Request.Form["startDate"], "MM/dd/yyyy", null);
            DateTime endDate = Request["endDate"] == null ? DateTime.Now : DateTime.ParseExact(Request.Form["endDate"], "MM/dd/yyyy", null);
            ReportSummaryModels modelResult = new ReportSummaryModels();

            modelResult.summaryDetailLists = ReportSummaryAppCode.getDocumentSummaryDetailByDate(startDate, endDate);


            if (Request.Form["txtRepDetailNo"] != "")
            {
                modelResult.summaryDetailLists = modelResult.summaryDetailLists.Where(r => r.summaryId == ReportSummaryAppCode.getSummaryIdByRepdetail(Request.Form["txtRepDetailNo"].ToString())).ToList();
            }
            else
            {

                if (Request.Form["ddlProductType"] != "")
                {
                    modelResult.summaryDetailLists = modelResult.summaryDetailLists.Where(r => r.productTypeId == Request.Form["ddlProductType"]).ToList();
                }
            }
            TempData["SearchDataModelSummary"] = modelResult;
            return RedirectToAction("ListDoc");
        }

        public ActionResult ListDoc()
        {
            ReportSummaryModels modelResult = new ReportSummaryModels();
            if (TempData["SearchDataModelSummary"] != null)
            {
                modelResult = (ReportSummaryModels)TempData["SearchDataModelSummary"];
            }
            else
            {
                modelResult.summaryDetailLists = ReportSummaryAppCode.getDocumentSummaryDetailByDate(DateTime.Now.AddDays(-15), DateTime.Now);

            }

            TempData.Clear();
            return PartialView(modelResult);
        }
    }
}