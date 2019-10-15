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


        public static string getAllUserRole(string actId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getAllUserRole"
                     , new SqlParameter[] { new SqlParameter("@actId", actId) });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new AdminUserModel.UserList()
                             {
                                 empId = dr["empid"].ToString(),
                                 userName = dr["empid"].ToString(),
                             }).ToList();

              

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("getEmailCCByActId >> " + ex.Message);
            }
        }

    }
}