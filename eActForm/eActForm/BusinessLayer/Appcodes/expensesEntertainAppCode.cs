using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace eActForm.BusinessLayer.Appcodes
{
    public class expensesEntertainAppCode
    {
        public static List<exPerryCashModel> getLimitByEmpId(string empId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getLimitByEmpId"
                    , new SqlParameter[] { new SqlParameter("@empId", empId) });
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var lists = (from DataRow dr in ds.Tables[0].Rows
                                 select new exPerryCashModel()
                                 {
                                     id = dr["idCashType"].ToString(),
                                     cashName = dr["displayVal"].ToString(),
                                     cash = dr["cash"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(dr["cash"].ToString())),
                                 }).ToList();
                    return lists;
                }
                else
                {
                    return new List<exPerryCashModel>();
                }


            }
            catch (Exception ex)
            {
                throw new Exception("getLimitByEmpId >>" + ex.Message);
            }
        }

        public static List<exPerryCashModel> getAmountLimitByEmpId(string empId, string docDate ,string productId)
        {
            try
            {
                DateTime date = BaseAppCodes.converStrToDatetimeWithFormat(docDate, ConfigurationManager.AppSettings["formatDateUse"]);
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_exGetAmountLimitByEmpId"
                    , new SqlParameter[] { new SqlParameter("@empId", empId),
                      new SqlParameter("@docDate", date),
                      new SqlParameter("@productId", productId)});
                if (ds.Tables.Count > 0)
                {
                    var lists = (from DataRow dr in ds.Tables[0].Rows
                                 select new exPerryCashModel()
                                 {
                                     cash = dr["total"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(dr["total"].ToString())),
                                 }).ToList();
                    return lists;
                }
                else
                {
                    return new List<exPerryCashModel>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("getAmountLimitByEmpId >>" + ex.Message);
            }
        }
    }
}