using eActForm.BusinessLayer;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    public class PreviewMasterFormController : Controller
    {
        // GET: PreviewMasterForm
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult PreviewFormDetail_Input(string typeLayout)
        {

            Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
            ActivityFormTBMMKT activityFormTBMMKT = new ActivityFormTBMMKT();
            activity_TBMMKT_Model.activityFormModel = new ActivityForm();
            activityFormTBMMKT.companyName = "CompanyName";
            activityFormTBMMKT.formName = "FormName";
            activity_TBMMKT_Model.master_Type_Form_Detail_Models = QueryGet_master_type_form_detail.get_AllMasterFormDetailByTypeLayout(typeLayout);
            activity_TBMMKT_Model.activityFormTBMMKT = activityFormTBMMKT;
            return PartialView(activity_TBMMKT_Model);
        }
    }


}