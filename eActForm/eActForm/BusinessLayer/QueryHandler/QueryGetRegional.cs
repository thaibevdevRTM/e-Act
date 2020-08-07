using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebLibrary;

namespace eActForm.BusinessLayer.QueryHandler
{
    public class QueryGetRegional
    {
        public static List<RegionalModel> getRegionalByCompanyId(string companyId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrConAuthen, CommandType.StoredProcedure, "usp_getRegionalByCompanyId"
                    , new SqlParameter("@companyId", companyId));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new RegionalModel()
                             {
                                 id = d["id"].ToString(),
                                 companyId = d["companyId"].ToString(),
                                 nameTH = d["regionalNameTH"].ToString(),
                                 nameEN = d["regionalNameEN"].ToString(),

                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getRegionalByCompanyId => " + ex.Message);
                return new List<RegionalModel>();
            }
        }
    }
}