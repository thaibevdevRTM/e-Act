using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
namespace eActForm.BusinessLayer
{
    public class UserAppCode
    {




        public static List<ActUserModel.UserAuthorized> GetUserAuthorizeds()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getUserAuthorizedByEmpId"
                    , new SqlParameter[] { new SqlParameter("@empId", UtilsAppCode.Session.User.empId) });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ActUserModel.UserAuthorized
                             {
                                 empId = dr["empId"].ToString(),
                                 customerId = dr["customerId"].ToString(),
                                 productCateId = dr["productCateId"].ToString(),
                                 productTypeId = dr["productTypeId"].ToString(),
                                 companyId = dr["companyId"].ToString()
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("GetUserAuthorizeds>>" + ex.Message);
            }
        }
        public static int setRoleUser(string strUserName)
        {
            try
            {
                int rtn = 0;
                if (UtilsAppCode.Session.User != null)
                {
                    DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getUserByEmpId"
                        , new SqlParameter[] { new SqlParameter("@empId", strUserName) });
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        rtn = ds.Tables[0].Rows.Count;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            switch (dr["roleId"].ToString())
                            {
                                case "1":
                                    UtilsAppCode.Session.User.isCreator = true; break;
                                case "2":
                                    UtilsAppCode.Session.User.isApprove = true; break;
                                case "3":
                                    UtilsAppCode.Session.User.isAdmin = true; break;
                                case "4":
                                    UtilsAppCode.Session.User.isSuperAdmin = true; break;
                                case "5":
                                    UtilsAppCode.Session.User.isAdminOMT = true; break;
                                case "6":
                                    UtilsAppCode.Session.User.isAdminTBM = true; break;
                                case "7":
                                    UtilsAppCode.Session.User.isAdminHCM = true; break;
                                case "8":
                                    UtilsAppCode.Session.User.isAdminNUM = true; break;
                                case "9":
                                    UtilsAppCode.Session.User.isAdminPOM = true; break;
                                case "10":
                                    UtilsAppCode.Session.User.isAdminChangInter = true; break;
                                case "11":
                                    UtilsAppCode.Session.User.isAdminCVM = true; break;
                                case "12":
                                    UtilsAppCode.Session.User.isAdminHCBP = true; break;

                            }


                            UtilsAppCode.Session.User.empCompanyId = !string.IsNullOrEmpty(dr["companyId"].ToString()) ? dr["companyId"].ToString() : UtilsAppCode.Session.User.empCompanyId;

                            UtilsAppCode.Session.User.empCompanyList.Add(dr["companyId"].ToString());

                            UtilsAppCode.Session.User.regionId = dr["regionId"].ToString();
                            UtilsAppCode.Session.User.empCompanyGroup = !string.IsNullOrEmpty(dr["companyId"].ToString()) ? ActFormAppCode.getGrpCompByCompId(dr["companyId"].ToString()) : UtilsAppCode.Session.User.empCompanyGroup;


                        }
                    }
                    else
                    {
                        bool checkRoleForInsert = getCheckRoleUserForInsert();
                        rtn = setRoleUser(strUserName);
                    }

                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception("setRoleUser>>" + ex.Message);
            }
        }

        public static List<ActUserModel.UserAuthorized> GetUserAuthorizedsByCompany(string subType)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getUserAuthorizedByCompany"
                    , new SqlParameter[] { new SqlParameter("@empId", UtilsAppCode.Session.User.empId)
                    , new SqlParameter("@subType",subType) });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ActUserModel.UserAuthorized
                             {
                                 empId = dr["empId"].ToString(),
                                 customerId = dr["customerId"].ToString(),
                                 productCateId = dr["productCateId"].ToString(),
                                 productTypeId = dr["productTypeId"].ToString(),
                                 companyId = dr["companyId"].ToString()
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("GetUserAuthorizeds>>" + ex.Message);
            }
        }


        public static bool getCheckRoleUserForInsert()
        {
            try
            {
                bool result = false;
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_checkRoleByEmpId"
                    , new SqlParameter[] { new SqlParameter("@empId", UtilsAppCode.Session.User.empId)
                                         , new SqlParameter("@companyId",UtilsAppCode.Session.User.empCompanyId) });

                if (ds.Tables[0].Rows.Count > 0)
                {
                    result = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("getCheckRoleUser >>" + ex.Message);
            }
        }

    }
}