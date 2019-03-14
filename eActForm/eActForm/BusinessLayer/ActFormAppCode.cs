using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
namespace eActForm.BusinessLayer
{
    public class ActFormAppCode
    {
        public static List<Activity_Model.actForm> getActFormByEmpId(string empId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getActivityFormByEmpId"
                    , new SqlParameter[] { new SqlParameter("@empId", empId) });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new Activity_Model.actForm()
                             {
                                 id = dr["id"].ToString(),
                                 statusId = dr["statusId"].ToString(),
                                 statusName = dr["statusName"].ToString(),
                                 activityNo = dr["activityNo"].ToString(),
                                 documentDate = dr["documentDate"] is DBNull ? null : (DateTime?)dr["documentDate"],
                                 reference = dr["reference"].ToString(),
                                 customerId = dr["customerId"].ToString(),
                                 channelName = dr["channelName"].ToString(),
                                 productTypeId = dr["productTypeId"].ToString(),
                                 productTypeNameEN = dr["nameEN"].ToString(),
                                 cusShortName = dr["cusShortName"].ToString(),
                                 productCategory = dr["productCateText"].ToString(),
                                 productGroup = dr["productGroupId"].ToString(),
                                 groupName = dr["groupName"].ToString(),
                                 activityPeriodSt = dr["activityPeriodSt"] is DBNull ? null : (DateTime?)dr["activityPeriodSt"],
                                 activityPeriodEnd = dr["activityPeriodEnd"] is DBNull ? null : (DateTime?)dr["activityPeriodEnd"],
                                 costPeriodSt = dr["costPeriodSt"] is DBNull ? null : (DateTime?)dr["costPeriodSt"],
                                 costPeriodEnd = dr["costPeriodEnd"] is DBNull ? null : (DateTime?)dr["costPeriodEnd"],
                                 activityName = dr["activityName"].ToString(),
                                 theme = dr["theme"].ToString(),
                                 objective = dr["objective"].ToString(),
                                 trade = dr["trade"].ToString(),
                                 activityDetail = dr["activityDetail"].ToString(),
                                 delFlag = (bool)dr["delFlag"],
                                 createdDate = (DateTime?)dr["createdDate"],
                                 createdByUserId = dr["createdByUserId"].ToString(),
                                 updatedDate = (DateTime?)dr["updatedDate"],
                                 updatedByUserId = dr["updatedByUserId"].ToString(),
                                 normalCost = dr["normalCost"] is DBNull ? 0 : (decimal?)dr["normalCost"],
                                 themeCost = dr["themeCost"] is DBNull ? 0 : (decimal?)dr["themeCost"],
                                 totalCost = dr["totalCost"] is DBNull ? 0 : (decimal?)dr["totalCost"]
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getActFormByEmpId >> " + ex.Message);
            }
        }
    }
}