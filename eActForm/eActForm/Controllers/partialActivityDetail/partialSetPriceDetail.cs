using eActForm.BusinessLayer;
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
    public partial class partialActivityDetailController
    {
        public ActionResult partialSetPriceDetail(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            try
            {
                activity_TBMMKT_Model.productSmellLists = new List<TB_Act_Product_Model.ProductSmellModel>();
                activity_TBMMKT_Model.customerslist = QueryGetAllCustomers.getCustomersMT();

                activity_TBMMKT_Model.productcatelist = QuerygetAllProductCate.getAllProductCate().ToList();
               

                if(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formEactBeer"])
                {
                    activity_TBMMKT_Model.activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup()
                    .Where(x => x.activityCondition.Contains(Activity_Model.activityType.MT.ToString()))
                    .GroupBy(item => item.activitySales)
                    .Select(grp => new TB_Act_ActivityGroup_Model { id = grp.First().id, activitySales = grp.First().activitySales }).ToList();

                    activity_TBMMKT_Model.activityTypeList = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.activityCondition.ToLower().Contains("actBeer".ToLower())).ToList();
                    activity_TBMMKT_Model.regionGroupList = QueryGetAllRegion.getAllRegion().Where(x => x.condition.Equals("actBeer")).OrderBy(x => x.descTh).ToList();
                    activity_TBMMKT_Model.otherList_1 = QueryOtherMaster.getOhterMaster("mainAgency", "");
                    activity_TBMMKT_Model.otherList_2 = QueryOtherMaster.getOhterMaster("subAgency", "");
                    activity_TBMMKT_Model.otherList_3 = QueryOtherMaster.getOhterMaster("pay", "");
                    activity_TBMMKT_Model.otherList_4 = QueryOtherMaster.getOhterMaster("game", "");
                    activity_TBMMKT_Model.otherList_5 = QueryOtherMaster.getOhterMaster("area", "");
                    activity_TBMMKT_Model.tB_Act_Chanel_Model = QueryGetAllChanel.getAllChanel().Where(x => x.no_tbmmkt.Equals("actBeer")).ToList();

                }
                else
                {
                    activity_TBMMKT_Model.activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup()
                   .Where(x => x.activityCondition.Contains("sp".ToLower()))
                   .GroupBy(item => item.activitySales)
                   .Select(grp => new TB_Act_ActivityGroup_Model { id = grp.First().id, activitySales = grp.First().activitySales }).ToList();

                    if (UtilsAppCode.Session.User.regionId != "")
                    {
                        activity_TBMMKT_Model.regionGroupList = QueryGetAllRegion.getRegoinByEmpId(UtilsAppCode.Session.User.empId);
                        activity_TBMMKT_Model.activityFormModel.regionId = UtilsAppCode.Session.User.regionId;
                    }
                    else
                    {

                        activity_TBMMKT_Model.regionGroupList = QueryGetAllRegion.getAllRegion().Where(x => x.condition.Equals("OMT")).ToList();
                    }

                }
                    
                if (activity_TBMMKT_Model.activityFormModel.mode == Activity_Model.modeForm.edit.ToString())
                {
                    activity_TBMMKT_Model.productSmellLists = QueryGetAllProduct.getProductSmellByGroupId(activity_TBMMKT_Model.activityFormModel.productGroupId);
                    activity_TBMMKT_Model.productBrandList = QueryGetAllBrand.GetAllBrand().Where(x => x.productGroupId == activity_TBMMKT_Model.activityFormModel.productGroupId).ToList();
                    activity_TBMMKT_Model.productGroupList = QueryGetAllProductGroup.getAllProductGroup().Where(x => x.cateId == activity_TBMMKT_Model.activityFormModel.productCateId).ToList();
                    ViewBag.chkClaim = activity_TBMMKT_Model.activityFormModel.chkAddIO;

                    //เก็บใส่ Temp ใช้กับหน้า Productlist เพราะ สูตรการคำนวณของเดิม ใช้Temp เก็บค่า
                    activity_TBMMKT_Model.productcostdetaillist1 = QueryGetCostDetailById.getcostDetailById(activity_TBMMKT_Model.activityFormTBMMKT.id);

                }

                
                TempData["actForm" + activity_TBMMKT_Model.activityFormModel.id] = activity_TBMMKT_Model;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("ActivityForm => " + ex.Message);
            }


            return PartialView(activity_TBMMKT_Model);
        }


    }
}