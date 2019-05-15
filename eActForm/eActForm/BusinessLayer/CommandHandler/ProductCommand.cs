using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using WebLibrary;

namespace eActForm.BusinessLayer.CommandHandler
{
    public class ProductCommand
    {
        public static List<TB_Act_Product_Model.Product_Model> InsertProduct(string smellId, string brandId)
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
                                 size = d["size"].ToString() == "" ? 0 : Convert.ToInt32(d["size"].ToString()),
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
    }
}