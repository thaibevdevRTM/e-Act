using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eActForm.Models;
using eActForm.BusinessLayer;
namespace eActForm.Controllers
{
    [LoginExpire]
    public class ApproveListsRepDetailController : Controller
    {
        // GET: ApproveListsRepDetail
        public ActionResult Index()
        {
            SearchActivityModels models = SearchAppCode.getMasterDataForSearch();
            return View(models);
        }

        public ActionResult ListView()
        {
            RepDetailModel.actApproveRepDetailModels model = new RepDetailModel.actApproveRepDetailModels();
            if (TempData["ApproveSearchResult"] == null)
            {
                model.repDetailLists = ApproveRepDetailAppCode.getApproveRepDetailListsByEmpId();
                TempData["ApproveFormLists"] = model.repDetailLists;
            }
            else
            {
                model.repDetailLists = (List<RepDetailModel.actApproveRepDetailModel>)TempData["ApproveSearchResult"];
            }
            return PartialView(model);
        }

        public ActionResult searchActForm()
        {
            RepDetailModel.actApproveRepDetailModels model = new RepDetailModel.actApproveRepDetailModels();
            model.repDetailLists = (List<RepDetailModel.actApproveRepDetailModel>)TempData["ApproveFormLists"];

            if (Request.Form["ddlStatus"] != "")
            {
                model.repDetailLists = ApproveRepDetailAppCode.getFilterFormByStatusId(model.repDetailLists, int.Parse(Request.Form["ddlStatus"]));
            }
            TempData["ApproveSearchResult"] = model.repDetailLists;
            return RedirectToAction("ListView");
        }
    }
}