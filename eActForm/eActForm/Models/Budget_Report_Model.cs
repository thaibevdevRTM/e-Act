﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static eActForm.Models.TB_Bud_Activity_Model;

namespace eActForm.Models
{
	public class Budget_Report_Model
	{
		public class Report_Budget_Activity
		{
			public List<Report_Budget_Activity_Att> Report_Budget_Activity_List { get; set; }
		}

		public class Report_Budget_Activity_Att
		{
			public string company { get; set; }
			public string channelName { get; set; }

			public string reportMMYY { get; set; }
			public string claim_actStatus { get; set; }
			public string claim_shareStatus { get; set; }
			public string claim_actValue { get; set; }
			public string claim_actIO { get; set; }
			public string product_IO { get; set; }
			
			public string act_activityNo { get; set; }
			public string sub_code { get; set; }
			public string act_activityName { get; set; }
			public string brandName { get; set; }
			public string themeId { get; set; }
			public string Theme { get; set; }

			public string cus_id { get; set; }
			public string cus_regionId { get; set; }
			public string cus_regionName { get; set; }
			public string cus_regionDesc { get; set; }
			public string cus_cusNameTH { get; set; }
			public string cus_cusNameEN { get; set; }
			
			public string prd_typeId { get; set; }
			public string prd_groupId { get; set; }

			public string prd_productDetail { get; set; }
			public string prd_productDetail50 { get; set; }
			public Int32 prd_productDetailCount { get; set; }
			public string activity_Period { get; set; }
			public string activity_costPeriod { get; set; }
			public string actCreatedDate { get; set; }

			[DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
			public decimal activityTotalBath { get; set; }

			[DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
			public decimal activityInvoiceTotalBath { get; set; }
			
			[DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
			public decimal activityBalanceBath { get; set; }

			public string productBudgetStatusGroupId { get; set; }
			public string ProductBudgetStatusId { get; set; }
			public string productBudgetStatusNameTH { get; set; }
			public string invoiceCreatedDate { get; set; }
			public string act_status { get; set; }
			public string actForm_CreatedByUserId { get; set; }
			public string actForm_CreatedByName { get; set; }

		}
	}
}