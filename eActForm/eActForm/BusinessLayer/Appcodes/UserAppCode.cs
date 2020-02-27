using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using eActForm.Models;
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
                                 productTypeId = dr["productTypeId"].ToString()
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
                    if (ds.Tables.Count > 0)
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
                            }

                            UtilsAppCode.Session.User.empCompanyId = dr["companyId"].ToString();
                            UtilsAppCode.Session.User.regionId = dr["regionId"].ToString();
                            UtilsAppCode.Session.User.customerId = dr["customerId"].ToString();
                        }
                    }
                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception("setRoleUser>>" + ex.Message);
            }
        }
    }
}