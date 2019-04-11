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
        public static int setRoleUser()
        {
            try
            {
                int rtn = 0;
                if (UtilsAppCode.Session.User != null)
                {
                    DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getUserByEmpId"
                        , new SqlParameter[] { new SqlParameter("@empId", UtilsAppCode.Session.User.empId) });
                    if (ds.Tables.Count > 0)
                    {
                        rtn = ds.Tables[0].Rows.Count;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            switch (dr["roleId"])
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