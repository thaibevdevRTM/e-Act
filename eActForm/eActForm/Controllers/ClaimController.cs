using eActForm.BusinessLayer;
using eActForm.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using WebLibrary;
using System.Configuration;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class ClaimController : Controller
    {
        // GET: Claim
        public ActionResult claimIndex()
        {
            return View();
        }

        public ActionResult searchClaimActivityForm(string typeForm)
        {
            string act_createdDateStart = DocumentsAppCode.convertDateTHToShowCultureDateEN(DateTime.Now.AddYears(-10), ConfigurationManager.AppSettings["formatDateUse"]);
            string act_createdDateEnd = DocumentsAppCode.convertDateTHToShowCultureDateEN(DateTime.Now.AddYears(2), ConfigurationManager.AppSettings["formatDateUse"]);
            string act_budgetStatusIdin = null;

            Budget_Activity_Model budget_activity = new Budget_Activity_Model();

            try
            {
                #region filter
                if (Request.Form["chk_all"] != null && Request.Form["chk_all"] == "true")
                {
                    act_createdDateStart = DocumentsAppCode.convertDateTHToShowCultureDateEN(DateTime.Now.AddYears(-10), ConfigurationManager.AppSettings["formatDateUse"]);
                    act_createdDateEnd = DocumentsAppCode.convertDateTHToShowCultureDateEN(DateTime.Now.AddYears(2), ConfigurationManager.AppSettings["formatDateUse"]);

                }
                else
                {
                    act_createdDateStart = DocumentsAppCode.convertDateTHToShowCultureDateEN(DateTime.ParseExact(Request.Form["startDate"].Trim(), "MM/dd/yyyy", null), ConfigurationManager.AppSettings["formatDateUse"]);
                    act_createdDateEnd = DocumentsAppCode.convertDateTHToShowCultureDateEN(DateTime.ParseExact(Request.Form["endDate"].Trim(), "MM/dd/yyyy", null), ConfigurationManager.AppSettings["formatDateUse"]);
                }

                if (Request.Form["ddlFormType"] == "Select All")
                {
                    typeForm = null;
                }
                else
                {
                    typeForm = Request.Form["ddlFormType"];
                }

                if (Request.Form["ddlBudgetStatusId"] != "" && Request.Form["ddlBudgetStatusId"] != "Select All")
                {
                    act_budgetStatusIdin = Request.Form["ddlBudgetStatusId"];
                }
                #endregion

                budget_activity.Budget_Activity_list = QueryGetBudgetActivity.getBudgetActivityList("3", null, null, null, typeForm, act_createdDateStart, act_createdDateEnd, act_budgetStatusIdin, null).ToList();
                TempData["searchBudgetActivityForm"] = budget_activity;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("searchBudgetActivityForm --> " + ex.Message);
            }

            return RedirectToAction("claimList");
        }

        public ActionResult claimList(string typeForm)
        {
            string act_createdDateStart = DocumentsAppCode.convertDateTHToShowCultureDateEN(DateTime.Now.AddDays(-30), ConfigurationManager.AppSettings["formatDateUse"]);
            string act_createdDateEnd = DocumentsAppCode.convertDateTHToShowCultureDateEN(DateTime.Now, ConfigurationManager.AppSettings["formatDateUse"]);
            Budget_Activity_Model models = new Budget_Activity_Model();

            try
            {
                if (TempData["searchBudgetActivityForm"] != null)
                {
                    models = (Budget_Activity_Model)TempData["searchBudgetActivityForm"];
                }
                else
                {
                    models.Budget_Activity_list = QueryGetBudgetActivity.getBudgetActivityList("3", null, null, null, typeForm, act_createdDateStart, act_createdDateEnd, null, null).ToList();
                }
                TempData["searchBudgetActivityForm"] = null;
                return PartialView(models);
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("claimList --> " + ex.Message);
            }
            return PartialView(models);
        }

        public ActionResult claimProduct(string activityId)
        {
            Budget_Activity_Model budget_activity = new Budget_Activity_Model();
            string act_createdDateStart = DocumentsAppCode.convertDateTHToShowCultureDateEN(DateTime.Now.AddYears(-10), ConfigurationManager.AppSettings["formatDateUse"]);
            string act_createdDateEnd = DocumentsAppCode.convertDateTHToShowCultureDateEN(DateTime.Now.AddYears(2), ConfigurationManager.AppSettings["formatDateUse"]);

            if (activityId == null) { activityId = Session["activityId"].ToString(); }

            if (activityId == null) return RedirectToAction("claimList", "Claim");
            else
            {
                try
                {
                    budget_activity.Budget_Activity_list = QueryGetBudgetActivity.getBudgetActivityList("3", activityId, null, null, null, act_createdDateStart, act_createdDateEnd, null, null).ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return View(budget_activity);
        }

        public ActionResult claimProductList(string activityId)
        {

            Session["activityId"] = activityId;
            Budget_Activity_Model budget_activity = new Budget_Activity_Model();
            string act_createdDateStart = DocumentsAppCode.convertDateTHToShowCultureDateEN(DateTime.Now.AddYears(-10), ConfigurationManager.AppSettings["formatDateUse"]);
            string act_createdDateEnd = DocumentsAppCode.convertDateTHToShowCultureDateEN(DateTime.Now.AddYears(2), ConfigurationManager.AppSettings["formatDateUse"]);

            try
            {
                budget_activity.Budget_Activity = QueryGetBudgetActivity.getBudgetActivityList(null, activityId, null, null, null, null, null, null, null).FirstOrDefault(); ;
                budget_activity.Budget_Activity_Product_list = QueryGetBudgetActivity.getBudgetActivityProduct(activityId, null);
                budget_activity.Budget_Activity_Ststus_list = QueryGetBudgetActivity.getBudgetActivityStatus();
                budget_activity.Budget_Activity_Last_Approve = QueryGetBudgetActivity.getBudgetActivityLastApprove(activityId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return PartialView(budget_activity);
        }

        public PartialViewResult claimProductInvoiceList(string activityId, string activityOfEstimateId)
        {
            Budget_Activity_Model budget_activity_model = new Budget_Activity_Model();
            string act_createdDateStart = DocumentsAppCode.convertDateTHToShowCultureDateEN(DateTime.Now.AddYears(-10), ConfigurationManager.AppSettings["formatDateUse"]);
            string act_createdDateEnd = DocumentsAppCode.convertDateTHToShowCultureDateEN(DateTime.Now.AddYears(2), ConfigurationManager.AppSettings["formatDateUse"]);

            budget_activity_model.Budget_Activity = QueryGetBudgetActivity.getBudgetActivityList(null, activityId, null, null, null, null, null, null, null).FirstOrDefault(); ;
            budget_activity_model.Budget_Activity_Invoice_list = QueryGetBudgetActivity.getBudgetActivityInvoice(activityId, activityOfEstimateId, null);
            //budget_activity_model.Budget_Activity_Last_Approve = QueryGetBudgetActivity.getBudgetActivityLastApprove(activityId).FirstOrDefault();

            return PartialView(budget_activity_model);
        }

        public PartialViewResult claimProductInvoiceEdit(string activityId, string activityOfEstimateId, string invoiceId)
        {
            Budget_Activity_Model Budget_Activity = new Budget_Activity_Model();
            Budget_Activity.Budget_Activity_Product = QueryGetBudgetActivity.getBudgetActivityProduct(activityId, activityOfEstimateId).FirstOrDefault();
            Budget_Activity.Budget_Activity_Ststus_list = QueryGetBudgetActivity.getBudgetActivityStatus();
            //Budget_Activity.Budget_Count_Wait_Approve = QueryGetBudgetActivity.getBudgetActivityWaitApprove(activityId).FirstOrDefault();


            if (!string.IsNullOrEmpty(invoiceId))
            {// for get invoice history 
                Budget_Activity.Budget_Activity_Invoice = QueryGetBudgetActivity.getBudgetActivityInvoice(activityId, activityOfEstimateId, invoiceId).FirstOrDefault();
            }
            return PartialView(Budget_Activity);
        }


    }
}