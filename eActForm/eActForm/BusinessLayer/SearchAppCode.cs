using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eActForm.Models;
using eActForm.BusinessLayer;
namespace eActForm.BusinessLayer
{
    public class SearchAppCode
    {
        public static SearchActivityModels getMasterDataForSearch()
        {
            try
            {
                SearchActivityModels models = new SearchActivityModels();
                models.approveStatusList = ApproveAppCode.getApproveStatus(AppCode.StatusType.app);
                models.productGroupList = QueryGetAllProductGroup.getAllProductGroup();
                models.customerslist = QueryGetAllCustomers.getAllCustomers().Where(x => x.cusNameEN != "").ToList();
                models.productTypelist = QuerygetAllProductCate.getAllProductType();
                models.activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup()
                    .GroupBy(item => item.activitySales)
                    .Select(grp => new TB_Act_ActivityGroup_Model { id = grp.First().id, activitySales = grp.First().activitySales }).ToList();

                return models;
            }
            catch(Exception ex)
            {
                throw new Exception("getMasterDataForSearch >>" + ex.Message);
            }
        }
    }
}