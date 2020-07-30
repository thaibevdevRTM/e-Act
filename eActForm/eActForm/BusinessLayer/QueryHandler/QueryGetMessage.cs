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
    public class QueryGetMessage
    {
        public static List<TB_Act_Other_Model> getOtherByType(string p_type)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getDataOtherbyType"
                     , new SqlParameter("@type", p_type));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_Other_Model()
                             {
                                 id = d["id"].ToString(),
                                 type = d["type"].ToString(),
                                 name = d["name"].ToString(),
                                 displayVal = d["displayVal"].ToString(),
                                 subType = d["subType"].ToString(),
                                 val1 = d["val1"].ToString(),
                                 val2 = d["val2"].ToString(),
                                 sort = d["sort"].ToString(),
                                 delFlag = bool.Parse(d["delFlag"].ToString()),
                                 createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                 createdByUserId = d["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                 updatedByUserId = d["updatedByUserId"].ToString(),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getOtherByType => " + ex.Message);
                return new List<TB_Act_Other_Model>();
            }

        }
    }
}