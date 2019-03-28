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
    public class QueryGetActivityDetailById
    {
      
        public static List<CostThemeDetailOfGroupByPrice> getActivityDetailById(string activityId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getActivitydetailById"
                 , new SqlParameter("@activityId", activityId));

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new CostThemeDetailOfGroupByPrice()
                              {
                                  id = d["Id"].ToString(),
                                  activityId = d["activityId"].ToString(),
                                  activityTypeId = d["activityTypeId"].ToString(),
                                  productId = d["productId"].ToString(),
                                  productName = d["productName"].ToString(),
                                  typeTheme = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.id == d["activityTypeId"].ToString()).FirstOrDefault().activitySales,
                                  normalCost = d["normalCost"].ToString() == "" ? 0 : decimal.Parse(d["normalCost"].ToString()),
                                  themeCost = d["themeCost"].ToString() == "" ? 0 : decimal.Parse(d["themeCost"].ToString()),
                                  growth = d["growth"].ToString() == "" ? 0 : decimal.Parse(d["growth"].ToString()),
                                  total = d["total"].ToString() == "" ? 0 : decimal.Parse(d["total"].ToString()),
                                  delFlag = bool.Parse(d["delFlag"].ToString()),
                                  createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                  createdByUserId = d["createdByUserId"].ToString(),
                                  updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                  updatedByUserId = d["updatedByUserId"].ToString(),
                              }).OrderBy(x => x.productName).ThenBy(x => x.typeTheme);

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getActivityDetailById => " + ex.Message);
                return new List<CostThemeDetailOfGroupByPrice>();
            }
        }

    }
}