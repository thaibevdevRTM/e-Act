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
                if (typeForm == Activity_Model.activityType.SetPrice.ToString()) typeForm = "reps";
                SearchActivityModels models = new SearchActivityModels
                {
                    showUIModel = new searchParameterFilterModel(),
                    approveStatusList = ApproveAppCode.getApproveStatus(AppCode.StatusType.app),
                    productGroupList = QueryGetAllProductGroup.getAllProductGroup(),
                    customerslist = typeForm == Activity_Model.activityType.MT.ToString() ? QueryGetAllCustomers.getCustomersMT().Where(x => x.cusNameEN != "").ToList() :
                    QueryGetAllCustomers.getCustomersOMT().Where(x => x.cusNameEN != "").ToList(),
                    productTypelist = QuerygetAllProductCate.getProductTypeByEmpId(),
                    productBrandList = QueryGetAllBrand.GetAllBrand().OrderBy(x => x.brandName).ToList(),
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

        public static SearchActivityModels getMasterDataForSearchForPostEVAReport(string typeForm)
        {
            try
            {
                if (typeForm == Activity_Model.activityType.SetPrice.ToString()) typeForm = "reps";
                SearchActivityModels models = new SearchActivityModels
                {
                    showUIModel = new searchParameterFilterModel(),
                    approveStatusList = ApproveAppCode.getApproveStatus(AppCode.StatusType.app),
                    productGroupList = QueryGetAllProductGroup.getAllProductGroup(),
                    customerslist = typeForm == Activity_Model.activityType.MT.ToString() ? QueryGetAllCustomers.getCustomersMT().Where(x => x.cusNameEN != "").ToList() :
                    QueryGetAllCustomers.getCustomersOMT().Where(x => x.cusNameEN != "").ToList(),
                    productTypelist = QuerygetAllProductCate.getProductTypeByEmpId(),
                    productBrandList = QueryGetAllBrand.GetAllBrand().OrderBy(x => x.brandName).ToList(),
                    //activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.activityCondition.Contains(typeForm))
                    activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.activityCondition.Contains(typeForm) & (x.activitySales == "Promotion Support" || x.activitySales == "Special Discount"))
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
                    companyList = ReportSummaryAppCode.getCompanyMTMList(),
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