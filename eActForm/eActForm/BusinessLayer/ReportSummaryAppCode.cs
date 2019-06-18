﻿using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebLibrary;
using static eActForm.Models.ReportSummaryModels;

namespace eActForm.BusinessLayer
{
    public class ReportSummaryAppCode
    {

        public static ApproveFlowModel.approveFlowModel getFlowByActFormId(string actId)
        {
            try
            {
                ApproveFlowModel.approveFlowModel model = new ApproveFlowModel.approveFlowModel();
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getFlowMainByActFormId"
                    , new SqlParameter[] { new SqlParameter("@actFormId", actId) });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ApproveFlowModel.flowApprove()
                             {
                                 id = dr["id"].ToString(),
                                 flowNameTH = dr["flowNameTH"].ToString(),
                                 cusNameTH = dr["cusNameTH"].ToString(),
                                 cusNameEN = dr["cusNameEN"].ToString(),
                                 nameTH = dr["nameTH"].ToString(),
                             }).ToList();
                model.flowMain = lists[0];
                model.flowDetail = getFlowDetailWithApproveSummaryDetail(model.flowMain.id, actId);
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getFlowByActFormId >>" + ex.Message);
            }
        }


        public static List<ApproveFlowModel.flowApproveDetail> getFlowDetailWithApproveSummaryDetail(string flowId, string actId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getFlowApproveWithStatusDetail"
                    , new SqlParameter[] {
                        new SqlParameter("@flowId", flowId)
                        ,new SqlParameter("@actFormId", actId)
                    });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ApproveFlowModel.flowApproveDetail()
                             {
                                 id = dr["id"].ToString(),
                                 rangNo = (int)dr["rangNo"],
                                 empId = dr["empId"].ToString(),
                                 empEmail = dr["empEmail"].ToString(),
                                 empFNameTH = dr["empFNameTH"].ToString(),
                                 empLNameTH = dr["empLNameTH"].ToString(),
                                 empPositionTitleTH = dr["empPositionTitleTH"].ToString(),
                                 approveGroupName = dr["approveGroupName"].ToString(),
                                 approveGroupNameEN = dr["approveGroupNameEN"].ToString(),
                                 isShowInDoc = (bool)dr["showInDoc"],
                                 description = dr["description"].ToString(),
                                 statusId = dr["statusId"].ToString(),
                                 remark = dr["remark"].ToString(),
                                 imgSignature = string.Format(ConfigurationManager.AppSettings["rootgetSignaURL"], dr["empId"].ToString()),
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getFlowDetailWithApproveDetail >>" + ex.Message);
            }
        }

        public static List<ReportSummaryModels.ReportSummaryModel> getSummaryDetailReportByDate(string startDate, string endDate)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getDetailSummarybyDate"
                      , new SqlParameter[] {
                        new SqlParameter("@startDate",DateTime.ParseExact(startDate,"MM/dd/yyyy",null))
                        ,new SqlParameter("@endDate",DateTime.ParseExact(endDate,"MM/dd/yyyy",null))
                    });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ReportSummaryModels.ReportSummaryModel()
                             {
                                 id = Guid.NewGuid().ToString(),
                                 productType = dr["productTypeTH"].ToString(),
                                 productTypeId = dr["productTypeId"].ToString(),
                                 repDetailId = dr["repDetailId"].ToString(),
                                 activityNo = dr["activityNo"].ToString(),
                                 customerId = dr["customerId"].ToString(),
                                 customerName = dr["cusNameTH"].ToString(),
                                 createdDate = (DateTime?)dr["createdDate"],
                             });

                return lists.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("getSummaryDetailReportByDate >>" + ex.Message);
            }
        }

        public static List<ReportSummaryModel> getReportSummary(string repId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getReportSummary"
                     , new SqlParameter[] {
                        new SqlParameter("@repId",repId)
                    });

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new ReportSummaryModel()
                              {

                                  customerName = d["customerName"].ToString(),
                                  activitySales = d["activitySales"].ToString(),
                                  activityId = d["actid"].ToString(),
                                  repDetailId = d["repDetailId"].ToString(),
                                  est = decimal.Parse(AppCode.checkNullorEmpty(d["est"].ToString())),
                                  crystal = decimal.Parse(AppCode.checkNullorEmpty(d["crystal"].ToString())),
                                  wranger = decimal.Parse(AppCode.checkNullorEmpty(d["wranger"].ToString())),
                                  plus100 = decimal.Parse(AppCode.checkNullorEmpty(d["100plus"].ToString())),
                                  jubjai = decimal.Parse(AppCode.checkNullorEmpty(d["jubjai"].ToString())),
                                  oishi = decimal.Parse(AppCode.checkNullorEmpty(d["oishi"].ToString())),
                                  soda = decimal.Parse(AppCode.checkNullorEmpty(d["soda"].ToString())),
                                  water = decimal.Parse(AppCode.checkNullorEmpty(d["water"].ToString())),
                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getReportSummary => " + ex.Message);
                return new List<ReportSummaryModel>();
            }
        }


        public static List<ReportSummaryModels.ReportSummaryModel> getFilterSummaryDetailByProductType(List<ReportSummaryModels.ReportSummaryModel> lists, string producttypeId)
        {
            try
            {
                return lists.Where(r => r.productTypeId == producttypeId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterSummaryDetailByProductType >>" + ex.Message);
            }
        }

        public static List<ReportSummaryModels.ReportSummaryModel> getFilterSummaryDetailByCustomer(List<ReportSummaryModels.ReportSummaryModel> lists, string cusId)
        {
            try
            {
                return lists.Where(r => r.customerId == cusId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterSummaryDetailByCustomer >>" + ex.Message);
            }
        }

        public static string insertActivitySummaryDetail(string customerId, string productTypeId, string startDate, string endDate, ReportSummaryModels model)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                int rtn = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertSummaryDetail"
                    , new SqlParameter[] {
                        new SqlParameter("@id",id)
                        ,new SqlParameter("@activityNo","")
                        ,new SqlParameter("@statusId",(int)AppCode.ApproveStatus.รออนุมัติ)
                        ,new SqlParameter("@startDate",DateTime.ParseExact(startDate,"MM/dd/yyyy",null))
                        ,new SqlParameter("@endDate",DateTime.ParseExact(endDate,"MM/dd/yyyy",null))
                        ,new SqlParameter("@customerId",customerId)
                        ,new SqlParameter("@productTypeId",productTypeId)
                        ,new SqlParameter("@delFlag",false)
                        ,new SqlParameter("@createdDate",DateTime.Now)
                        ,new SqlParameter("@createdByUserId",UtilsAppCode.Session.User.empId)
                        ,new SqlParameter("@updatedDate",DateTime.Now)
                        ,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
                    });

                if (rtn > 0)
                {
                    string actIdTemp = "";
                    foreach (var item in model.activitySummaryList)
                    {
                        if (actIdTemp != item.id)
                        {
                            SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertSummaryMapForm"
                            , new SqlParameter[] {
                                new SqlParameter("@id",Guid.NewGuid().ToString())
                                ,new SqlParameter("@repDetailId",item.repDetailId)
                                ,new SqlParameter("@summaryId",id)
                                ,new SqlParameter("@delFlag",false)
                                ,new SqlParameter("@createdDate",DateTime.Now)
                                ,new SqlParameter("@createdByUserId",UtilsAppCode.Session.User.empId)
                                ,new SqlParameter("@updatedDate",DateTime.Now)
                                ,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
                            });
                        }
                        actIdTemp = item.id;
                    }
                }
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception("insertActivitySummaryDetail >>" + ex.Message);
            }
        }


        public static int insertApproveForReportSummaryDetail(string customerId, string productTypeId, string summaryId)
        {
            try
            {
                int rtn = 0;
                ApproveFlowModel.approveFlowModel flowModel = ApproveFlowAppCode.getFlowForReportDetail(
                    ConfigurationManager.AppSettings["subjectSummaryId"]
                    , customerId
                    , productTypeId);
                if (ApproveAppCode.insertApproveByFlow(flowModel, summaryId) > 0)
                {
                    rtn = ApproveAppCode.updateApproveWaitingByRangNo(summaryId);
                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception("insertApproveForReportSummaryDetail >>" + ex.Message);
            }
        }
    }
}

