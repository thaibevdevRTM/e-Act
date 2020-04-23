﻿using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    public class QueryGetActivityEstimateByActivityId
    {
        public static List<CostThemeDetailOfGroupByPriceTBMMKT> getByActivityId(string activityId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getTB_Act_ActivityOfEstimateByActivityId"
                 , new SqlParameter("@activityId", activityId));

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new CostThemeDetailOfGroupByPriceTBMMKT()
                              {
                                  id = d["id"].ToString(),
                                  activityId = d["activityId"].ToString(),
                                  activityTypeId = d["activityTypeId"].ToString(),
                                  productId = d["productId"].ToString(),
                                  productDetail = d["productDetail"].ToString(),
                                  unit = int.Parse(d["unit"].ToString()),
                                  unitPrice = d["unitPrice"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["unitPrice"].ToString())),
                                  unitPriceDisplay = d["unitPrice"].ToString() == "" ? "0.00" : string.Format("{0:n2}", decimal.Parse(AppCode.checkNullorEmpty(d["unitPrice"].ToString()))),
                                  unitPriceDisplayReport = "",//ปัจจุบันไม่ได้ใช้งานฟิลด์นี้ เก็บไว้เพื่ออยากเอาไปใช้อนาคต
                                  total = d["total"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["total"].ToString())),
                                  normalCost = d["normalCost"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["normalCost"].ToString())),
                                  IO = d["IO"].ToString(),
                                  QtyName = d["QtyName"].ToString(),
                                  remark = d["remark"].ToString(),
                                  typeTheme = d["typeTheme"].ToString(),
                                  detail = d["detail"].ToString(),
                                  date = !string.IsNullOrEmpty(d["date"].ToString()) ? DateTime.Parse(d["date"].ToString()) : (DateTime?)null,
                                  compensate = string.IsNullOrEmpty(d["compensate"].ToString()) ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["compensate"].ToString())),
                                  listChoiceId = d["listChoiceId"].ToString(),
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
                return new List<CostThemeDetailOfGroupByPriceTBMMKT>();
            }
        }
        public static List<CostThemeDetailOfGroupByPriceTBMMKT> getWithListChoice(string activityId, string formId, string type)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getEstimateWithListchoiceByActivityId"
                 , new SqlParameter("@activityId", activityId)
                  , new SqlParameter("@formId", formId)
                   , new SqlParameter("@type", type));

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new CostThemeDetailOfGroupByPriceTBMMKT()
                              {

                                  listChoiceId = d["listChoiceId"].ToString(),
                                  listChoiceName = d["listChoiceName"].ToString(),
                                  productDetail = d["productDetail"].ToString(),
                                  unit = d["unit"].ToString() == "" ? 0 : int.Parse(d["unit"].ToString()),
                                  unitPrice = d["unitPrice"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["unitPrice"].ToString())),
                                  unitPriceDisplay = d["unitPrice"].ToString() == "" ? "0.00" : string.Format("{0:n2}", decimal.Parse(AppCode.checkNullorEmpty(d["unitPrice"].ToString()))),
                                  total = d["total"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["total"].ToString())),
                                  displayType = d["displayType"].ToString(),
                                  subDisplayType = d["subDisplayType"].ToString(),
                                   updatedByUserId = d["updatedByUserId"].ToString(),
                                  createdByUserId = d["createdByUserId"].ToString(),


                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getWithListChoice => " + ex.Message);
                return new List<CostThemeDetailOfGroupByPriceTBMMKT>();
            }
        }
        public static List<CostThemeDetailOfGroupByPriceTBMMKT> getHistoryByActivityId(string activityId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getActivityOfEstimateHistoryByActivityId"
                 , new SqlParameter("@activityId", activityId));

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new CostThemeDetailOfGroupByPriceTBMMKT()
                              {
                                  id = d["id"].ToString(),
                                  activityId = d["activityId"].ToString(),
                                  activityTypeId = d["activityTypeId"].ToString(),                              
                                  productDetail = d["productDetail"].ToString(),
                                  unit = int.Parse(d["unit"].ToString()),
                                  unitPrice = d["unitPrice"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["unitPrice"].ToString())),
                                  unitPriceDisplay = d["unitPrice"].ToString() == "" ? "0.00" : string.Format("{0:n2}", decimal.Parse(AppCode.checkNullorEmpty(d["unitPrice"].ToString()))),
                                  total = d["total"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["total"].ToString())),
                                listChoiceId = d["listChoiceId"].ToString(),
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
                return new List<CostThemeDetailOfGroupByPriceTBMMKT>();
            }
        }

    }
}