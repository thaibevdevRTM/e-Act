using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    public class QueryGetAllPrice
    {

        public static List<TB_Act_ProductPrice_Model.ProductPrice_Model> getAllPrice()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getAllProductPrice");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_ProductPrice_Model.ProductPrice_Model()
                             {
                                 id = d["id"].ToString(),
                                 productCode = d["productCode"].ToString(),
                                 customerId = d["customerId"].ToString(),
                                 price = decimal.Parse(d["price"].ToString()),
                                 startDate = DateTime.Parse(d["startDate"].ToString()),
                                 endDate = DateTime.Parse(d["endDate"].ToString()),
                                 delFlag = d["delFlag"].ToString(),
                                 createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                 createdBy = d["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                 updatedBy = d["updatedByUserId"].ToString(),
                             });
                return lists.ToList();
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("getActivityById => " + ex.Message);
                return new List<TB_Act_ProductPrice_Model.ProductPrice_Model>();
            }
        }
    }
}