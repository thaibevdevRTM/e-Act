using eActForm.BusinessLayer;
using eActForm.Models;
using System;
using System.Web.Mvc;
using eForms.Models.Reports;
using eForms.Presenter.Reports;
using WebLibrary;
using System.Collections.Generic;
using eForms.Models.Forms;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class actFormRepPostEvaController : Controller
    {
        // GET: actFormRepPostEva
        public ActionResult index()
        {
            SearchActivityModels models = SearchAppCode.getMasterDataForSearchForDetailReport();
            models.showUIModel = new searchParameterFilterModel { isShowActNo = false, isShowStatus = false, isShowActType = false, isShowProductGroup = false, isShowProductType = false, isShowMonthText = false };
            return View(models);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult searchActForm()
        {
            try
            {

                return RedirectToAction("postEvaListView", new { startDate = Request.Form["startDate"], endDate = Request.Form["endDate"], customerId = Request.Form["ddlCustomer"], mountText = Request.Form["reportDate"] });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActionResult postEvaListView(string startDate, string endDate, string customerId, string mountText)
        {
            RepPostEvaModels model = null;
            try
            {
                ViewBag.mountText = mountText;
                model = RepPostEvaPresenter.getDataPostEva(AppCode.StrCon, startDate, endDate, customerId);
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