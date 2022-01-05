using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.BusinessLayer.Tbmmkt.QueryHandler;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using WebLibrary;
using static eActForm.Controllers.GetDataMainFormController;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class MainFormController : Controller
    {

        public ActionResult Index(string activityId, string mode, string typeForm, string master_type_form_id)
        {

            Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
            ActivityFormTBMMKT activityFormTBMMKT = new ActivityFormTBMMKT();

            try
            {

                if (!string.IsNullOrEmpty(activityId))
                {
                    activity_TBMMKT_Model = ActivityFormTBMMKTCommandHandler.getDataForEditActivity(activityId);

                    if (ConfigurationManager.AppSettings["masterEmpExpense"] == master_type_form_id)
                    {
                        activityFormTBMMKT.master_type_form_id = ConfigurationManager.AppSettings["masterEmpExpense"];
                        activity_TBMMKT_Model = exPerryCashAppCode.processDataExpense(activity_TBMMKT_Model, activityId);
                    }

                    activityFormTBMMKT.master_type_form_id = activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id;
                    activityFormTBMMKT.formCompanyId = string.IsNullOrEmpty(activity_TBMMKT_Model.activityFormTBMMKT.formCompanyId) ? activity_TBMMKT_Model.activityFormTBMMKT.companyId : activity_TBMMKT_Model.activityFormTBMMKT.formCompanyId;

                    #region Get Subject
                    objGetDataSubjectByChanelOrBrand objGetDataSubjectBy = new objGetDataSubjectByChanelOrBrand();
                    objGetDataSubjectBy.companyId = activity_TBMMKT_Model.activityFormTBMMKT.companyId;
                    objGetDataSubjectBy.master_type_form_id = activityFormTBMMKT.master_type_form_id;

                    if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther != null)
                    {
                        activity_TBMMKT_Model.activityFormTBMMKT.selectedBrandOrChannel = activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.groupName;
                        if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.productBrandId != "")
                        {
                            objGetDataSubjectBy.idBrandOrChannel = activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.productBrandId;
                        }
                        else//Channel
                        {
                            objGetDataSubjectBy.idBrandOrChannel = activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.channelId;
                        }
                    }

                    activity_TBMMKT_Model.tB_Reg_Subject = QueryGetSelectAllTB_Reg_Subject.GetQueryGetSelectAllTB_Reg_Subject_ByFormAndFlow(objGetDataSubjectBy);
                    activity_TBMMKT_Model.channelMasterTypeList = QueryGet_channelByGroup.get_channelByGroup(activityFormTBMMKT.master_type_form_id, activityFormTBMMKT.formCompanyId, activity_TBMMKT_Model.activityFormTBMMKT.selectedBrandOrChannel);
                    #endregion

                    #region getEO formPaymentVoucherTbm
                    if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPaymentVoucherTbmId"])
                    {
                        ObjGetDataEO objGetDataEO = new ObjGetDataEO();
                        if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.channelId == "") { objGetDataEO.channelId = ""; } else { objGetDataEO.channelId = activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.channelId; }
                        if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.productBrandId == "") { objGetDataEO.productBrandId = ""; } else { objGetDataEO.productBrandId = activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.productBrandId; }
                        if (activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPaymentVoucherTbmId"]) { objGetDataEO.master_type_form_id = ConfigurationManager.AppSettings["formBgTbmId"]; }
                        objGetDataEO.fiscalYear = activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.fiscalYear;
                        activity_TBMMKT_Model.listGetDataEO = QueryGetSelectMainForm.GetQueryDataEOPaymentVoucher(objGetDataEO);

                        ObjGetDataIO objGetDataIO = new ObjGetDataIO();
                        objGetDataIO.ActivityByEOSelect = "";
                        objGetDataIO.EOSelect = "";
                        foreach (var item in activity_TBMMKT_Model.activityFormTBMMKT.list_1_multi_select)
                        {
                            int lenghtCut = 37;
                            int maxLenght = item.Length;
                            objGetDataIO.ActivityByEOSelect += (item.Substring(0, 36) + "|");
                            objGetDataIO.EOSelect += (item.Substring(lenghtCut, maxLenght - lenghtCut) + "|");
                        }
                        activity_TBMMKT_Model.listGetDataIO = QueryGetSelectMainForm.GetQueryDataIOPaymentVoucher(objGetDataIO);
                    }
                    #endregion

                    TempData["actForm" + activityId] = activity_TBMMKT_Model;

                }
                else
                {
                    mode = AppCode.Mode.addNew.ToString();
                    string actId = Guid.NewGuid().ToString();
                    activityFormTBMMKT.statusId = 1;
                    activityFormTBMMKT.createdByUserId = @UtilsAppCode.Session.User.empId;
                    activity_TBMMKT_Model.activityFormModel.id = actId;
                    activityFormTBMMKT.master_type_form_id = master_type_form_id;// for production
                    activityFormTBMMKT.formCompanyId = QueryGet_master_type_form.get_master_type_form(activityFormTBMMKT.master_type_form_id).FirstOrDefault().companyId;
                    //activityFormTBMMKT.formCompanyId = !string.IsNullOrEmpty(activityFormTBMMKT.formCompanyId) ? activityFormTBMMKT.formCompanyId : @UtilsAppCode.Session.User.empCompanyId;
                    activityFormTBMMKT.chkUseEng = Request.Cookies[ConfigurationManager.AppSettings["nameCookieLanguageEact"]].Value.ToString() == ConfigurationManager.AppSettings["cultureEng"];

                    #region mock data for first input
                    //===mock data for first input====
                    int rowEstimateTable = 14;
                    if (activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPaymentVoucherTbmId"])//ใบสั่งจ่าย
                    {
                        rowEstimateTable = 1;
                    }
                    List<CostThemeDetailOfGroupByPriceTBMMKT> costThemeDetailOfGroupByPriceTBMMKT = new List<CostThemeDetailOfGroupByPriceTBMMKT>();
                    for (int i = 0; i < rowEstimateTable; i++)
                    {
                        costThemeDetailOfGroupByPriceTBMMKT.Add(new CostThemeDetailOfGroupByPriceTBMMKT() { id = "", IO = "", activityTypeId = "", productDetail = "", unit = 0, unitPrice = 0, total = 0, EO = "", UseYearSelect = "" });
                    }

                    TB_Act_ActivityForm_DetailOther tB_Act_ActivityForm_DetailOther = new TB_Act_ActivityForm_DetailOther();
                    activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther = tB_Act_ActivityForm_DetailOther;
                    activity_TBMMKT_Model.activityFormTBMMKT = activityFormTBMMKT;
                    activity_TBMMKT_Model.activityOfEstimateList = costThemeDetailOfGroupByPriceTBMMKT;
                    activity_TBMMKT_Model.activityFormTBMMKT.selectedBrandOrChannel = "";
                    activity_TBMMKT_Model.totalCostThisActivity = decimal.Parse("0.00");
                    activity_TBMMKT_Model.activityFormTBMMKT.list_2_select = "";
                    activity_TBMMKT_Model.activityFormTBMMKT.list_3_select = "";
                    activity_TBMMKT_Model.activityFormTBMMKT.brand_select = "";

                    //dev date 20200413 fream;
                    //=END==mock data for first input=====
                    activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOtherList = new List<TB_Act_ActivityForm_DetailOtherList>
                    {
                        new TB_Act_ActivityForm_DetailOtherList() { id = "", IO = "", GL = "", select_list_choice_id_ChReg = "", productBrandId = "" }
                    };//dev date 20200413 fream;
                    #endregion

                    TempData["actForm" + actId] = activity_TBMMKT_Model;
                }


                activity_TBMMKT_Model.tB_Act_ProductBrand_Model = QueryGetAllBrandByForm.GetAllBrandByForm(activityFormTBMMKT.master_type_form_id, activityFormTBMMKT.formCompanyId).Where(x => x.no_tbmmkt != "").ToList();
                activity_TBMMKT_Model.tB_Act_ActivityForm_SelectBrandOrChannel = QueryGet_channelMaster.get_channelMaster(activityFormTBMMKT.master_type_form_id, activityFormTBMMKT.formCompanyId);
                activity_TBMMKT_Model.activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.activityCondition == "tbmmkt_ChooseActivityOrDetail").ToList();
                activity_TBMMKT_Model.activityFormModel.typeForm = typeForm;
                activity_TBMMKT_Model.activityFormModel.mode = mode;
                activity_TBMMKT_Model.master_Type_Form_Detail_Models = QueryGet_master_type_form_detail.get_master_type_form_detail(activityFormTBMMKT.master_type_form_id, "input");
                activity_TBMMKT_Model.activityFormTBMMKT.formName = QueryGet_master_type_form.get_master_type_form(activityFormTBMMKT.master_type_form_id).FirstOrDefault().nameForm.Replace("<br/>", "");
                activity_TBMMKT_Model.activityFormTBMMKT.formNameEn = QueryGet_master_type_form.get_master_type_form(activityFormTBMMKT.master_type_form_id).FirstOrDefault().nameForm_EN;
                activity_TBMMKT_Model.scristModelList = QueryGetScriptByMasterFormId.getScriptByMasterFormId(activityFormTBMMKT.master_type_form_id);
                if (activityFormTBMMKT.formCompanyId != "")
                {
                    activity_TBMMKT_Model.activityFormTBMMKT.companyName = QueryGet_master_company.get_master_company(activityFormTBMMKT.formCompanyId).FirstOrDefault().companyNameTH;
                    activity_TBMMKT_Model.activityFormTBMMKT.companyNameEN = QueryGet_master_company.get_master_company(activityFormTBMMKT.formCompanyId).FirstOrDefault().companyNameEN;
                }
                else
                {
                    activity_TBMMKT_Model.activityFormTBMMKT.companyName = "";
                    activity_TBMMKT_Model.activityFormTBMMKT.companyNameEN = "";

                }


                TempData.Keep();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("MainFormController[Action:Index] => " + ex.Message);
            }
            return View(activity_TBMMKT_Model);
        }


        [HttpPost] //post method
        [ValidateAntiForgeryToken] // prevents cross site attacks ต้องใส่   @Html.AntiForgeryToken() ในหน้า เว็บด้วย
        [ValidateInput(false)]
        public JsonResult insertDataActivity(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            var result = new AjaxResult();
            try
            {
                string statusId = "";

                statusId = ActivityFormCommandHandler.getStatusActivity(activity_TBMMKT_Model.activityFormModel.id);
                if (statusId == "")
                {
                    activity_TBMMKT_Model.activityFormTBMMKT.statusId = 1;
                }
                else
                {
                    activity_TBMMKT_Model.activityFormTBMMKT.statusId = int.Parse(statusId);
                }


                if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formTrvTbmId"] // แบบฟอร์มเดินทางปฏิบัติงานนอกสถานที่
                    || activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formTrvHcmId"] // แบบฟอร์มเดินทางปฏิบัติงานนอกสถานที่
                    || (AppCode.hcForm.Contains(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id))
                    )
                {
                    activity_TBMMKT_Model.activityOfEstimateList = activity_TBMMKT_Model.expensesDetailModel.costDetailLists;
                    activity_TBMMKT_Model.activityOfEstimateSubList = activity_TBMMKT_Model.expensesDetailSubModel.costDetailLists;
                    //activity_TBMMKT_Model.activityFormModel.documentDate = BaseAppCodes.converStrToDatetimeWithFormat(activity_TBMMKT_Model.activityFormModel.documentDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                    if (AppCode.hcForm.Contains(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id))//ฟอร์มเบิกค่าเดินทางและเบี้ยเลี่ยงnum
                    {
                        activity_TBMMKT_Model.requestEmpModel.Add(new RequestEmpModel() { empId = activity_TBMMKT_Model.empInfoModel.empId, empTel = activity_TBMMKT_Model.empInfoModel.empTel });
                        activity_TBMMKT_Model.activityFormTBMMKT.empId = activity_TBMMKT_Model.empInfoModel.empId;
                    }
                }

                if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formCR_IT_FRM_314"])//ฟอร์มChangeRequest_IT314
                {
                    activity_TBMMKT_Model.activityFormTBMMKT.empId = activity_TBMMKT_Model.empInfoModel.empId;
                    string formCompanyIdBySubject = QueryGetSelectAllTB_Reg_Subject.GetQueryGetDataSubjectByid(activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.SubjectId).FirstOrDefault().companyId;
                    activity_TBMMKT_Model.activityFormTBMMKT.formCompanyId = formCompanyIdBySubject;
                }

                if (ActFormAppCode.checkFormAddTBDetailOther(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id))
                {
                    activity_TBMMKT_Model = ActFormAppCode.addDataToDetailOther(activity_TBMMKT_Model);
                }

                activity_TBMMKT_Model.activityFormTBMMKT.languageDoc = Request.Cookies[ConfigurationManager.AppSettings["nameCookieLanguageEact"]].Value.ToString();

                int countSuccess = ActivityFormTBMMKTCommandHandler.insertAllActivity(activity_TBMMKT_Model, activity_TBMMKT_Model.activityFormModel.id);

                result.Data = activity_TBMMKT_Model.activityFormModel.id;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                ExceptionManager.WriteError("insertDataActivityMainForm => " + ex.Message);
            }

            return Json(result);
        }
        public JsonResult getChannelByGroup(string formId, string groupName)
        {
            var result = new AjaxResult();
            try
            {
                var getChannel = QueryGet_channelByGroup.get_channelByGroup(formId, UtilsAppCode.Session.User.empCompanyId, groupName);
                var resultData = new
                {
                    channelList = getChannel.ToList(),
                };
                result.Data = resultData;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}