using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace eActForm.Models
{
    public class TB_Act_Product_Cate_Model
    {
        public class Product_Cate_Model
        {
            public string id { get; set; }
            public string cateName { get; set; }
            public string productTypeId { get; set; }
            public bool delFlag { get; set; }
            public DateTime createdDate { get; set; }
            public string createdByUserId { get; set; }
            public DateTime updatedDate { get; set; }
            public string updatedByUserId { get; set; }
        }

     
    }
}