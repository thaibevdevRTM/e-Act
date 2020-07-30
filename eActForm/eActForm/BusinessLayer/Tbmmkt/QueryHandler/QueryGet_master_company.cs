using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    public class QueryGet_master_company
    {
        public static List<Master_Company_Model> get_master_company(string companyId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_get_companyMaster"
                  , new SqlParameter("@companyId", companyId));

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new Master_Company_Model()
                              {
                                  id = d["id"].ToString(),
                                  companyId = d["companyId"].ToString(),
                                  companyNameEN = d["companyNameEN"].ToString(),
                                  companyNameTH = ("บริษัท " + d["companyNameTH"].ToString()),
                                  delFlag = bool.Parse(d["delFlag"].ToString()),
                                  createDate = DateTime.Parse(d["createDate"].ToString()),
                                  createBy = d["createBy"].ToString(),
                                  updateDate = DateTime.Parse(d["updateDate"].ToString()),
                                  updateBy = d["updateBy"].ToString(),
                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("get_master_company => " + ex.Message);
                return new List<Master_Company_Model>();
            }
        }

    }
}