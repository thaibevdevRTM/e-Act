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
    public class QueryGetAllRegion
    {
        public static List<TB_Act_Region_Model> getAllRegion()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getAllRegion");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_Region_Model()
                             {
                                 id = d["id"].ToString(),
                                 name = d["name"].ToString(),
                                 nameShot = d["nameShot"].ToString(),
                                 descEn = d["descEn"].ToString(),
                                 descTh = "(" + d["name"].ToString() + ")" + d["descTh"].ToString(),
                                 condition = d["condition"].ToString(),
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
                ExceptionManager.WriteError("getAllRegion => " + ex.Message);
                return new List<TB_Act_Region_Model>();
            }
        }

        public static List<TB_Act_Region_Model> getRegoinByEmpId(string empId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getRegoinByEmpId"
                    , new SqlParameter[] { new SqlParameter("@empId", empId) });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new TB_Act_Region_Model()
                             {
                                 id = dr["id"].ToString(),
                                 name = dr["name"].ToString(),
                                 nameShot = dr["nameShot"].ToString(),
                                 descEn = dr["descEn"].ToString(),
                                 descTh = dr["descTh"].ToString(),
                             }).ToList();

                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getRegoinByEmpId >> " + ex.Message);
            }
        }
    }
}