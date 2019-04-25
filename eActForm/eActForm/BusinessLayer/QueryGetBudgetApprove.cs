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

		public static List<Budget_Activity_Model.Budget_Invoice_history_Att> getBudgetInvoiceHistory( string activityId)
		{
			try
			{
				DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetInvoiceHistory"
				 , new SqlParameter("@activityId", activityId));

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
								  invoiceTotalBath = d["invoiceTotalBath"].ToString() == "" ? 0 : decimal.Parse(d["invoiceTotalBath"].ToString()),

								  productBalanceBath = d["productBalanceBath"].ToString() == "" ? 0 : decimal.Parse(d["productBalanceBath"].ToString()),

								  productBudgetStatusId = d["productBudgetStatusId"].ToString() == "" ? 0 : int.Parse(d["productBudgetStatusId"].ToString()),
								  productBudgetStatusNameTH = d["productBudgetStatusNameTH"].ToString(),
								  invoiceActionDate = DateTime.Parse(d["invoiceActionDate"].ToString()),
								  //invoiceActionDate = d["invoiceActionDate"].ToString(),

								  invoiceBudgetStatusId = d["invoiceBudgetStatusId"].ToString() == "" ? 0 : int.Parse(d["invoiceBudgetStatusId"].ToString()),
								  invoiceBudgetStatusNameTH = d["invoiceBudgetStatusNameTH"].ToString(),
								  invoiceSeq = d["invoiceSeq"].ToString() == "" ? 0 : int.Parse(d["invoiceSeq"].ToString()),
								  productCountInvoice = d["productCountInvoice"].ToString() == "" ? 0 : int.Parse(d["productCountInvoice"].ToString()),
								  productSumInvoiceBath = d["productSumInvoiceBath"].ToString() == "" ? 0 : decimal.Parse(d["productSumInvoiceBath"].ToString()),

								  sum_cost_product_inv = d["sum_cost_product_inv"].ToString() == "" ? 0 : decimal.Parse(d["sum_cost_product_inv"].ToString()),
								  sum_total_invoice = d["sum_total_invoice"].ToString() == "" ? 0 : decimal.Parse(d["sum_total_invoice"].ToString()),
								  sum_balance_product_inv = d["sum_balance_product_inv"].ToString() == "" ? 0 : decimal.Parse(d["sum_balance_product_inv"].ToString()),

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