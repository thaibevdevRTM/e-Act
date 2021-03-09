using eActForm.BusinessLayer;
using eActForm.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using WebLibrary;
using System.Collections.Generic;


namespace eActForm.Controllers
{
    public partial class ExpenseEstimateReportController
    {
        // GET: partialTransferListDetail
        public ActionResult partialTransferListRpt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            activity_TBMMKT_Model.activityOfEstimateList = QueryGetActivityEstimateByActivityId.getByActivityId(activity_TBMMKT_Model.activityFormModel.id);
            if (!activity_TBMMKT_Model.activityOfEstimateList.Any())
            {
                activity_TBMMKT_Model.activityOfEstimateList.Add(new CostThemeDetailOfGroupByPriceTBMMKT());
                activity_TBMMKT_Model.activityFormModel.mode = AppCode.Mode.addNew.ToString();
            }

            return PartialView(activity_TBMMKT_Model);
        }

    }
}