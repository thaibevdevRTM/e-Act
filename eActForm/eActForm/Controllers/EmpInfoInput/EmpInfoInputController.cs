using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using eForms.Models.MasterData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    [LoginExpire]
    public partial class EmpInfoInputController : Controller
    {
        public ActionResult empInfoDetail(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
             bool chk = AppCode.hcForm.Contains(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id);

            if (activity_TBMMKT_Model.activityFormTBMMKT.mode == AppCode.Mode.edit.ToString())
            {

                activity_TBMMKT_Model.empInfoModel = QueryGet_ReqEmpByActivityId.getReqEmpByActivityId(activity_TBMMKT_Model.activityFormModel.id, activity_TBMMKT_Model.activityFormTBMMKT.chkUseEng, chk).FirstOrDefault();

                if (activity_TBMMKT_Model.regionalModel == null || activity_TBMMKT_Model.regionalModel.Count == 0)
                {
                    activity_TBMMKT_Model.regionalModel = QueryGetRegional.getRegionalByCompanyId(activity_TBMMKT_Model.activityFormModel.companyId).OrderBy(x => x.nameEN).ToList();
                }
            }
            else
            {
                if (chk)
                {
                    activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.SubjectId = QueryGetSubject.getAllSubject().Where(x => x.typeFormId == activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id).FirstOrDefault().id;
                    activity_TBMMKT_Model.activityFormTBMMKT.SubjectId = activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.SubjectId;
                }

                activity_TBMMKT_Model.regionalModel.Add(new RegionalModel() { id = "", companyId = "", nameEN = "", nameTH = "" });
            }
            return PartialView(activity_TBMMKT_Model);
        }

        public ActionResult empInfoDetailV2(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            activity_TBMMKT_Model.empInfoModel = QueryGet_ReqEmpByActivityId.getReqEmpByMainTableActivityId(activity_TBMMKT_Model.activityFormModel.id, activity_TBMMKT_Model.activityFormTBMMKT.chkUseEng).FirstOrDefault();
            return PartialView(activity_TBMMKT_Model);
        }

        public ActionResult empInfoDetail_Department_Tel(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            if (activity_TBMMKT_Model.listGetDepartmentMaster == null)
            {
                List<departmentMasterModel> departmentMasterModels = new List<departmentMasterModel>();
                activity_TBMMKT_Model.listGetDepartmentMaster = departmentMasterModels;
            }
            return PartialView(activity_TBMMKT_Model);
        }

    }
}