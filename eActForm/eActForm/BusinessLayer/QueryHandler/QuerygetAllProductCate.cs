using eActForm.Models;
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
    public class QuerygetAllProductCate
    {

        public static List<TB_Act_ProductType_Model> getProductTypeByEmpId()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getProductTypeByEmpId"
                    , new SqlParameter[] { new SqlParameter("@empid", UtilsAppCode.Session.User.empId) });
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_ProductType_Model()
                             {
                                 id = d["id"].ToString(),
                                 nameTH = d["nameTH"].ToString(),
                                 nameEN = d["nameEN"].ToString(),
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
                ExceptionManager.WriteError("getProductTypeByEmpId => " + ex.Message);
                return new List<TB_Act_ProductType_Model>();
            }
        }
        public static List<TB_Act_ProductType_Model> getAllProductType()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getAllProductType");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_ProductType_Model()
                             {
                                 id = d["id"].ToString(),
                                 nameTH = d["nameTH"].ToString(),
                                 nameEN = d["nameEN"].ToString(),
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
                ExceptionManager.WriteError("getAllProductCate => " + ex.Message);
                return new List<TB_Act_ProductType_Model>();
            }
        }

        public static List<TB_Act_ProductCate_Model> getAllProductCate()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getAllProductCate");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_ProductCate_Model()
                             {
                                 id = d["id"].ToString(),
                                 cateName = d["cateName"].ToString(),
                                 productTypeId = d["productTypeId"].ToString(),
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
                ExceptionManager.WriteError("getAllProductCate => " + ex.Message);
                return new List<TB_Act_ProductCate_Model>();
            }
        }

    }
}