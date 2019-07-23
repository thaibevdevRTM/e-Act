using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
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
                                 descTh = d["descTh"].ToString() + "(" + d["name"].ToString() + ")",
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
                ExceptionManager.WriteError("getAllPrice => " + ex.Message);
                return new List<TB_Act_Region_Model>();
            }
        }
    }
}