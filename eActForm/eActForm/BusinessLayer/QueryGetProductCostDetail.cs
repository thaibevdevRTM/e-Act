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
    public class QueryGetProductCostDetail
    {
        public static List<Productcostdetail> getProductCostByProductId(string productId, string customerId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getProductCostById"
                     , new SqlParameter("@productId", productId)
                     , new SqlParameter("@customerId", customerId));

                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new Productcostdetail()
                             {
                                 id = Guid.NewGuid().ToString(),
                                 productId = d["productId"].ToString(),
                                 brandId = d["brandId"].ToString(),
                                 smellId = d["smellId"].ToString(),
                                 brandName = d["brandName"].ToString(),
                                 smellName = d["smellName"].ToString(),
                                 productName = d["productName"].ToString(),
                                 wholeSalesPrice = d["price"] is DBNull ? 0 : decimal.Parse(d["price"].ToString()),
                                 normalCost = d["price"] is DBNull ? 0 : decimal.Parse(d["price"].ToString()),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getProductCostByProductId => " + ex.Message);
                return new List<Productcostdetail>();
            }
        }

        public static List<ProductCostOfGroupByPrice> getProductcostdetail(string brandId,string smellId, string size, string p_customerid)
        {
            try
            {
                List<ProductCostOfGroupByPrice> groupByPrice = new List<ProductCostOfGroupByPrice>();
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getProductcost"
                     , new SqlParameter("@p_brand", brandId)
                     , new SqlParameter("@smellId", smellId)
                     , new SqlParameter("@p_size", size)
                     , new SqlParameter("@p_customerid", p_customerid));

                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new Productcostdetail()
                             {
                                 id = Guid.NewGuid().ToString(),
                                 productId = d["productId"].ToString(),
                                 productName = d["productName"].ToString(),
                                 brandId = d["brandId"].ToString(),
                                 smellId = d["smellId"].ToString(),
                                 brandName = d["brandName"].ToString(),
                                 smellName = d["smellName"].ToString(),
                                 wholeSalesPrice = decimal.Parse(d["price"].ToString()),
                                 normalCost = decimal.Parse(d["price"].ToString()),
                             }).ToList();

                if (size == "")
                {
                    // group same price
                    groupByPrice = (from p in lists
                                  group p by p.wholeSalesPrice into g
                                  select new ProductCostOfGroupByPrice {
                                   id = Guid.NewGuid().ToString(),
                                   brandId = lists.FirstOrDefault().brandId,
                                   smellId = lists.FirstOrDefault().smellId,
                                   smellName = lists.FirstOrDefault().smellName,
                                   brandName = lists.FirstOrDefault().brandName,
                                   wholeSalesPrice = g.Key,
                                   isShowGroup = true,
                                  detailGroup = g.ToList() }).ToList();
                    
                }
                else
                {
                    groupByPrice[0].isShowGroup = false;
                    groupByPrice[0].detailGroup = lists;
                }
                
                return groupByPrice;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getProductcostdetail => " + ex.Message);
                throw new Exception("getProductcostdetail >>" + ex.Message);
            }
        }
    }
}