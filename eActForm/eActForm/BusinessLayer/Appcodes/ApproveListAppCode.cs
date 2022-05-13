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
    public class ApproveListAppCode
    {
        public static List<Activity_Model.actForm> getFilterFormByStatusId(List<Activity_Model.actForm> lists, int statusId)
        {
            try
            {
                return lists.Where(r => r.statusId == statusId.ToString()).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterFormByStatusId >> " + ex.Message);
            }
        }
        public static List<Activity_Model.actForm> getApproveListsByEmpId(string empId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getApproveFormByEmpId"
                    , new SqlParameter[] { new SqlParameter("@empId", empId) });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new Activity_Model.actForm(dr["createdByUserId"].ToString())
                             {
                                 id = dr["id"].ToString(),
                                 statusId = dr["statusId"].ToString(),
                                 statusName = dr["statusName"].ToString(),
                                 statusNameEN = dr["statusNameEN"].ToString(),
                                 languageDoc = dr["languageDoc"].ToString(),
                                 activityNo = dr["activityNo"].ToString(),
                                 documentDate = dr["documentDate"] is DBNull ? null : (DateTime?)dr["documentDate"],
                                 reference = dr["reference"].ToString(),
                                 customerId = dr["customerId"].ToString(),
                                 channelName = dr["channelName"].ToString(),
                                 productTypeId = dr["productTypeId"].ToString(),
                                 productTypeNameEN = dr["nameEN"].ToString(),
                                 cusShortName = dr["cusShortName"].ToString(),
                                 productCategory = dr["productCateText"].ToString(),
                                 productGroupid = dr["productGroupId"].ToString(),
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
                                 perTotal = dr["perTotal"] is DBNull ? 0 : (decimal?)dr["perTotal"],
                                 delFlag = (bool)dr["delFlag"],
                                 createdDate = (DateTime?)dr["createdDate"],
                                 createdByUserId = dr["createdByUserId"].ToString(),
                                 updatedDate = (DateTime?)dr["updatedDate"],
                                 updatedByUserId = dr["updatedByUserId"].ToString(),
                                 normalCost = dr["normalCost"] is DBNull ? 0 : (decimal?)dr["normalCost"],
                                 themeCost = dr["themeCost"] is DBNull ? 0 : (decimal?)dr["themeCost"],
                                 totalCost = dr["totalCost"] is DBNull ? 0 : (decimal?)dr["totalCost"],
                                 dateSentApprove = dr["dateSentApprove"] is DBNull ? null : (DateTime?)dr["dateSentApprove"],
                                 brandName = dr["brandName"].ToString(),
                                 master_type_form_id = dr["master_type_form_id"].ToString(),
                                 mainAgency = dr["mainAgency"].ToString(),
                                 regionId = dr["region"].ToString(),
                             }).ToList();

                return lists;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getApproveListsByEmpId >> " + ex.Message);
                return new List<Activity_Model.actForm>();
            }
        }

        public static List<Activity_Model.actForm> getRejectApproveListsByEmpId(string empId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getRejectApproveFormByEmpId"
                    , new SqlParameter[] { new SqlParameter("@empId", empId) });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new Activity_Model.actForm(dr["createdByUserId"].ToString())
                             {
                                 id = dr["id"].ToString(),
                                 statusId = dr["statusId"].ToString(),
                                 statusName = dr["statusName"].ToString(),
                                 statusNameEN = dr["statusNameEN"].ToString(),
                                 languageDoc = dr["languageDoc"].ToString(),
                                 activityNo = dr["activityNo"].ToString(),
                                 documentDate = dr["documentDate"] is DBNull ? null : (DateTime?)dr["documentDate"],
                                 reference = dr["reference"].ToString(),
                                 customerId = dr["customerId"].ToString(),
                                 channelName = dr["channelName"].ToString(),
                                 productTypeId = dr["productTypeId"].ToString(),
                                 productTypeNameEN = dr["nameEN"].ToString(),
                                 cusShortName = dr["cusShortName"].ToString(),
                                 productCategory = dr["productCateText"].ToString(),
                                 productGroupid = dr["productGroupId"].ToString(),
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
                                 perTotal = dr["perTotal"] is DBNull ? 0 : (decimal?)dr["perTotal"],
                                 delFlag = (bool)dr["delFlag"],
                                 createdDate = (DateTime?)dr["createdDate"],
                                 createdByUserId = dr["createdByUserId"].ToString(),
                                 updatedDate = (DateTime?)dr["updatedDate"],
                                 updatedByUserId = dr["updatedByUserId"].ToString(),
                                 normalCost = dr["normalCost"] is DBNull ? 0 : (decimal?)dr["normalCost"],
                                 themeCost = dr["themeCost"] is DBNull ? 0 : (decimal?)dr["themeCost"],
                                 totalCost = dr["totalCost"] is DBNull ? 0 : (decimal?)dr["totalCost"],
                                 dateSentApprove = dr["dateSentApprove"] is DBNull ? null : (DateTime?)dr["dateSentApprove"],
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getRejectApproveListsByEmpId >> " + ex.Message);
                return new List<Activity_Model.actForm>();
            }
        }


        public static List<ApproveModel.approveListSummaryModels> callApproveListSummary(string actId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_callApproveListSummary"
                    , new SqlParameter[] { new SqlParameter("@actId", actId) });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ApproveModel.approveListSummaryModels
                             {
                                 companyId = dr["companyId"].ToString(),
                                 activityGroup = dr["activitySales"].ToString(),
                                 channel = dr["chanelId"].ToString(),
                                 budgetAmount = dr["budgetAmount"] is DBNull ? 0 : (decimal?)dr["budgetAmount"],
                                 total = dr["total"] is DBNull ? 0 : (decimal?)dr["total"],
                                 balanceAmount = dr["balanceAmount"] is DBNull ? 0 : (decimal?)dr["balanceAmount"],
                                 fiscalYear = dr["fiscalYear"].ToString(),
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("callApproveListSummary >> " + ex.Message);
                return new List<ApproveModel.approveListSummaryModels>();
            }
        }


    } 
}