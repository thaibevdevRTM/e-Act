using eActForm.Models;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    public class exPerryCashController : Controller
    {
        // GET: Expense
        public Activity_TBMMKT_Model getMasterExPerryCash()
        {

            Activity_TBMMKT_Model model = new Activity_TBMMKT_Model();
            return model;
        }


    }
}