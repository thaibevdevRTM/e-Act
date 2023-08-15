using eActForm.BusinessLayer;
using eActForm.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    [LoginExpire]
    public partial class ExpenseEstimateReportController : Controller
    {

        public ActionResult expensesTrvDetailRpt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }

        public ActionResult expensesMedDetailRpt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }


    }
}