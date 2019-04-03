﻿using System;
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
        public static List<RepDetailModel.actFormRepDetailModel> getRepDetailReportByCreateDateAndStatusId(AppCode.ApproveStatus statusId, string startDate, string endDate)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getReportDetailByCreateDateAndStatusId"
                    , new SqlParameter[] {
                        new SqlParameter("@statusId",(int)statusId)
                        ,new SqlParameter("@startDate",DateTime.ParseExact(startDate,"MM/dd/yyyy",null))
                        ,new SqlParameter("@endDate",DateTime.ParseExact(endDate,"MM/dd/yyyy",null))
                    });

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
                                 cusNameTH = dr["cusNameTH"].ToString(),
                                 productId = dr["productId"].ToString(),
                                 productName = dr["productName"].ToString(),
                                 size = dr["size"].ToString(),
                                 typeTheme = dr["typeTheme"].ToString(),
                                 //normalSale = dr["normalSale"].ToString(),
                                 //promotionSale = dr["promotionSale"].ToString(),
                                 //total = (decimal?)dr["total"] ?? 0,
                                 //specialDisc = (decimal?)dr["specialDisc"] ?? 0,
                                 //specialDiscBaht = (decimal?)dr["specialDiscBaht"] ?? 0,
                                 //promotionCost = (decimal?)dr["promotionCost"] ?? 0,
                                 channelName = dr["channelName"].ToString(),
                                 productTypeId = dr["productTypeId"].ToString(),
                                 productTypeNameEN = dr["nameEN"].ToString(),
                                 cusShortName = dr["cusShortName"].ToString(),
                                 productCategory = dr["productCateText"].ToString(),
                                 productGroup = dr["productGroupId"].ToString(),
                                 groupName = dr["groupName"].ToString(),
                                 activityPeriodSt = (DateTime?)dr["activityPeriodSt"] ?? null,
                                 activityPeriodEnd = (DateTime?)dr["activityPeriodEnd"] ?? null,
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
            catch (Exception ex)
            {
                throw new Exception("getRepDetailReportByCreateDateAndStatusId >>" + ex.Message);
            }
        }
    }
}