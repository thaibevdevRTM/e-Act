using eActForm.BusinessLayer;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    [LoginExpire]

    public class BudgetApproveListController : Controller
    {
        public ActionResult Index()
        {
            SearchActivityModels models = SearchAppCode.getMasterDataForSearch();
            return View(models);
        }

        public ActionResult searchActForm()
        {

            string count = Request.Form.AllKeys.Count().ToString();

            DateTime startDate = Request["startDate"] == null ? DateTime.Now.AddDays(-15) : DateTime.ParseExact(Request.Form["startDate"], "dd/MM/yyyy", null);
            DateTime endDate = Request["endDate"] == null ? DateTime.Now : DateTime.ParseExact(Request.Form["endDate"], "dd/MM/yyyy", null);

            Budget_Approve_Detail_Model.budgetForms model = new Budget_Approve_Detail_Model.budgetForms();
            model.budgetFormLists = (List<Budget_Approve_Detail_Model.budgetForm>)TempData["ApproveFormLists"];

            if (!string.IsNullOrEmpty(Request.Form["txtActivityNo"]))
            {
                model.budgetFormLists = model.budgetFormLists.Where(r => r.activityNo == Request.Form["txtActivityNo"]).ToList();
            }

            if (!string.IsNullOrEmpty(Request.Form["ddlStatus"]))
            {
                model.budgetFormLists = model.budgetFormLists.Where(r => r.statusId == Request.Form["ddlStatus"]).ToList();
            }

            if (!string.IsNullOrEmpty(Request.Form["ddlCustomer"]))
            {
                model.budgetFormLists = model.budgetFormLists.Where(r => r.customerId == Request.Form["ddlCustomer"]).ToList();
            }

            if (!string.IsNullOrEmpty(Request.Form["ddlTheme"]))
            {
                model.budgetFormLists = model.budgetFormLists.Where(r => r.theme == Request.Form["ddlTheme"]).ToList();
            }

            if (!string.IsNullOrEmpty(Request.Form["ddlProductType"]))
            {
                model.budgetFormLists = model.budgetFormLists.Where(r => r.productTypeId == Request.Form["ddlProductType"]).ToList();
            }

            if (!string.IsNullOrEmpty(Request.Form["ddlProductGrp"]))
            {
                model.budgetFormLists = model.budgetFormLists.Where(r => r.productGroupid == Request.Form["ddlProductGrp"]).ToList();
            }

            TempData["ApproveSearchResult"] = model.budgetFormLists;
            return RedirectToAction("budgetApproveList");


        }

        public ActionResult budgetApproveList()
        {

            Budget_Approve_Detail_Model.budgetForms model = new Budget_Approve_Detail_Model.budgetForms();
            if (TempData["ApproveSearchResult"] == null)
            {
                model = new Budget_Approve_Detail_Model.budgetForms();
                model.budgetFormLists = QueryGetBudgetApprove.getApproveListsByEmpId(UtilsAppCode.Session.User.empId);
                TempData["ApproveFormLists"] = model.budgetFormLists;
                model.budgetFormLists = BudgetApproveController.getFilterFormByStatusId(model.budgetFormLists, (int)AppCode.ApproveStatus.รออนุมัติ);
            }
            else
            {
                model.budgetFormLists = (List<Budget_Approve_Detail_Model.budgetForm>)TempData["ApproveSearchResult"];
            }
            return PartialView(model);
        }
    }
}