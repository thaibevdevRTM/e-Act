using eActForm.BusinessLayer;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using System;
using System.Collections.Generic;
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
    }
}