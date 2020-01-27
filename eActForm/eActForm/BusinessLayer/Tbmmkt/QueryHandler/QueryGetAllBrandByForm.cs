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
    public class QueryGetAllBrandByForm
    {
        public static List<TB_Act_ProductBrand_Model> GetAllBrand(string master_type_form_id, string companyId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getAllProductBrandByForm", new SqlParameter("@master_type_form_id", master_type_form_id), new SqlParameter("@companyId", companyId));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_ProductBrand_Model()
                             {
                                 id = d["id"].ToString(),
                                 brandName = d["brandName"].ToString(),
                                 productGroupId = d["productGroupId"].ToString(),
                                 digit_EO = d["digit_EO"].ToString(),
                                 digit_IO = d["digit_IO"].ToString(),
                                 no_tbmmkt = d["no_tbmmkt"].ToString(),
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
                ExceptionManager.WriteError("getAllChanel => " + ex.Message);
                return new List<TB_Act_ProductBrand_Model>();
            }
        }
    }
}