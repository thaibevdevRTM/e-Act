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
        // GET: expenseEntertainReport
        public ActionResult reportExpenseEntertain(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            var estimateList = activity_TBMMKT_Model.activityOfEstimateList;
            activity_TBMMKT_Model.activityOfEstimateList = estimateList.Where(x => x.activityTypeId == "1").ToList();
            activity_TBMMKT_Model.activityOfEstimateList2 = estimateList.Where(x => x.activityTypeId == "2").ToList();
            activity_TBMMKT_Model.masterRequestEmp = QueryGet_empDetailById.getEmpDetailById(activity_TBMMKT_Model.activityFormTBMMKT.empId);
            return PartialView(activity_TBMMKT_Model);
        }
    }
}