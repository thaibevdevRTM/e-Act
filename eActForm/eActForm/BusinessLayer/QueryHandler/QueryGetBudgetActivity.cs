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
    public class QueryGetBudgetActivity
    {
        public static List<Budget_Activity_Model.Budget_Activity_Status_Att> getBudgetActivityStatus()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetActivityStstus");

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new Budget_Activity_Model.Budget_Activity_Status_Att()
                              {
                                  id = d["id"].ToString(),
                                  nameTH = d["nameTH"].ToString(),
                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getBudgetActivityStatus => " + ex.Message);
                return new List<Budget_Activity_Model.Budget_Activity_Status_Att>();
            }
        }

        public static List<TB_Bud_Activity_Model.Budget_Activity_Att> getBudgetActivity(string act_approveStatusId, string act_activityId, string act_activityNo, string budgetApproveId, string companyTH, DateTime act_createdDateStart, DateTime act_createdDateEnd, string act_budgetStatusIdIn)
        {
            try
            {
                //act_budgetStatusIdIn =null;
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetActivity"
                 , new SqlParameter("@act_approveStatusId", act_approveStatusId)
                 , new SqlParameter("@act_activityId", act_activityId)
                 , new SqlParameter("@act_activityNo", act_activityNo)
                 , new SqlParameter("@budgetApproveId", budgetApproveId)
                 , new SqlParameter("@companyTH", companyTH)

                 , new SqlParameter("@act_createdDateStart", act_createdDateStart)
                 , new SqlParameter("@act_createdDateEnd", act_createdDateEnd)
                 , new SqlParameter("@act_budgetStatusIdIn", act_budgetStatusIdIn)
                 );

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new TB_Bud_Activity_Model.Budget_Activity_Att()
                              {
                                  budget_id = d["budget_Id"].ToString(),
                                  act_form_id = d["act_form_id"].ToString(),
                                  act_approveStatusId = int.Parse(d["act_approveStatusId"].ToString()),
                                  act_activityNo = d["act_activityNo"].ToString(),
                                  act_reference = d["act_reference"].ToString(),
                                  act_customerId = d["act_customerId"].ToString(),

                                  act_companyEN = d["act_companyEN"].ToString(),

                                  cus_cusShortName = d["cus_cusShortName"].ToString(),
                                  cus_cusNameEN = d["cus_cusNameEN"].ToString(),
                                  cus_cusNameTH = d["cus_cusNameTH"].ToString(),

                                  ch_chanelCust = d["ch_chanelCust"].ToString(),
                                  ch_chanelGroup = d["ch_chanelGroup"].ToString(),
                                  ch_chanelTradingPartner = d["ch_chanelTradingPartner"].ToString(),

                                  prd_groupName = d["prd_groupName"].ToString(),
                                  prd_groupNameTH = d["prd_groupNameTH"].ToString(),
                                  prd_groupShort = d["prd_groupShort"].ToString(),

                                  act_brandNameTH = d["act_brandNameTH"].ToString(),
                                  act_brandName = d["act_brandName"].ToString(),
                                  act_shortBrand = d["act_shortBrand"].ToString(),

                                  act_activityPeriodSt = d["act_activityPeriodSt"] is DBNull ? null : (DateTime?)d["act_activityPeriodSt"],
                                  act_activityPeriodEnd = d["act_activityPeriodEnd"] is DBNull ? null : (DateTime?)d["act_activityPeriodEnd"],
                                  act_costPeriodSt = d["act_costPeriodSt"] is DBNull ? null : (DateTime?)d["act_costPeriodSt"],
                                  act_costPeriodEnd = d["act_costPeriodEnd"] is DBNull ? null : (DateTime?)d["act_costPeriodEnd"],

                                  act_activityName = d["act_activityName"].ToString(),
                                  act_theme = d["act_activitySales"].ToString(),

                                  act_objective = d["act_objective"].ToString(),
                                  act_trade = d["act_trade"].ToString(),
                                  act_activityDetail = d["act_activityDetail"].ToString(),

                                  act_normalCost = d["act_normalCost"].ToString() == "" ? 0 : decimal.Parse(d["act_normalCost"].ToString()),
                                  act_themeCost = d["act_themeCost"].ToString() == "" ? 0 : decimal.Parse(d["act_themeCost"].ToString()),
                                  act_totalCost = d["act_totalCost"].ToString() == "" ? 0 : decimal.Parse(d["act_totalCost"].ToString()),
                                  act_total_invoive = d["act_total_invoive"].ToString() == "" ? 0 : decimal.Parse(d["act_total_invoive"].ToString()),
                                  act_balance = d["act_balance"].ToString() == "" ? 0 : decimal.Parse(d["act_balance"].ToString()),

                                  act_createdDate = d["act_createdDate"] is DBNull ? null : (DateTime?)d["act_createdDate"],
                                  act_updatedDate = d["act_updatedDate"] is DBNull ? null : (DateTime?)d["act_updatedDate"],

                                  act_createdByUserId = d["act_createdByUserId"].ToString(),
                                  act_updatedByUserId = d["act_updatedByUserId"].ToString(),

                                  bud_ActivityStatusId = d["bud_ActivityStatusId"].ToString(),
                                  bud_ActivityStatus = d["bud_ActivityStatus"].ToString(),

                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getBudgetActivity => " + ex.Message);
                return new List<TB_Bud_Activity_Model.Budget_Activity_Att>();
            }
        }

        public static List<TB_Bud_Activity_Model.Budget_Activity_Att> getBudgetActivityList(string act_approveStatusId, string act_activityId, string act_activityNo, string budgetApproveId, string companyTH, DateTime act_createdDateStart, DateTime act_createdDateEnd, string act_budgetStatusIdIn)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetActivityList"
                 , new SqlParameter("@act_approveStatusId", act_approveStatusId)
                 , new SqlParameter("@act_activityId", act_activityId)
                 , new SqlParameter("@act_activityNo", act_activityNo)
                 , new SqlParameter("@budgetApproveId", budgetApproveId)
                 , new SqlParameter("@companyTH", companyTH)

                 , new SqlParameter("@act_createdDateStart", act_createdDateStart)
                 , new SqlParameter("@act_createdDateEnd", act_createdDateEnd)
                 , new SqlParameter("@act_budgetStatusIdIn", act_budgetStatusIdIn)
                 );

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new TB_Bud_Activity_Model.Budget_Activity_Att()
                              {
                                  budget_id = d["budget_Id"].ToString(),
                                  act_form_id = d["act_form_id"].ToString(),
                                  act_approveStatusId = int.Parse(d["act_approveStatusId"].ToString()),
                                  act_activityNo = d["act_activityNo"].ToString(),
                                  act_reference = d["act_reference"].ToString(),
                                  act_customerId = d["act_customerId"].ToString(),

                                  act_companyEN = d["act_companyEN"].ToString(),

                                  cus_cusShortName = d["cus_cusShortName"].ToString(),
                                  cus_cusNameEN = d["cus_cusNameEN"].ToString(),
                                  cus_cusNameTH = d["cus_cusNameTH"].ToString(),

                                  //ch_chanelCust = d["ch_chanelCust"].ToString(),
                                  //ch_chanelGroup = d["ch_chanelGroup"].ToString(),
                                  //ch_chanelTradingPartner = d["ch_chanelTradingPartner"].ToString(),

                                  //prd_groupName = d["prd_groupName"].ToString(),
                                  //prd_groupNameTH = d["prd_groupNameTH"].ToString(),
                                  //prd_groupShort = d["prd_groupShort"].ToString(),

                                  //act_brandNameTH = d["act_brandNameTH"].ToString(),
                                  //act_brandName = d["act_brandName"].ToString(),
                                  //act_shortBrand = d["act_shortBrand"].ToString(),

                                  //act_objective = d["act_objective"].ToString(),
                                  //act_trade = d["act_trade"].ToString(),
                                  //act_activityDetail = d["act_activityDetail"].ToString(),

                                  act_activityPeriodSt = d["act_activityPeriodSt"] is DBNull ? null : (DateTime?)d["act_activityPeriodSt"],
                                  act_activityPeriodEnd = d["act_activityPeriodEnd"] is DBNull ? null : (DateTime?)d["act_activityPeriodEnd"],
                                  act_costPeriodSt = d["act_costPeriodSt"] is DBNull ? null : (DateTime?)d["act_costPeriodSt"],
                                  act_costPeriodEnd = d["act_costPeriodEnd"] is DBNull ? null : (DateTime?)d["act_costPeriodEnd"],

                                  act_activityName = d["act_activityName"].ToString(),
                                  act_theme = d["act_activitySales"].ToString(),

                                  act_normalCost = d["act_normalCost"].ToString() == "" ? 0 : decimal.Parse(d["act_normalCost"].ToString()),
                                  act_themeCost = d["act_themeCost"].ToString() == "" ? 0 : decimal.Parse(d["act_themeCost"].ToString()),
                                  act_totalCost = d["act_totalCost"].ToString() == "" ? 0 : decimal.Parse(d["act_totalCost"].ToString()),
                                  act_total_invoive = d["act_total_invoive"].ToString() == "" ? 0 : decimal.Parse(d["act_total_invoive"].ToString()),
                                  act_balance = d["act_balance"].ToString() == "" ? 0 : decimal.Parse(d["act_balance"].ToString()),

                                  act_createdDate = d["act_createdDate"] is DBNull ? null : (DateTime?)d["act_createdDate"],
                                  act_updatedDate = d["act_updatedDate"] is DBNull ? null : (DateTime?)d["act_updatedDate"],

                                  act_createdByUserId = d["act_createdByUserId"].ToString(),
                                  act_updatedByUserId = d["act_updatedByUserId"].ToString(),

                                  bud_ActivityStatusId = d["bud_ActivityStatusId"].ToString(),
                                  bud_ActivityStatus = d["bud_ActivityStatus"].ToString(),

                                  // claim
                                  act_claimNo = d["act_claimNo"].ToString(),
                                  act_claimShare = d["act_claimShare"].ToString(),
                                  act_claimStatus = d["act_claimStatus"].ToString(),

                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getBudgetActivity => " + ex.Message);
                return new List<TB_Bud_Activity_Model.Budget_Activity_Att>();
            }
        }

        public static List<Budget_Activity_Model.Budget_Activity_Product_Att> getBudgetActivityProduct(string act_activityID, string act_activityOfEstimateId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetActivityProduct"
                 , new SqlParameter("@activityID", act_activityID)
                 , new SqlParameter("@productID", null)
                 , new SqlParameter("@activityOfEstimateID", act_activityOfEstimateId)
                 //, new SqlParameter("@productID", prd_productID)
                 //, new SqlParameter("@activityOfEstimateID", act_activityOfEstimateId)
                 );

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new Budget_Activity_Model.Budget_Activity_Product_Att()
                              {
                                  act_activityId = d["act_activityId"].ToString(),
                                  act_activityNo = d["act_activityNo"].ToString(),
                                  prd_productId = d["prd_productId"].ToString(),
                                  activityOfEstimateId = d["activityOfEstimateId"].ToString(),
                                  act_typeTheme = d["act_typeTheme"].ToString(),
                                  prd_productDetail = d["prd_productDetail"].ToString(),
                                  normalCost = d["normalCost"].ToString() == "" ? 0 : decimal.Parse(d["normalCost"].ToString()),
                                  themeCost = d["themeCost"].ToString() == "" ? 0 : decimal.Parse(d["themeCost"].ToString()),
                                  totalCost = d["totalCost"].ToString() == "" ? 0 : decimal.Parse(d["totalCost"].ToString()),
                                  invoiceTotalBath = d["invoiceTotalBath"].ToString() == "" ? 0 : decimal.Parse(d["invoiceTotalBath"].ToString()),

                                  productBalanceBath = d["productBalanceBath"].ToString() == "" ? 0 : decimal.Parse(d["productBalanceBath"].ToString()),
                                  budgetStatusId = d["budgetStatusId"].ToString(), /*สภานะเงินของรายการ product*/
                                  budgetStatusNameTH = d["budgetStatusNameTH"].ToString(), /*สภานะเงินของรายการ product*/
                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getActivityByApproveStatusId => " + ex.Message);
                return new List<Budget_Activity_Model.Budget_Activity_Product_Att>();
            }
        }

        public static List<Budget_Activity_Model.Budget_Activity_Invoice_Att> getBudgetActivityInvoice(string activityId, string activityOfEstimateId, string invoiceId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetActivityInvoice"
                 , new SqlParameter("@activityID", activityId)
                 , new SqlParameter("@activityOfEstimateID", activityOfEstimateId)
                 , new SqlParameter("@invoiceID", invoiceId)
                 );

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new Budget_Activity_Model.Budget_Activity_Invoice_Att()
                              {
                                  invoiceId = d["invoiceId"].ToString(),
                                  activityId = d["act_activityId"].ToString(),
                                  activityNo = d["act_activityNo"].ToString(),
                                  activityOfEstimateId = d["act_EstimateId"].ToString(),
                                  activityTypeTheme = d["act_typeTheme"].ToString(),
                                  productId = d["prd_productId"].ToString(),
                                  productDetail = d["prd_productDetail"].ToString(),

                                  normalCost = d["normalCost"].ToString() == "" ? 0 : decimal.Parse(d["normalCost"].ToString()),
                                  themeCost = d["themeCost"].ToString() == "" ? 0 : decimal.Parse(d["themeCost"].ToString()),
                                  totalCost = d["totalCost"].ToString() == "" ? 0 : decimal.Parse(d["totalCost"].ToString()),
                                  productStandBath = d["productStandBath"].ToString() == "" ? 0 : decimal.Parse(d["productStandBath"].ToString()),

                                  //paymentNo = d["paymentNo"].ToString(),
                                  //saleActCase = d["saleActCase"].ToString(),
                                  //saleActBath = d["saleActBath"].ToString(),

                                  budgetImageId = d["budgetImageId"].ToString(),
                                  actCustomerId = d["act_customerId"].ToString(),

                                  invoiceNo = d["invoiceNo"].ToString(),
                                  invoiceTotalBath = d["invoiceTotalBath"].ToString() == "" ? 0 : decimal.Parse(d["invoiceTotalBath"].ToString()),
                                  productBalanceBath = d["productBalanceBath"].ToString() == "" ? 0 : decimal.Parse(d["productBalanceBath"].ToString()),
                                  productBudgetStatusId = d["productBudgetStatusId"].ToString() == "" ? 0 : int.Parse(d["productBudgetStatusId"].ToString()),
                                  productBudgetStatusNameTH = d["productBudgetStatusNameTH"].ToString(),

                                  //invoiceActionDate = DateTime.Parse(d["invoiceActionDate"].ToString()),
                                  invoiceActionDate = d["invoiceActionDate"].ToString(), //is DBNull ? null : (DateTime?)d["invoiceActionDate"],
                                  dateInvoiceAction = d["dateInvoiceAction"] is DBNull ? null : (DateTime?)d["dateInvoiceAction"],

                                  invoiceBudgetStatusId = d["invoiceBudgetStatusId"].ToString() == "" ? 0 : int.Parse(d["invoiceBudgetStatusId"].ToString()),
                                  invoiceBudgetStatusNameTH = d["invoiceBudgetStatusNameTH"].ToString(),
                                  invoiceSeq = d["invoiceSeq"].ToString() == "" ? 0 : int.Parse(d["invoiceSeq"].ToString()),

                                  invoiceApproveStatusId = d["invoiceApproveStatusId"].ToString() == "" ? 0 : int.Parse(d["invoiceApproveStatusId"].ToString()),
                                  approveInvoiceId = d["approveInvoiceId"].ToString(),
                                  invoiceApproveStatusName = d["invoiceApproveStatusName"].ToString(),
                                  budgetApproveId = d["budgetApproveId"].ToString(),

                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("usp_getBudgetActivityInvoice => " + ex.Message);
                return new List<Budget_Activity_Model.Budget_Activity_Invoice_Att>();
            }
        }

        public static List<Budget_Activity_Model.Budget_Count_Wait_Approve_Att> getBudgetActivityWaitApprove(string act_activityId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetCountWatingApproveByActivityId"
                 , new SqlParameter("@activityId", act_activityId)
                 //, new SqlParameter("@productID", prd_productID)
                 //, new SqlParameter("@activityOfEstimateID", act_activityOfEstimateId)
                 );

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new Budget_Activity_Model.Budget_Count_Wait_Approve_Att()
                              {
                                  activityId = d["activityId"].ToString(),
                                  count_wait_approve = d["count_wait_approve"].ToString() == "" ? 0 : int.Parse(d["count_wait_approve"].ToString()),
                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getBudgetActivityWaitApprove => " + ex.Message);
                return new List<Budget_Activity_Model.Budget_Count_Wait_Approve_Att>();
            }
        }

        public static List<Budget_Activity_Model.Budget_Activity_Last_Approve_Att> getBudgetActivityLastApprove(string act_activityId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetApproveLastId"
                 , new SqlParameter("@activityId", act_activityId)
                 //, new SqlParameter("@productID", prd_productID)
                 //, new SqlParameter("@activityOfEstimateID", act_activityOfEstimateId)
                 );

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new Budget_Activity_Model.Budget_Activity_Last_Approve_Att()
                              {
                                  budgetActivityId = d["budgetActivityId"] is DBNull ? "" : d["budgetActivityId"].ToString(),
                                  budgetApproveId = d["budgetApproveId"] is DBNull ? "" : d["budgetApproveId"].ToString(),
                              });
                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getBudgetActivityLastApprove => " + ex.Message);
                return new List<Budget_Activity_Model.Budget_Activity_Last_Approve_Att>();
            }
        }

        public static List<Claim_Report_Model.Claim_Activity_Att> getClaimActivityList(string act_companyEn, string act_activityNo, string act_createdEmpId, string act_claimStatus, string act_s40Status, DateTime act_createdDateStart, DateTime act_createdDateEnd)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetReportClaimPDG"
                 , new SqlParameter("@companyEn", act_companyEn)
                 , new SqlParameter("@activityNo", act_activityNo)
                 , new SqlParameter("@createdEmpId", act_createdEmpId)
                 , new SqlParameter("@claimStatus", act_claimStatus)
                 , new SqlParameter("@s40Status", act_s40Status)
                 , new SqlParameter("@actCreateStartDate", act_createdDateStart)
                 , new SqlParameter("@actCreateEndDate", act_createdDateEnd)
                 );

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new Claim_Report_Model.Claim_Activity_Att()
                              {
                                  act_formId = d["act_formId"].ToString(),
                                  act_activityNo = d["act_activityNo"].ToString(),
                                  act_subCode = d["act_subCode"].ToString(),
                                  est_rowno = d["est_rowno"].ToString(),
                                  s40_status = d["s40_status"].ToString(),
                                  s40_Assignment = d["s40_Assignment"].ToString(),
                                  s40_GL = d["s40_GL"].ToString(),
                                  prd_productDetailShort = d["prd_productDetailShort"].ToString(),
                                  s40_Order = d["s40_Order"].ToString(),
                                  cus_cusNameEN = d["cus_cusNameEN"].ToString(),
                                  prd_brandName = d["brandName"].ToString(),
                                  s40_PstngDate = d["s40_PstngDate"].ToString(),
                                  s40_Reference = d["s40_Reference"].ToString(),
                                  s40_DocumentNo = d["s40_DocumentNo"].ToString(),
                                  s40_Amount = d["s40_Amount"].ToString() == "" ? 0 : decimal.Parse(d["s40_Amount"].ToString()),

                                  s20_Assignment = d["S20_Assignment"].ToString(),
                                  s20_DocumentDate = d["S20_DocumentDate"].ToString(),
                                  s20_DocumentNo = d["S20_DocumentNo"].ToString(),
                                  s20_Amount = d["S20_Amount"].ToString() == "" ? 0 : decimal.Parse(d["S20_Amount"].ToString()),

                                  claim_actStatus = d["claim_actStatus"].ToString(),
                                  claim_shareStatus = d["claim_shareStatus"].ToString(),
                                  claim_actValue = d["claim_actValue"].ToString(),
                                  claim_actIO = d["claim_actIO"].ToString(),
                                  product_IO = d["product_IO"].ToString(),

                                  act_activityName = d["act_activityName"].ToString(),
                                  prd_themeId = d["prd_themeId"].ToString(),
                                  prd_Theme = d["prd_Theme"].ToString(),
                                  cus_cusId = d["cus_cusId"].ToString(),
                                  cus_cusNameTH = d["cus_cusNameTH"].ToString(),
                                  prd_productDetail = d["prd_productDetail"].ToString(),
                                  act_activityPeriodSt = d["act_activityPeriodSt"].ToString(),
                                  act_activityPeriodEnd = d["act_activityPeriodEnd"].ToString(),
                                  act_Period = d["act_Period"].ToString(),
                                  act_costPeriod = d["act_costPeriod"].ToString(),
                                  act_createdDate = d["act_createdDate"].ToString(),
                                  invoiceSeq = d["invoiceSeq"].ToString(),
                                  invoiceNo = d["invoiceNo"].ToString(),

                                  est_totalBath = d["est_totalBath"].ToString() == "" ? 0 : decimal.Parse(d["est_totalBath"].ToString()),
                                  inv_totalBath = d["inv_totalBath"].ToString() == "" ? 0 : decimal.Parse(d["inv_totalBath"].ToString()),
                                  est_balanceBath = d["est_balanceBath"].ToString() == "" ? 0 : decimal.Parse(d["est_balanceBath"].ToString()),

                                  est_budgetStatusNameTH = d["est_budgetStatusNameTH"].ToString(),
                                  inv_createdDate = d["inv_createdDate"].ToString(),
                                  act_companyId = d["act_companyId"].ToString(),
                                  companyName = d["companyName"].ToString(),
                                  act_createdByUserId = d["act_createdByUserId"].ToString(),
                                  act_createdByName = d["act_createdByName"].ToString(),

                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getClaimActivityList => " + ex.Message);
                return new List<Claim_Report_Model.Claim_Activity_Att>();
            }
        }

    }
}