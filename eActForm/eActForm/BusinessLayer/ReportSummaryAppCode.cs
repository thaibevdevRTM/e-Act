using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
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
        public static List<ReportSummaryModels.ReportSummaryModel> getSummaryDetailReportByDate(string startDate, string endDate)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getReportDetailByCreateDate"
                      , new SqlParameter[] {
                        new SqlParameter("@startDate",DateTime.ParseExact(startDate,"MM/dd/yyyy",null))
                        ,new SqlParameter("@endDate",DateTime.ParseExact(endDate,"MM/dd/yyyy",null))
                    });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ReportSummaryModels.ReportSummaryModel()
                             {
                                 activitySales = dr["chanelGroup"].ToString(),
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

        public static List<ReportSummaryModel> getReportSummary()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getReportBudgetActivity");

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new ReportSummaryModel()
                              {

                                  customerName = d["customerName"].ToString(),
                                  activitySales = d["activitySales"].ToString(),
                                  activityId = d["actid"].ToString(),
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


        public static List<ReportSummaryModels.ReportSummaryModel> getFilterSummaryDetailByRepNo(List<ReportSummaryModels.ReportSummaryModel> lists, string repNo)
        {
            try
            {
                return lists.Where(r => r.repDetailId == repNo).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterSummaryDetailByRepNo >>" + ex.Message);
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
                throw new Exception("getFilterSummaryDetailByRepNo >>" + ex.Message);
            }
        }
    }
}

