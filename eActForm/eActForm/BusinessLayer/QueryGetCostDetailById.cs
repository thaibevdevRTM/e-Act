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
                List<ProductCostOfGroupByPrice> groupByPrice = new List<ProductCostOfGroupByPrice>();
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getcostdetailById"
                 , new SqlParameter("@activityId", activityId));

                var lists = (from DataRow d in ds.Tables[0].Rows
                              select new ProductCostOfGroupByPrice()
                              {
                                  id = d["Id"].ToString(),
                                  productGroupId = d["productGroupId"].ToString(),
                                  activityId = d["activityId"].ToString(),
                                  productId = d["productId"].ToString(),
                                  brandId = d["brandId"].ToString(),
                                  brandName = d["brandName"].ToString(),
                                  smellName = d["smellName"].ToString(),
                                  size = int.Parse(AppCode.checkNullorEmpty(d["size"].ToString())),
                                  productName = d["productName"].ToString(),
                                  pack = d["productId"].ToString() != "" ? QueryGetAllProduct.getProductById(d["productId"].ToString()).FirstOrDefault().pack.ToString() : "",
                                  wholeSalesPrice = d["wholeSalesPrice"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["wholeSalesPrice"].ToString())),
                                  saleIn = d["saleIn"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["saleIn"].ToString())),
                                  saleOut = d["saleOut"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["saleOut"].ToString())),
                                  disCount1 = d["discount1"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["discount1"].ToString())),
                                  disCount2 = d["discount2"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["discount2"].ToString())),
                                  disCount3 = d["discount3"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["discount3"].ToString())),
                                  normalCost = d["normalCost"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["normalCost"].ToString())),
                                  normalGp = d["normalGp"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["normalGp"].ToString())),
                                  promotionGp = d["promotionGp"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["promotionGp"].ToString())),
                                  specialDisc = d["specialDisc"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["specialDisc"].ToString())),
                                  specialDiscBaht = d["specialDiscBaht"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["specialDiscBaht"].ToString())),
                                  promotionCost = d["promotionCost"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["promotionCost"].ToString())),
                                  isShowGroup = bool.Parse(d["isShowGroup"].ToString()),
                                  rowNo = int.Parse(d["rowNo"].ToString()),
                                  delFlag = bool.Parse(d["delFlag"].ToString()),
                                  createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                  createdByUserId = d["createdByUserId"].ToString(),
                                  updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                  updatedByUserId = d["updatedByUserId"].ToString(),
                              });

                groupByPrice = lists
                    .OrderBy(x => x.rowNo)
                    .GroupBy(item => new { item.wholeSalesPrice, item.size , item.rowNo })
               .Select((group, index) => new ProductCostOfGroupByPrice
               {
                   productGroupId = group.First().productGroupId,
                   brandId = group.First().brandId,
                   smellId = group.First().smellId,
                   smellName = group.First().smellName,
                   brandName = group.First().brandName,
                   productId = group.First().productId,
                   productName = group.First().productName,
                   size = group.First().size,
                   wholeSalesPrice = group.First().wholeSalesPrice,
                   saleIn = group.First().saleIn,
                   saleOut = group.First().saleOut,
                   disCount1 = group.First().disCount1,
                   disCount2 = group.First().disCount2,
                   disCount3 = group.First().disCount3,
                   normalCost = group.First().normalCost,
                   normalGp = group.First().normalGp,
                   promotionGp = group.First().promotionGp,
                   specialDisc = group.First().specialDisc,
                   specialDiscBaht = group.First().specialDiscBaht,
                   promotionCost = group.First().promotionCost,
                   pack = QueryGetAllProduct.getProductById(group.First().productId).Any() ? "Pack" + QueryGetAllProduct.getProductById(group.First().productId).FirstOrDefault().pack.ToString() : "",
                   rowNo = group.First().rowNo,
                   isShowGroup = group.First().isShowGroup,
                   detailGroup = group.ToList()

               }).ToList();


                return groupByPrice;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getcostDetailById => " + ex.Message);
                return new List<ProductCostOfGroupByPrice>();
            }

        }
    }
}