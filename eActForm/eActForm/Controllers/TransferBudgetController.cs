using eActForm.BusinessLayer.Appcodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    public class TransferBudgetController : Controller
    {
        // GET: transferBudget
        public JsonResult getBudgetBalanceByEOIO(string EO, string IO)
        {
            var result = new AjaxResult();
            try
            {

                var budgetPrice = transferBudgetAppcode.GetBudgetBalanceByEOIO(EO, IO);

                var resultData = new
                {
                    budgetPrice = budgetPrice.FirstOrDefault().total,
                    paymentBlance = budgetPrice.FirstOrDefault().total - budgetPrice.FirstOrDefault().paymentBlance

                };

                result.Data = resultData;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                ExceptionManager.WriteError("getBudgetEOIOByAct => " + ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getBudgetBalanceNonEO(string brandId, string channelId, string activityGroupId)
        {
            var result = new AjaxResult();
            try
            {

                var budgetPrice = transferBudgetAppcode.GetBudgetBalanceNonEO(brandId, channelId, activityGroupId);

                var resultData = new
                {
                    EO = budgetPrice.FirstOrDefault().EO,
                    amount = budgetPrice.FirstOrDefault().amount

                };

                result.Data = resultData;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                ExceptionManager.WriteError("getBudgetEOIOByAct => " + ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}