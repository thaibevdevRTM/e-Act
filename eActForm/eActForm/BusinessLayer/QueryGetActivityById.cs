﻿using eActForm.Models;
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
    public class QueryGetActivityById
    {
        public static List<ActivityForm> getActivityById(string activityId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getActivityFormById"
                 , new SqlParameter("@activityId", activityId));

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new ActivityForm()
                              {
                                  id = d["Id"].ToString(),
                                  statusId = int.Parse(d["statusId"].ToString()), 
                                  activityNo = d["activityNo"].ToString(),
                                  documentDate = d["documentDate"] is DBNull ? null : (DateTime?)d["documentDate"],//!string.IsNullOrEmpty(d["documentDate"].ToString()) ? DateTime.Parse(d["documentDate"].ToString()) : (DateTime)null,
                                  reference = d["reference"].ToString(),
                                  customerName = d["customerName"].ToString(),
                                  cusShortName = d["cusShortName"].ToString(),
                                  customerId = d["customerId"].ToString(),
                                  chanel = d["channelName"].ToString(),
                                  chanelShort = d["chanelShort"].ToString(),
                                  chanel_Id = d["chanel_Id"].ToString(),
                                  productCateText = d["productCateText"].ToString(),
                                  productCateId = d["productCateId"].ToString(),
                                  productGroupText = d["productGroupText"].ToString(),
                                  productGroupId = d["productGroupId"].ToString(),
                                  productTypeId = d["productTypeId"].ToString(),
                                  groupShort = d["groupShort"].ToString(),
                                  brandName = d["brandName"].ToString(),
                                  shortBrand = d["shortBrand"].ToString(),
                                  activityPeriodSt = !string.IsNullOrEmpty(d["activityPeriodSt"].ToString()) ? DateTime.Parse(d["activityPeriodSt"].ToString()) : (DateTime?)null,
                                  activityPeriodEnd = !string.IsNullOrEmpty(d["activityPeriodEnd"].ToString()) ? DateTime.Parse(d["activityPeriodEnd"].ToString()) : (DateTime?)null,
                                  costPeriodSt = !string.IsNullOrEmpty(d["costPeriodSt"].ToString()) ? DateTime.Parse(d["costPeriodSt"].ToString()) : (DateTime?)null,
                                  costPeriodEnd = !string.IsNullOrEmpty(d["costPeriodEnd"].ToString()) ? DateTime.Parse(d["costPeriodEnd"].ToString()) : (DateTime?)null,
                                  activityName = d["activityName"].ToString(),
                                  theme =  d["theme"].ToString(),
                                  txttheme = d["activitySales"].ToString(),
                                  objective = d["objective"].ToString(),
                                  trade = d["trade"].ToString(),
                                  activityDetail = d["activityDetail"].ToString(),
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
                return new List<ActivityForm>();
            }
        }

    }
}