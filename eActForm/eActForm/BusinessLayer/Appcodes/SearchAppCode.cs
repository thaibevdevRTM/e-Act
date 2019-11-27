﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eActForm.Models;
using eActForm.BusinessLayer;
namespace eActForm.BusinessLayer
{
    public class SearchAppCode
    {
        public static SearchActivityModels getMasterDataForSearchForDetailReport()
        {
            try
            {
                SearchActivityModels models = new SearchActivityModels
                {
                    approveStatusList = ApproveAppCode.getApproveStatus(AppCode.StatusType.app),
                    productGroupList = QueryGetAllProductGroup.getAllProductGroup(),
                    customerslist = QueryGetAllCustomers.getCustomersByEmpId().Where(x => x.cusNameEN != "").ToList(),
                    productTypelist = QuerygetAllProductCate.getProductTypeByEmpId(),
                    activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup()
                    .Where(x => x.activityCondition.Equals("mtm".ToLower()))
                   .GroupBy(item => item.activitySales)
                   .Select(grp => new TB_Act_ActivityGroup_Model { id = grp.First().id, activitySales = grp.First().activitySales }).ToList()
                };
                return models;
            }
            catch (Exception ex)
            {
                throw new Exception("getMasterDataForSearchForDetailReport >>" + ex.Message);
            }
        }
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
    }
}