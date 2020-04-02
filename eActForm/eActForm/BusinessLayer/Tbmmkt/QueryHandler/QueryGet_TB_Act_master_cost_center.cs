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
    public class QueryGet_TB_Act_master_cost_center
    {
        public static List<TB_Act_master_cost_centerModel> get_TB_Act_master_cost_center(string productBrandId, string companyId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_get_TB_Act_master_cost_center"
                         , new SqlParameter("@productBrandId", productBrandId)
                           , new SqlParameter("@companyId", companyId));

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new TB_Act_master_cost_centerModel()
                              {
                                  id = d["id"].ToString(),
                                  costCenter = d["costCenter"].ToString(),
                                  productBrandId = d["productBrandId"].ToString(),
                                  companyId = d["companyId"].ToString(),
                                  delFlag = bool.Parse(d["delFlag"].ToString()),
                                  createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                  createdByUserId = d["createdByUserId"].ToString(),
                                  updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                  updatedByUserId = d["updatedByUserId"].ToString(),
                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("get_TB_Act_master_cost_center => " + ex.Message);
                return new List<TB_Act_master_cost_centerModel>();
            }
        }
    }
}