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
    public class QueryGetAllCustomers
    {
        
        public static List<TB_Act_Customers_Model.Customers_Model> getCustomersByEmpId()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getCustomersByEmpId"
                    ,new SqlParameter[] {new SqlParameter("@empId",UtilsAppCode.Session.User.empId) });
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_Customers_Model.Customers_Model()
                             {
                                 id = d["id"].ToString(),
                                 cusTrading = d["cusTrading"].ToString(),
                                 cusNameTH = d["cusNameTH"].ToString(),
                                 cusNameEN = d["cusNameEN"].ToString(),
                                 cusShortName = d["cusShortName"].ToString(),
                                 cust = d["cust"].ToString(),
                                 chanel_Id = d["chanel_Id"].ToString(),
                                 delFlag = d["delFlag"].ToString(),
                                 createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                 createdByUserId = d["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                 updatedByUserId = d["updatedByUserId"].ToString(),
                             });
                return lists.OrderBy(x => x.cusNameTH).ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getCustomersByEmpId => " + ex.Message);
                throw new Exception("getCustomersByEmpId >> " + ex.Message);
            }
        }
        public static List<TB_Act_Customers_Model.Customers_Model> getAllCustomers()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getAllCustomers");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_Customers_Model.Customers_Model()
                             {
                                 id = d["id"].ToString(),
                                 cusTrading = d["cusTrading"].ToString(),
                                 cusNameTH = d["cusNameTH"].ToString(),
                                 cusNameEN = d["cusNameEN"].ToString(),
                                 cusShortName = d["cusShortName"].ToString(),
                                 cust = d["cust"].ToString(),
                                 chanel_Id = d["chanel_Id"].ToString(),
                                 delFlag = d["delFlag"].ToString(),
                                 createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                 createdByUserId = d["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                 updatedByUserId = d["updatedByUserId"].ToString(),
                             });
                return lists.OrderBy(x => x.cusNameTH).ToList();
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("getAllCustomers => " + ex.Message);
                return new List<TB_Act_Customers_Model.Customers_Model>();
            }
        }
    }
}