using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace eActForm.Models
{
    public class TB_Act_ProductPrice_Model
    {
        public class ProductPrice_Model
        {
            public string id { get; set; }
            public string productCode { get; set; }
            public string customerId { get; set; }
            public decimal price { get; set; }
            public DateTime startDate { get; set; }
            public DateTime endDate { get; set; }
            public string delFlag { get; set; }
            public DateTime createdDate { get; set; }
            public string createdBy { get; set; }
            public DateTime updatedDate { get; set; }
            public string updatedBy { get; set; }
        }

        

    }
}