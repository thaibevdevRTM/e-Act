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


        public static bool checkUserDBAuthen()
        {

            int result = 0;
            bool insertResult = false;
            try
            {
                 result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_checkUserDBAuthenInsert"
                    , new SqlParameter[] {new SqlParameter("@empId",UtilsAppCode.Session.User.empId)
                    ,new SqlParameter("@empStatus",UtilsAppCode.Session.User.empStatus)
                    ,new SqlParameter("@empPrefix",UtilsAppCode.Session.User.empPrefix)
                    ,new SqlParameter("@empGender",UtilsAppCode.Session.User.empGender)
                    ,new SqlParameter("@empFNameEN",UtilsAppCode.Session.User.empFNameEN)
                    ,new SqlParameter("@empLNameEN",UtilsAppCode.Session.User.empLNameEN)
                    ,new SqlParameter("@empFNameTH",UtilsAppCode.Session.User.empFNameTH)
                    ,new SqlParameter("@empLNameTH",UtilsAppCode.Session.User.empLNameTH)
                    ,new SqlParameter("@empDateofBirth",UtilsAppCode.Session.User.empDateofBirth)
                    ,new SqlParameter("@empMaritalStatus",UtilsAppCode.Session.User.empMaritalStatus)
                    ,new SqlParameter("@empNationality",UtilsAppCode.Session.User.empNationality)
                    ,new SqlParameter("@empPosition",UtilsAppCode.Session.User.empPosition)
                    ,new SqlParameter("@empPositionTitleEN",UtilsAppCode.Session.User.empPositionTitleEN)
                    ,new SqlParameter("@empPositionTitleTH",UtilsAppCode.Session.User.empPositionTitleTH)
                    ,new SqlParameter("@empLocalPositionEN",UtilsAppCode.Session.User.empLocalPositionEN)
                    ,new SqlParameter("@empLocalPositionTH",UtilsAppCode.Session.User.empLocalPositionTH)
                    ,new SqlParameter("@empExternalPositionEN",UtilsAppCode.Session.User.empExternalPositionEN)
                    ,new SqlParameter("@empExternalPositionTH",UtilsAppCode.Session.User.empExternalPositionTH)
                    ,new SqlParameter("@empPositionLevel",UtilsAppCode.Session.User.empPositionLevel)
                    ,new SqlParameter("@empPositionRange",UtilsAppCode.Session.User.empPositionRange)
                    ,new SqlParameter("@empLevel",UtilsAppCode.Session.User.empLevel)
                    ,new SqlParameter("@empClass",UtilsAppCode.Session.User.empClass)
                    ,new SqlParameter("@empType",UtilsAppCode.Session.User.empType)
                    ,new SqlParameter("@empCategory",UtilsAppCode.Session.User.empCategory)
                    ,new SqlParameter("@empCompanyGroup",UtilsAppCode.Session.User.empCompanyGroup)
                    ,new SqlParameter("@empCompanyId",UtilsAppCode.Session.User.empCompanyId)
                    ,new SqlParameter("@empDivisionEN",UtilsAppCode.Session.User.empDivisionEN)
                    ,new SqlParameter("@empDivisionTH",UtilsAppCode.Session.User.empDivisionTH)
                    ,new SqlParameter("@empDepartmentEN",UtilsAppCode.Session.User.empDepartmentEN)
                    ,new SqlParameter("@empDepartmentTH",UtilsAppCode.Session.User.empDepartmentTH)
                    ,new SqlParameter("@empManagerId",UtilsAppCode.Session.User.empManagerId)
                    ,new SqlParameter("@empEmail",UtilsAppCode.Session.User.empEmail)
                    });

                if (result > 0) insertResult = true;
            }
            catch (Exception ex)
            {
                throw new Exception("checkUserDBAuthen >> " + ex.Message);
            }

            return insertResult;
        }
    }
}