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
    public class exPerryCashAppCode
    {
        public static Activity_TBMMKT_Model getMaster(Activity_TBMMKT_Model model)
        {
            try
            {
                model.requestEmpModel = QueryGet_empDetailById.getEmpDetailById(UtilsAppCode.Session.User.empId);
                model.exPerryCashList = getCashPosition(UtilsAppCode.Session.User.empId).Where(x => x.cashLimitId.Equals("87757B5B-C946-4001-A74B-AB6C9003AD25")).ToList();
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("getMaster" + ex.Message);
            }

            return model;
        }

        public static List<exPerryCashModel> getCashPosition(string empId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getLimitPerryCash"
                    , new SqlParameter[] { new SqlParameter("@empId", empId) });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new exPerryCashModel()
                             {
                                 cashLimitId = dr["cashLimitId"].ToString(),
                                 cashName = dr["cashName"].ToString(),
                                 positionId = dr["positionId"].ToString(),
                                 positionName = dr["positionName"].ToString(),
                                 cash = decimal.Parse(dr["cash"].ToString()),
                                 empId = dr["empId"].ToString(),
                             }).ToList();

                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getCashPosition >> " + ex.Message);
            }
        }
    }
}