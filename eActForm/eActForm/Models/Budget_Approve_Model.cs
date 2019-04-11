using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static eActForm.Models.TB_Budget_Activity_Model;

namespace eActForm.Models
{
	public class Budget_Approve_Model
	{

		public List<Budget_Approve_Att> Budget_Approve_list { get; set; }
		public List<Budget_Activity_Att> Budget_Activity_list { get; set; }

		//public Budget_Approve_Att Budget_Approve_Form { get; set; }

		public class Budget_Approve_Att
		{
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

			[DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
			public decimal productSumInvoiceBath { get; set; }

		}

	}
}