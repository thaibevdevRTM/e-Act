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
                                 //productDetail = d["productDetail"].ToString(),
                                 pack = d["productId"].ToString() != "" ? QueryGetAllProduct.getProductById(d["productId"].ToString()).FirstOrDefault().pack.ToString() : "",
                                 smellName = d["smellName"].ToString(),
                                 brandName = d["brandName"].ToString(),
                                 size = int.Parse(AppCode.checkNullorEmpty(d["size"].ToString())),
                                 wholeSalesPrice = decimal.Parse(AppCode.checkNullorEmpty(d["wholeSalesPrice"].ToString())),
                                 typeTheme = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.id == d["activityTypeId"].ToString()).FirstOrDefault().activitySales,
                                 normalCost = d["normalCost"].ToString() == "" ? 0 : decimal.Parse(d["normalCost"].ToString()),
                                 themeCost = d["themeCost"].ToString() == "" ? 0 : decimal.Parse(d["themeCost"].ToString()),
                                 isShowGroup = true,
                                 growth = d["growth"].ToString() == "" ? 0 : decimal.Parse(d["growth"].ToString()),
                                 total = d["total"].ToString() == "" ? 0 : decimal.Parse(d["total"].ToString()),
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
                             });

                groupByPrice = lists.OrderBy(x => x.rowNo).ToList();
               //     .GroupBy(item => new { item.normalCost, item.size, item.rowNo, item.pack })
               //.Select((group, index) => new CostThemeDetailOfGroupByPrice
               //{
               //    productGroupId = group.First().productGroupId,
               //    brandId = group.First().brandId,
               //    smellId = group.First().smellId,
               //    smellName = group.First().smellName,
               //    activityTypeId = group.First().activityTypeId,
               //    brandName = group.First().brandName/*.Trim() + " " + group.First().size + "ALL(" + group.ToList().Count + ")"*/,
               //    productId = group.First().productId,
               //    wholeSalesPrice = group.First().wholeSalesPrice,
               //    typeTheme = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.id == group.First().activityTypeId).FirstOrDefault().activitySales,
               //    productName = group.First().isShowGroup ? group.First().brandName.Trim() + " " + group.First().size + "ALL(" + group.ToList().Count + ")" : group.First().productName,
               //    size = group.First().size,
               //    normalCost = group.First().normalCost,
               //    themeCost = group.First().themeCost,
               //    growth = group.First().growth,
               //    total = group.First().total,
               //    perTotal = group.First().perTotal,
               //    unit = group.First().unit,
               //    compensate = group.First().compensate,
               //    LE = group.First().LE,
               //    pack = QueryGetAllProduct.getProductById(group.First().productId).Any() ? "Pack" + QueryGetAllProduct.getProductById(group.First().productId).FirstOrDefault().pack.ToString() : "",
               //    isShowGroup = group.First().isShowGroup,
               //    rowNo = group.First().rowNo,
               //    detailGroup = group.ToList()

               //}).ToList();

                return groupByPrice;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getActivityDetailById => " + ex.Message);
                return new List<CostThemeDetailOfGroupByPrice>();
            }
        }
        ////public static List<CostThemeDetailOfGroupByPrice> getActivityDetailById(string activityId)
        //{
        //    try
        //    {
        //        List<CostThemeDetailOfGroupByPrice> groupByPrice = new List<CostThemeDetailOfGroupByPrice>();
        //        DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getActivitydetailById"
        //         , new SqlParameter("@activityId", activityId));

        //        var lists = (from DataRow d in ds.Tables[0].Rows
        //                     select new ProductCostOfGroupByPrice()
        //                     {
        //                         id = d["Id"].ToString(),
        //                         productGroupId = d["productGroupId"].ToString(),
        //                         activityId = d["activityId"].ToString(),
        //                         activityTypeId = d["activityTypeId"].ToString(),
        //                         productId = d["productId"].ToString(),
        //                         productName = d["productDetail"].ToString(),//d["productName"].ToString() == "" ? d["productDetail"].ToString() : d["productName"].ToString(),
        //                         //productDetail = d["productDetail"].ToString(),
        //                         pack = d["productId"].ToString() != "" ? QueryGetAllProduct.getProductById(d["productId"].ToString()).FirstOrDefault().pack.ToString() : "",
        //                         smellName = d["smellName"].ToString(),
        //                         brandName = d["brandName"].ToString(),
        //                         size = int.Parse(AppCode.checkNullorEmpty(d["size"].ToString())),
        //                         wholeSalesPrice = decimal.Parse(AppCode.checkNullorEmpty(d["wholeSalesPrice"].ToString())),
        //                         typeTheme = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.id == d["activityTypeId"].ToString()).FirstOrDefault().activitySales,
        //                         normalCost = d["normalCost"].ToString() == "" ? 0 : decimal.Parse(d["normalCost"].ToString()),
        //                         themeCost = d["themeCost"].ToString() == "" ? 0 : decimal.Parse(d["themeCost"].ToString()),
        //                         isShowGroup = bool.Parse(d["isShowGroup"].ToString()),
        //                         growth = d["growth"].ToString() == "" ? 0 : decimal.Parse(d["growth"].ToString()),
        //                         total = d["total"].ToString() == "" ? 0 : decimal.Parse(d["total"].ToString()),
        //                         perTotal = d["perTotal"].ToString() == "" ? 0 : decimal.Parse(d["perTotal"].ToString()),
        //                         unit = d["unit"].ToString() == "" ? 0 : int.Parse(d["unit"].ToString()),
        //                         compensate = d["compensate"].ToString() == "" ? 0 : decimal.Parse(d["compensate"].ToString()),
        //                         LE = d["Le"].ToString() == "" ? 0 : decimal.Parse(d["Le"].ToString()),
        //                         rowNo = int.Parse(d["rowNo"].ToString()),
        //                         delFlag = bool.Parse(d["delFlag"].ToString()),
        //                         createdDate = DateTime.Parse(d["createdDate"].ToString()),
        //                         createdByUserId = d["createdByUserId"].ToString(),
        //                         updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
        //                         updatedByUserId = d["updatedByUserId"].ToString(),
        //                     });

        //        groupByPrice = lists
        //            .OrderBy(x => x.rowNo)
        //            .GroupBy(item => new { item.normalCost, item.size  , item.rowNo , item.pack})
        //       .Select((group, index) => new CostThemeDetailOfGroupByPrice
        //       {
        //           productGroupId = group.First().productGroupId,
        //           brandId = group.First().brandId,
        //           smellId = group.First().smellId,
        //           smellName = group.First().smellName,
        //           activityTypeId = group.First().activityTypeId,
        //           brandName = group.First().brandName/*.Trim() + " " + group.First().size + "ALL(" + group.ToList().Count + ")"*/,
        //           productId = group.First().productId,
        //           wholeSalesPrice = group.First().wholeSalesPrice,
        //           typeTheme = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.id == group.First().activityTypeId).FirstOrDefault().activitySales,
        //           productName = group.First().isShowGroup ? group.First().brandName.Trim() + " " + group.First().size + "ALL(" + group.ToList().Count + ")" : group.First().productName,
        //           size = group.First().size,
        //           normalCost = group.First().normalCost,
        //           themeCost = group.First().themeCost,
        //           growth = group.First().growth,
        //           total = group.First().total,
        //           perTotal = group.First().perTotal,
        //           unit = group.First().unit,
        //           compensate = group.First().compensate,
        //           LE = group.First().LE,
        //           pack = QueryGetAllProduct.getProductById(group.First().productId).Any() ? "Pack" + QueryGetAllProduct.getProductById(group.First().productId).FirstOrDefault().pack.ToString() : "" ,
        //           isShowGroup = group.First().isShowGroup,
        //           rowNo = group.First().rowNo,
        //           detailGroup = group.ToList()

        //       }).ToList();

        //        return groupByPrice;
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionManager.WriteError("getActivityDetailById => " + ex.Message);
        //        return new List<CostThemeDetailOfGroupByPrice>();
        //    }
        //}

    }
}