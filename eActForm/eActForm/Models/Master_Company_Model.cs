using System;

namespace eActForm.Models
{
    public class Master_Company_Model
    {
        public string id { get; set; }
        public string companyId { get; set; }
        public string companyNameEN { get; set; }
        public string companyNameTH { get; set; }
        public Boolean delFlag { get; set; }
        public DateTime? createDate { get; set; }
        public string createBy { get; set; }
        public DateTime? updateDate { get; set; }
        public string updateBy { get; set; }
    }
}