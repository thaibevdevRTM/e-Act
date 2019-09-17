using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eActForm.Models
{
	public class TB_Bud_Activity_Model
	{

		public class Budget_Activity_Att
		{

			public string budget_id { get; set; }
			public string act_form_id { get; set; }
			public int act_approveStatusId { get; set; }
			public string act_activityNo { get; set; }

			[DataType(DataType.Date)]
			[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
			public DateTime? act_documentDate { get; set; }
			public string act_reference { get; set; }
			public string act_customerId { get; set; }

			public string act_companyEN { get; set; }

			public string cus_cusShortName { get; set; }
			public string cus_cusNameEN { get; set; }
			public string cus_cusNameTH { get; set; }

			public string ch_chanelCust { get; set; }
			public string ch_chanelGroup { get; set; }
			public string ch_chanelTradingPartner { get; set; }

			public string prd_groupName { get; set; }
			public string prd_groupNameTH { get; set; }
			public string prd_groupShort { get; set; }

			public string act_brandNameTH { get; set; }
			public string act_brandName { get; set; }
			public string act_shortBrand { get; set; }

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
			//public string act_themeName { get; set; }
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