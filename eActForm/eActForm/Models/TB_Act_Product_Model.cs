using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace eActForm.Models
{
    public class TB_Act_Product_Model
    {
        public class ProductSmellModel : ActBaseModel
        {
            public string id { get; set; }
            public string productGroupId { get; set; }
            public string nameTH { get; set; }
            public string nameEN { get; set; }
        }
        public class Product_Model : ActBaseModel
        {
            public string id { get; set; }
            public string productCode { get; set; }
            public string productName { get; set; }
            public string productNameEN { get; set; }
            public string smellId { get; set; }
            public string smellname { get; set; }
            public string brandId { get; set; }
            public string cateId { get; set; }
            public string brand { get; set; }
            public int size { get; set; }
            public Int32 pack { get; set; }
            public Int32 unit { get; set; }
            public Int32 litre { get; set; }
            public string productCate { get; set; }
            public string productGroup { get; set; }
            public string groupId { get; set; }
        }

        public class ProductList
        {
            public List<Product_Model> productLists { get; set; }
        }
    }
}