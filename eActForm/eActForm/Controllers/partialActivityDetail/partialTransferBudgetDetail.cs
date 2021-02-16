using eActForm.Models;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    public partial class partialActivityDetailController
    {
        // GET: partialTransferDetail
        public ActionResult transferBudgetDetail(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {


            return PartialView(activity_TBMMKT_Model);
        }
    }
}