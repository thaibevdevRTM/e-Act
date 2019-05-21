using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eActForm.Models;
using eActForm.BusinessLayer;
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
                customerSpendingLists = DashBoardAppCode.getInfoGroupCustomerSpending()
            };
            if (Request.QueryString["s"] != null)
            {
                if( Request.QueryString["s"] == AppCode.ApproveEmailype.document.ToString())
                {
                    return RedirectToAction("index", "Home", new { actId = Request.QueryString["actId"] });
                }else if(Request.QueryString["s"] == AppCode.ApproveEmailype.approve.ToString())
                {
                    return RedirectToAction("index", "ApproveLists", new { actId = Request.QueryString["actId"] });
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