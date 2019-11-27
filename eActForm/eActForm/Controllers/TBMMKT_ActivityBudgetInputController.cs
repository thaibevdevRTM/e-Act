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
            //activityId = "51f08411-39d0-4702-9410-79f77cddb22a";

            Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
            try
            {
                activity_TBMMKT_Model.activityFormModel.typeForm = typeForm;
                activity_TBMMKT_Model.activityFormModel.mode = mode;

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
                    string actId = Guid.NewGuid().ToString();
                    activity_TBMMKT_Model.activityFormModel.id = actId;

                    //===mock data for first input====
                    List<CostThemeDetailOfGroupByPriceTBMMKT> CostThemeDetailOfGroupByPriceTBMMKT = new List<CostThemeDetailOfGroupByPriceTBMMKT>{
                    new CostThemeDetailOfGroupByPriceTBMMKT() { id="",IO = "", productDetail = "", unit = 0,unitPrice = 0,total=0 }
                    ,new CostThemeDetailOfGroupByPriceTBMMKT() { id="",IO = "", productDetail = "", unit = 0,unitPrice = 0,total=0 }
                    ,new CostThemeDetailOfGroupByPriceTBMMKT() { id="",IO = "", productDetail = "", unit = 0,unitPrice = 0,total=0 }
                    ,new CostThemeDetailOfGroupByPriceTBMMKT() { id="",IO = "", productDetail = "", unit = 0,unitPrice = 0,total=0 }
                    ,new CostThemeDetailOfGroupByPriceTBMMKT() { id="",IO = "", productDetail = "", unit = 0,unitPrice = 0,total=0 }
                    ,new CostThemeDetailOfGroupByPriceTBMMKT() { id="",IO = "", productDetail = "", unit = 0,unitPrice = 0,total=0 }
                    ,new CostThemeDetailOfGroupByPriceTBMMKT() { id="",IO = "", productDetail = "", unit = 0,unitPrice = 0,total=0 }};
                    List<TB_Act_ActivityLayout> List_TB_Act_ActivityLayout = new List<TB_Act_ActivityLayout>{
                    new TB_Act_ActivityLayout() { id="",no = "", io = "", activity = "",amount = 0 }
                    ,new TB_Act_ActivityLayout() { id="",no = "", io = "", activity = "",amount = 0 }
                    ,new TB_Act_ActivityLayout() { id="",no = "", io = "", activity = "",amount = 0 }
                    ,new TB_Act_ActivityLayout() {id="", no = "", io = "", activity = "",amount = 0 }
                    ,new TB_Act_ActivityLayout() { id="",no = "", io = "", activity = "",amount = 0 }
                    ,new TB_Act_ActivityLayout() { id="",no = "", io = "", activity = "",amount = 0 }
                    ,new TB_Act_ActivityLayout() { id="",no = "", io = "", activity = "",amount = 0 }};
                    TB_Act_ActivityForm_DetailOther tB_Act_ActivityForm_DetailOther = new TB_Act_ActivityForm_DetailOther();
                    activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther = tB_Act_ActivityForm_DetailOther;
                    activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.SubjectId = "";
                    activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.productBrandId = "";
                    activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.channelId = "";
                    ActivityFormTBMMKT activityFormTBMMKT = new ActivityFormTBMMKT();
                    activity_TBMMKT_Model.activityFormTBMMKT = activityFormTBMMKT;
                    activity_TBMMKT_Model.activityFormTBMMKT.selectedBrandOrChannel = "";
                    activity_TBMMKT_Model.costThemeDetailOfGroupByPriceTBMMKT = CostThemeDetailOfGroupByPriceTBMMKT;
                    activity_TBMMKT_Model.list_TB_Act_ActivityLayout = List_TB_Act_ActivityLayout;
                    //=END==mock data for first input=====

                    TempData["actForm" + actId] = activity_TBMMKT_Model;
                }

                activity_TBMMKT_Model.tB_Act_ProductBrand_Model = QueryGetAllBrand.GetAllBrand();
                activity_TBMMKT_Model.tB_Act_Chanel_Model = QueryGetAllChanel.getAllChanel();
                activity_TBMMKT_Model.tB_Act_ActivityForm_SelectBrandOrChannel = QueryGetSelectBrandOrChannel.GetAllQueryGetSelectBrandOrChannel();
                activity_TBMMKT_Model.tB_Reg_Subject = QueryGetSelectAllTB_Reg_Subject.GetAllQueryGetSelectAllTB_Reg_Subject().Where(x => x.companyId == UtilsAppCode.Session.User.empCompanyId).ToList();

                TempData.Keep();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("TBMMKT_ActivityBudgetInputController => " + ex.Message);
            }
            return View(activity_TBMMKT_Model);
        }

        [HttpPost] //post method
        [ValidateAntiForgeryToken] // prevents cross site attacks
        [ValidateInput(false)]
        public JsonResult insertDataActivity(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            var result = new AjaxResult();
            try
            {
                string statusId = "";
                //Activity_Model activityModel = TempData["actForm" + activity_TBMMKT_Model.activityFormModel.id] == null ? new Activity_Model() : (Activity_Model)TempData["actForm" + activity_TBMMKT_Model.activityFormModel.id];
                //activityModel.activityFormModel = activityFormModel;
                statusId = ActivityFormCommandHandler.getStatusActivity(activity_TBMMKT_Model.activityFormModel.id);
                if (statusId == "1" || statusId == "5" || statusId == "")
                {
                    int countSuccess = ActivityFormTBMMKTCommandHandler.insertAllActivity(activity_TBMMKT_Model, activity_TBMMKT_Model.activityFormModel.id);
                }
                else
                {

                }
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

    }
}