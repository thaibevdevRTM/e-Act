﻿using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebLibrary;
using System.Data.SqlClient;
namespace eActForm.BusinessLayer
{
    public class QueryGetAllProduct
    {
        public static List<TB_Act_Product_Model.ProductSmellModel> getProductSmellByGroupId(string productGroupId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getProductSmellByGroupId"
                    , new SqlParameter("@productGroupId", productGroupId));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_Product_Model.ProductSmellModel()
                             {
                                 id = d["id"].ToString(),
                                 productGroupId = d["productGroupId"].ToString(),
                                 nameTH = d["nameTH"].ToString(),
                                 nameEN = d["nameEN"].ToString(),
                                 delFlag = (bool)d["delFlag"],
                                 createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                 createdByUserId = d["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                 updatedByUserId = d["updatedByUserId"].ToString(),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("getProductSmellByGroupId >>" + ex.Message);
            }
        }
        public static List<TB_Act_Product_Model.Product_Model> getProductBySmellIdAndBrandId(string smellId, string brandId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getProductBySmellIdAndBrandId"
                    , new SqlParameter[] {
                        new SqlParameter("@smellId", smellId)
                        ,new SqlParameter("@brandId",brandId)
                    });
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_Product_Model.Product_Model()
                             {
                                 id = d["id"].ToString(),
                                 productCode = d["productCode"].ToString(),
                                 productName = d["productName"].ToString(),
                                 brandId = d["brandId"].ToString(),
                                 size = d["size"].ToString() == "" ? "0" : d["size"].ToString(),
                                 unit = d["unit"].ToString() == "" ? 0 : Convert.ToInt32(d["unit"].ToString()),
                                 litre = d["litre"].ToString() == "" ? 0 : Convert.ToInt32(d["litre"].ToString()),
                                 delFlag = (bool)d["delFlag"],
                                 createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                 createdByUserId = d["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                 updatedByUserId = d["updatedByUserId"].ToString(),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getProductBySmellId => " + ex.Message);
                return new List<TB_Act_Product_Model.Product_Model>();
            }
        }
        public static List<TB_Act_Product_Model.Product_Model> getProductBySmellId(string smellId,string productGroupId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getProductBySmellId"
                    ,new SqlParameter[] {
                        new SqlParameter("@smellId", smellId)
                        ,new SqlParameter("@productGroupId",productGroupId)
                    });
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_Product_Model.Product_Model()
                             {
                                 id = d["id"].ToString(),
                                 productCode = d["productCode"].ToString(),
                                 productName = d["productName"].ToString(),
                                 brandId = d["brandId"].ToString(),
                                 size = d["size"].ToString() == "" ? "0" : d["size"].ToString(),
                                 unit = d["unit"].ToString() == "" ? 0 : Convert.ToInt32(d["unit"].ToString()),
                                 litre = d["litre"].ToString() == "" ? 0 : Convert.ToInt32(d["litre"].ToString()),
                                 delFlag = (bool)d["delFlag"],
                                 createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                 createdByUserId = d["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                 updatedByUserId = d["updatedByUserId"].ToString(),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getProductBySmellId => " + ex.Message);
                return new List<TB_Act_Product_Model.Product_Model>();
            }
        }
        public static List<TB_Act_Product_Model.Product_Model> getAllProduct(string productGroupId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getAllProduct"
                    ,new SqlParameter[] { new SqlParameter("@productGroupId", productGroupId) });
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_Product_Model.Product_Model()
                             {
                                 id = d["id"].ToString(),
                                 productCode = d["productCode"].ToString(),
                                 productName = d["productName"].ToString(),
                                 smellId = d["smellId"].ToString(),
                                 brandId = d["brandId"].ToString(),
                                 size = d["size"].ToString() == "" ? "0" : d["size"].ToString(),
                                 unit = d["unit"].ToString() == "" ? 0 : Convert.ToInt32(d["unit"].ToString()),
                                 litre = d["litre"].ToString() == "" ? 0 : Convert.ToInt32(d["litre"].ToString()),
                                 delFlag = (bool)d["delFlag"],
                                 createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                 createdByUserId = d["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                 updatedByUserId = d["updatedByUserId"].ToString(),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getAllProduct => " + ex.Message);
                return new List<TB_Act_Product_Model.Product_Model>();
            }
        }

        public static List<TB_Act_Product_Model.Product_Model> getProductByBrandId(string brandId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getProductByBrandId"
                    , new SqlParameter("@brandId", brandId));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_Product_Model.Product_Model()
                             {
                                 id = d["id"].ToString(),
                                 productCode = d["productCode"].ToString(),
                                 productName = d["productName"].ToString(),
                                 brandId = d["brandId"].ToString(),
                                 size = d["size"].ToString() == "" ? "0" : d["size"].ToString(),
                                 unit = d["unit"].ToString() == "" ? 0 : Convert.ToInt32(d["unit"].ToString()),
                                 litre = d["litre"].ToString() == "" ? 0 : Convert.ToInt32(d["litre"].ToString()),
                                 delFlag = (bool)d["delFlag"],
                                 createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                 createdByUserId = d["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                 updatedByUserId = d["updatedByUserId"].ToString(),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getProductByBrandId => " + ex.Message);
                return new List<TB_Act_Product_Model.Product_Model>();
            }
        }

        public static List<TB_Act_Product_Model.Product_Model> getProductById(string productId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getProductByProductId"
                    , new SqlParameter("@productId", productId));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_Product_Model.Product_Model()
                             {
                                 id = d["id"].ToString(),
                                 productCode = d["productCode"].ToString(),
                                 productName = d["productName"].ToString(),
                                 brandId = d["brandId"].ToString(),
                                 size = d["size"].ToString() == "" ? "0" : d["size"].ToString(),
                                 unit = d["unit"].ToString() == "" ? 0 : Convert.ToInt32(d["unit"].ToString()),
                                 litre = d["litre"].ToString() == "" ? 0 : Convert.ToInt32(d["litre"].ToString()),
                                 pack = d["pack"].ToString() == "" ? 1 : Convert.ToInt32(d["pack"].ToString()),
                                 delFlag = (bool)d["delFlag"],
                                 createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                 createdByUserId = d["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                 updatedByUserId = d["updatedByUserId"].ToString(),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getProductByBrandId => " + ex.Message);
                return new List<TB_Act_Product_Model.Product_Model>();
            }
        }
    }
}
    