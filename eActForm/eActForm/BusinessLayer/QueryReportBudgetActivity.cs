using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebLibrary;
using static eActForm.Models.ReportActivityBudgetModels;

namespace eActForm.BusinessLayer
{
    public class QueryReportBudgetActivity
    {
        public static List<ReportActivityBudgetModel> getReportBudgetActivity()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getReportBudgetActivity");

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new ReportActivityBudgetModel()
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
                ExceptionManager.WriteError("getReportBudgetActivity => " + ex.Message);
                return new List<ReportActivityBudgetModel>();
            }
        }
    }
}