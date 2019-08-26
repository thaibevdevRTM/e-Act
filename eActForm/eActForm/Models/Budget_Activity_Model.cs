using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static eActForm.Models.TB_Bud_Activity_Model;
//using static eActForm.Models.Budget_Approve_Detail_Model;

namespace eActForm.Models
{
	public class Budget_Activity_Model
	{

		public List<TB_Bud_Image_Model.BudImageModel> Budget_ImageList { get; set; }

		public List<Budget_Activity_Att> Budget_Activity_list { get; set; }
		public List<Budget_Activity_Status_Att> Budget_Activity_Ststus_list { get; set; }
		public List<Budget_Activity_Product_Att> Budget_Activity_Product_list { get; set; }
		public List<Budget_Activity_Invoice_Att> Budget_Activity_Invoice_list { get; set; }
		public List<Budget_Invoice_history_Att> Budget_Invoce_History_list { get; set; }
		public List<Budget_Approve_Detail_Model> Budget_Approve_Detail_list { get; set; }

		public Budget_Activity_Product_Att Budget_Activity_Product { get; set; }
		public Budget_Activity_Invoice_Att Budget_Activity_Invoice { get; set; }

		public class Budget_Invoice_history_Att
		{
			public string budgetId { get; set; }
			public string budgetActivityId { get; set; }
			public string activityId { get; set; }
			public string activityNo { get; set; }
			public string activityName { get; set; }
			public string activitEstimateId { get; set; }
			public string activityTypeTheme { get; set; }

			public string productId { get; set; }
			public string productDetail { get; set; }

			[DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
			public decimal normalCost { get; set; }

			[DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
			public decimal themeCost { get; set; }

			[DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
			public decimal totalCost { get; set; }

			[DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
			public decimal productStandBath { get; set; }

			public string invoiceId { get; set; }
			public string invoiceNo { get; set; }

			[DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
			public decimal invoiceTotalBath { get; set; }

			[DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
			public decimal productBalanceBath { get; set; }

			public Int32 productBudgetStatusId { get; set; }
			public string productBudgetStatusNameTH { get; set; }

			//[DataType(DataType.Date)]
			//[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
			public DateTime? invoiceActionDate { get; set; }
			//public string invoiceActionDate { get; set; }

			public Int32 invoiceBudgetStatusId { get; set; }
			public string invoiceBudgetStatusNameTH { get; set; }
			public Int32 invoiceSeq { get; set; }
			public Int32 productCountInvoice { get; set; }

			public Int32 invoiceApproveStatusId { get; set; }
			public string  invoiceApproveStatusName { get; set; }

			[DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
			public decimal productSumInvoiceBath { get; set; }
			[DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
			public decimal sum_cost_product_inv { get; set; }
			[DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
			public decimal sum_total_invoice { get; set; }
			[DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
			public decimal sum_balance_product_inv { get; set; }

		}

		public class Budget_Activity_Invoice_Att
		{

			public string invoiceId { get; set; }
			public string activityId { get; set; }
			public string activityNo { get; set; }
			public string activityOfEstimateId { get; set; }
			public string activityTypeTheme { get; set; }

			public string productId { get; set; }
			public string productDetail { get; set; }
			public decimal normalCost { get; set; }
			public decimal themeCost { get; set; }
			public decimal totalCost { get; set; }
			public decimal productStandBath { get; set; }

			public string paymentNo { get; set; }
			public string invoiceNo { get; set; }
			public decimal saleActCase { get; set; }
			public decimal saleActBath { get; set; }

			[DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
			public decimal invoiceTotalBath { get; set; }

			public decimal productBalanceBath { get; set; }
			public Int32 productBudgetStatusId { get; set; }
			public string productBudgetStatusNameTH { get; set; }

			[DataType(DataType.Date)]
			[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
			public DateTime? dateInvoiceAction { get; set; }
			public String invoiceActionDate { get; set; }
			//public string invoiceActionDate { get; set; }
			public Int32 invoiceBudgetStatusId { get; set; }
			public string invoiceBudgetStatusNameTH { get; set; }
			public Int32 invoiceSeq { get; set; }

			public Int32 invoiceApproveStatusId { get; set; }
			public string invoiceApproveStatusName { get; set; }
			public string approveInvoiceId { get; set; }
			public string budgetApproveId { get; set; }

			public Boolean delFlag { get; set; }
			public DateTime createdDate { get; set; }
			public string createdByUserId { get; set; }
			public DateTime updatedDate { get; set; }
			public string updatedByUserId { get; set; }
		}

		public class Budget_Activity_Product_Att
		{
			//usp_getBudgetActivityProduct for Query Biz
			public string act_activityId { get; set; }
			public string act_activityNo { get; set; }
			public string prd_productId { get; set; }
			public string activityOfEstimateId { get; set; }
			public string act_typeTheme { get; set; }
			public string prd_productDetail { get; set; }
			public decimal normalCost { get; set; }
			public decimal themeCost { get; set; }
			public decimal totalCost { get; set; }
			//public string invoiceId { get; set; }
			//public string invoiceNo { get; set; }
			public decimal invoiceTotalBath { get; set; }       /*จำนวนเงินจ่าย*/
			//public decimal productStandBath { get; set; } /*ยอดยกมา มันก็เท่ากับผลต่างไง*/
			public decimal productBalanceBath { get; set; } /*ผลต่าง*/
			public string budgetStatusId { get; set; }
			public string budgetStatusNameTH { get; set; }
			
			//public Int32 invoiceSeq { get; set; }

			//public string invoiceNo { get; set; }
			//public Int32 invoiceSeq { get; set; }
			//public string paymentNo { get; set; }		/*ใบสำคัญจ่าย*/
			//public decimal saleActCase { get; set; }	/*ยอดขายช่วงทำกิจกรรม case*/
			//public decimal saleActBath { get; set; }	/*ยอดขายช่วงทำกิจกรรม bath*/
			//public decimal invTotalBath { get; set; }	/*จำนวนเงินจ่าย*/
			//public decimal balanceBath { get; set; }	/*ผลต่าง*/

			[DataType(DataType.Date)]
			[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
			public DateTime? invActionDate { get; set; } /*วันที่ทำรายการ*/

			//public string invoiceBudgetStatusId { get; set; }
			//public string invoiceBudgetStatusNameTH { get; set; }

		}

		public class Budget_Activity_Status_Att
		{
			public string id { get; set; }
			public string nameEN { get; set; }
			public string nameTH { get; set; }
			public string description { get; set; }
			public Boolean delFlag { get; set; }
						
			[DataType(DataType.Date)]
			[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
			public DateTime? createdDate { get; set; }

			[DataType(DataType.Date)]
			[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
			public DateTime? updatedDate { get; set; }

			public string createdByUserId { get; set; }
			public string updatedByUserId { get; set; }
		}

		public class Budget_Product_Status_Att
		{
			public string id { get; set; }
			public string nameEN { get; set; }
			public string nameTH { get; set; }
			public string description { get; set; }
		}

	}
}