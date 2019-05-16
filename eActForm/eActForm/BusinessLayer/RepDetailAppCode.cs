using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using WebLibrary;
using eActForm.Models;
namespace eActForm.BusinessLayer
{
    public class RepDetailAppCode
    {
        public static List<RepDetailModel.actFormRepDetailModel> getFilterRepDetailByActNo(List<RepDetailModel.actFormRepDetailModel> lists, string actNo)
        {
            try
            {
                return lists.Where(r => r.activityNo == actNo).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterRepDetailByActNo >>" + ex.Message);
            }
        }
        public static List<RepDetailModel.actFormRepDetailModel> getFilterRepDetailByStatusId(List<RepDetailModel.actFormRepDetailModel> lists, string statusId)
        {
            try
            {
                return lists.Where(r => r.statusId == statusId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterRepDetailByActNo >>" + ex.Message);
            }
        }
        public static List<RepDetailModel.actFormRepDetailModel> getFilterRepDetailByCustomer(List<RepDetailModel.actFormRepDetailModel> lists, string customerId)
        {
            try
            {
                return lists.Where(r => r.customerId == customerId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterRepDetailByActNo >>" + ex.Message);
            }
        }
        public static List<RepDetailModel.actFormRepDetailModel> getFilterRepDetailByActivity(List<RepDetailModel.actFormRepDetailModel> lists, string activityId)
        {
            try
            {
                return lists.Where(r => r.theme == activityId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterRepDetailByActNo >>" + ex.Message);
            }
        }
        public static List<RepDetailModel.actFormRepDetailModel> getFilterRepDetailByProductType(List<RepDetailModel.actFormRepDetailModel> lists, string productType)
        {
            try
            {
                return lists.Where(r => r.productTypeId == productType).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterRepDetailByProductType >>" + ex.Message);
            }
        }
        public static List<RepDetailModel.actFormRepDetailModel> getFilterRepDetailByProductGroup(List<RepDetailModel.actFormRepDetailModel> lists, string productGroup)
        {
            try
            {
                return lists.Where(r => r.productGroupid == productGroup).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterRepDetailByActNo >>" + ex.Message);
            }
        }
        public static List<RepDetailModel.actFormRepDetailModel> getRepDetailReportByCreateDateAndStatusId(string startDate, string endDate)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getReportDetailByCreateDate"
                    , new SqlParameter[] {
                        new SqlParameter("@startDate",DateTime.ParseExact(startDate,"MM/dd/yyyy",null))
                        ,new SqlParameter("@endDate",DateTime.ParseExact(endDate,"MM/dd/yyyy",null))
                    });

                return dataTableToRepDetailModels(ds);
            }
            catch (Exception ex)
            {
                throw new Exception("getRepDetailReportByCreateDateAndStatusId >>" + ex.Message);
            }
        }

        public static List<RepDetailModel.actFormRepDetailModel> getRepDetailReportByCreateDateAndStatusId(string repDetailId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getReportDetailByRepDetailId"
                    , new SqlParameter[] {
                        new SqlParameter("@repDetailId",repDetailId)
                    });

                return dataTableToRepDetailModels(ds);
            }
            catch (Exception ex)
            {
                throw new Exception("getRepDetailReportByCreateDateAndStatusId >>" + ex.Message);
            }
        }

        private static List<RepDetailModel.actFormRepDetailModel> dataTableToRepDetailModels(DataSet ds)
        {
            try
            {
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new RepDetailModel.actFormRepDetailModel()
                             {
                                 #region detail parse
                                 id = dr["id"].ToString(),
                                 statusId = dr["statusId"].ToString(),
                                 statusName = dr["statusName"].ToString(),
                                 activityNo = dr["activityNo"].ToString(),
                                 documentDate = (DateTime?)dr["documentDate"] ?? null,
                                 reference = dr["reference"].ToString(),
                                 customerId = dr["customerId"].ToString(),
                                 productCateId = dr["productCateId"].ToString(),
                                 productGroupid = dr["productGroupid"].ToString(),
                                 cusNameTH = dr["cusNameTH"].ToString(),
                                 productId = dr["productId"].ToString(),
                                 productName = dr["productName"].ToString(),
                                 size = dr["size"].ToString(),
                                 typeTheme = dr["typeTheme"].ToString(),
                                 normalSale = dr["normalSale"].ToString(),
                                 promotionSale = dr["promotionSale"].ToString(),
                                 total = dr["total"] is DBNull ? 0 : (decimal?)dr["total"],
                                 specialDisc = dr["specialDisc"] is DBNull ? 0 : (decimal?)dr["specialDisc"],
                                 specialDiscBaht = dr["specialDiscBaht"] is DBNull ? 0 : (decimal?)dr["specialDiscBaht"],
                                 promotionCost = dr["promotionCost"] is DBNull ? 0 : (decimal?)dr["promotionCost"],
                                 channelName = dr["channelName"].ToString(),
                                 productTypeId = dr["productTypeId"].ToString(),
                                 productTypeNameEN = dr["nameEN"].ToString(),
                                 cusShortName = dr["cusShortName"].ToString(),
                                 productCategory = dr["productCateText"].ToString(),
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
                                 #endregion

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