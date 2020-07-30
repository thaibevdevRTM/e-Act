using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eForms.Presenter.MasterData;
namespace eActForm.Controllers
{
    public class apiGetDataMasterController : Controller
    {
        // GET: getDataMaster
        public JsonResult index()
        {
            var resultAjax = new AjaxResult();

            return Json(resultAjax, "text/plain");
        }
        public JsonResult getDataAmphures(string provinceId)
        {
            var resultAjax = new AjaxResult();

            return Json(resultAjax, "text/plain");
        }

        public JsonResult getDataSaleTeam(string provinceId)
        {
            var resultAjax = new AjaxResult();
            salesTeamModel model = new salesTeamModel();
            model.saleTeamList = SalesTeamPresenter.getSalesTeamCVM(AppCode.StrConAuthen, provinceId);
            resultAjax.Data = model;
            return Json(resultAjax, JsonRequestBehavior.AllowGet);
        }

    }
}