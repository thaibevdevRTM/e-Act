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
                //model.actLists = (List<Activity_Model.actForm>)TempData["ApproveSearchResult"];
            }
            return PartialView(model);
        }

        public ActionResult searchActForm()
        {
            Activity_Model.actForms model = new Activity_Model.actForms();
            model.actLists = (List<Activity_Model.actForm>)TempData["ApproveFormLists"];

            if (Request.Form["ddlStatus"] != "")
            {
                model.actLists = ApproveListAppCode.getFilterFormByStatusId(model.actLists, int.Parse(Request.Form["ddlStatus"]));
            }
            TempData["ApproveSearchResult"] = model.actLists;
            return RedirectToAction("ListView");
        }
    }
}