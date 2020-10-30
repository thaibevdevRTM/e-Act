using eActForm.BusinessLayer;
using eActForm.Models;
using eForms.Models.Forms;
using eForms.Models.Reports;
using eForms.Presenter.Reports;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class actFormRepPostEvaController : Controller
    {
        // GET: actFormRepPostEva
        public ActionResult index()
        {
            SearchActivityModels models = SearchAppCode.getMasterDataForSearchForDetailReport("");
            models.showUIModel = new searchParameterFilterModel { isShowActNo = false, isShowStatus = false, isShowActType = true, isShowProductGroup = true, isShowProductType = true, isShowMonthText = false , isShowProductBrand = true};
            return View(models);
        }

        [HttpPost]
        [ValidateInput(false)]
        public FileResult repExportExcel(string gridHtml)
        {
            try
            {
                //RepDetailModel.actFormRepDetails model = (RepDetailModel.actFormRepDetails)Session["ActFormRepDetail"] ?? new RepDetailModel.actFormRepDetails();
                //gridHtml = gridHtml.Replace("\n", "<br>");
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }

            return File(Encoding.UTF8.GetBytes(gridHtml), "application/vnd.ms-excel", "DetailReport.xls");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult searchActForm()
        {
            try
            {

                return RedirectToAction("postEvaListView", new { startDate = Request.Form["startDate"], endDate = Request.Form["endDate"], customerId = Request.Form["ddlCustomer"], mountText = Request.Form["reportDate"], productType = Request.Form["ddlProductType"]
                , productGroup = Request.Form["ddlProductGrp"] , productBrand = Request.Form["ddlProductBrand"], actType = Request.Form["ddlTheme"]
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActionResult postEvaListView(string startDate, string endDate, string customerId, string mountText , string productType,string productGroup,string productBrand,string actType)
        {
            RepPostEvaModels model = null;
            try
            {
                ViewBag.mountText = mountText;
                model = RepPostEvaPresenter.getDataPostEva(AppCode.StrCon, startDate, endDate, customerId);
                model.repPostEvaLists = RepPostEvaPresenter.filterConditionPostEva(model.repPostEvaLists ,productType, productGroup, productBrand, actType);
                //model.repPostEvaGroupBrand = RepPostEvaPresenter.getPostEvaGroupByBrand(model.repPostEvaLists);
                Session["postEvaModel"] = model;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("postEvaListView >> " + ex.Message);
            }
            return PartialView(model);
        }

        public ActionResult postEvaGroupBrandView(RepPostEvaModels model)
        {
            try
            {
                // model.repPostEvaGroupBrand = RepPostEvaPresenter.getPostEvaGroupByBrand(model.repPostEvaLists);
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("PostEvaGroupBrandView >> " + ex.Message);
            }
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult postEvaGroupBrandData()
        {
            List<RepPostEvaGroup> list = null;
            try
            {
                RepPostEvaModels model = (RepPostEvaModels)Session["postEvaModel"];
                list = RepPostEvaPresenter.getPostEvaGroupByBrand(model.repPostEvaLists);
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("PostEvaGroupBrandView >> " + ex.Message);
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

    }
}