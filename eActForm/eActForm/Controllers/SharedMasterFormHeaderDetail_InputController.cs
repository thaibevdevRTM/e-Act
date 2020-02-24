using eActForm.BusinessLayer;
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
        public ActionResult headerDetails_Pos_Premium(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
                        return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult headerDetailsDate_dmy(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            if (string.IsNullOrEmpty(activity_TBMMKT_Model.activityFormTBMMKT.documentDateStr) && (activity_TBMMKT_Model.activityFormModel.documentDate!= null))
            {
                activity_TBMMKT_Model.activityFormTBMMKT.documentDateStr = activity_TBMMKT_Model.activityFormModel.documentDate?.ToString("dd-MM-yyyy");
            }

            if (activity_TBMMKT_Model.list_0 == null|| activity_TBMMKT_Model.list_0.Count == 0 )
            {

               // if (activityFormTBMMKT.master_type_form_id == "24BA9F57-586A-4A8E-B54C-00C23C41BFC5")//ใบเบิกผลิตภัณฑ์,POS/PREMIUM
            
                activity_TBMMKT_Model.list_0 = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id, "travelling").OrderBy(x => x.name).ToList();
            }
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult headerViewManual(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            if (string.IsNullOrEmpty(activity_TBMMKT_Model.activityFormTBMMKT.documentDateStr) && (activity_TBMMKT_Model.activityFormModel.documentDate != null))
            {
                activity_TBMMKT_Model.activityFormTBMMKT.documentDateStr = activity_TBMMKT_Model.activityFormModel.documentDate?.ToString("dd-MM-yyyy");
            }
            return PartialView(activity_TBMMKT_Model);
        }
        
    }
}