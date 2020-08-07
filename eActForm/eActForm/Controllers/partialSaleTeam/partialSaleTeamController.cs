using eActForm.Models;
using eForms.Presenter.MasterData;
using System.Web.Mvc;
namespace eActForm.Controllers
{
    public partial class partialSaleTeamController : Controller
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