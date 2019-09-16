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

		public static List<TB_Bud_Activity_Model.Budget_Activity_Att> getBudgetActivity(string act_approveStatusId, string act_activityId, string act_activityNo,string budgetApproveId, string companyTH)
		{
			try
			{
				DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetActivity"
				 , new SqlParameter("@act_approveStatusId", act_approveStatusId)
				 , new SqlParameter("@act_activityId", act_activityId)
				 , new SqlParameter("@act_activityNo", act_activityNo)
				 , new SqlParameter("@budgetApproveId", budgetApproveId)
				 , new SqlParameter("@companyTH", companyTH)
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
				ExceptionManager.WriteError("getActivityByApproveStatusId => " + ex.Message);
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
								  budgetActivityId = d["budgetActivityId"].ToString(),
								  budgetApproveId = d["budgetApproveId"].ToString(),
							  });

				return result.ToList();
			}
			catch (Exception ex)
			{
				ExceptionManager.WriteError("getBudgetActivityLastApprove => " + ex.Message);
				return new List<Budget_Activity_Model.Budget_Activity_Last_Approve_Att>();
			}
		}


	}
}