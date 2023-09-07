using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    public class QueryGetBudgetActivity
    {
        public static List<Budget_Activity_Model.Budget_Activity_Year_Att> getYearActivity()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetActivityYearList");

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new Budget_Activity_Model.Budget_Activity_Year_Att()
                              {
                                  activityYear = d["activityYear"].ToString()
                              }
                              );

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getYearActivity => " + ex.Message);
                return new List<Budget_Activity_Model.Budget_Activity_Year_Att>();
            }
        }
        public static List<TB_Bud_Activity_Model.Budget_Activity_Att> getBudgetActivityList(string act_approveStatusId, string act_activityId, string act_activityNo, string budgetApproveId, string companyTH, string act_createdDateStart, string act_createdDateEnd, string act_budgetStatusIdIn, string actYear)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetActivityList"
                 , new SqlParameter("@act_approveStatusId", act_approveStatusId)
                 , new SqlParameter("@act_activityId", act_activityId)
                 , new SqlParameter("@act_activityNo", act_activityNo)
                 , new SqlParameter("@budgetApproveId", budgetApproveId)
                 , new SqlParameter("@companyTH", companyTH)

                 , new SqlParameter("@act_createdDateStart", act_createdDateStart)
                 , new SqlParameter("@act_createdDateEnd", act_createdDateEnd)
                 , new SqlParameter("@act_budgetStatusIdIn", act_budgetStatusIdIn)
                 , new SqlParameter("@actYear", actYear)
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

                                  act_activityPeriodSt = d["act_activityPeriodSt"] is DBNull ? null : (DateTime?)d["act_activityPeriodSt"],
                                  act_activityPeriodEnd = d["act_activityPeriodEnd"] is DBNull ? null : (DateTime?)d["act_activityPeriodEnd"],
                                  act_costPeriodSt = d["act_costPeriodSt"] is DBNull ? null : (DateTime?)d["act_costPeriodSt"],
                                  act_costPeriodEnd = d["act_costPeriodEnd"] is DBNull ? null : (DateTime?)d["act_costPeriodEnd"],

                                  act_compensateStatus = d["act_compensateStatus"].ToString(),
                                  act_compensateDateStart = d["act_compensateDateStart"] is DBNull ? null : (DateTime?)d["act_compensateDateStart"],
                                  act_compensateDateEnd = d["act_compensateDateEnd"] is DBNull ? null : (DateTime?)d["act_compensateDateEnd"],

                                  act_activityName = d["act_activityName"].ToString(),
                                  act_theme = d["act_activitySales"].ToString(),

                                  act_normalCost = d["act_normalCost"].ToString() == "" ? 0 : decimal.Parse(d["act_normalCost"].ToString()),
                                  act_themeCost = d["act_themeCost"].ToString() == "" ? 0 : decimal.Parse(d["act_themeCost"].ToString()),
                                  act_totalCost = d["act_totalCost"].ToString() == "" ? 0 : decimal.Parse(d["act_totalCost"].ToString()),
                                  act_total_invoive = d["act_total_invoive"].ToString() == "" ? 0 : decimal.Parse(d["act_total_invoive"].ToString()),
                                  act_balance = d["act_balance"].ToString() == "" ? 0 : decimal.Parse(d["act_balance"].ToString()),
                                  act_grandTotalCost = d["act_grandTotalCost"].ToString() == "" ? 0 : decimal.Parse(d["act_grandTotalCost"].ToString()),
                                  act_grandTotalBalance = d["act_grandTotalBalance"].ToString() == "" ? 0 : decimal.Parse(d["act_grandTotalBalance"].ToString()),
                                  
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
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetActivityProductSelect"
                 , new SqlParameter("@activityID", act_activityID)
                 , new SqlParameter("@activityOfEstimateID", act_activityOfEstimateId)
                 );

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new Budget_Activity_Model.Budget_Activity_Product_Att()
                              {
                                  act_activityId = d["act_activityId"].ToString(),
                                  act_activityNo = d["act_activityNo"].ToString(),
                                  activityOfEstimateId = d["activityOfEstimateId"].ToString(),
                                  act_typeTheme = d["act_typeTheme"].ToString(),

                                  prd_productId = d["prd_productId"].ToString(),
                                  prd_productDetail = d["prd_productDetail"].ToString(),
                                  normalCost = d["normalCost"].ToString() == "" ? 0 : decimal.Parse(d["normalCost"].ToString()),
                                  themeCost = d["themeCost"].ToString() == "" ? 0 : decimal.Parse(d["themeCost"].ToString()),
                                  totalCost = d["totalCost"].ToString() == "" ? 0 : decimal.Parse(d["totalCost"].ToString()),
                                  invoiceTotalBath = d["invoiceTotalBath"].ToString() == "" ? 0 : decimal.Parse(d["invoiceTotalBath"].ToString()),

                                  productBalanceBath = d["productBalanceBath"].ToString() == "" ? 0 : decimal.Parse(d["productBalanceBath"].ToString()),
                                  productGrandTotalaCost = d["productGrandTotalaCost"].ToString() == "" ? 0 : decimal.Parse(d["productGrandTotalaCost"].ToString()),
                                  
                                  budgetStatusId = d["budgetStatusId"].ToString(), /*สภานะเงินของรายการ product*/
                                  budgetStatusNameTH = d["budgetStatusNameTH"].ToString(), /*สภานะเงินของรายการ product*/
                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getBudgetActivityProduct => " + ex.Message);
                return new List<Budget_Activity_Model.Budget_Activity_Product_Att>();
            }
        }

        public static List<Budget_Approve_Detail_Model.budgetForm> getBudgetDocumentLists(string empId, string companyEN, string createdDateStart, string createdDateEnd)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetDocumentList"
                    , new SqlParameter[] {
                    new SqlParameter("@empId", empId),
                    new SqlParameter("@companyEN", companyEN),
                    new SqlParameter("@createdDateStart", createdDateStart),
                    new SqlParameter("@createdDateEnd", createdDateEnd)
                    });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new Budget_Approve_Detail_Model.budgetForm()
                             {
                                 statusId = dr["statusId"].ToString(),
                                 statusName = dr["statusName"].ToString(),
                                 activityId = dr["activityId"].ToString(),
                                 activityNo = dr["activityNo"].ToString(),

                                 budgetApproveId = dr["budgetApproveId"].ToString(),
                                 documentDate = dr["documentDate"] is DBNull ? null : (DateTime?)dr["documentDate"],

                                 reference = dr["reference"].ToString(),
                                 customerId = dr["customerId"].ToString(),
                                 channelName = dr["channelName"].ToString(),
                                 productTypeId = dr["productTypeId"].ToString(),
                                 productTypeNameEN = dr["productTypeNameEN"].ToString(),

                                 cusShortName = dr["cusShortName"].ToString(),
                                 cusNameTH = dr["cusNameTH"].ToString(),
                                 productCategory = dr["productCateText"].ToString(),
                                 productGroup = dr["productGroupId"].ToString(),
                                 productGroupName = dr["productGroupName"].ToString(),

                                 activityPeriodSt = dr["activityPeriodSt"] is DBNull ? null : (DateTime?)dr["activityPeriodSt"],
                                 activityPeriodEnd = dr["activityPeriodEnd"] is DBNull ? null : (DateTime?)dr["activityPeriodEnd"],
                                 costPeriodSt = dr["costPeriodSt"] is DBNull ? null : (DateTime?)dr["costPeriodSt"],
                                 costPeriodEnd = dr["costPeriodEnd"] is DBNull ? null : (DateTime?)dr["costPeriodEnd"],
                                 activityName = dr["activityName"].ToString(),

                                 themeId = dr["themeId"].ToString(),
                                 theme = dr["theme"].ToString(),
                                 objective = dr["objective"].ToString(),
                                 trade = dr["trade"].ToString(),
                                 activityDetail = dr["activityDetail"].ToString(),

                                 budgetActivityId = dr["budgetActivityId"].ToString(),
                                 approveId = dr["approveId"].ToString(),

                                 createdDate = (DateTime?)dr["createdDate"],
                                 createdByUserId = dr["createdByUserId"].ToString(),
                                 updatedDate = (DateTime?)dr["updatedDate"],
                                 updatedByUserId = dr["updatedByUserId"].ToString(),

                                 normalCost = dr["normalCost"] is DBNull ? 0 : (decimal?)dr["normalCost"],
                                 themeCost = dr["themeCost"] is DBNull ? 0 : (decimal?)dr["themeCost"],
                                 totalCost = dr["totalCost"] is DBNull ? 0 : (decimal?)dr["totalCost"],
                                 totalInvoiceApproveBath = dr["totalInvoiceApproveBath"] is DBNull ? 0 : (decimal?)dr["totalInvoiceApproveBath"]

                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getBudgetListsByEmpId >> " + ex.Message);
                return new List<Budget_Approve_Detail_Model.budgetForm>();
            }
        }


        public static List<Budget_Activity_Model.Budget_Activity_Status_Att> getBudgetActivityStatus()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetActivityStatusList");

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

        public static List<TB_Bud_Activity_Model.Budget_Activity_Att> getBudgetActivityDetail(string act_approveStatusId, string act_activityId, string budget_approveId, string companyTH, string act_createdDateStart, string act_createdDateEnd, string act_budgetStatusIdIn)
        {
            try
            {
                //act_budgetStatusIdIn =null;
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetActivitySelect"
                 , new SqlParameter("@act_approveStatusId", act_approveStatusId)
                 , new SqlParameter("@act_activityId", act_activityId)
                 , new SqlParameter("@budget_approveId", budget_approveId)
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

                                  act_compensateStatus = d["act_compensateStatus"].ToString(),
                                  act_compensateDateStart = (DateTime?)d["act_compensateDateStart"],
                                  act_compensateDateEnd = (DateTime?)d["act_compensateDateEnd"]
                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getBudgetActivity => " + ex.Message);
                return new List<TB_Bud_Activity_Model.Budget_Activity_Att>();
            }
        }

        public static List<Budget_Activity_Model.Budget_Activity_Invoice_Att> getBudgetActivityInvoice(string activityId, string activityOfEstimateId, string invoiceId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetActivityInvoiceSelect"
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
                                  budgetImageId = d["budgetImageId"].ToString(),
                                  actCustomerId = d["act_customerId"].ToString(),

                                  invoiceNo = d["invoiceNo"].ToString(),
                                  invoiceTotalBath = d["invoiceTotalBath"].ToString() == "" ? 0 : decimal.Parse(d["invoiceTotalBath"].ToString()),
                                  productTotalBath = d["productTotalBath"].ToString() == "" ? 0 : decimal.Parse(d["productTotalBath"].ToString()),
                                  productBalanceBath = d["productBalanceBath"].ToString() == "" ? 0 : decimal.Parse(d["productBalanceBath"].ToString()),
                                  productBudgetStatusId = d["productBudgetStatusId"].ToString() == "" ? 0 : int.Parse(d["productBudgetStatusId"].ToString()),
                                  productBudgetStatusNameTH = d["productBudgetStatusNameTH"].ToString(),

                                  invoiceActionDate = d["invoiceActionDate"].ToString(), 
                                  dateInvoiceAction = d["dateInvoiceAction"] is DBNull ? null : (DateTime?)d["dateInvoiceAction"],
                                  invoiceBudgetStatusId = d["invoiceBudgetStatusId"].ToString() == "" ? 0 : int.Parse(d["invoiceBudgetStatusId"].ToString()),
                                  invoiceBudgetStatusNameTH = d["invoiceBudgetStatusNameTH"].ToString(),
                                  invoiceSeq = d["invoiceSeq"].ToString() == "" ? 0 : int.Parse(d["invoiceSeq"].ToString()),

                                  invoiceApproveStatusId = d["invoiceApproveStatusId"].ToString() == "" ? 0 : int.Parse(d["invoiceApproveStatusId"].ToString()),
                                  approveInvoiceId = d["approveInvoiceId"].ToString(),
                                  invoiceApproveStatusName = d["invoiceApproveStatusName"].ToString(),
                                  budgetApproveId = d["budgetApproveId"].ToString(),
                                  invoiceRemark = d["invoiceRemark"].ToString(),
                                  invoiceType = d["invoiceType"].ToString(),
                                  count_approved = d["count_approved"].ToString() == "" ? 0 : int.Parse(d["count_approved"].ToString()),
                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("usp_mtm_BudgetActivityInvoiceSelect => " + ex.Message);
                return new List<Budget_Activity_Model.Budget_Activity_Invoice_Att>();
            }
        }

        public static List<Budget_Activity_Model.Budget_Count_Wait_Approve_Att> getBudgetActivityWaitApprove(string act_activityId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetCountWatingApproveOfAct"
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
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetApproveLastIdSelect"
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
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_ReportBudgetActivityClaimPDG"
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

        public static List<TB_Act_AmountBudget> getBudgetAmountList(string activityId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetAmountList"
                 , new SqlParameter("@activityId", activityId)
                 );

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new TB_Act_AmountBudget()
                              {
                                  id = d["id"].ToString(),
                                  activityId = d["activityId"].ToString(),
                                  budgetTotal = d["budgetTotal"].ToString() == "" ? 0 : decimal.Parse(d["budgetTotal"].ToString()),
                                  useAmount = d["useAmount"].ToString() == "" ? 0 : decimal.Parse(d["useAmount"].ToString()),
                                  returnAmount = d["returnAmount"].ToString() == "" ? 0 : decimal.Parse(d["returnAmount"].ToString()),
                                  amountBalance = d["amountBalance"].ToString() == "" ? 0 : decimal.Parse(d["amountBalance"].ToString()),
                                  EO = d["EO"].ToString(),
                                  activityType = d["activityType"].ToString(),
                                  brandName = d["brandName"].ToString(),
                                  typeShowBudget = d["budgetType"].ToString(),
                                  yearBG = d["yearBG"].ToString(),
                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getBudgetAmountList => " + ex.Message);
                return new List<TB_Act_AmountBudget>();
            }
        }

    }

    public class QueryGetBudgetApprove
    {

        public static List<Budget_Approve_Detail_Model.budgetForm> getApproveListsByEmpId(string empId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetApproveList"
                    , new SqlParameter[] { new SqlParameter("@empId", empId) });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new Budget_Approve_Detail_Model.budgetForm()
                             {
                                 activityId = dr["ActivityFormId"].ToString(),
                                 statusId = dr["statusId"].ToString(),
                                 statusName = dr["statusName"].ToString(),
                                 activityNo = dr["activityNo"].ToString(),

                                 regApproveId = dr["regApproveId"].ToString(),
                                 regApproveFlowId = dr["regApproveFlowId"].ToString(),
                                 budgetApproveId = dr["budgetApproveId"].ToString(),
                                 documentDate = dr["documentDate"] is DBNull ? null : (DateTime?)dr["documentDate"],

                                 reference = dr["reference"].ToString(),
                                 customerId = dr["customerId"].ToString(),
                                 channelName = dr["channelName"].ToString(),
                                 productTypeId = dr["productTypeId"].ToString(),
                                 productTypeNameEN = dr["productTypeNameEN"].ToString(),

                                 cusShortName = dr["cusShortName"].ToString(),
                                 cusNameTH = dr["cusNameTH"].ToString(),
                                 productCategory = dr["productCateText"].ToString(),
                                 productGroup = dr["productGroupId"].ToString(),
                                 productGroupName = dr["productGroupName"].ToString(),

                                 activityPeriodSt = dr["activityPeriodSt"] is DBNull ? null : (DateTime?)dr["activityPeriodSt"],
                                 activityPeriodEnd = dr["activityPeriodEnd"] is DBNull ? null : (DateTime?)dr["activityPeriodEnd"],
                                 costPeriodSt = dr["costPeriodSt"] is DBNull ? null : (DateTime?)dr["costPeriodSt"],
                                 costPeriodEnd = dr["costPeriodEnd"] is DBNull ? null : (DateTime?)dr["costPeriodEnd"],
                                 activityName = dr["activityName"].ToString(),
                                 theme = dr["theme"].ToString(),
                                 objective = dr["objective"].ToString(),
                                 trade = dr["trade"].ToString(),
                                 activityDetail = dr["activityDetail"].ToString(),

                                 budgetActivityId = dr["budgetActivityId"].ToString(),
                                 approveId = dr["approveId"].ToString(),
                                 approveDetailId = dr["approveDetailId"].ToString(),
                                 createdDate = (DateTime?)dr["createdDate"],
                                 createdByUserId = dr["createdByUserId"].ToString(),
                                 updatedDate = (DateTime?)dr["updatedDate"],
                                 updatedByUserId = dr["updatedByUserId"].ToString(),

                                 normalCost = dr["normalCost"] is DBNull ? 0 : (decimal?)dr["normalCost"],
                                 themeCost = dr["themeCost"] is DBNull ? 0 : (decimal?)dr["themeCost"],
                                 totalCost = dr["totalCost"] is DBNull ? 0 : (decimal?)dr["totalCost"],
                                 totalInvoiceApproveBath = dr["totalInvoiceApproveBath"] is DBNull ? 0 : (decimal?)dr["totalInvoiceApproveBath"]

                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getApproveListsByEmpId >> " + ex.Message);
                return new List<Budget_Approve_Detail_Model.budgetForm>();
            }
        }

        public static List<Budget_Approve_Detail_Model.Budget_Approve_Detail_Att> getBudgetApproveId(string budgetApproveId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetApproveDetailList"
                 , new SqlParameter("@budgetApproveId", budgetApproveId));

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new Budget_Approve_Detail_Model.Budget_Approve_Detail_Att()
                              {
                                  id = d["regApproveId"].ToString(),
                                  budgetApproveId = d["budgetApproveId"].ToString(),
                              });
                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getBudgetActivityApprove => " + ex.Message);
                return new List<Budget_Approve_Detail_Model.Budget_Approve_Detail_Att>();
            }
        }

        public static List<Budget_Activity_Model.Budget_Invoice_history_Att> getBudgetInvoiceHistory(string activityId, string activityOfEstimateId, string budgetApproveId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetApproveInvoiceHistoryList"
                 , new SqlParameter("@activityId", activityId)
                 , new SqlParameter("@activityOfEstimateId", activityOfEstimateId)
                 , new SqlParameter("@budgetApproveId", budgetApproveId)
                 );

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new Budget_Activity_Model.Budget_Invoice_history_Att()
                              {
                                  activityNo = d["activityNo"].ToString(),
                                  activityTypeTheme = d["activityTypeTheme"].ToString(),
                                  productDetail = d["productDetail"].ToString(),
                                  invoiceNo = d["invoiceNo"].ToString(),

                                  productCostBath = d["productCostBath"].ToString() == "" ? 0 : decimal.Parse(d["productCostBath"].ToString()),
                                  productStandBath = d["productStandBath"].ToString() == "" ? 0 : decimal.Parse(d["productStandBath"].ToString()),
                                  invoiceTotalBath = d["invoiceTotalBath"].ToString() == "" ? 0 : decimal.Parse(d["invoiceTotalBath"].ToString()),
                                  productBalanceBath = d["productBalanceBath"].ToString() == "" ? 0 : decimal.Parse(d["productBalanceBath"].ToString()),

                                  invoiceType = d["invoiceType"].ToString(),
                                  row_type = d["row_type"].ToString(),
                                  invoiceActionDate = d["invoiceActionDate"] is DBNull ? null : (DateTime?)d["invoiceActionDate"],
                                  productBudgetStatusNameTH = d["productBudgetStatusNameTH"].ToString(),
                                  invoiceBudgetStatusNameTH = d["invoiceBudgetStatusNameTH"].ToString(),
                                  invoiceRemark = d["invoiceRemark"].ToString(),
                                  
                                  budgetActivityId = d["budgetActivityId"].ToString(),
                                  activityId = d["activityId"].ToString(),
                                  activitEstimateId = d["activitEstimateId"].ToString(),
                                  normalCost = d["normalCost"].ToString() == "" ? 0 : decimal.Parse(d["normalCost"].ToString()),
                                  themeCost = d["themeCost"].ToString() == "" ? 0 : decimal.Parse(d["themeCost"].ToString()),
                                  totalCost = d["totalCost"].ToString() == "" ? 0 : decimal.Parse(d["totalCost"].ToString()),

                                  invoiceId = d["invoiceId"].ToString(),
                                  invoiceSeq = d["invoiceSeq"].ToString() == "" ? 0 : int.Parse(d["invoiceSeq"].ToString()),
                                  invoiceBudgetStatusId = d["invoiceBudgetStatusId"].ToString() == "" ? 0 : int.Parse(d["invoiceBudgetStatusId"].ToString()),
                                  invoiceApproveStatusId = d["invoiceApproveStatusId"].ToString() == "" ? 0 : int.Parse(d["invoiceApproveStatusId"].ToString()),
                                  invoiceApproveStatusName = d["invoiceApproveStatusName"].ToString(),

                                  productId = d["productId"].ToString(),
                                  productBudgetStatusId = d["productBudgetStatusId"].ToString() == "" ? 0 : int.Parse(d["productBudgetStatusId"].ToString()),
                                  productCountInvoice = d["productCountInvoice"].ToString() == "" ? 0 : int.Parse(d["productCountInvoice"].ToString()),
                                  productSumInvoiceBath = d["productSumInvoiceBath"].ToString() == "" ? 0 : decimal.Parse(d["productSumInvoiceBath"].ToString()),

                                  total_act_product_cost = d["total_act_product_cost"].ToString() == "" ? 0 : decimal.Parse(d["total_act_product_cost"].ToString()),
                                  total_act_invoice = d["total_act_invoice"].ToString() == "" ? 0 : decimal.Parse(d["total_act_invoice"].ToString()),
                                  total_act_balance = d["total_act_balance"].ToString() == "" ? 0 : decimal.Parse(d["total_act_balance"].ToString()),
                                  
                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getBudgetInvoiceHistory => " + ex.Message);
                return new List<Budget_Activity_Model.Budget_Invoice_history_Att>();
            }
        }
    }

    public class QueryGetBudgetReport
    {
        public static List<Budget_Report_Model.Report_Budget_Activity_Att> getReportBudgetActivity(string act_StatusId, string act_activityNo, string companyEN,  string actYear)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_ReportBudgetActivity"
                 , new SqlParameter("@act_StatusId", act_StatusId)
                 , new SqlParameter("@act_activityNo", act_activityNo)
                 , new SqlParameter("@companyEN", companyEN)
                 , new SqlParameter("@actYear", actYear)
                 );

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new Budget_Report_Model.Report_Budget_Activity_Att()
                              {
                                  company = d["company"].ToString(),                            //company
                                  reportMMYY = d["reportMMYY"].ToString(),                      //เดือน-ปี
                                  act_activityNo = d["act_activityNo"].ToString(),              //เลขที่กิจกรรม
                                  sub_code = d["sub_code"].ToString(),                          //Sub Code

                                  claim_actStatus = d["claim_actStatus"].ToString(),            //เคลม Product
                                  claim_actValue = d["claim_actValue"].ToString(),              //เคลม(%)
                                  claim_actIO = d["claim_actIO"].ToString(),                    //เคลม IO

                                  product_IO = d["product_IO"].ToString(),                      //เลข IO
                                  act_activityName = d["act_activityName"].ToString(),          //ชื่อกิจกรรม
                                  brandName = d["brandName"].ToString(),                        //กลุ่มสินค้า
                                  act_theme = d["act_theme"].ToString(),                        //ประเภทกิจกรรม

                                  cus_regionName = d["cus_regionName"].ToString(),              //ภาค
                                  cus_regionDesc = d["cus_regionDesc"].ToString(),              //ชื่อภาค
                                  cus_cusNameTH = d["cus_cusNameTH"].ToString(),                //ลูกค้า

                                  act_reference = d["act_reference"].ToString(),                //reference
                                  prd_productDetail = d["prd_productDetail"].ToString(),        //รายละเอียด
                                  prd_productDetail50 = d["prd_productDetail50"].ToString(),    //รายละเอียด (50)

                                  activity_Period = d["activity_Period"].ToString(),            //ระยะเวลาการทำกิจกรรม
                                  activity_costPeriod = d["activity_costPeriod"].ToString(),    //ระยะเวลาการให้ทุน
                                  actCreatedDate = d["actCreatedDate"].ToString(),              //วันที่สร้างกิจกรรม

                                  //งบจัด
                                  activityTotalBath = d["activityTotalBaht"].ToString() == "" ? 0 : decimal.Parse(d["activityTotalBaht"].ToString()),
                                  //เพิ่มงบ
                                  activityTotalAddonBaht = d["activityTotalAddonBaht"].ToString() == "" ? 0 : decimal.Parse(d["activityTotalAddonBaht"].ToString()),
                                  //รวมงบจัด
                                  activityGrandTotalBaht = d["activityGrandTotalBaht"].ToString() == "" ? 0 : decimal.Parse(d["activityGrandTotalBaht"].ToString()),
                                  //งบจ่าย
                                  activityInvoiceTotalBath = d["activityInvoiceTotalBath"].ToString() == "" ? 0 : decimal.Parse(d["activityInvoiceTotalBath"].ToString()),
                                  //งบคงเหลือ
                                  activityBalanceBath = d["activityBalanceBath"].ToString() == "" ? 0 : decimal.Parse(d["activityBalanceBath"].ToString()),
                                  //งบจัด vs งบจ่าย
                                  activityCostRemainBath = d["activityCostRemainBath"].ToString() == "" ? 0 : decimal.Parse(d["activityCostRemainBath"].ToString()),

                                  productBudgetStatusNameTH = d["productBudgetStatusNameTH"].ToString(),    //สถานะ
                                  invoiceCreatedDate = d["invoiceCreatedDate"].ToString(),                  //วันที่ทำรายการ (ตัดจ่าย)
                                  act_status = d["act_status"].ToString(),                                  //สถานะกิจกรรม
                                  actForm_CreatedByUserId = d["act_createdByUserId"].ToString(),        //รหัสผู้สร้างกิจกรรม
                                  actForm_CreatedByName = d["act_createdByName"].ToString(),            //ชื่อผู้สร้างกิจกรรม
                                  budget_CurrentApproveName = d["budget_CurrentApproveName"].ToString(),    //ผู้อนุมัติ รออนุมัติ
                                  wait_activityInvoiceTotalBath = d["wait_activityInvoiceTotalBaht"].ToString(), //งบจ่ายจ่ายรออนุมัติ

                                  channelName = d["channelName"].ToString(),
                                  claim_shareStatus = d["claim_shareStatus"].ToString(),
                                  themeId = d["themeId"].ToString(),
                                  cus_id = d["cus_id"].ToString(),
                                  cus_regionId = d["cus_regionId"].ToString(),
                                  cus_cusNameEN = d["cus_cusNameEN"].ToString(),
                                  prd_typeId = d["prd_typeId"].ToString(),
                                  prd_groupId = d["prd_groupId"].ToString(),
                                  prd_productDetailCount = int.Parse(d["prd_productDetailCount"].ToString()),
                                  productBudgetStatusGroupId = d["productBudgetStatusGroupId"].ToString(),
                                  ProductBudgetStatusId = d["ProductBudgetStatusId"].ToString()
                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getReportBudgetActivity => " + ex.Message);
                return new List<Budget_Report_Model.Report_Budget_Activity_Att>();
            }
        }

        public static List<Budget_Report_Invoice_Model.Report_Budget_Invoice_Att> getReportBudgetInvoice(string companyEN, string beginDateYYYYMMDD, string toDateYYYYMMDD , string keyword)
        {
            try
            {

                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_ReportBudgetInvoice",
                 new SqlParameter("@companyShortName", companyEN),
                 new SqlParameter("@BeginYYYYMMDD", beginDateYYYYMMDD),
                 new SqlParameter("@ToYYYYMMDD", toDateYYYYMMDD),
                 new SqlParameter("@Keyword", SqlDbType.NVarChar, 250) { Value = keyword }
                );

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new Budget_Report_Invoice_Model.Report_Budget_Invoice_Att()
                              {
                                  row_no = int.Parse(d["row_no"].ToString()),
                                  report_date = d["report_date"].ToString(),
                                  customer_name = d["customer_name"].ToString(),
                                  customer_short_name = d["customer_short_name"].ToString(),
                                  invoice_number = d["invoice_number"].ToString(),
                                  invoice_action_date = d["invoice_action_date"].ToString(),
                                  invoice_create_date = d["invoice_create_date"].ToString(),
                                  invoice_send_approve_date = d["invoice_send_approve_date"].ToString(),
                                  bank_account_date = d["bank_account_date"].ToString(),
                                  act_gl = d["act_gl"].ToString(),
                                  vat_status = (bool)d["vat_status"],
                                  invoice_baht = d["invoice_baht"].ToString() == "" ? 0 : decimal.Parse(d["invoice_baht"].ToString()),
                                  invoice_vat_baht = d["invoice_vat_baht"].ToString() == "" ? 0 : decimal.Parse(d["invoice_vat_baht"].ToString()),
                                  invoice_wtax_baht = d["invoice_wtax_baht"].ToString() == "" ? 0 : decimal.Parse(d["invoice_wtax_baht"].ToString()),
                                  invoice_net_baht = d["invoice_net_baht"].ToString() == "" ? 0 : decimal.Parse(d["invoice_net_baht"].ToString()),
                                  wtx = d["wtx"].ToString(),
                                  sub_code = d["sub_code"].ToString(),
                                  commit_paper_status = (bool)d["commit_paper_status"],
                                  commit_paper_date = d["commit_paper_date"].ToString(),
                                  invoice_detail = d["invoice_detail"].ToString(),
                                  activityId = d["activityId"].ToString(),
                                  activityOfEstimateId = d["activityOfEstimateId"].ToString(),
                                  budgetActivityId = d["budgetActivityId"].ToString(),
                                  budgetActivityInvoiceId = d["budgetActivityInvoiceId"].ToString()
                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getReportBudgetInvoice => " + ex.Message);
                return new List<Budget_Report_Invoice_Model.Report_Budget_Invoice_Att>();
            }
        }

    }

    public class BudgetFormCommandHandler
    {
        //update Invoice Product
        public static int updateInvoiceProduct(Budget_Activity_Model.Budget_Activity_Invoice_Att model)
        {
            int result = 0;
            model.dateInvoiceAction = DateTime.ParseExact(model.invoiceActionDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            try
            {

                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetActivityInvoiceUpdate"
                    , new SqlParameter[] {new SqlParameter("@id", model.invoiceId)
                    ,new SqlParameter("@activityId",model.activityId)
                    ,new SqlParameter("@activityNo",model.activityNo)
                    ,new SqlParameter("@productId",model.productId)
                    ,new SqlParameter("@activityOfEstimateId",model.activityOfEstimateId)
                    ,new SqlParameter("@paymentNo",null)
                    ,new SqlParameter("@budgetImageId",model.budgetImageId)

                    ,new SqlParameter("@invoiceBudgetStatusId",model.invoiceBudgetStatusId)
                    ,new SqlParameter("@invoiceNo",model.invoiceNo)
                    ,new SqlParameter("@invoiceTotalBath",model.invoiceTotalBath)
                    ,new SqlParameter("@actionDate",model.dateInvoiceAction) //invoiceActionDate
                    ,new SqlParameter("@invoiceRemark",model.invoiceRemark)

                    ,new SqlParameter("@createdByUserId",UtilsAppCode.Session.User.empId)
                    ,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> usp_mtm_BudgetActivityInvoiceUpdate");
            }

            return result;
        }

        public static int insertInvoiceProduct(Budget_Activity_Model.Budget_Activity_Invoice_Att model)
        {

            int result = 0;
            model.dateInvoiceAction = DateTime.ParseExact(model.invoiceActionDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            try
            {

                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetActivityInvoiceInsert"
                    , new SqlParameter[] {new SqlParameter("@id", Guid.NewGuid().ToString())
                    ,new SqlParameter("@activityId",model.activityId)
                    ,new SqlParameter("@activityNo",model.activityNo)
                    ,new SqlParameter("@productId",model.productId)
                    ,new SqlParameter("@activityOfEstimateId",model.activityOfEstimateId)
                    ,new SqlParameter("@paymentNo",null)
                    ,new SqlParameter("@budgetImageId",model.budgetImageId)

                    ,new SqlParameter("@invoiceBudgetStatusId",model.invoiceBudgetStatusId)
                    ,new SqlParameter("@invoiceNo",model.invoiceNo)
                    ,new SqlParameter("@invoiceTotalBath",model.invoiceTotalBath)

					,new SqlParameter("@actionDate",model.dateInvoiceAction)
                    ,new SqlParameter("@invoiceRemark",model.invoiceRemark)
                    ,new SqlParameter("@createdByUserId",UtilsAppCode.Session.User.empId)
                    ,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> usp_mtm_BudgetActivityInvoiceInsert");
            }
            return result;
        }

        public static int commBudgetProductInvoiceDelete(string activityId, string estimateId, string invoiceId, string delType)
        {

            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetActivityInvoiceDelete"
                    , new SqlParameter[] {new SqlParameter("@activityId",activityId)
                    ,new SqlParameter("@activityOfEstimateId",estimateId)
                    ,new SqlParameter("@invoiceId",invoiceId)
                    ,new SqlParameter("@delType",delType)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> usp_mtm_BudgetActivityInvoiceDelete");
            }
            return result;
        }

        public static int commBudgetCompensateUpdate(string activityId, string compensateStatus, string compensateStartDate, string compensateEndDate)
        {

            int result = 0;

            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetActivityCompensateUpdate"
                    , new SqlParameter[] {new SqlParameter("@activityId",activityId)
                    ,new SqlParameter("@compensateStatus",compensateStatus)
                    ,new SqlParameter("@compensateStartDate",compensateStartDate)
                    ,new SqlParameter("@compensateEndDate",compensateEndDate)
                    ,new SqlParameter("@updateBy",UtilsAppCode.Session.User.empId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> commBudgetCompensateUpdate");
            }
            return result;
        }

        public static int deleteBudgetApproveByActNo(string activityNo)
        {

            int result = 0;

            try
            {

                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetApproveDelete"
                    , new SqlParameter[] {new SqlParameter("@activityNo",activityNo)
                    ,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> deleteBudgetApproveByActNo");
            }

            return result;
        }

    }

}