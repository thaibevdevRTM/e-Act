using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eActForm.Models;
using eActForm.BusinessLayer;
using WebLibrary;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class ApproveListsRepDetailController : Controller
    {
        // GET: ApproveListsRepDetail
        public ActionResult Index()
        {
            SearchActivityModels models = new SearchActivityModels();
            try
            {
                models = SearchAppCode.getMasterDataForSearch();
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("ApproveListRepDetail >> Index >> " + ex.Message);
            }
            return View(models);
        }

        public ActionResult ListView()
        {
            RepDetailModel.actApproveRepDetailModels model = new RepDetailModel.actApproveRepDetailModels();
            try
            {
                if (TempData["ApproveSearchResult"] == null)
                {
                    model.repDetailLists = ApproveRepDetailAppCode.getApproveRepDetailListsByEmpId();
                    TempData["ApproveFormLists"] = model.repDetailLists;
                }
                else
                {
                    model.repDetailLists = (List<RepDetailModel.actApproveRepDetailModel>)TempData["ApproveSearchResult"];
                }
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("ApproveListRepDetail >> ListView >> " + ex.Message);
            }
            return PartialView(model);
        }

        public ActionResult searchActForm()
        {
            RepDetailModel.actApproveRepDetailModels model = new RepDetailModel.actApproveRepDetailModels();
            try
            {
                model.repDetailLists = (List<RepDetailModel.actApproveRepDetailModel>)TempData["ApproveFormLists"];

                if (Request.Form["ddlStatus"] != "")
                {
                    model.repDetailLists = ApproveRepDetailAppCode.getFilterFormByStatusId(model.repDetailLists, int.Parse(Request.Form["ddlStatus"]));
                }
                TempData["ApproveSearchResult"] = model.repDetailLists;
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("ApproveListRepDetail >> searchActForm >> " + ex.Message);
            }
            return RedirectToAction("ListView");
        }
    }
}