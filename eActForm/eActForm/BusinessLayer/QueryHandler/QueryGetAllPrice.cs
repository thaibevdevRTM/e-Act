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
    public class QueryGetAllPrice
    {

        public static List<TB_Act_ProductPrice_Model.ProductPrice> getAllPrice()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getAllProductPrice");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_ProductPrice_Model.ProductPrice()
                             {
                                 id = d["id"].ToString(),
                                 productCode = d["productCode"].ToString(),
                                 customerId = d["customerId"].ToString(),
                                 price = decimal.Parse(d["price"].ToString()),
                                 startDate = DateTime.Parse(d["startDate"].ToString()),
                                 endDate = DateTime.Parse(d["endDate"].ToString()),
                                 delFlag = bool.Parse(d["delFlag"].ToString()),
                                 createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                 createdByUserId = d["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                 updatedByUserId = d["updatedByUserId"].ToString(),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getActivityById => " + ex.Message);
                return new List<TB_Act_ProductPrice_Model.ProductPrice>();
            }
        }


        public static List<TB_Act_ProductPrice_Model.ProductPrice> getPriceByProductCode(string p_productCode)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "ups_getProductPriceByProductCode"
                      , new SqlParameter("@productcode", p_productCode));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_ProductPrice_Model.ProductPrice()
                             {
                                 id = d["id"].ToString(),
                                 productCode = d["productId"].ToString(),
                                 productName = d["productName"].ToString(),
                                 customerName = d["cusNameEN"].ToString(),
                                 customerId = d["customerId"].ToString(),
                                 normalCost = d["normalCost"] == null ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["normalCost"].ToString())),
                                 wholeSalesPrice = d["wholeSalesPrice"] == null ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["wholeSalesPrice"].ToString())),
                                 discount1 = d["discount1"] == null ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["discount1"].ToString())),
                                 discount2 = d["discount2"] == null ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["discount2"].ToString())),
                                 discount3 = d["discount3"] == null ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["discount3"].ToString())),
                                 saleNormal = d["saleNormal"] == null ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["saleNormal"].ToString())),
                                 updatedDate = string.IsNullOrEmpty(d["updatedDate"].ToString()) ? DateTime.Now : DateTime.Parse(d["updatedDate"].ToString()),
                             });
                return lists.OrderBy(x => x.customerName).ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getPriceByProductCode => " + ex.Message);
                return new List<TB_Act_ProductPrice_Model.ProductPrice>();
            }
        }
    }
}