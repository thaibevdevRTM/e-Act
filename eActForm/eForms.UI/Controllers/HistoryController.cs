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
            ActivityModel activityModel = new ActivityModel();
            activityModel.brandList = HistoryHelpercs.GetBrandBudgetControl();

            return View();
        }
    }
}
