using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebLibrary;


namespace eActForm.BusinessLayer
{
    public class QueryGetAllProductGroup
    {
        public static List<TB_Act_ProductGroup_Model> getAllProductGroup()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getAllProductGroup");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_ProductGroup_Model()
                             {
                                 id = d["id"].ToString(),
                                 groupName = d["groupName"].ToString(),
                                 cateId = d["cateId"].ToString(),
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
                ExceptionManager.WriteError("getAllProductCate => " + ex.Message);
                return new List<TB_Act_ProductGroup_Model>();
            }
        }

    }
}