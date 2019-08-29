using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using eActForm.Models;
namespace eActForm.BusinessLayer
{
    public class DocumentsAppCode
    {
        public static List<DocumentsModel.actRepDetailModel> getActRepDetailLists(DateTime startDate, DateTime endDate)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_GetActivityRepDetailAll");
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new DocumentsModel.actRepDetailModel()
                             {
                                 id = dr["id"].ToString(),
                                 statusId = dr["statusId"].ToString(),
                                 activityNo = dr["activityNo"].ToString(),
                                 statusName = dr["statusName"].ToString(),
                                 customerId = dr["customerId"].ToString(),
                                 customerName = dr["customerName"].ToString(),
                                 productTypeId = dr["productTypeId"].ToString(),
                                 ProductTypeName = dr["ProductTypeName"].ToString(),
                                 startDate = dr["startDate"] is DBNull ? null : (DateTime?)dr["startDate"],
                                 endDate = dr["endDate"] is DBNull ? null : (DateTime?)dr["endDate"],
                                 delFlag = (bool)dr["delFlag"],
                                 createdDate = (DateTime?)dr["createdDate"],
                                 createdByUserId = dr["createdByUserId"].ToString(),
                                 updatedDate = (DateTime?)dr["updatedDate"],
                                 updatedByUserId = dr["updatedByUserId"].ToString(),
                             }).ToList();
                return lists;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




    }
}