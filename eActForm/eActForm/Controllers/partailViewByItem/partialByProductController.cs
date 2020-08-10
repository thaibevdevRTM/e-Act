using eActForm.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    public partial class partialByProductController : Controller
    {
        // GET: partialByProduct
        public ActionResult index(string actId)
        {
            Activity_Model model = new Activity_Model();
            model.costthemedetail = new List<CostThemeDetail> {
                new CostThemeDetail { activityId = actId ,productDetail = "crystal 600 ml. pack 12" , wholeSalesPrice=50},
                new CostThemeDetail { activityId = actId ,productDetail = "crystal 1500 ml. pack 12" , wholeSalesPrice=50}
            };
            return PartialView(model);
        }
    }
}