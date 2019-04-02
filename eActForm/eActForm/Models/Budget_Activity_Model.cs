using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
//using static eActForm.Models.TB_Bud_ActivityInvoice_Model;

namespace eActForm.Models
{
	public class Budget_Activity_Model
	{
		public List<Budget_Activity_Att> Budget_Activity_list { get; set; }
		public List<Budget_Activity_Status_Att> Budget_Activity_Ststus_list { get; set; }
		public List<Budget_Activity_Product_Att> Budget_Activity_Product_list { get; set; }
		public List<Budget_Activity_Invoice_Att> Budget_Activity_Invoice_list { get; set; }

		//public TB_Bud_ActivityInvoice_Model Bud_ActivityInvoice_Model { get; set; }
		//public List<Budget_Activity_Product_Att> Budget_Activity_Product_model { get; set; }

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
			public decimal invoiceTotalBath { get; set; }

			public decimal productBalanceBath { get; set; }
			public Int32 productBudgetStatusId { get; set; }
			public string productBudgetStatusNameTH { get; set; }

			[DataType(DataType.Date)]
			[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
			public DateTime? invoiceActionDate { get; set; }
			public Int32 invoiceBudgetStatusId { get; set; }
			public string invoiceBudgetStatusNameTH { get; set; }
			public Int32 invoiceSeq { get; set; }
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
			public decimal invTotalBath { get; set; }       /*จำนวนเงินจ่าย*/
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

		public class Budget_Activity_Att
		{
			public string act_form_id { get; set; }
			public int act_approveStatusId { get; set; }
			public string act_activityNo { get; set; }

			[DataType(DataType.Date)]
			[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
			public DateTime? act_documentDate { get; set; }

			public string act_reference { get; set; }
			public string act_customerId { get; set; }
			public string cus_cusShortName { get; set; }
			public string cus_cusNameEN { get; set; }
			public string cus_cusNameTH { get; set; }

			[DataType(DataType.Date)]
			[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
			public DateTime? act_activityPeriodSt { get; set; }

			[DataType(DataType.Date)]
			[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
			public DateTime? act_activityPeriodEnd { get; set; }

			[DataType(DataType.Date)]
			[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
			public DateTime? act_costPeriodSt { get; set; }

			[DataType(DataType.Date)]
			[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
			public DateTime? act_costPeriodEnd { get; set; }

			public string act_activityName { get; set; }
			public string act_theme { get; set; }
			public string act_objective { get; set; }
			public string act_trade { get; set; }
			public string act_activityDetail { get; set; }
			public string act_delFlag { get; set; }

			[DataType(DataType.Date)]
			[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
			public DateTime? act_createdDate { get; set; }

			[DataType(DataType.Date)]
			[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
			public DateTime? act_updatedDate { get; set; }

			public string act_createdByUserId { get; set; }
			public string act_updatedByUserId { get; set; }
			public decimal act_normalCost { get; set; }
			public decimal act_themeCost { get; set; }
			public decimal act_totalCost { get; set; }
			public decimal act_balance { get; set; } /*ผลต่าง*/
			public decimal act_total_invoive { get; set; }/*ยอดยกมา*/

			public string bud_ActivityStatusId { get; set; }
			public string bud_ActivityStatus { get; set; }

		}

	}
}