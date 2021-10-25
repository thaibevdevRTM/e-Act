using eActForm.BusinessLayer.Appcodes;
using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    public class QueryGetBenefit
    {
        public static bool getAllowAutoApproveForFormHC(string actId)
        {
            try
            {
                object obj = SqlHelper.ExecuteScalar(AppCode.StrCon, CommandType.StoredProcedure, "usp_checkApproveDetailForFormHC"
                    , new SqlParameter[] { new SqlParameter("@actId", actId )});
                return obj != null ? (bool)obj : false;
            }
            catch(Exception ex)
            {
                throw new Exception("getAllowAutoApproveForFormHC >> " + ex.Message);
            }
        }
        public static List<CashEmpModel> getCashLimitByEmpId(string empId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getCashLimitByEmpId"
                     , new SqlParameter("@empId", empId));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new CashEmpModel()
                             {
                                 empId = d["empId"].ToString(),
                                 choiceID = d["id"].ToString(),
                                 choiceName = d["name"].ToString(),
                                 cashPerDay = d["cashPerDay"] is DBNull ? 0 : decimal.Parse(d["cashPerDay"].ToString()),
                                 empLevel = d["empLevel"].ToString(),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getCashLimitByEmpId => " + ex.Message);
                return new List<CashEmpModel>();
            }
        }

        public static List<ProductCostOfGroupByPrice> getProductcostdetail(string brandId, string smellId, string size, string p_customerid, string p_productId, string p_theme, string typeForm)
        {
            try
            {
                List<ProductCostOfGroupByPrice> groupByPrice = new List<ProductCostOfGroupByPrice>();
                DataSet ds;
                if (typeForm == Activity_Model.activityType.MT.ToString())
                {
                    ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getProductcost"
                        , new SqlParameter("@p_brand", brandId)
                        , new SqlParameter("@smellId", smellId)
                        , new SqlParameter("@p_size", size)
                        , new SqlParameter("@p_customerid", p_customerid));
                }
                else
                {
                    ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getProductcostOMT"
                        , new SqlParameter("@p_brand", brandId)
                        , new SqlParameter("@smellId", smellId)
                        , new SqlParameter("@p_size", size)
                        , new SqlParameter("@p_customerid", p_customerid));
                }

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
                                 unit = d["unit"] is DBNull ? 0 : int.Parse(d["unit"].ToString()),
                                 pack = d["pack"].ToString(),
                                 typeTheme = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.id == p_theme).FirstOrDefault().activitySales,
                                 wholeSalesPrice = d["wholeSalesPrice"] is DBNull ? 0 : decimal.Parse(d["wholeSalesPrice"].ToString()),
                                 normalCost = d["normalCost"] is DBNull ? 0 : decimal.Parse(d["normalCost"].ToString()),
                                 disCount1 = d["discount1"] is DBNull ? 0 : decimal.Parse(d["discount1"].ToString()),
                                 disCount2 = d["discount2"] is DBNull ? 0 : decimal.Parse(d["discount2"].ToString()),
                                 disCount3 = d["discount3"] is DBNull ? 0 : decimal.Parse(d["discount3"].ToString()),
                                 saleNormal = d["saleNormal"] is DBNull ? 0 : decimal.Parse(d["saleNormal"].ToString()),
                             }).ToList();
                if (p_productId != "")
                {
                    lists = lists.Where(x => x.productId == p_productId).ToList();
                }
                else if (brandId != "" && smellId != "")
                {
                    lists = lists.Where(x => x.brandId == brandId && x.smellId == smellId).ToList();
                }

                if (typeForm != Activity_Model.activityType.OMT.ToString())
                {
                    lists = lists.Where(x => x.wholeSalesPrice > 0).ToList();
                }

                groupByPrice = lists.OrderByDescending(o => o.normalCost)
                    .OrderByDescending(x => x.size)
                    .GroupBy(item => new { item.normalCost, item.size, item.pack })

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
                   unit = group.First().unit,
                   pack = "Pack(" + group.First().pack + ")",
                   wholeSalesPrice = group.First().wholeSalesPrice,
                   normalCost = group.First().normalCost,
                   disCount1 = group.First().disCount1,
                   disCount2 = group.First().disCount2,
                   disCount3 = group.First().disCount3,
                   saleNormal = group.First().saleNormal,
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

        public static List<CashEmpModel> getCashLimitByTypeId(string typeId, string hireDate, string jobLevel)
        {
            try
            {
                DateTime? hireDatee = DateTime.ParseExact(hireDate, "dd/MM/yyyy", null);
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getCashLimitByTypeId"
                     , new SqlParameter("@typeId", typeId)
                      , new SqlParameter("@hireDate", hireDatee)
                       , new SqlParameter("@jobLevel", jobLevel));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new CashEmpModel()
                             {
                                 cashPerDay = d["cash"] is DBNull ? 0 : decimal.Parse(d["cash"].ToString()),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getCashLimitByTypeId => " + ex.Message);
                return new List<CashEmpModel>();
            }
        }
        public static List<CashEmpModel> getCumulativeByEmpId(string empId,DateTime? docDate)
        {
            try
            {
                
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getCumulativeByEmpId"
                     , new SqlParameter("@empId", empId)
                     , new SqlParameter("@docDate", docDate));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new CashEmpModel()
                             {
                                 cashPerDay = d["amountReceived"] is DBNull ? 0 : decimal.Parse(d["amountReceived"].ToString()),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getCumulativeByEmpId => " + ex.Message);
                return new List<CashEmpModel>();
            }
        }

    }
}