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
                                  productDetail = d["productDetail"].ToString(),                                
                                  unit = int.Parse(d["unit"].ToString()),
                                  unitPrice = d["unitPrice"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["unitPrice"].ToString())),
                                  unitPriceDisplay = d["unitPrice"].ToString() == "" ? "0.00" : string.Format("{0:n2}", decimal.Parse(AppCode.checkNullorEmpty(d["unitPrice"].ToString()))),
                                  total = d["total"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["total"].ToString())),
                                  IO = d["IO"].ToString(),
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