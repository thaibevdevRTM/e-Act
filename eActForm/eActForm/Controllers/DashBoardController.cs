using eActForm.BusinessLayer;
using eActForm.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class DashBoardController : Controller
    {
        // GET: DashBoard
        public ActionResult Index()
        {

            DashBoardModel.infoDashBoardModels model = new DashBoardModel.infoDashBoardModels
            {
                dashBoardLists = DashBoardAppCode.getInfoDashBoard(),
                customerSpendingLists = DashBoardAppCode.getInfoGroupCustomerSpending(),
                sumSpendingOfYear = DashBoardAppCode.getInfoSumSepndingOfYear(),
            };
            if (Request.QueryString["s"] != null)
            {
                if (Request.QueryString["s"] == AppCode.ApproveEmailype.document.ToString())
                {
                    return RedirectToAction("index", "Home", new { actId = Request.QueryString["actId"] });
                }
                else if (Request.QueryString["s"] == AppCode.ApproveEmailype.approve.ToString())
                {
                    return RedirectToAction("index", "ApproveLists", new { actId = Request.QueryString["actId"] });
                }
                else if (Request.QueryString["s"] == AppCode.ApproveEmailype.budget_form.ToString())
                {
                    return RedirectToAction("index", "BudgetApproveList", new { actId = Request.QueryString["actId"] });
                }
            }
            return View(model);
        }
        public ActionResult monthTotalSpending()
        {
            List<DashBoardModel.infoMonthTotalSpending> model = DashBoardAppCode.getInfoMonthSpending();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult sumTotalDashBoard()
        {
            return PartialView();
        }
    }
}