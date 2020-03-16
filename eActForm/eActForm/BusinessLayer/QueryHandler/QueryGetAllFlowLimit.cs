using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebLibrary;

namespace eActForm.BusinessLayer.QueryHandler
{
    public class QueryGetAllFlowLimit
    {
        public static List<TB_Reg_FlowLimit_Model> getAllFlowLimit()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getAllFlowLimit");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Reg_FlowLimit_Model()
                             {
                                 id = d["id"].ToString(),
                                 subjectId = d["subjectId"].ToString(),
                                 limitBegin = d["limitBegin"].ToString(),
                                 limitTo = d["limitTo"].ToString(),
                                 displayVal = d["displayVal"].ToString(),
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
                ExceptionManager.WriteError("getAllFlowLimit => " + ex.Message);
                return new List<TB_Reg_FlowLimit_Model>();
            }
        }


    }
}