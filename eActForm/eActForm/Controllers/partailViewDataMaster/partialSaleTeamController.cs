using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eForms.Presenter.MasterData;
namespace eActForm.Controllers
{
    public class partialSaleTeamController : Controller
    {
        // GET: partialSaleTeamCVM
        public ActionResult index()
        {
            salesTeamModel model = new salesTeamModel();
            model.provinceList = ProvincePresenter.getProvince(AppCode.StrConAuthen);
            //model.saleTeamList = SalesTeamPresenter.getSalesTeamCVM(AppCode.StrCon,)
            return PartialView(model);
        }

    }
}