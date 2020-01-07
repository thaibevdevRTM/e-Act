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
    public class QueryGetAllApproveGroup
    {
        public static List<TB_Reg_ApproveGroup_Model> getAllApproveGroup()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getAllApproveGroup");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Reg_ApproveGroup_Model()
                             {
                                 id = d["id"].ToString(),
                                 nameTH = d["nameTH"].ToString(),
                                 nameEN = d["nameEN"].ToString(),
                                 description = d["description"].ToString(),
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
                ExceptionManager.WriteError("getAllApproveGroup => " + ex.Message);
                return new List<TB_Reg_ApproveGroup_Model>();
            }
        }
    }
}