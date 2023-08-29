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
    public class QueryGetAllBrand
    {
        public static List<TB_Act_ProductBrand_Model> GetAllBrand()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getAllProductBrand");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_ProductBrand_Model()
                             {
                                 id = d["id"].ToString(),
                                 brandName = d["brandName"].ToString(),
                                 productGroupId = d["productGroupId"].ToString(),
                                 digit_EO = d["digit_EO"].ToString(),
                                 digit_IO = d["digit_IO"].ToString(),
                                 no_tbmmkt = d["no_tbmmkt"].ToString(),
                                 companyId = d["companyId"].ToString(),
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
                ExceptionManager.WriteError("GetAllBrand => " + ex.Message);
                return new List<TB_Act_ProductBrand_Model>();
            }
        }

        public static List<TB_Act_ProductBrand_Model> GetBrandBySubject(string subjectId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBrandBySubjectId"
                    , new SqlParameter("@subjectId", subjectId));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_ProductBrand_Model()
                             {
                                 id = d["id"].ToString(),
                                 brandName = d["brandName"].ToString(),

                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("GetBrandBySubject => " + ex.Message);
                return new List<TB_Act_ProductBrand_Model>();
            }
        }

        public static List<TB_Act_ProductBrand_Model> GetBrandBudgetControl()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getProductBrandBGControl");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_ProductBrand_Model()
                             {
                                 id = d["id"].ToString(),
                                 brandName = d["brandName"].ToString(),

                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("GetBrandBudgetControl => " + ex.Message);
                return new List<TB_Act_ProductBrand_Model>();
            }
        }
    }
}