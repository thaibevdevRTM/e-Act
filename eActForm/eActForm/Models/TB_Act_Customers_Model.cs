using System;

namespace eActForm.Models
{
    public class TB_Act_Customers_Model
    {
        public class Customers_Model
        {
            public string id { get; set; }
            public string cusTrading { get; set; }
            public string companyId { get; set; }
            public string customerId { get; set; }
            public string cusNameTH { get; set; }
            public string cusNameEN { get; set; }
            public string cusShortName { get; set; }
            public string cust { get; set; }
            public string chanel_Id { get; set; }
            public string regionId { get; set; }
            public string region { get; set; }
            public string delFlag { get; set; }
            public DateTime createdDate { get; set; }
            public string createdByUserId { get; set; }
            public DateTime updatedDate { get; set; }
            public string updatedByUserId { get; set; }
        }
    }
}