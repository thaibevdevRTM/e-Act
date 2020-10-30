using eForms.Models.Reports;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace eForms.Presenter.Reports
{
    public class RepPostEvaPresenter : BasePresenter
    {
        public static List<RepPostEvaGroup> getPostEvaGroupByBrand(List<RepPostEvaModel> model)
        {
            try
            {
                var list = model.GroupBy(x => x.brandName)
                                .Select(cl => new RepPostEvaGroup
                                {
                                    name = cl.First().brandName.Trim(),
                                    value = (cl.Sum(c => c.total) / 1000000).ToString(),
                                    sumActSalesParti = (cl.Sum(c => c.actAmount)).ToString(),
                                    sumNormalCase = (cl.Sum(c => c.normalCost)),
                                    sumPromotionCase = (cl.Sum(c => c.themeCost)),
                                    sumSalesInCase = (cl.Sum(c => c.actReportQuantity)),
                                    countGroup = cl.Count().ToString()
                                }).ToList();
                return list;

            }
            catch (Exception ex)
            {
                throw new Exception("getPostEvaGroupByBrand >> " + ex.Message);
            }
        }

        public static List<RepPostEvaModel> filterConditionPostEva(List<RepPostEvaModel> repPostEvaLists, string productType, string productGroup, string productBrand, string actType)
        {
            try
            {
                if (!string.IsNullOrEmpty(productType))
                {
                    repPostEvaLists = repPostEvaLists.Where(x => x.productTypeId == productType).ToList();
                }
                if (!string.IsNullOrEmpty(productGroup))
                {
                    repPostEvaLists = repPostEvaLists.Where(x => x.productGroupId == productGroup).ToList();
                }
                if (!string.IsNullOrEmpty(productBrand))
                {
                    repPostEvaLists = repPostEvaLists.Where(x => x.productBrandId == productBrand).ToList();
                }
                if (!string.IsNullOrEmpty(actType))
                {
                    repPostEvaLists = repPostEvaLists.Where(x => x.activityTypeId == actType).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("getPostEvaGroupByBrand >> " + ex.Message);
            }
            return repPostEvaLists;
        }

        public static RepPostEvaModels getDataPostEva(string strConn, string startDate, string endDate, string customerId)
        {
            try
            {
                RepPostEvaModels model = new RepPostEvaModels();

                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_getReportPostEva"
                    , new SqlParameter[] {new SqlParameter("@startDate",startDate)
                    , new SqlParameter("@endDate",endDate)
                    , new SqlParameter("@customerId",customerId)});

                #region toLists
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new RepPostEvaModel()
                             {
                                 id = dr["id"].ToString(),
                                 customerId = dr["customerId"].ToString(),
                                 customerName = dr["customerName"].ToString(),
                                 preLoadStock = dr["preLoadStock"].ToString(),
                                 activitySales = dr["activitySales"].ToString(),
                                 activityNo = dr["activityNo"].ToString(),
                                 activityName = dr["activityName"].ToString(),
                                 productCode = dr["productCode"].ToString(),
                                 productName = dr["productName"].ToString(),
                                 brandName = dr["brandName"].ToString(),
                                 groupName = dr["groupName"].ToString(),
                                 cusNameEN = dr["cusNameEN"].ToString(),
                                 size = dr["size"].ToString(),
                                 activityPeriodSt = (DateTime?)dr["activityPeriodSt"],
                                 activityPeriodEnd = (DateTime?)dr["activityPeriodEnd"],
                                 costPeriodSt = dr["costPeriodSt"] is DBNull ? null : (DateTime?)dr["costPeriodSt"],
                                 costPeriodEnd = dr["costPeriodEnd"] is DBNull ? null : (DateTime?)dr["costPeriodEnd"],
                                 le = dr["activitySales"].ToString() == "Promotion Support" ? dr["le"] is DBNull ? 0 : Convert.ToDouble(dr["le"].ToString()) : 100,
                                 unit = dr["unit"].ToString(),
                                 compensate = dr["compensate"].ToString(),
                                 dayAddStart = dr["dayAddStart"].ToString(),
                                 dayAddEnd = dr["dayAddEnd"].ToString(),
                                 productId = dr["productId"].ToString(),
                                 normalCost = dr["normalCost"] is DBNull ? 0 : Convert.ToDouble(dr["normalCost"].ToString()),
                                 themeCost = dr["themeCost"] is DBNull ? 0 : Convert.ToDouble(dr["themeCost"].ToString()),
                                 total = dr["total"] is DBNull ? 0 : Convert.ToDouble(dr["total"].ToString()),
                                 tempAPNormalCost = dr["tempAPNormalCost"] is DBNull ? 0 : Convert.ToDouble(dr["tempAPNormalCost"].ToString()),
                                 estimateSaleBathAll = dr["estimateSaleBathAll"] is DBNull ? 0 : Convert.ToDouble(dr["estimateSaleBathAll"].ToString()),
                                 actReportQuantity = dr["actReportQuantity"] is DBNull ? 0 : Convert.ToDouble(dr["actReportQuantity"].ToString()),
                                 actVolumeQuantity = dr["actVolumeQuantity"] is DBNull ? 0 : Convert.ToDouble(dr["actVolumeQuantity"].ToString()),
                                 actAmount = dr["themeCost"] is DBNull ? 0 : Convert.ToDouble(dr["actAmount"].ToString()),
                                 saleActual = dr["saleActual"] is DBNull ? 0 : Convert.ToDouble(dr["saleActual"].ToString()),
                                 billedQuantityMT = dr["billedQuantityMT"] is DBNull ? 0 : Convert.ToDouble(dr["billedQuantityMT"].ToString()),
                                 volumeMT = dr["volumeMT"] is DBNull ? 0 : Convert.ToDouble(dr["volumeMT"].ToString()),
                                 netValueMT = dr["netValueMT"] is DBNull ? 0 : Convert.ToDouble(dr["netValueMT"].ToString()),
                                 specialDiscountMT = dr["specialDiscountMT"] is DBNull ? 0 : Convert.ToDouble(dr["specialDiscountMT"].ToString()),
                                 activityTypeId = dr["activityTypeId"].ToString(),
                                 productTypeId = dr["productTypeId"].ToString(),
                                 productGroupId = dr["productGroupId"].ToString(),
                                 productBrandId = dr["productBrandId"].ToString(),
                                 //presentToSale = dr["presentToSale"] is DBNull ? 0 : Convert.ToDouble(dr["presentToSale"].ToString()), 
                                 //bathParti = dr["bathParti"] is DBNull ? 0 : Convert.ToDouble(dr["bathParti"].ToString()),
                                 //presentToSaleParti = dr["presentToSaleParti"] is DBNull ? 0 : Convert.ToDouble(dr["presentToSaleParti"].ToString()),
                                 //presentSE = dr["presentSE"] is DBNull ? 0 : Convert.ToDouble(dr["presentSE"].ToString()),
                                 //salePartiCase = dr["salePartiCase"] is DBNull ? 0 : Convert.ToDouble(dr["salePartiCase"].ToString()),
                                 //salePartiBath = dr["salePartiBath"] is DBNull ? 0 : Convert.ToDouble(dr["salePartiBath"].ToString()),
                                 //accuracySaleCase = dr["accuracySaleCase"] is DBNull ? 0 : Convert.ToDouble(dr["accuracySaleCase"].ToString()),
                                 //accuracySaleBath = dr["accuracySaleBath"] is DBNull ? 0 : Convert.ToDouble(dr["accuracySaleBath"].ToString()),
                                 //accuracySpendingBath = dr["accuracySpendingBath"] is DBNull ? 0 : Convert.ToDouble(dr["accuracySpendingBath"].ToString()),
                                 //presentAcctual = dr["presentAcctual"] is DBNull ? 0 : Convert.ToDouble(dr["presentAcctual"].ToString()),

                             }).ToList();
                #endregion

                model.repPostEvaLists = lists.OrderBy(x => x.activityNo).OrderBy(x => x.activityPeriodSt).ToList();

                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getDataPostEva >> " + ex.Message);
            }
        }
    }
}
