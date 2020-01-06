using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebLibrary;

namespace eActForm.BusinessLayer.Appcodes
{
    public class AdminUserAppCode
    {
        public static int insertRole(string empId, string roleId)
        {
            int result = 0;
            try
            {

                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertRole"
                    , new SqlParameter[] {new SqlParameter("@empId",empId)
                    ,new SqlParameter("@roleId",roleId)
                    ,new SqlParameter("@createdBy",UtilsAppCode.Session.User.empId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> insertRole");
            }

            return result;
        }

        public static int delUserandAuthorByEmpId(string empId)
        {
            int result = 0;
            try
            {

                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_delUserandAuthorByEmpId"
                    , new SqlParameter[] {new SqlParameter("@empId",empId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> delUserandAuthorByEmpId");
            }

            return result;
        }


        public static int insertAuthorized(string empId, string companyId ,string customerId, string productTypeId)
        {
            int result = 0;
            try
            {

                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertAuthorized"
                    , new SqlParameter[] {new SqlParameter("@empId",empId)
                    ,new SqlParameter("@companyId",companyId)
                    ,new SqlParameter("@customerId",customerId)
                    ,new SqlParameter("@productTypeId",productTypeId)
                    ,new SqlParameter("@createdBy",UtilsAppCode.Session.User.empId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> insertAuthorized");
            }

            return result;
        }


        public static List<AdminUserModel.User> getAllUserRole()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getAllUserRole");
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new AdminUserModel.User()
                             {
                                 empId = dr["empid"].ToString(),
                                 userName = dr["empFNameTH"].ToString(),
                                 userLName = dr["empLNameTH"].ToString(),
                             }).ToList();

                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getAllUserRole >> " + ex.Message);
            }
        }

        public static List<AdminUserModel.User> getUserRoleByEmpId(string empId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getUserRoleByEmpId"
                    , new SqlParameter[] { new SqlParameter("@empId", empId) });
            var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new AdminUserModel.User()
                             {
                                 empId = dr["empId"].ToString(),
                                 roleId = dr["roleId"].ToString(),
                                 companyId = dr["empCompanyId"].ToString(),
                             }).ToList();

                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getUserRoleByEmpId >> " + ex.Message);
            }
        }

        public static List<AdminUserModel.Customer> getcustomerRoleByEmpId(string empId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getcustomerRoleByEmpId"
                    , new SqlParameter[] { new SqlParameter("@empId", empId) });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new AdminUserModel.Customer()
                             {
                                 cusId = dr["customerId"].ToString(),
                                 customerName = dr["cusName"].ToString(),
                                 productTypeId = dr["productTypeId"].ToString(),
                                 companyId = dr["companyId"].ToString(),

                             }).ToList();

                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getcustomerRoleByEmpId >> " + ex.Message);
            }
        }

        public static List<TB_Act_Other_Model> getCompany()
        {
            return QueryOtherMaster.getOhterMaster("company", "");
        }

    }
}