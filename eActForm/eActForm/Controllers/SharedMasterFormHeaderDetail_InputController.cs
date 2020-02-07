using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class SharedMasterFormHeaderDetail_InputController : Controller
    {
        public ActionResult dropdownCondtionTbmmkt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult headerDetailsDate(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult headerDetails(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult headerDetailsDate_dmy(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            if (string.IsNullOrEmpty(activity_TBMMKT_Model.activityFormTBMMKT.documentDateStr) && (activity_TBMMKT_Model.activityFormModel.documentDate!= null))
            {
                activity_TBMMKT_Model.activityFormTBMMKT.documentDateStr = activity_TBMMKT_Model.activityFormModel.documentDate?.ToString("dd-MM-yyyy");
            }
            return PartialView(activity_TBMMKT_Model);
        }
    }
}