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