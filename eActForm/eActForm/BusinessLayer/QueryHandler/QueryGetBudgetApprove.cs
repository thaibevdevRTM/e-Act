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
    public class QueryGetBudgetApprove
    {

        public static List<Budget_Approve_Detail_Model.budgetForm> getApproveListsByEmpId(string empId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getApproveBudgetByEmpId"
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
                                 //budgetApproveId = dr["budgetApproveId"].ToString(),
                                 approveId = dr["approveId"].ToString(),
                                 approveDetailId = dr["approveDetailId"].ToString(),

                                 //delFlag = (bool)dr["delFlag"],
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
                //throw new Exception("getApproveListsByStatusId >> " + ex.Message);
                ExceptionManager.WriteError("getApproveListsByStatusId >> " + ex.Message);
                return new List<Budget_Approve_Detail_Model.budgetForm>();
            }
        }

        public static List<Budget_Approve_Detail_Model.Budget_Approve_Detail_Att> getBudgetApproveId(string budgetApproveId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetApproveDetailByBudgetId"
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

        public static List<Budget_Activity_Model.Budget_Invoice_history_Att> getBudgetInvoiceHistory(string activityId, string budgetApproveId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetInvoiceHistory"
                 , new SqlParameter("@activityId", activityId)
                 , new SqlParameter("@budgetApproveId", budgetApproveId)
                 );

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new Budget_Activity_Model.Budget_Invoice_history_Att()
                              {
                                  budgetActivityId = d["budgetActivityId"].ToString(),
                                  activityId = d["activityId"].ToString(),
                                  activityNo = d["activityNo"].ToString(),
                                  activitEstimateId = d["activitEstimateId"].ToString(),
                                  productId = d["productId"].ToString(),
                                  activityTypeTheme = d["activityTypeTheme"].ToString(),
                                  productDetail = d["productDetail"].ToString(),

                                  normalCost = d["normalCost"].ToString() == "" ? 0 : decimal.Parse(d["normalCost"].ToString()),
                                  themeCost = d["themeCost"].ToString() == "" ? 0 : decimal.Parse(d["themeCost"].ToString()),
                                  totalCost = d["totalCost"].ToString() == "" ? 0 : decimal.Parse(d["totalCost"].ToString()),
                                  productStandBath = d["productStandBath"].ToString() == "" ? 0 : decimal.Parse(d["productStandBath"].ToString()),

                                  invoiceId = d["invoiceId"].ToString(),
                                  invoiceNo = d["invoiceNo"].ToString(),
                                  //invoiceCustomerId d["invoiceCustomerId"].ToString(),
                                  invoiceTotalBath = d["invoiceTotalBath"].ToString() == "" ? 0 : decimal.Parse(d["invoiceTotalBath"].ToString()),

                                  productBalanceBath = d["productBalanceBath"].ToString() == "" ? 0 : decimal.Parse(d["productBalanceBath"].ToString()),

                                  productBudgetStatusId = d["productBudgetStatusId"].ToString() == "" ? 0 : int.Parse(d["productBudgetStatusId"].ToString()),
                                  productBudgetStatusNameTH = d["productBudgetStatusNameTH"].ToString(),

                                  invoiceActionDate = d["invoiceActionDate"] is DBNull ? null : (DateTime?)d["invoiceActionDate"],
                                  //invoiceActionDate = DateTime.Parse(d["invoiceActionDate"].ToString()),
                                  //invoiceActionDate = d["invoiceActionDate"].ToString(),

                                  invoiceBudgetStatusId = d["invoiceBudgetStatusId"].ToString() == "" ? 0 : int.Parse(d["invoiceBudgetStatusId"].ToString()),
                                  invoiceBudgetStatusNameTH = d["invoiceBudgetStatusNameTH"].ToString(),
                                  invoiceSeq = d["invoiceSeq"].ToString() == "" ? 0 : int.Parse(d["invoiceSeq"].ToString()),
                                  productCountInvoice = d["productCountInvoice"].ToString() == "" ? 0 : int.Parse(d["productCountInvoice"].ToString()),
                                  productSumInvoiceBath = d["productSumInvoiceBath"].ToString() == "" ? 0 : decimal.Parse(d["productSumInvoiceBath"].ToString()),

                                  sum_cost_product_inv = d["sum_cost_product_inv"].ToString() == "" ? 0 : decimal.Parse(d["sum_cost_product_inv"].ToString()),
                                  sum_total_invoice = d["sum_total_invoice"].ToString() == "" ? 0 : decimal.Parse(d["sum_total_invoice"].ToString()),
                                  sum_balance_product_inv = d["sum_balance_product_inv"].ToString() == "" ? 0 : decimal.Parse(d["sum_balance_product_inv"].ToString()),

                                  invoiceApproveStatusId = d["invoiceApproveStatusId"].ToString() == "" ? 0 : int.Parse(d["invoiceApproveStatusId"].ToString()),
                                  invoiceApproveStatusName = d["invoiceApproveStatusName"].ToString(),

                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getBudgetActivityApprove => " + ex.Message);
                return new List<Budget_Activity_Model.Budget_Invoice_history_Att>();
            }
        }




    }
}