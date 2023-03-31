using eForms.Data;
using eForms.Models.MasterData;
using eForms.UI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace eForms.UI.Controllers
{
    public class HistoryController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IConfiguration _config = null;
        public HistoryController(IConfiguration config)
        {
            _config = config;
        }
        public IActionResult Index()
        {
            string strCon = _config["AppSettings:DBConnection"].ToString();
            ActivityModel activityModel = new ActivityModel();
            activityModel.brandList = DdlData.GetBrandBudgetControl(strCon);
            activityModel.chanelList = DdlData.GetChannelBudgetControl(strCon);
            activityModel.activityGroupList = DdlData.getActivityGroupBudgetControl(strCon, "bg");
            //activityModel.companyList = DdlData.getOhterMaster(strCon, "company", "");

            return View(activityModel);
        }
    }
}
