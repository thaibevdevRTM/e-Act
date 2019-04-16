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

        public static List<ProductCostOfGroupByPrice> getProductcostdetail(string brandId, string smellId, string size, string p_customerid, string p_productId, string p_theme)
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
                             select new ProductCostOfGroupByPrice()
                             {
                                 id = Guid.NewGuid().ToString(),
                                 productId = d["productId"].ToString(),
                                 productName = d["productName"].ToString(),
                                 brandId = d["brandId"].ToString(),
                                 smellId = d["smellId"].ToString(),
                                 size = int.Parse(AppCode.checkNullorEmpty(d["size"].ToString())),
                                 brandName = d["brandName"].ToString(),
                                 smellName = d["smellName"].ToString(),
                                 pack = QueryGetAllProduct.getProductById(d["productId"].ToString()).FirstOrDefault().pack.ToString(),
                                 typeTheme = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.id == p_theme).FirstOrDefault().activitySales,
                                 wholeSalesPrice = d["wholeSalesPrice"] is DBNull ? 0 : decimal.Parse(d["wholeSalesPrice"].ToString()),
                                 normalCost = d["normalCost"] is DBNull ? 0 : decimal.Parse(d["normalCost"].ToString()),
                                 disCount1 = d["discount1"] is DBNull ? 0 : decimal.Parse(d["discount1"].ToString()),
                                 disCount2 = d["discount2"] is DBNull ? 0 : decimal.Parse(d["discount2"].ToString()),
                                 disCount3 = d["discount3"] is DBNull ? 0 : decimal.Parse(d["discount3"].ToString()),
                                 saleIn = d["saleNormal"] is DBNull ? 0 : decimal.Parse(d["saleNormal"].ToString()),
                             }).ToList();
                if (p_productId != "")
                {
                    lists = lists.Where(x => x.productId == p_productId).ToList();
                }
                else if (brandId != "" && smellId != "")
                {
                    lists = lists.Where(x => x.brandId == brandId && x.smellId == smellId).ToList();
                }


                groupByPrice = lists.OrderByDescending(o => o.wholeSalesPrice)
                    .OrderByDescending(x => x.size)
                    .GroupBy(item => new { item.wholeSalesPrice, item.size })
                    
               .Select((group, index) => new ProductCostOfGroupByPrice
               {
                   productGroupId = Guid.NewGuid().ToString(),
                   brandId = group.First().brandId,
                   smellId = group.First().smellId,
                   smellName = smellId == "" ? "" : group.First().smellName,
                   brandName = group.First().brandName,
                   productId = group.First().productId,
                   productName = group.First().productName,
                   size = group.First().size,
                   pack = QueryGetAllProduct.getProductById(group.First().productId).FirstOrDefault().pack.ToString(),
                   wholeSalesPrice = group.First().wholeSalesPrice,
                   normalCost = group.First().normalCost,
                   disCount1 = group.First().disCount1,
                   disCount2 = group.First().disCount2,
                   disCount3 = group.First().disCount3,
                   saleIn = group.First().saleIn,
                   isShowGroup = p_productId != "" ? false : true,
                   detailGroup = group.ToList()
               }).ToList();

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