using eActForm.BusinessLayer;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    public partial class partialActivityDetailController
    {
        public ActionResult partialactSetPriceMT(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            try
            {
                activity_TBMMKT_Model.productSmellLists = new List<TB_Act_Product_Model.ProductSmellModel>();
                activity_TBMMKT_Model.customerslist = QueryGetAllCustomers.getCustomersMT();

                activity_TBMMKT_Model.productcatelist = QuerygetAllProductCate.getAllProductCate().ToList();
                activity_TBMMKT_Model.activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup()
                    .Where(x => x.activityCondition.Equals("mtm".ToLower()))
                    .GroupBy(item => item.activitySales)
                    .Select(grp => new TB_Act_ActivityGroup_Model { id = grp.First().id, activitySales = grp.First().activitySales }).ToList();

                if (activity_TBMMKT_Model.activityFormModel.mode == Activity_Model.modeForm.edit.ToString())
                {

                    activity_TBMMKT_Model.activityFormModel = QueryGetActivityById.getActivityById(activity_TBMMKT_Model.activityFormTBMMKT.id).FirstOrDefault();
                    //activityModel.productcostdetaillist1 = QueryGetCostDetailById.getcostDetailById(activityId);
                    //activityModel.activitydetaillist = QueryGetActivityDetailById.getActivityDetailById(activityId);
                    activity_TBMMKT_Model.productSmellLists = QueryGetAllProduct.getProductSmellByGroupId(activity_TBMMKT_Model.activityFormModel.productGroupId);
                    activity_TBMMKT_Model.productBrandList = QueryGetAllBrand.GetAllBrand().Where(x => x.productGroupId == activity_TBMMKT_Model.activityFormModel.productGroupId).ToList();
                    activity_TBMMKT_Model.productGroupList = QueryGetAllProductGroup.getAllProductGroup().Where(x => x.cateId == activity_TBMMKT_Model.activityFormModel.productCateId).ToList();
                    //TempData["actForm" + activity_TBMMKT_Model.activityFormTBMMKT.id] = activity_TBMMKT_Model;
                    ViewBag.chkClaim = activity_TBMMKT_Model.activityFormModel.chkAddIO;
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("ActivityForm => " + ex.Message);
            }


            return PartialView(activity_TBMMKT_Model);
        }

    }
}