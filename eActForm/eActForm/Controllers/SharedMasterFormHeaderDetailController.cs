using eActForm.Models;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class SharedMasterFormHeaderDetailController : Controller
    {
        public ActionResult headerDetailsDate(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult headerDetails(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
    }
}