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
    public class QueryGetActivityDetailById
    {
        public static List<CostThemeDetailOfGroupByPrice> getActivityDetailById(string activityId)
        {
            try
            {
                List<CostThemeDetailOfGroupByPrice> groupByPrice = new List<CostThemeDetailOfGroupByPrice>();
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getActivitydetailById"
                 , new SqlParameter("@activityId", activityId));

                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new CostThemeDetailOfGroupByPrice()
                             {
                                 id = d["Id"].ToString(),
                                 productGroupId = d["productGroupId"].ToString(),
                                 activityId = d["activityId"].ToString(),
                                 activityTypeId = d["activityTypeId"].ToString(),
                                 productId = d["productId"].ToString(),
                                 productName = d["productDetail"].ToString(),//d["productName"].ToString() == "" ? d["productDetail"].ToString() : d["productName"].ToString(),
                                 pack = d["productId"].ToString() != "" ? QueryGetAllProduct.getProductById(d["productId"].ToString()).FirstOrDefault()?.pack.ToString() : "",
                                 smellName = d["smellName"].ToString(),
                                 brandName = d["brandName"].ToString(),
                                 size = int.Parse(AppCode.checkNullorEmpty(d["size"].ToString())),
                                 wholeSalesPrice = decimal.Parse(AppCode.checkNullorEmpty(d["wholeSalesPrice"].ToString())),
                                 typeTheme = !string.IsNullOrEmpty(d["activityTypeId"].ToString()) ? QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.id == d["activityTypeId"].ToString()).FirstOrDefault()?.activitySales : "",
                                 normalCost = d["normalCost"].ToString() == "" ? 0 : decimal.Parse(d["normalCost"].ToString()),
                                 themeCost = d["themeCost"].ToString() == "" ? 0 : decimal.Parse(d["themeCost"].ToString()),
                                 isShowGroup = true,
                                 growth = d["growth"].ToString() == "" ? 0 : decimal.Parse(d["growth"].ToString()),
                                 total = d["total"].ToString() == "" ? 0 : decimal.Parse(d["total"].ToString()),
                                 totalCase = d["totalCase"].ToString() == "" ? 0 : decimal.Parse(d["totalCase"].ToString()),
                                 perTotal = d["perTotal"].ToString() == "" ? 0 : decimal.Parse(d["perTotal"].ToString()),
                                 unit = d["unit"].ToString() == "" ? 0 : int.Parse(d["unit"].ToString()),
                                 compensate = d["compensate"].ToString() == "" ? 0 : decimal.Parse(d["compensate"].ToString()),
                                 LE = d["Le"].ToString() == "" ? 0 : decimal.Parse(d["Le"].ToString()),
                                 IO = d["IO"].ToString(),
                                 rowNo = int.Parse(d["rowNo"].ToString()),
                                 mechanics = d["mechanics"].ToString(),
                                 delFlag = bool.Parse(d["delFlag"].ToString()),
                                 createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                 createdByUserId = d["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                 updatedByUserId = d["updatedByUserId"].ToString(),
                             }).ToList();

                var tt = lists.ToList();

                groupByPrice = lists.OrderBy(x => x.rowNo).ToList();

                return groupByPrice;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getActivityDetailById => " + ex.Message);
                return new List<CostThemeDetailOfGroupByPrice>();
            }
        }


        public static List<CostThemeDetailOfGroupByPrice> getSubActivityDetailById(string activityId)
        {
            try
            {
                List<CostThemeDetailOfGroupByPrice> groupByPrice = new List<CostThemeDetailOfGroupByPrice>();
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getSubActivitydetailById"
                 , new SqlParameter("@subActivityId", activityId));

                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new CostThemeDetailOfGroupByPrice()
                             {
                                 id = d["Id"].ToString(),
                                 productId = d["productId"].ToString(),
                                 productName = d["productDetail"].ToString(),
                                 subActivityId = d["subActivityId"].ToString(),
                                 ref_Estimate = d["ref_Estimate"].ToString(),
                                 total = d["total"].ToString() == "" ? 0 : decimal.Parse(d["total"].ToString()),
                                 delFlag = bool.Parse(d["delFlag"].ToString()),
                                 createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                 createdByUserId = d["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                 updatedByUserId = d["updatedByUserId"].ToString(),
                             }).ToList();

                return lists;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getActivityDetailById => " + ex.Message);
                return new List<CostThemeDetailOfGroupByPrice>();
            }
        }

    }
}