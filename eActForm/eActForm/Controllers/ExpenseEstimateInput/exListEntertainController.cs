using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    public partial class ExpenseEstimateInputController
    {
        // GET: exListEntertain


        public ActionResult expensesEntertainList(Activity_TBMMKT_Model activity_TBMMKT_Model, string actId)
        {
            try
            {
                var estimateList = QueryGetActivityEstimateByActivityId.getByActivityId(activity_TBMMKT_Model.activityFormModel.id);
                activity_TBMMKT_Model.activityOfEstimateList = estimateList.Where(x => x.activityTypeId.Equals("1")).ToList();
                if (!activity_TBMMKT_Model.activityOfEstimateList.Any())
                {
                    activity_TBMMKT_Model.activityOfEstimateList.Add(new CostThemeDetailOfGroupByPriceTBMMKT());
                    activity_TBMMKT_Model.activityFormModel.mode = AppCode.Mode.addNew.ToString();
                }

                activity_TBMMKT_Model.activityOfEstimateList2 = estimateList.Where(x => x.activityTypeId.Equals("2")).ToList();
                if (!activity_TBMMKT_Model.activityOfEstimateList2.Any())
                {
                    activity_TBMMKT_Model.activityOfEstimateList2.Add(new CostThemeDetailOfGroupByPriceTBMMKT());
                    activity_TBMMKT_Model.activityFormModel.mode = AppCode.Mode.addNew.ToString();
                }
                activity_TBMMKT_Model.objExpenseCashList = QueryOtherMaster.getOhterMaster("CashLimitType","");

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("expensesEntertainList => " + ex.Message);
            }
            return PartialView(activity_TBMMKT_Model);
        }

        public JsonResult getLimitEmpByEmpId(string empId)
        {
            List<exPerryCashModel> empCashList = new List<exPerryCashModel>();
            var result = new AjaxResult();
            try
            {
                empCashList = expensesEntertainAppCode.getLimitByEmpId(empId).ToList();
                if (empCashList.Any())
                {
                    var resultData = new
                    {
                        getEntertain = empCashList.ToList()
                    };
                    result.Data = resultData;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getLimitEmpByEmpId => " + ex.Message);
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult getCumulativeByEmpId(string docDate, string empId)
        {
            List<exPerryCashModel> empCashList = new List<exPerryCashModel>();
            var result = new AjaxResult();
            try
            {
                docDate = BaseAppCodes.converStrToDatetimeWithFormat(docDate + "-" + DateTime.Today.ToString("dd"), "yyyy-MM-dd").ToString("dd/MM/yyyy");

                empCashList = expensesEntertainAppCode.getAmountLimitByEmpId(empId, docDate).ToList();
                if (empCashList.Any())
                {
                    var resultData = new
                    {
                        empList = empCashList.ToList()
                    };
                    result.Data = resultData;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getLimitEmpByEmpId => " + ex.Message);
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}