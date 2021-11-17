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
        public JsonResult getBudgetBalanceByEOIO(string EO, string IO,string fiscalYear)
        {
            var result = new AjaxResult();
            try
            {
               if (!string.IsNullOrEmpty(IO))
                {
                    var budgetPrice = TransferBudgetAppcode.GetBudgetBalanceByEOIO(EO, IO, fiscalYear);

                    if (budgetPrice.Any())
                    {
                        var resultData = new
                        {
                            budgetPrice = budgetPrice.FirstOrDefault().total,
                            paymentBlance = budgetPrice.FirstOrDefault().paymentBlance > 0 ? budgetPrice.FirstOrDefault().total - budgetPrice.FirstOrDefault().paymentBlance :0

                        };
                        result.Data = resultData;
                        result.Success = true;
                    }
                }
                
                
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                ExceptionManager.WriteError("getBudgetEOIOByAct => " + ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getBudgetBalanceNonEO(string brandId, string channelId, string activityGroupId , string bgYear)
        {
            var result = new AjaxResult();
            try
            {
                var budgetPrice = TransferBudgetAppcode.GetBudgetBalanceNonEO(brandId, channelId,activityGroupId, bgYear);
                var resultData = new
                {

                    EO = budgetPrice.Any() ? budgetPrice.FirstOrDefault().EO : "",
                    amount = budgetPrice.Any() ? budgetPrice.FirstOrDefault().amount : 0,
     
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