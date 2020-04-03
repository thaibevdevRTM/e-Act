﻿using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebLibrary;

namespace eActForm.BusinessLayer.QueryHandler
{
    public class QueryGetActivityByActNo
    {
        public static List<ActivityFormTBMMKT> getCheckRefActivityByActNo(string actNo)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getRefActivityByIdActNo"
                 , new SqlParameter("@actNo", actNo));

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
                                  languageDoc = d["languageDoc"].ToString(),
                                  companyNameEN = d["companyNameEN"].ToString(),
                                  companyNameTH = d["companyNameTH"].ToString(),
                                  reference = d["reference"].ToString(),
                                  customerId = d["customerId"].ToString(),
                                  costPeriodSt = !string.IsNullOrEmpty(d["costPeriodSt"].ToString()) ? DateTime.Parse(d["costPeriodSt"].ToString()) : (DateTime?)null,
                                  costPeriodEnd = !string.IsNullOrEmpty(d["costPeriodEnd"].ToString()) ? DateTime.Parse(d["costPeriodEnd"].ToString()) : (DateTime?)null,
                                  activityDetail = d["activityDetail"].ToString(),
                                  empId = d["empId"].ToString(),
                                  delFlag = bool.Parse(d["delFlag"].ToString()),
                                  createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                  createdByUserId = d["createdByUserId"].ToString(),
                                  updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                  updatedByUserId = d["updatedByUserId"].ToString(),
                                  createdByName = "คุณ" + d["createdByName"].ToString(),
                                  createdByNameEN = d["createdByNameEN"].ToString(),
                                  piorityDoc = d["piorityDoc"].ToString(),

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