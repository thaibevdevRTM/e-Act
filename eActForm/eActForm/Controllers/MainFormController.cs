﻿using eActForm.BusinessLayer;
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
                    activityFormTBMMKT.master_type_form_id = activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id;
                    activity_TBMMKT_Model.activityFormTBMMKT.formCompanyId = QueryGet_master_type_form.get_master_type_form(activityFormTBMMKT.master_type_form_id).FirstOrDefault().companyId;
                    activityFormTBMMKT.formCompanyId = activity_TBMMKT_Model.activityFormTBMMKT.formCompanyId;

                    //===================Get Subject=======================
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
                    //====END===============Get Subject=======================
                    activity_TBMMKT_Model.channelMasterTypeList = QueryGet_channelByGroup.get_channelByGroup(activityFormTBMMKT.master_type_form_id, activityFormTBMMKT.formCompanyId, activity_TBMMKT_Model.activityFormTBMMKT.selectedBrandOrChannel);
                    TempData["actForm" + activityId] = activity_TBMMKT_Model;

                }
                else
                {
                    mode = "new";

                    string actId = Guid.NewGuid().ToString();
                    activity_TBMMKT_Model.activityFormModel.id = actId;
                    activityFormTBMMKT.master_type_form_id = master_type_form_id;// for production
                    activityFormTBMMKT.formCompanyId = QueryGet_master_type_form.get_master_type_form(activityFormTBMMKT.master_type_form_id).FirstOrDefault().companyId;
                    activityFormTBMMKT.chkUseEng = Request.Cookies[ConfigurationManager.AppSettings["nameCookieLanguageEact"]].Value.ToString() == ConfigurationManager.AppSettings["cultureEng"];
                    //===mock data for first input====
                    List<CostThemeDetailOfGroupByPriceTBMMKT> costThemeDetailOfGroupByPriceTBMMKT = new List<CostThemeDetailOfGroupByPriceTBMMKT>();
                    for (int i = 0; i < 14; i++)
                    {
                        costThemeDetailOfGroupByPriceTBMMKT.Add(new CostThemeDetailOfGroupByPriceTBMMKT() { id = "", IO = "", activityTypeId = "", productDetail = "", unit = 0, unitPrice = 0, total = 0 });
                    }

                    TB_Act_ActivityForm_DetailOther tB_Act_ActivityForm_DetailOther = new TB_Act_ActivityForm_DetailOther();
                    activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther = tB_Act_ActivityForm_DetailOther;
                    activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.SubjectId = "";
                    activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.productBrandId = "";
                    activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.channelId = "";
                    activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.BudgetNumber = "";
                    activity_TBMMKT_Model.activityFormTBMMKT = activityFormTBMMKT;
                    activity_TBMMKT_Model.activityFormTBMMKT.selectedBrandOrChannel = "";
                    activity_TBMMKT_Model.costThemeDetailOfGroupByPriceTBMMKT = costThemeDetailOfGroupByPriceTBMMKT;
                    activity_TBMMKT_Model.totalCostThisActivity = decimal.Parse("0.00");
                    activity_TBMMKT_Model.activityFormTBMMKT.list_2_select = "";
                    activity_TBMMKT_Model.activityFormTBMMKT.list_3_select = "";
                    activity_TBMMKT_Model.activityFormTBMMKT.brand_select = "";
                    activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.costCenter = "";
                    activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.channelRegionName = "";
                    activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.glNo = "";
                    activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.glName = "";
                    //=END==mock data for first input=====

                    //===================Get Subject=======================
                    List<TB_Reg_Subject> tB_Reg_Subjects = new List<TB_Reg_Subject>(); activity_TBMMKT_Model.tB_Reg_Subject = tB_Reg_Subjects;
                    //======END=============Get Subject=======================

                    TempData["actForm" + actId] = activity_TBMMKT_Model;
                }

                activity_TBMMKT_Model.tB_Act_ProductBrand_Model = QueryGetAllBrandByForm.GetAllBrandByForm(activityFormTBMMKT.master_type_form_id, activityFormTBMMKT.formCompanyId).Where(x => x.no_tbmmkt != "").ToList();
                activity_TBMMKT_Model.tB_Act_ActivityForm_SelectBrandOrChannel = QueryGet_channelMaster.get_channelMaster(activityFormTBMMKT.master_type_form_id, activityFormTBMMKT.formCompanyId);
                activity_TBMMKT_Model.activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.activityCondition == "tbmmkt_ChooseActivityOrDetail").ToList();
                activity_TBMMKT_Model.activityFormModel.typeForm = typeForm;
                activity_TBMMKT_Model.activityFormModel.mode = mode;
                activity_TBMMKT_Model.master_Type_Form_Detail_Models = QueryGet_master_type_form_detail.get_master_type_form_detail(activityFormTBMMKT.master_type_form_id, "input");
                activity_TBMMKT_Model.activityFormTBMMKT.companyName = QueryGet_master_company.get_master_company(activityFormTBMMKT.formCompanyId).FirstOrDefault().companyNameTH;
                activity_TBMMKT_Model.activityFormTBMMKT.companyNameEN = QueryGet_master_company.get_master_company(activityFormTBMMKT.formCompanyId).FirstOrDefault().companyNameEN;
                activity_TBMMKT_Model.activityFormTBMMKT.formName = QueryGet_master_type_form.get_master_type_form(activityFormTBMMKT.master_type_form_id).FirstOrDefault().nameForm;
                activity_TBMMKT_Model.activityFormTBMMKT.formNameEn = QueryGet_master_type_form.get_master_type_form(activityFormTBMMKT.master_type_form_id).FirstOrDefault().nameForm_EN;
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


                if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formTrvTbmId"] || activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formTrvHcmId"])//แบบฟอร์มเดินทางปฏิบัติงานนอกสถานที่
                {
                    activity_TBMMKT_Model.costThemeDetailOfGroupByPriceTBMMKT = activity_TBMMKT_Model.expensesDetailModel.costDetailLists;
                    //activity_TBMMKT_Model.activityFormModel.documentDate = BaseAppCodes.converStrToDatetimeWithFormat(activity_TBMMKT_Model.activityFormModel.documentDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
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