using System;
using System.Collections.Generic;

namespace eActForm.Models
{
    public class TB_Act_ProductPrice_Model
    {

        public List<ProductPrice> ProductPriceList { get; set; }

        public TB_Act_ProductPrice_Model()
        {
            ProductPriceList = new List<ProductPrice>();
        }


        public class ProductPrice : ActBaseModel
        {
            public string id { get; set; }
            public string productCode { get; set; }
            public string productName { get; set; }
            public string customerId { get; set; }
            public string customerName { get; set; }
            public decimal price { get; set; }
            public decimal? normalCost { get; set; }
            public decimal? wholeSalesPrice { get; set; }
            public decimal? discount1 { get; set; }
            public decimal? discount2 { get; set; }
            public decimal? discount3 { get; set; }
            public decimal? saleNormal { get; set; }
            public DateTime startDate { get; set; }
            public DateTime endDate { get; set; }

        }



    }
}