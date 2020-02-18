using eActForm.Models;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class SharedMasterFormHeaderCompanyController : Controller
    {
        public ActionResult headerCompany(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
    }
}