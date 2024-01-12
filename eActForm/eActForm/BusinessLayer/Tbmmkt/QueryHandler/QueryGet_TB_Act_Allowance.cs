using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using WebLibrary;
using eActForm.Models;

namespace eActForm.BusinessLayer.Tbmmkt.QueryHandler
{
    public class QueryGet_TB_Act_Allowance
    {
        public static List<TB_Act_Allowance_Model> getAllowanceListByActId(string actId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getAllowanceList"
                    , new SqlParameter("@actId", actId));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_Allowance_Model()
                             {
                                 id = d["id"].ToString(),
                                 date = DateTime.Parse(d["date"].ToString()),
                                 chkPersonal = bool.Parse(d["chkPersonal"].ToString()),
                                 chkBreakfast = bool.Parse(d["chkBreakfast"].ToString()),
                                 chkLunch = bool.Parse(d["chkLunch"].ToString()),
                                 chkDinner = bool.Parse(d["chkDinner"].ToString()),
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
                ExceptionManager.WriteError("getAllowanceListByActId => " + ex.Message);
                return new List<TB_Act_Allowance_Model>();
            }
        }
    }
}