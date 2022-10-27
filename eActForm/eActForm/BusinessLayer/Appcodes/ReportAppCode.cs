using eActForm.BusinessLayer.Appcodes;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Controllers;
using eActForm.Models;
using eForms.Models.Forms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    public class ReportAppCode
    {

        public static SearchActivityModels getMasterDataForSearch()
        {
            try
            {
                SearchActivityModels models = new SearchActivityModels
                {
                    approveStatusList = ApproveAppCode.getApproveStatus(AppCode.StatusType.app),
                    productGroupList = QueryGetAllProductGroup.getAllProductGroup(),
                    customerslist = QueryGetAllCustomers.getAllCustomers().Where(x => x.cusNameEN != "").ToList(),
                    productTypelist = QuerygetAllProductCate.getAllProductType(),
                    activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup()
                    .GroupBy(item => item.activitySales)
                    .Select(grp => new TB_Act_ActivityGroup_Model { id = grp.First().id, activitySales = grp.First().activitySales }).ToList()
                };

                return models;
            }
            catch (Exception ex)
            {
                throw new Exception("getMasterDataForSearch >>" + ex.Message);
            }
        }
        public static List<CompanyModel> getCompanyByRole(string typeFormId)
        {
            String compId = "";
            try
            {
                List<CompanyModel> companyList = new List<CompanyModel>();

                companyList = QueryGetAllCompany.getCompanyByTypeFormId(typeFormId);

                List<ActUserModel.UserAuthorized> lst = new List<ActUserModel.UserAuthorized>();
                lst = UserAppCode.GetUserAuthorizedsByCompany(UtilsAppCode.Session.User.empCompanyGroup);

                compId = lst.Count > 0 ? lst.FirstOrDefault().companyId : "";
                if (UtilsAppCode.Session.User.isAdminHCBP)
                {

                }
                else if (UtilsAppCode.Session.User.isAdminNUM || UtilsAppCode.Session.User.isAdminPOM || UtilsAppCode.Session.User.isAdminCVM)
                {
                    companyList = companyList.Where(x => x.companyId == compId).OrderBy(x => x.companyNameTH).ToList();
                }
                else
                { //user ปกติดูได้หรือป่าว

                }


                return companyList; //ถ้าเป็น superadmim ถึงจะดึงทั้ง 8 ถ้าไม่ดึงตัวเอง
            }
            catch (Exception ex)
            {
                throw new Exception("getCompanyByRole >>" + ex.Message);
            }
        }

        public static Activity_TBMMKT_Model mainReport(string activityId, string empId)
        {
            Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
            try
            {
                ActivityFormTBMMKT activityFormTBMMKT = new ActivityFormTBMMKT();


                activity_TBMMKT_Model = ActivityFormTBMMKTCommandHandler.getDataForEditActivity(activityId);
                List<Master_type_form_Model> listMasterType = QueryGet_master_type_form.get_master_type_form(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id);
                activity_TBMMKT_Model.activityFormTBMMKT.companyName = QueryGet_master_company.get_master_company(activity_TBMMKT_Model.activityFormTBMMKT.companyId).FirstOrDefault().companyNameTH;
                activity_TBMMKT_Model.activityFormTBMMKT.formName = listMasterType.FirstOrDefault().nameForm;
                activity_TBMMKT_Model.activityFormTBMMKT.formNameEn = listMasterType.FirstOrDefault().nameForm_EN;
                activity_TBMMKT_Model.activityFormTBMMKT.formCompanyId = listMasterType.FirstOrDefault().companyId;
                activity_TBMMKT_Model.activityFormTBMMKT.chkUseEng = (activity_TBMMKT_Model.activityFormTBMMKT.languageDoc == ConfigurationManager.AppSettings["cultureEng"]);
                activity_TBMMKT_Model.master_Type_Form_Detail_Models = QueryGet_master_type_form_detail.get_master_type_form_detail(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id, "report");

                activity_TBMMKT_Model.approveFlowDetail = ActivityFormTBMMKTCommandHandler.get_flowApproveDetail(activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.SubjectId, activityId, empId);
                //===ดึงผู้อนุมัติทั้งหมด=เพือเอาไปใช้แสดงในรายงาน===
                if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formCR_IT_FRM_314"])
                {
                    activity_TBMMKT_Model.approveModels = ApproveAppCode.getApproveByActFormId(activityId);
                }
                //=END==ดึงผู้อนุมัติทั้งหมด=เพือเอาไปใช้แสดงในรายงาน===

                //=====layout doc=============
                if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPaymentVoucherTbmId"]
                    || activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPurchaseTbm"])
                {
                    ObjGetDataLayoutDoc objGetDataLayoutDoc = new ObjGetDataLayoutDoc();
                    objGetDataLayoutDoc.typeKeys = "PVFormBreakSignatureNewPage";
                    objGetDataLayoutDoc.activityId = activityId;
                    activity_TBMMKT_Model.list_ObjGetDataLayoutDoc = QueryGetSelectMainForm.GetQueryDataMasterLayoutDoc(objGetDataLayoutDoc);
                }
                //===END==layout doc===========

                //===========Set Language By Document Dev date 20200310 Peerapop=====================
                //ไม่ต้องไปกังวลว่าภาษาหลักของWebที่Userใช้งานอยู่จะมีปัญหาเพราะ _ViewStart จะเปลี่ยนภาษาปัจจุบันที่Userใช้เว็บปรับCultureกลับให้เอง
                DocumentsAppCode.setCulture(activity_TBMMKT_Model.activityFormModel.languageDoc);


                //====END=======Set Language By Document Dev date 20200310 Peerapop==================
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("reportAppCode >> mainReport >>" + activityId + "___" + ex.Message);
            }
            return activity_TBMMKT_Model;
        }


        public static Activity_Model previewApprove(string actId, string empId)
        {
            Activity_Model activityModel = new Activity_Model();
            try
            {
                activityModel.activityFormModel = QueryGetActivityById.getActivityById(actId).FirstOrDefault();
                activityModel.productcostdetaillist1 = QueryGetCostDetailById.getcostDetailById(actId);
                activityModel.activitydetaillist = QueryGetActivityDetailById.getActivityDetailById(actId);
                activityModel.productImageList = ImageAppCode.GetImage(actId).Where(x => x.extension != ".pdf").ToList();
                activityModel.activityFormModel.typeForm = BaseAppCodes.getactivityTypeByCompanyId(activityModel.activityFormModel.companyId);
                activityModel.approveModels = ApproveFlowAppCode.getFlowId(ConfigurationManager.AppSettings["subjectActivityFormId"], actId);

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("reportAppCode >> previewApprove >>" + actId + "___" + ex.Message);
            }
            return activityModel;
        }

    }
}