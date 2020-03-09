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
    public class SharedMasterFormHeaderCompany_InputController : Controller
    {
        public ActionResult headerCompany(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }

        public ActionResult headerCompanySubject(Activity_TBMMKT_Model activity_TBMMKT_Model, string actId)
        {

            if (!string.IsNullOrEmpty(actId))
            {
                activity_TBMMKT_Model.activityFormModel = QueryGetActivityById.getActivityById(actId).FirstOrDefault();
                activity_TBMMKT_Model.activityFormTBMMKT.companyName = QueryGet_master_company.get_master_company(activity_TBMMKT_Model.activityFormModel.companyId).FirstOrDefault().companyNameTH;
                activity_TBMMKT_Model.activityFormTBMMKT.formName = QueryGet_master_type_form.get_master_type_form(activity_TBMMKT_Model.activityFormModel.master_type_form_id).FirstOrDefault().nameForm;
            }
            return PartialView(activity_TBMMKT_Model);
        }
    }


}