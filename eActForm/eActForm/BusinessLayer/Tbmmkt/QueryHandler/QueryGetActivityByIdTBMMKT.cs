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
    public class QueryGetActivityByIdTBMMKT
    {
        public static List<ActivityFormTBMMKT> getActivityById(string activityId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getActivityFormByIdTBMMKT"
                 , new SqlParameter("@activityId", activityId));

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new ActivityFormTBMMKT()
                              {
                                  id = d["Id"].ToString(),
                                  statusId = int.Parse(d["statusId"].ToString()),
                                  activityNo = d["activityNo"].ToString(),
                                  documentDate = !string.IsNullOrEmpty(d["documentDate"].ToString()) ? DateTime.Parse(d["documentDate"].ToString()) : (DateTime?)null,
                                  activityPeriodSt = !string.IsNullOrEmpty(d["activityPeriodSt"].ToString()) ? DateTime.Parse(d["activityPeriodSt"].ToString()) : (DateTime?)null,
                                  activityPeriodEnd = !string.IsNullOrEmpty(d["activityPeriodEnd"].ToString()) ? DateTime.Parse(d["activityPeriodEnd"].ToString()) : (DateTime?)null,
                                  activityName = d["activityName"].ToString(),
                                  objective = d["objective"].ToString(),
                                  remark = d["remark"].ToString(),
                                  companyId = d["companyId"].ToString(),
                                  master_type_form_id = d["master_type_form_id"].ToString(),
                                  benefit = d["benefit"].ToString(),
                                  companyNameEN = d["companyNameEN"].ToString(),
                                  companyNameTH = d["companyNameTH"].ToString(),
                                  delFlag = bool.Parse(d["delFlag"].ToString()),
                                  createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                  createdByUserId = d["createdByUserId"].ToString(),
                                  updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                  updatedByUserId = d["updatedByUserId"].ToString(),
                                  createdByName = "คุณ" + d["createdByName"].ToString(),
                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getActivityById => " + ex.Message);
                return new List<ActivityFormTBMMKT>();
            }
        }

    }
}