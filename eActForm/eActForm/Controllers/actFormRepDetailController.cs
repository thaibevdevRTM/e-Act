using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using eActForm.BusinessLayer;
using eActForm.Models;
using iTextSharp.text;
using WebLibrary;
namespace eActForm.Controllers
{
    [LoginExpire]
    public class actFormRepDetailController : Controller
    {
        // GET: actFormRepDetail
        public ActionResult Index()
        {
            SearchActivityModels models = SearchAppCode.getMasterDataForSearch();
            return View(models);
        }

        public ActionResult searchActForm()
        {
            try
            {
                RepDetailModel.actFormRepDetails model = new RepDetailModel.actFormRepDetails();
                model.actFormRepDetailLists = RepDetailAppCode.getRepDetailReportByCreateDateAndStatusId(Request.Form["startDate"], Request.Form["endDate"]);
                if (Request.Form["txtActivityNo"] != "")
                {
                    model.actFormRepDetailLists = RepDetailAppCode.getFilterRepDetailByActNo(model.actFormRepDetailLists, Request.Form["txtActivityNo"]);
                }
                else
                {
                    #region filter
                    if (Request.Form["ddlStatus"] != "")
                    {
                        model.actFormRepDetailLists = RepDetailAppCode.getFilterRepDetailByStatusId(model.actFormRepDetailLists, Request.Form["ddlStatus"]);
                    }
                    if (Request.Form["ddlCustomer"] != "")
                    {
                        model.actFormRepDetailLists = RepDetailAppCode.getFilterRepDetailByCustomer(model.actFormRepDetailLists, Request.Form["ddlCustomer"]);
                    }
                    if (Request.Form["ddlTheme"] != "")
                    {
                        model.actFormRepDetailLists = RepDetailAppCode.getFilterRepDetailByActivity(model.actFormRepDetailLists, Request.Form["ddlTheme"]);
                    }
                    if (Request.Form["ddlProductType"] != "")
                    {
                        model.actFormRepDetailLists = RepDetailAppCode.getFilterRepDetailByProductType(model.actFormRepDetailLists, Request.Form["ddlProductType"]);
                    }
                    if (Request.Form["ddlProductGrp"] != "")
                    {
                        model.actFormRepDetailLists = RepDetailAppCode.getFilterRepDetailByProductGroup(model.actFormRepDetailLists, Request.Form["ddlProductGrp"]);
                    }
                    if (Request.Form["ddlCustomer"] != "" && Request.Form["ddlProductType"] != "")
                    {
                        model.flowList = ApproveFlowAppCode.getFlowForReportDetail(
                                        ConfigurationManager.AppSettings["subjectReportDetailId"]
                                        , Request.Form["ddlCustomer"]
                                        , Request.Form["ddlProductType"]);
                    }
                    #endregion
                }
                TempData["ActFormRepDetail"] = model;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }

            return RedirectToAction("repListView", new { startDate = Request.Form["startDate"] });
        }

        public ActionResult approvePositionSignature(ApproveFlowModel.approveFlowModel flowModel)
        {
            return PartialView(flowModel);
        }

        /// <summary>
        /// for approve
        /// </summary>
        /// <param name="actId"></param>
        /// <returns></returns>
        public ActionResult repPreviewListView(string actId)
        {
            RepDetailModel.actFormRepDetails model = new RepDetailModel.actFormRepDetails();
            try
            {
                model.actFormRepDetailLists = RepDetailAppCode.getRepDetailReportByCreateDateAndStatusId(actId);
                model.flowList = ApproveFlowAppCode.getFlowByActFormId(actId);
                TempData["ActFormRepDetail"] = model;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }

            return RedirectToAction("repListView", new { startDate = model.actFormRepDetailLists.Count > 0 ? model.actFormRepDetailLists[0].createdDate.Value.ToString("MM/dd/yyyy") : DateTime.Now.ToString("MM/dd/yyyy") });
        }
        public ActionResult repListView(string startDate)
        {
            RepDetailModel.actFormRepDetails model = null;
            try
            {
                model = (RepDetailModel.actFormRepDetails)TempData["ActFormRepDetail"] ?? new RepDetailModel.actFormRepDetails();
                ViewBag.MouthText = DateTime.ParseExact(startDate, "MM/dd/yyyy", null).ToString("MMM yyyy");
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }

            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult repReportDetailApprove(string gridHtml, string customerId, string productTypeId, string startDate, string endDate)
        {
            var result = new AjaxResult();
            try
            {
                string actRepDetailId = ApproveRepDetailAppCode.insertActivityRepDetail(customerId, productTypeId, startDate, endDate);
                if (ApproveRepDetailAppCode.insertApproveForReportDetail(customerId, productTypeId, actRepDetailId) > 0)
                {
                    var rootPath = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootRepDetailPdftURL"], actRepDetailId));
                    List<Attachment> file = AppCode.genPdfFile(gridHtml, new Document(PageSize.A4.Rotate(), 2, 2, 10, 10), rootPath);
                    EmailAppCodes.sendApprove(actRepDetailId, AppCode.ApproveType.Report_Detail);
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

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult repDetailGenPDF(string gridHtml, string actId)
        {
            var result = new AjaxResult();
            try
            {
                var rootPath = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootRepDetailPdftURL"], actId));
                List<Attachment> file = AppCode.genPdfFile(gridHtml, new Document(PageSize.A4.Rotate(), 2, 2, 10, 10), rootPath);
                EmailAppCodes.sendApprove(actId, AppCode.ApproveType.Report_Detail);
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
    }
}