using eForms.Models.MasterData;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace eForms.Presenter.MasterData
{
    public class FiscalYearPresenter
    {
        public static List<FiscalYearModel> getFiscalNow(string strConn, string typePeriod)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_getFiscalYearNow", new SqlParameter("@typePeriod", typePeriod));
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new FiscalYearModel()
                             {
                                 id = dr["id"].ToString(),
                                 FromMonthYear = dr["FromMonthYear"].ToString(),
                                 ToMonthYear = dr["ToMonthYear"].ToString(),
                                 UseYear = dr["UseYear"].ToString(),
                                 typePeriod = dr["typePeriod"].ToString(),
                                 delFlag = bool.Parse(dr["delFlag"].ToString()),
                                 createdDate = DateTime.Parse(dr["createdDate"].ToString()),
                                 createdByUserId = dr["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(dr["updatedDate"].ToString()),
                                 updatedByUserId = dr["updatedByUserId"].ToString()
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getPhysicalYearByYear >>" + ex.Message);
            }
        }
        public static List<FiscalYearModel> getFiscalYearByYear(string strConn, string yearFrom, string yearTo)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_getFiscalYearByYear", new SqlParameter("@yearFrom", yearFrom), new SqlParameter("@yearTo", yearTo));
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new FiscalYearModel()
                             {
                                 id = dr["id"].ToString(),
                                 FromMonthYear = dr["FromMonthYear"].ToString(),
                                 ToMonthYear = dr["ToMonthYear"].ToString(),
                                 UseYear = dr["UseYear"].ToString(),
                                 typePeriod = dr["typePeriod"].ToString(),
                                 delFlag = bool.Parse(dr["delFlag"].ToString()),
                                 createdDate = DateTime.Parse(dr["createdDate"].ToString()),
                                 createdByUserId = dr["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(dr["updatedDate"].ToString()),
                                 updatedByUserId = dr["updatedByUserId"].ToString()
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getPhysicalYearByYear >>" + ex.Message);
            }
        }
    }
}
