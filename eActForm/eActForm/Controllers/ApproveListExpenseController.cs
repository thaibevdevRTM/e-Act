using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    public class ApproveListExpenseController : Controller
    {
        // GET: ApproveListExpense
        public ActionResult Index()
        {
            SearchActivityModels models = SearchAppCode.getMasterDataForSearch();
            return View(models);
        }

        public ActionResult ApproveList()
        {
            exPerryCashModels model = new exPerryCashModels();
            if (TempData["ApproveSearchResult"] == null)
            {
                model.exPrettyModelList = exPerryCashAppCode.getApproveExpenseListsByEmpId();
                TempData["ApproveFormLists"] = model.exPrettyModelList;
            }
            else
            {
                //model.exPrettyModelList = (List<exPerryCashModels>)TempData["ApproveSearchResult"];
            }
            return PartialView(model);
        }

        public ActionResult ApproveDetail()
        {
            return View();
        }
    }
}