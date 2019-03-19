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
    public class QueryGetCostDetailById
    {
        public static List<ProductCostOfGroupByPrice> getcostDetailById(string activityId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getcostdetailById"
                 , new SqlParameter("@activityId", activityId));

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new ProductCostOfGroupByPrice()
                              {
                                  id = d["Id"].ToString(),
                                  activityId = d["activityId"].ToString(),
                                  productId = d["productId"].ToString(),
                                  productName = d["productName"].ToString(),
                                  wholeSalesPrice = d["wholeSalesPrice"].ToString() == "" ? 0 : decimal.Parse(d["wholeSalesPrice"].ToString()),
                                  disCount1 = d["discount1"].ToString() == "" ? 0 : decimal.Parse(d["discount1"].ToString()),
                                  disCount2 = d["discount2"].ToString() == "" ? 0 : decimal.Parse(d["discount2"].ToString()),
                                  disCount3 = d["discount3"].ToString() == "" ? 0 : decimal.Parse(d["discount3"].ToString()),
                                  normalCost = d["wholeSalesPrice"].ToString() == "" ? 0 : decimal.Parse(d["wholeSalesPrice"].ToString()),
                                  normalGp = d["normalGp"].ToString() == "" ? 0 : decimal.Parse(d["normalGp"].ToString()),
                                  promotionGp = d["promotionGp"].ToString() == "" ? 0 : decimal.Parse(d["promotionGp"].ToString()),
                                  specialDisc = d["specialDisc"].ToString() == "" ? 0 : decimal.Parse(d["specialDisc"].ToString()),
                                  promotionCost = d["promotionCost"].ToString() == "" ? 0 : decimal.Parse(d["promotionCost"].ToString()),
                                  delFlag = bool.Parse(d["delFlag"].ToString()),
                                  createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                  createdByUserId = d["createdByUserId"].ToString(),
                                  updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                  updatedByUserId = d["updatedByUserId"].ToString(),
                              }).OrderBy(x => x.createdDate);

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getcostDetailById => " + ex.Message);
                return new List<ProductCostOfGroupByPrice>();
            }

        }
    }
}