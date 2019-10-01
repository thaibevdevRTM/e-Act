﻿using eActForm.Models;
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
		public static List<Budget_Report_Model.Report_Budget_Activity_Att> getReportBudgetActivity(string act_StatusId, string act_activityNo, string companyEN ,string act_createdDateStart ,string act_createdDateEnd)
		{
			try
			{
				DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetReportActivity"
				 , new SqlParameter("@act_StatusId", act_StatusId)
				 , new SqlParameter("@act_activityNo", act_activityNo)
				 , new SqlParameter("@companyEN", companyEN)
				 , new SqlParameter("@createdDateStart", act_createdDateStart)
				 , new SqlParameter("@createdDateEnd", act_createdDateEnd)
				 );

				var result = (from DataRow d in ds.Tables[0].Rows
							  select new Budget_Report_Model.Report_Budget_Activity_Att()
							  {
								  company = d["company"].ToString(),
								  channelName = d["channelName"].ToString(),
								  act_activityNo = d["act_activityNo"].ToString(),
								  sub_code = d["sub_code"].ToString(),
								  act_activityName = d["act_activityName"].ToString(),
								  brandName = d["brandName"].ToString(),
								  Theme = d["Theme"].ToString(),
								  cus_cusNameTH = d["cus_cusNameTH"].ToString(),
								  prd_productDetail = d["prd_productDetail"].ToString(),
								  prd_productDetail50 = d["prd_productDetail50"].ToString(),
								  prd_productDetailCount = int.Parse(d["prd_productDetailCount"].ToString()),
								  activity_Period = d["activity_Period"].ToString(),
								  activity_costPeriod = d["activity_costPeriod"].ToString(),
								  actCreatedDate = d["actCreatedDate"].ToString(),
								  activityTotalBath = d["activityTotalBath"].ToString() == "" ? 0 : decimal.Parse(d["activityTotalBath"].ToString()),
								  activityBalanceBath = d["activityBalanceBath"].ToString() == "" ? 0 : decimal.Parse(d["activityBalanceBath"].ToString()),
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