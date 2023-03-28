using eActForm.BusinessLayer;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebLibrary;


namespace eActForm.Controllers
{
    public partial class ExpenseEstimateInputController
    {
        // GET: partialTransferListDetail
        public ActionResult partialTransferList(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            activity_TBMMKT_Model.activityOfEstimateList = QueryGetActivityEstimateByActivityId.getByActivityId(activity_TBMMKT_Model.activityFormModel.id);

            for (int i = 0; i < activity_TBMMKT_Model.activityOfEstimateList.Count; i++)
            {
                activity_TBMMKT_Model.activityOfEstimateList[i].productName = QueryGetAllBrand.GetAllBrand().Where(x => x.id.Contains(activity_TBMMKT_Model.activityOfEstimateList[i].productId)).FirstOrDefault().brandName;
                if (!string.IsNullOrEmpty(activity_TBMMKT_Model.activityOfEstimateList[i].typeTheme))
                {
                    activity_TBMMKT_Model.activityOfEstimateList[i].productName += ", " + QueryGetAllChanel.getAllChanel().Where(x => x.id.Contains(activity_TBMMKT_Model.activityOfEstimateList[i].typeTheme)).FirstOrDefault().chanelGroup + "," + activity_TBMMKT_Model.activityOfEstimateList[i].UseYearSelect;
                }
                else
                {
                    activity_TBMMKT_Model.activityOfEstimateList[i].productName += ", " + QueryGetAllActivityGroup.getActivityGroupBudgetControl("bg").Where(x => x.id.Contains(activity_TBMMKT_Model.activityOfEstimateList[i].activityTypeId)).FirstOrDefault().activitySales + "," + activity_TBMMKT_Model.activityOfEstimateList[i].UseYearSelect;

                }

            }
            if (!activity_TBMMKT_Model.activityOfEstimateList.Any())
            {
                activity_TBMMKT_Model.activityOfEstimateList.Add(new CostThemeDetailOfGroupByPriceTBMMKT());
                activity_TBMMKT_Model.activityFormModel.mode = AppCode.Mode.addNew.ToString();
            }

            return PartialView(activity_TBMMKT_Model);
        }

    }
}