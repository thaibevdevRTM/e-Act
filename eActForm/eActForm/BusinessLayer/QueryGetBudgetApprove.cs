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

		public static List<Budget_Approve_Model.Budget_Approve_Att> getBudgetActivityApprove( string activityId)
		{
			try
			{
				DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetActivityApprove"
				 , new SqlParameter("@activityId", activityId));

				var result = (from DataRow d in ds.Tables[0].Rows
							  select new Budget_Approve_Model.Budget_Approve_Att()
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

							  });

				return result.ToList();
			}
			catch (Exception ex)
			{
				ExceptionManager.WriteError("getBudgetActivityApprove => " + ex.Message);
				return new List<Budget_Approve_Model.Budget_Approve_Att>();
			}
		}

	}
}