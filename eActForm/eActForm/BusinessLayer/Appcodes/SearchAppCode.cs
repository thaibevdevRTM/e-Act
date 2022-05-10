using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using eForms.Models.Forms;
using System;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;

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
                    masterTypeFormList = QueryGet_master_type_form.get_master_type_form("").Where(x => x.companyId == UtilsAppCode.Session.User.empCompanyId).ToList(),
                    departmentList = QueryOtherMaster.getOhterMaster("department", "search").ToList(),
                    channelList = QueryGetAllChanel.getAllChanel().Where(x => x.typeChannel == "data").ToList(),
                    activityGroupList = !string.IsNullOrEmpty(typeForm) ? QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.activityCondition.Contains(typeForm))
                   .GroupBy(item => item.activitySales)
                   .Select(grp => new TB_Act_ActivityGroup_Model { id = grp.First().id, activitySales = grp.First().activitySales }).ToList() : QueryGetAllActivityGroup.getAllActivityGroup(),
                    activityGroupBeerList = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.activityCondition.Contains(ConfigurationManager.AppSettings["conditionActBeer"]))
                    .GroupBy(item => item.activitySales)
                    .Select(grp => new TB_Act_ActivityGroup_Model { id = grp.First().id, activitySales = grp.First().activitySales }).ToList(),
                    mainAgencyList = QueryOtherMaster.getOhterMaster("mainAgency", "").ToList(),
                    regionGroupList = QueryGetAllRegion.getAllRegion().Where(x => x.condition.Equals(ConfigurationManager.AppSettings["conditionActBeer"])).OrderBy(x => x.name).ToList(),

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
                    masterTypeFormList = QueryGet_master_type_form.get_master_type_form("").Where(x => x.companyId == UtilsAppCode.Session.User.empCompanyId).ToList(),
                    departmentList = QueryOtherMaster.getOhterMaster("department", "search").ToList(),
                    channelList = QueryGetAllChanel.getAllChanel().Where(x => x.typeChannel == "data").ToList(),
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
            string getRegion = "";
            try
            {
                try
                {
                    getRegion = Regex.Match(UtilsAppCode.Session.User.empDepartmentEN, @"\d+").Value;
                }
                catch (Exception ex)
                {

                }

                SearchActivityModels models = new SearchActivityModels();
                models.companyList = ReportSummaryAppCode.getCompanyMTMList();
                models.approveStatusList = ApproveAppCode.getApproveStatus(AppCode.StatusType.app);
                models.approveStatusList2 = ApproveAppCode.getApproveStatus(AppCode.StatusType.app);
                models.productGroupList = QueryGetAllProductGroup.getAllProductGroup();
                models.customerslist = @UtilsAppCode.Session.User.empCompanyId == ConfigurationManager.AppSettings["companyId_MT"] ? QueryGetAllCustomers.getCustomersMT().Where(x => x.cusNameEN != "").ToList() : QueryGetAllCustomers.getCustomersOMT().Where(x => x.cusNameEN != "").ToList();
                models.productTypelist = QuerygetAllProductCate.getAllProductType();
                models.masterTypeFormList = QueryGet_master_type_form.getmastertypeformByEmpId(UtilsAppCode.Session.User.empId);
                models.productBrandList = QueryGetAllBrand.GetAllBrand().OrderBy(x => x.brandName).ToList();
                models.departmentList = QueryOtherMaster.getOhterMaster("department", "search").ToList();
                models.channelList = QueryGetAllChanel.getAllChanel().Where(x => x.typeChannel == "data").ToList();


                models.activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.activityCondition.Contains("MT"))
                 .GroupBy(item => item.activitySales)
                 .Select(grp => new TB_Act_ActivityGroup_Model { id = grp.First().id, activitySales = grp.First().activitySales }).ToList();
                models.activityGroupBeerList = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.activityCondition.Contains(ConfigurationManager.AppSettings["conditionActBeer"]))
                .GroupBy(item => item.activitySales)
                .Select(grp => new TB_Act_ActivityGroup_Model { id = grp.First().id, activitySales = grp.First().activitySales }).ToList();
                models.mainAgencyList = QueryOtherMaster.getOhterMaster("mainAgency", "").ToList();
                models.regionGroupList = QueryGetAllRegion.getAllRegion().Where(x => x.condition.Equals(ConfigurationManager.AppSettings["conditionActBeer"]))
                     .GroupBy(item => new
                     {
                         item.name
                     })
                     .Select((group, index) => new TB_Act_Region_Model
                     {
                         id = group.First().id,
                         name = group.First().name
                     }).OrderBy(x => x.name).ToList();

                if (!string.IsNullOrEmpty(getRegion))
                {
                    models.regionGroupList = models.regionGroupList.Where(x => x.name.Contains(getRegion)).ToList();
                }

                return models;
            }
            catch (Exception ex)
            {
                throw new Exception("getMasterDataForSearch >>" + ex.Message);
            }
        }
    }
}
