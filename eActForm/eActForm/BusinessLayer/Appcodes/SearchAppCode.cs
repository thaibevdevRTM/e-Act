using eActForm.Models;
using eForms.Models.Forms;
using System;
using System.Configuration;
using System.Linq;
namespace eActForm.BusinessLayer
{
    public class SearchAppCode
    {
        public static SearchActivityModels getMasterDataForSearchForDetailReport(string typeForm)
        {
            try
            {
                typeForm = typeForm == Activity_Model.activityType.SetPrice.ToString() ? "reps" : "mtm";
                SearchActivityModels models = new SearchActivityModels
                {
                    showUIModel = new searchParameterFilterModel(),
                    approveStatusList = ApproveAppCode.getApproveStatus(AppCode.StatusType.app),
                    productGroupList = QueryGetAllProductGroup.getAllProductGroup(),
                    customerslist = QueryGetAllCustomers.getCustomersByEmpId().Where(x => x.cusNameEN != "").ToList(),
                    productTypelist = QuerygetAllProductCate.getProductTypeByEmpId(),
                    activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.activityCondition.Contains(typeForm))
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
                    customerslist = @UtilsAppCode.Session.User.empCompanyId == ConfigurationManager.AppSettings["companyId_MT"] ? QueryGetAllCustomers.getCustomersMT().Where(x => x.cusNameEN != "").ToList() : QueryGetAllCustomers.getCustomersOMT().Where(x => x.cusNameEN != "").ToList(),
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