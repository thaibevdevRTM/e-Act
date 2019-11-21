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
    public class TBMMKT_ActivityBudgetInputController : Controller
    {
        // GET: TBMMKT_ActivityBudgetInput
        public ActionResult Index(string activityId, string mode, string typeForm)
        {
            Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
            try
            {

                //activity_TBMMKT_Model.activityFormModel = new ActivityForm();
                //activity_TBMMKT_Model.productSmellLists = new List<TB_Act_Product_Model.ProductSmellModel>();

                //if (typeForm == Activity_Model.activityType.OMT.ToString())
                //{
                //    activity_TBMMKT_Model.customerslist = QueryGetAllCustomers.getCustomersOMT();
                //}
                //else
                //{
                //    activity_TBMMKT_Model.customerslist = QueryGetAllCustomers.getCustomersMT();
                //}

                //activity_TBMMKT_Model.productcatelist = QuerygetAllProductCate.getAllProductCate().ToList();
                //activity_TBMMKT_Model.activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup()
                //    .GroupBy(item => item.activitySales)
                //    .Select(grp => new TB_Act_ActivityGroup_Model { id = grp.First().id, activitySales = grp.First().activitySales }).ToList();
                //if (UtilsAppCode.Session.User.regionId != "")
                //{
                //    activity_TBMMKT_Model.regionGroupList = QueryGetAllRegion.getAllRegion().Where(x => x.id == UtilsAppCode.Session.User.regionId).ToList();
                //    activity_TBMMKT_Model.activityFormModel.regionId = UtilsAppCode.Session.User.regionId;
                //}
                //else
                //{
                //    activity_TBMMKT_Model.regionGroupList = QueryGetAllRegion.getAllRegion();
                //}


                activity_TBMMKT_Model.tB_Act_ProductBrand_Model = QueryGetAllBrand.GetAllBrand();
                activity_TBMMKT_Model.tB_Act_Chanel_Model = QueryGetAllChanel.getAllChanel();
                activity_TBMMKT_Model.tB_Act_ActivityForm_SelectBrandOrChannel = QueryGetSelectBrandOrChannel.GetAllQueryGetSelectBrandOrChannel();
                activity_TBMMKT_Model.tB_Reg_Subject = QueryGetSelectAllTB_Reg_Subject.GetAllQueryGetSelectAllTB_Reg_Subject().Where(x => x.companyId == UtilsAppCode.Session.User.empCompanyId).ToList();

                if (!string.IsNullOrEmpty(activityId))
                {

                    activity_TBMMKT_Model.activityFormModel = QueryGetActivityById.getActivityById(activityId).FirstOrDefault();
                    activity_TBMMKT_Model.activityFormModel.mode = mode;
                    //activity_TBMMKT_Model.productcostdetaillist1 = QueryGetCostDetailById.getcostDetailById(activityId);
                    //activity_TBMMKT_Model.activitydetaillist = QueryGetActivityDetailById.getActivityDetailById(activityId);
                    //activity_TBMMKT_Model.productSmellLists = QueryGetAllProduct.getProductSmellByGroupId(activity_TBMMKT_Model.activityFormModel.productGroupId);
                    //activity_TBMMKT_Model.productBrandList = QueryGetAllBrand.GetAllBrand().Where(x => x.productGroupId == activity_TBMMKT_Model.activityFormModel.productGroupId).ToList();
                    //activity_TBMMKT_Model.productGroupList = QueryGetAllProductGroup.getAllProductGroup().Where(x => x.cateId == activity_TBMMKT_Model.activityFormModel.productCateId).ToList();
                    TempData["actForm" + activityId] = activity_TBMMKT_Model;
                }
                else
                {
                    string actId = Guid.NewGuid().ToString();
                    activity_TBMMKT_Model.activityFormModel.id = actId;
                    //activity_TBMMKT_Model.activityFormModel.mode = mode;
                    //activity_TBMMKT_Model.activityFormModel.statusId = 1;

                    //===mock uat====
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
                    activity_TBMMKT_Model.costThemeDetailOfGroupByPriceTBMMKT = CostThemeDetailOfGroupByPriceTBMMKT;
                    activity_TBMMKT_Model.list_TB_Act_ActivityLayout = List_TB_Act_ActivityLayout;
                    //=END==mock uat====

                    TempData["actForm" + actId] = activity_TBMMKT_Model;
                }

                activity_TBMMKT_Model.activityFormModel.typeForm = typeForm;
                TempData.Keep();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("TBMMKT_ActivityBudgetInputController => " + ex.Message);
            }
            return View(activity_TBMMKT_Model);
        }


        public ActionResult insertDataActivity(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {

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

                TempData.Keep();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("insertDataActivityTBMMKT => " + ex.Message);
            }

            return RedirectToAction("index", new { activityId= activity_TBMMKT_Model.activityFormModel.id, mode="edit", typeForm="" });
        }

    }
}