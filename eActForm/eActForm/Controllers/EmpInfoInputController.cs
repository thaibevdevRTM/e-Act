using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class EmpInfoInputController : Controller
    {
        public ActionResult empInfoDetail(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            if (activity_TBMMKT_Model.activityFormTBMMKT.mode == AppCode.Mode.edit.ToString())
            {
                bool chk = activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formExpTrvNumId"] ? true : false;
                activity_TBMMKT_Model.empInfoModel = QueryGet_ReqEmpByActivityId.getReqEmpByActivityId(activity_TBMMKT_Model.activityFormModel.id, activity_TBMMKT_Model.activityFormTBMMKT.chkUseEng,chk).FirstOrDefault();
               
                if (activity_TBMMKT_Model.regionalModel == null || activity_TBMMKT_Model.regionalModel.Count == 0)
                {
                    activity_TBMMKT_Model.regionalModel = QueryGetRegional.getRegionalByCompanyId(activity_TBMMKT_Model.activityFormModel.companyId).OrderBy(x => x.nameEN).ToList();
                }
            }
            else
            {
                if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formExpTrvNumId"]|| activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formExpMedNumId"])
                {
                    activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.SubjectId = QueryGetSubject.getAllSubject().Where(x => x.typeFormId == activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id).FirstOrDefault().id;
                    activity_TBMMKT_Model.activityFormTBMMKT.SubjectId = activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.SubjectId;
                }

                activity_TBMMKT_Model.regionalModel.Add(new RegionalModel() { id = "", companyId = "", nameEN = "", nameTH = "" });
            }
            return PartialView(activity_TBMMKT_Model);
        }
    }
}