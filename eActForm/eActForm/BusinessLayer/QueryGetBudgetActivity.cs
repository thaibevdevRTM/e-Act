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
	public class QueryBudgetBiz
    {
        public static List<Budget_Activity_Model.Budget_Activity_Att> getBudgetActivity(string act_approveStatusId, string act_activityNo)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetActivity"
				 , new SqlParameter("@act_approveStatusId", act_approveStatusId)
				 , new SqlParameter("@act_activityNo", act_activityNo));

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new Budget_Activity_Model.Budget_Activity_Att()
                              {
                                  act_form_id = d["act_form_id"].ToString(),
                                  act_approveStatusId = int.Parse(d["act_approveStatusId"].ToString()),
                                  act_activityNo = d["act_activityNo"].ToString(),
                                  
                                  act_reference = d["act_reference"].ToString(),
                                  act_customerId = d["act_customerId"].ToString(),
                                  cus_cusShortName = d["cus_cusShortName"].ToString(),
                                  cus_cusNameEN = d["cus_cusNameEN"].ToString(),
                                  cus_cusNameTH = d["cus_cusNameTH"].ToString(),
                                  act_activityPeriodSt = !string.IsNullOrEmpty(d["act_activityPeriodSt"].ToString()) ? DateTime.Parse(d["act_activityPeriodSt"].ToString()) : (DateTime?)null,
                                  act_activityPeriodEnd = !string.IsNullOrEmpty(d["act_activityPeriodEnd"].ToString()) ? DateTime.Parse(d["act_activityPeriodEnd"].ToString()) : (DateTime?)null,
                                  act_costPeriodSt = !string.IsNullOrEmpty(d["act_costPeriodSt"].ToString()) ? DateTime.Parse(d["act_costPeriodSt"].ToString()) : (DateTime?)null,
                                  act_costPeriodEnd = !string.IsNullOrEmpty(d["act_costPeriodEnd"].ToString()) ? DateTime.Parse(d["act_costPeriodEnd"].ToString()) : (DateTime?)null,
                                  act_activityName = d["act_activityName"].ToString(),
                                  act_theme = d["act_theme"].ToString(),
                                  act_objective = d["act_objective"].ToString(),
                                  act_trade = d["act_trade"].ToString(),
                                  act_activityDetail = d["act_activityDetail"].ToString(),

                                  act_normalCost = d["act_normalCost"].ToString() == "" ? 0 : decimal.Parse(d["act_normalCost"].ToString()),
                                  act_themeCost = d["act_themeCost"].ToString() == "" ? 0 : decimal.Parse(d["act_themeCost"].ToString()),
                                  act_totalCost = d["act_totalCost"].ToString() == "" ? 0 : decimal.Parse(d["act_totalCost"].ToString()),

                                  //delFlag = bool.Parse(d["delFlag"].ToString()),
                                  act_createdDate = DateTime.Parse(d["act_createdDate"].ToString()),
                                  act_createdByUserId = d["act_createdByUserId"].ToString(),
                                  act_updatedDate = DateTime.Parse(d["act_updatedDate"].ToString()),
                                  act_updatedByUserId = d["act_updatedByUserId"].ToString(),

								  bud_ActivityStatusId = d["bud_ActivityStatusId"].ToString(),
								  bud_ActivityStatus = d["bud_ActivityStatus"].ToString(),

								  //inv_invoiceStatusID = d["inv_invoiceStatusID"].ToString() =="" ? "1" : d["inv_invoiceStatusID"].ToString(),
								  //inv_activityStatus = d["inv_activityStatus"].ToString()=="" ? "ค้างจ่าย" : d["inv_activityStatus"].ToString(),

							  });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getActivityByApproveStatusId => " + ex.Message);
                return new List<Budget_Activity_Model.Budget_Activity_Att>();
            }
        }

		public static List<Budget_Activity_Model.Budget_Activity_Product_Att> getBudgetActivityProduct(string act_activityID, string prd_productID, string inv_invoiceID)
		{
			try
			{
				DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetActivityProduct"
				 , new SqlParameter("@activityID", act_activityID)
				 , new SqlParameter("@productID", prd_productID)
				 , new SqlParameter("@invoiceID", inv_invoiceID));

				var result = (from DataRow d in ds.Tables[0].Rows
							  select new Budget_Activity_Model.Budget_Activity_Product_Att()
							  {
								  act_activityId = d["act_activityId"].ToString(),
								  act_activityNo = d["act_activityNo"].ToString(),
								  prd_productId = d["prd_productId"].ToString(),
								  act_typeTheme = d["act_typeTheme"].ToString(),
								  prd_productDetail = d["prd_productDetail"].ToString(),
								  normalCost = d["normalCost"].ToString() == "" ? 0 : decimal.Parse(d["normalCost"].ToString()),
								  themeCost = d["themeCost"].ToString() == "" ? 0 : decimal.Parse(d["themeCost"].ToString()),
								  totalCost = d["totalCost"].ToString() == "" ? 0 : decimal.Parse(d["totalCost"].ToString()),

								  invoiceId = d["invoiceId"].ToString(),
								  invoiceNo = d["invoiceNo"].ToString(),
								  invTotalBath = d["invTotalBath"].ToString() == "" ? 0 : decimal.Parse(d["invTotalBath"].ToString()),

								  productStandBath = d["productStandBath"].ToString() == "" ? 0 : decimal.Parse(d["productStandBath"].ToString()),
								  productBalanceBath = d["productBalanceBath"].ToString() == "" ? 0 : decimal.Parse(d["productBalanceBath"].ToString()),
								  invoiceProductStatusId = d["invoiceProductStatusId"].ToString(), /*สภานะเงินของรายการ product*/
								  invoiceProductStatusNameTH = d["invoiceProductStatusNameTH"].ToString(), /*สภานะเงินของรายการ product*/
								  invoiceSeq = d["invoiceSeq"].ToString() == "" ? 0 : int.Parse(d["invoiceSeq"].ToString()),
								  invActionDate = d["invActionDate"] is DBNull ? null : (DateTime?)d["invActionDate"],
								  //invActionDate = DateTime.Parse(d["invActionDate"].ToString()), /*วันที่ทำรายการ*/
								  //invActionDate = DateTime.ParseExact(d["invActionDate"].ToString(), "MM/dd/yyyy HH:mm:ss"),

								  //paymentNo = d["paymentNo"].ToString(),     /*ใบสำคัญจ่าย*/
								  //saleActCase = d["saleActCase"].ToString() == "" ? 0 : decimal.Parse(d["saleActCase"].ToString()),    /*ยอดขายช่วงทำกิจกรรม case*/
								  //saleActBath = d["saleActBath"].ToString() == "" ? 0 : decimal.Parse(d["saleActBath"].ToString()),    /*ยอดขายช่วงทำกิจกรรม bath*/
								  //invTotalBath = d["invTotalBath"].ToString() == "" ? 0 : decimal.Parse(d["invTotalBath"].ToString()),   /*จำนวนเงินจ่าย*/
								  //balanceBath = d["balanceBath"].ToString() == "" ? 0 : decimal.Parse(d["balanceBath"].ToString()),    /*ผลต่าง ดูก่อนอาจไม่เก็บใช้คำนวนแทน ถ้าเก็บน่าจะเก็บที่ระดับกิจกรรม*/

							  });

				return result.ToList();
			}
			catch (Exception ex)
			{
				ExceptionManager.WriteError("getActivityByApproveStatusId => " + ex.Message);
				return new List<Budget_Activity_Model.Budget_Activity_Product_Att>();
			}
		}


	}



}