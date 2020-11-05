using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebLibrary;

namespace eActForm.BusinessLayer.QueryHandler
{
    public class QueryGetBudgetReport
    {
        public static List<Budget_Report_Model.Report_Budget_Activity_Att> getReportBudgetActivity(string act_StatusId, string act_activityNo, string companyEN, string act_createdDateStart, string act_createdDateEnd , string actYear)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetReportActivity"
                 , new SqlParameter("@act_StatusId", act_StatusId)
                 , new SqlParameter("@act_activityNo", act_activityNo)
                 , new SqlParameter("@companyEN", companyEN)
                 , new SqlParameter("@createdDateStart", act_createdDateStart)
                 , new SqlParameter("@createdDateEnd", act_createdDateEnd)
                 , new SqlParameter("@actYear", actYear)
                 );

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new Budget_Report_Model.Report_Budget_Activity_Att()
                              {
                                  company = d["company"].ToString(),
                                  channelName = d["channelName"].ToString(),

                                  reportMMYY = d["reportMMYY"].ToString(),
                                  claim_actStatus = d["claim_actStatus"].ToString(),
                                  claim_shareStatus = d["claim_shareStatus"].ToString(),
                                  claim_actValue = d["claim_actValue"].ToString(),
                                  claim_actIO = d["claim_actIO"].ToString(),
                                  product_IO = d["product_IO"].ToString(),

                                  act_activityNo = d["act_activityNo"].ToString(),
                                  sub_code = d["sub_code"].ToString(),
                                  act_activityName = d["act_activityName"].ToString(),
                                  brandName = d["brandName"].ToString(),
                                  themeId = d["themeId"].ToString(),
                                  Theme = d["Theme"].ToString(),

                                  cus_id = d["cus_id"].ToString(),
                                  cus_regionId = d["cus_regionId"].ToString(),
                                  cus_regionName = d["cus_regionName"].ToString(),
                                  cus_regionDesc = d["cus_regionDesc"].ToString(),
                                  cus_cusNameTH = d["cus_cusNameTH"].ToString(),
                                  cus_cusNameEN = d["cus_cusNameEN"].ToString(),

                                  prd_typeId = d["prd_typeId"].ToString(),
                                  prd_groupId = d["prd_groupId"].ToString(),
                                  prd_productDetail = d["prd_productDetail"].ToString(),
                                  prd_productDetail50 = d["prd_productDetail50"].ToString(),
                                  prd_productDetailCount = int.Parse(d["prd_productDetailCount"].ToString()),

                                  activity_Period = d["activity_Period"].ToString(),
                                  activity_costPeriod = d["activity_costPeriod"].ToString(),
                                  actCreatedDate = d["actCreatedDate"].ToString(),

                                  activityTotalBath = d["activityTotalBath"].ToString() == "" ? 0 : decimal.Parse(d["activityTotalBath"].ToString()),
                                  activityInvoiceTotalBath = d["activityInvoiceTotalBath"].ToString() == "" ? 0 : decimal.Parse(d["activityInvoiceTotalBath"].ToString()),
                                  activityBalanceBath = d["activityBalanceBath"].ToString() == "" ? 0 : decimal.Parse(d["activityBalanceBath"].ToString()),

                                  productBudgetStatusGroupId = d["productBudgetStatusGroupId"].ToString(),
                                  ProductBudgetStatusId = d["ProductBudgetStatusId"].ToString(),
                                  productBudgetStatusNameTH = d["productBudgetStatusNameTH"].ToString(),
                                  invoiceCreatedDate = d["invoiceCreatedDate"].ToString(),
                                  act_status = d["act_status"].ToString(),
                                  actForm_CreatedByUserId = d["actForm_CreatedByUserId"].ToString(),
                                  actForm_CreatedByName = d["actForm_CreatedByName"].ToString()
                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getReportBudgetActivity => " + ex.Message);
                return new List<Budget_Report_Model.Report_Budget_Activity_Att>();
            }
        }

    }
}