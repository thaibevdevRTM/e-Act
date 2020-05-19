using eActForm.BusinessLayer;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    public partial class ExpenseEstimateReportController
    {
        // GET: parialPriceStructureRpt
        public ActionResult partialProductCost(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            activity_TBMMKT_Model.productcostdetaillist1 = QueryGetCostDetailById.getcostDetailById(activity_TBMMKT_Model.activityFormTBMMKT.id);
            return PartialView(activity_TBMMKT_Model);
        }
    }
}