using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using eForms.Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
namespace eActForm.BusinessLayer
{
    public class ReportAppCode
    {
        //public static ReportTypeModel getReporttypeByTypeFormId(string typeFormId)
        //{
        //    try
        //    {
        //      ReportTypeModel models = new ReportTypeModel
        //        {
        //           // showUIModel = new searchParameterFilterModel(),
        //           // approveStatusList = ApproveAppCode.getApproveStatus(AppCode.StatusType.app),
        //           // productGroupList = QueryGetAllProductGroup.getAllProductGroup(),
        //           // customerslist = QueryGetAllCustomers.getCustomersByEmpId().Where(x => x.cusNameEN != "").ToList(),
        //           // productTypelist = QuerygetAllProductCate.getProductTypeByEmpId(),
        //           // activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup()
        //           //.GroupBy(item => item.activitySales)
        //           //.Select(grp => new TB_Act_ActivityGroup_Model { id = grp.First().id, activitySales = grp.First().activitySales }).ToList()
        //        };
        //        return ReportTypeModel;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("getMasterDataForSearchForDetailReport >>" + ex.Message);
        //    }
        //}
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
        public static List<CompanyModel> getCompanyByRole(string typeFormId)
        {
            String compId = "";
            try
            {
                List<CompanyModel> companyList = new List<CompanyModel>();

                companyList = QueryGetAllCompany.getCompanyByTypeFormId(typeFormId);

                List<ActUserModel.UserAuthorized> lst = new List<ActUserModel.UserAuthorized>();
                lst = UserAppCode.GetUserAuthorizedsByCompany(UtilsAppCode.Session.User.empCompanyGroup);

                compId = lst.Count > 0 ? lst.FirstOrDefault().companyId : "";
                if (UtilsAppCode.Session.User.isAdminHCBP)
                {

                }
                else if (UtilsAppCode.Session.User.isAdminNUM || UtilsAppCode.Session.User.isAdminPOM || UtilsAppCode.Session.User.isAdminCVM)
                {
                    companyList = companyList.Where(x => x.companyId == compId).OrderBy(x => x.companyNameTH).ToList();
                }
                else
                { //user ปกติดูได้หรือป่าว

                }


                //if (actType == Activity_Model.activityType.HCForm.ToString())
                //{

                //    if (UtilsAppCode.Session.User.isSuperAdmin)
                //    {
                //        List<TB_Act_Other_Model> lst = new List<TB_Act_Other_Model>();
                //        lst = QueryOtherMaster.getOhterMaster("company", Activity_Model.groupCompany.NUM.ToString());
                //        foreach (var item in lst)
                //        {
                //            compId += item.val1 + ",";
                //        }

                //        lst = QueryOtherMaster.getOhterMaster("company", Activity_Model.groupCompany.POM.ToString());
                //        foreach (var item in lst)
                //        {
                //            compId += item.val1 + ",";
                //        }

                //        lst = QueryOtherMaster.getOhterMaster("company", Activity_Model.groupCompany.CVM.ToString());
                //        foreach (var item in lst)
                //        {
                //            compId += item.val1 + ",";
                //        }

                //        compId = compId.Substring(0, compId.Length - 1);
                //    }
                //    else
                //    {

                //    }



                //    //    List<CompanyModel> getCompanyByTypeFormId
                //    //DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getUserAuthorizedByEmpId"
                //    //    , new SqlParameter[] { new SqlParameter("@empId", UtilsAppCode.Session.User.empId) });
                //    //var lists = (from DataRow dr in ds.Tables[0].Rows
                //    //             select new CompanyModel
                //    //             {
                //    //                 empId = dr["empId"].ToString(),
                //    //                 customerId = dr["customerId"].ToString(),
                //    //                 productCateId = dr["productCateId"].ToString(),
                //    //                 productTypeId = dr["productTypeId"].ToString(),
                //    //                 companyId = dr["companyId"].ToString()
                //    //             }).ToList();
                //    //return lists;
                //}
                return companyList; //ถ้าเป็น superadmim ถึงจะดึงทั้ง 8 ถ้าไม่ดึงตัวเอง
            }
            catch (Exception ex)
            {
                throw new Exception("getCompanyByRole >>" + ex.Message);
            }
        }
    }
}