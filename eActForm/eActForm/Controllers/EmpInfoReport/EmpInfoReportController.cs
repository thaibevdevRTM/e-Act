using eActForm.Models;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    [LoginExpire]
    public partial class EmpInfoReportController : Controller
    {

        public ActionResult empInfoRpt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            //empDetailList = QueryGet_empDetailById.getEmpDetailById(empId).ToList();

            //List<RequestEmpModel> empDetailList = new List<RequestEmpModel>();
            // activity_TBMMKT_Model.empInfoModel = eActController.QueryGet_empDetailById.getEmpDetailById(activity_TBMMKT_Model.).ToList();
            return PartialView(activity_TBMMKT_Model);
        }

    }
}