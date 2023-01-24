using eActForm.BusinessLayer;
using eActForm.Models;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    public partial class ExpenseEstimateReportController
    {
        // GET: parialPriceStructureRpt
        public ActionResult parialPriceStructureRpt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {

            return PartialView(activity_TBMMKT_Model);
        }
    }
}