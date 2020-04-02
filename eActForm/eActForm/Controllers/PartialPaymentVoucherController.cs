using eActForm.Models;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class PartialPaymentVoucherController : Controller
    {
        public ActionResult headerPv(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model); 
        }
        public ActionResult headerPvDetails(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult detailSectionOneToFive(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult detailSectionSix(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
    }
}