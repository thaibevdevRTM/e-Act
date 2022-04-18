using eActForm.BusinessLayer;
using eActForm.Models;
using System.Linq;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    [LoginExpire]
    public partial class EmpInfoReportController : Controller
    {

        public ActionResult empInfoRpt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }

    }
}