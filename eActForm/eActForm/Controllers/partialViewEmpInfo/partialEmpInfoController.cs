using System.Web.Mvc;

namespace eActForm.Controllers
{
    public class partialEmpInfoController : Controller
    {
        // GET: partialEmpInfo
        public ActionResult index()
        {
            return PartialView();
        }
    }
}