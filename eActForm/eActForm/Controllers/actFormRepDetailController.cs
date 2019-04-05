using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
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
                model.actFormRepDetailLists = RepDetailAppCode.getRepDetailReportByCreateDateAndStatusId(Request.Form["startDate"]
                    , Request.Form["endDate"]);

                if (Request.Form["txtActivityNo"] != "")
                {
                    model.actFormRepDetailLists = RepDetailAppCode.getFilterRepDetailByActNo(model.actFormRepDetailLists, Request.Form["txtActivityNo"]);
                }
                else
                {
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
                }


                TempData["ActFormRepDetail"] = model;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }

            return RedirectToAction("repListView");
        }

        public ActionResult repListView()
        {
            RepDetailModel.actFormRepDetails model = null;
            try
            {
                model = (RepDetailModel.actFormRepDetails)TempData["ActFormRepDetail"] ?? new RepDetailModel.actFormRepDetails();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }

            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult repReportDetailApprove(string gridHtml)
        {
            var result = new AjaxResult();
            try
            {
                AppCode.genPdfFile(gridHtml, new Document(PageSize.A4.Rotate(), 2, 2, 10, 10), "pdfRepDetail");
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