using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eActForm.Models
{
	public class TB_Bud_ActivityInvoice_Model
	{

		public class Bud_ActivityInvoice_Model
		{
			public string id { get; set; }
			public string activityId { get; set; }
			public string productId { get; set; }

			public string paymentNo { get; set; }
			public string invoiceNo { get; set; }
			public decimal saleActCase { get; set; }
			public decimal saleActBath { get; set; }
			public decimal invTotalBath { get; set; }

			[DataType(DataType.Date)]
			[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
			public DateTime? actionDate { get; set; }
			public Int32 invoiceProductStatusId { get; set; }
			public Int32 invoiceSeq { get; set; }
			public Boolean delFlag { get; set; }

			public DateTime createdDate { get; set; }
			public string createdByUserId { get; set; }
			public DateTime updatedDate { get; set; }
			public string updatedByUserId { get; set; }

		}

	}
}