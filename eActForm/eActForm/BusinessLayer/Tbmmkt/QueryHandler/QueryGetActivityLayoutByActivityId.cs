using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    public class QueryGetActivityLayoutByActivityId
    {
        public static List<TB_Act_ActivityLayout> getByActivityId(string activityId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getTB_Act_ActivityLayoutByActivityId"
                 , new SqlParameter("@activityId", activityId));

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new TB_Act_ActivityLayout()
                              {
                                  id = d["id"].ToString(),
                                  activityId = d["activityId"].ToString(),
                                  no = d["no"].ToString(),
                                  io = d["io"].ToString(),
                                  activity = d["activity"].ToString(),
                                  amount =  Decimal.Parse(d["amount"].ToString()),
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
                ExceptionManager.WriteError("getActivityById => " + ex.Message);
                return new List<TB_Act_ActivityLayout>();
            }
        }

    }
}