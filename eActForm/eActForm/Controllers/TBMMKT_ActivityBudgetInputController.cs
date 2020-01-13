using eActForm.BusinessLayer;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class TBMMKT_ActivityBudgetInputController : Controller
    {
        // GET: TBMMKT_ActivityBudgetInput
        public ActionResult Index(string activityId, string mode, string typeForm)
        {

            Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
            try
            {

                if (!string.IsNullOrEmpty(activityId))
                {

                    activity_TBMMKT_Model = ActivityFormTBMMKTCommandHandler.getDataForEditActivity(activityId);

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
                   ActivityFormTBMMKT activityFormTBMMKT = new ActivityFormTBMMKT();
                    activity_TBMMKT_Model.activityFormTBMMKT = activityFormTBMMKT;
                    activity_TBMMKT_Model.activityFormTBMMKT.selectedBrandOrChannel = "";
                    activity_TBMMKT_Model.costThemeDetailOfGroupByPriceTBMMKT = costThemeDetailOfGroupByPriceTBMMKT;
                    activity_TBMMKT_Model.totalCostThisActivity = decimal.Parse("0.00");
                    //=END==mock data for first input=====

                    TempData["actForm" + actId] = activity_TBMMKT_Model;
                }

                activity_TBMMKT_Model.tB_Act_ProductBrand_Model = QueryGetAllBrand.GetAllBrand().Where(x => x.no_tbmmkt != "").ToList();
                activity_TBMMKT_Model.tB_Act_Chanel_Model = QueryGetAllChanel.getAllChanel().Where(x => x.no_tbmmkt != "").ToList(); ;
                activity_TBMMKT_Model.tB_Act_ActivityForm_SelectBrandOrChannel = QueryGetSelectBrandOrChannel.GetAllQueryGetSelectBrandOrChannel();
                activity_TBMMKT_Model.tB_Reg_Subject = QueryGetSelectAllTB_Reg_Subject.GetAllQueryGetSelectAllTB_Reg_Subject().Where(x => x.companyId == UtilsAppCode.Session.User.empCompanyId && x.master_type_form_id == "").ToList();
                activity_TBMMKT_Model.activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.activityCondition == "tbmmkt_ChooseActivityOrDetail").ToList();
                activity_TBMMKT_Model.activityFormModel.typeForm = typeForm;
                activity_TBMMKT_Model.activityFormModel.mode = mode;

                TempData.Keep();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("TBMMKT_ActivityBudgetInputController => " + ex.Message);
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
                ExceptionManager.WriteError("insertDataActivityTBMMKT => " + ex.Message);
            }

            return Json(result);
        }

        //==============เก็บไว้ก่อน ถ้า On Production ใบยืมเงินทดลองไปแล้วไม่เกิดปัยหาอะไร ตามมาลบทิ้งได้เลย==========20200110 fream=====
        //public JsonResult updateDataIOActivity(Activity_TBMMKT_Model activity_TBMMKT_Model)
        //{
        //    var result = new AjaxResult();
        //    try
        //    {

        //        int countSuccess = ActivityFormTBMMKTCommandHandler.updateIOActivity(activity_TBMMKT_Model, activity_TBMMKT_Model.activityFormModel.id);
        //        result.Data = activity_TBMMKT_Model.activityFormModel.id;
        //        result.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Success = false;
        //        result.Message = ex.Message;
        //        ExceptionManager.WriteError("updateDataIOActivityTBMMKT => " + ex.Message);
        //    }

        //    return Json(result);
        //}
        //=====END=========เก็บไว้ก่อน ถ้า On Production ใบยืมเงินทดลองไปแล้วไม่เกิดปัยหาอะไร ตามมาลบทิ้งได้เลย==========20200110 fream=====
    }
}