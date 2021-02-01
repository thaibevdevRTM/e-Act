using eActForm.Models;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    public partial class ExpenseEstimateInputController
    {
        // GET: partialTransferListDetail
        public ActionResult partialTransferList(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
    }
}