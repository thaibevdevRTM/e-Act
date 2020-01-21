using eActForm.BusinessLayer;
using eActForm.Models;
using eActForm.Models;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class MainFormController : Controller
    {

        public ActionResult Index(string activityId, string mode, string typeForm, string master_type_form_id)
        {
            Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
            ActivityFormTBMMKT activityFormTBMMKT = new ActivityFormTBMMKT();

            //activityFormTBMMKT.idTypeForm = "8C4511BA-E0D6-4E6F-AD8D-62A5431E4BD4"; // for test            
            try
            {

                if (!string.IsNullOrEmpty(activityId))
                {

                    activity_TBMMKT_Model = ActivityFormTBMMKTCommandHandler.getDataForEditActivity(activityId);
                    activityFormTBMMKT.master_type_form_id = activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id;
                    if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.productBrandId != "")
                    {
                        activity_TBMMKT_Model.activityFormTBMMKT.selectedBrandOrChannel = "Brand";
                    }
                    else//Channel
                    {
                        activity_TBMMKT_Model.activityFormTBMMKT.selectedBrandOrChannel = "Channel";
                    }

                    TempData["actForm" + activityId] = activity_TBMMKT_Model;
                }
                else
                {
                    mode = "new";
                    string actId = Guid.NewGuid().ToString();
                    activity_TBMMKT_Model.activityFormModel.id = actId;
                    activityFormTBMMKT.master_type_form_id = master_type_form_id;// for production

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
                    //=END==mock data for first input=====

                    //========================ฟอร์มเดินทางปฏฏิบัติงานนอกสถานที่=================
                    #region "ฟอร์มเดินทางปฏฏิบัติงานนอกสถานที่"

                    List<RequestEmpModel> RequestEmp = new List<RequestEmpModel>();
                    for (int i = 0; i < 3; i++)
                    {
                        RequestEmp.Add(new RequestEmpModel() { id = "", empId = "", empName = "", position = "", bu = "" });
                    }
                    activity_TBMMKT_Model.RequestEmp = RequestEmp;

                    List<PlaceDetailModel> PlaceDetailModel = new List<PlaceDetailModel>();
                    for (int i = 0; i < 3; i++)
                    {
                        PlaceDetailModel.Add(new PlaceDetailModel() { place = "", forProject = "", period = "", departureDate = "", arrivalDate = "" });
                    }
                    activity_TBMMKT_Model.PlaceDetailModel = PlaceDetailModel;

                
                    List<CostThemeDetailOfGroupByPriceTBMMKT> expensesDetailModel = new List<CostThemeDetailOfGroupByPriceTBMMKT>();
                    for (int i = 0; i < 6; i++)
                    {
                        expensesDetailModel.Add(new CostThemeDetailOfGroupByPriceTBMMKT() { productDetail = "", QtyName = "", unitPrice = 0, typeTheme = "", unit = 0,  total = 0 });
                    }
                    activity_TBMMKT_Model.expensesDetailModel = expensesDetailModel;
                    #endregion
                    //=======================ฟอร์มเดินทางปฏฏิบัติงานนอกสถานที่====================


                    TempData["actForm" + actId] = activity_TBMMKT_Model;
                }

                activity_TBMMKT_Model.tB_Act_ProductBrand_Model = QueryGetAllBrand.GetAllBrand().Where(x => x.no_tbmmkt != "").ToList();
                activity_TBMMKT_Model.tB_Act_Chanel_Model = QueryGetAllChanel.getAllChanel().Where(x => x.no_tbmmkt != "").ToList(); ;
                activity_TBMMKT_Model.tB_Act_ActivityForm_SelectBrandOrChannel = QueryGetSelectBrandOrChannel.GetAllQueryGetSelectBrandOrChannel();
                activity_TBMMKT_Model.tB_Reg_Subject = QueryGetSelectAllTB_Reg_Subject.GetAllQueryGetSelectAllTB_Reg_Subject().Where(x => x.companyId == UtilsAppCode.Session.User.empCompanyId && x.master_type_form_id == activityFormTBMMKT.master_type_form_id).ToList();
                activity_TBMMKT_Model.activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.activityCondition == "tbmmkt_ChooseActivityOrDetail").ToList();
                activity_TBMMKT_Model.activityFormModel.typeForm = typeForm;
                activity_TBMMKT_Model.activityFormModel.mode = mode;
                activity_TBMMKT_Model.master_Type_Form_Detail_Models = QueryGet_master_type_form_detail.get_master_type_form_detail(activityFormTBMMKT.master_type_form_id, "input");
                activity_TBMMKT_Model.activityFormTBMMKT.companyName = QueryGet_master_company.get_master_company(UtilsAppCode.Session.User.empCompanyId).FirstOrDefault().companyNameTH;
                activity_TBMMKT_Model.activityFormTBMMKT.formName = QueryGet_master_type_form.get_master_type_form(activityFormTBMMKT.master_type_form_id).FirstOrDefault().nameForm;
              
                
                //=======================ฟอร์มเดินทางปฏฏิบัติงานนอกสถานที่====================
                activity_TBMMKT_Model.TB_Reg_RequestEmp = QueryGet_empByComp.getEmpByComp("3030").ToList();
                activity_TBMMKT_Model.TB_Reg_Purpose = QueryGet_master_purpose.getAllPurpose().ToList();
                //=======================ฟอร์มเดินทางปฏฏิบัติงานนอกสถานที่====================


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

    }
}