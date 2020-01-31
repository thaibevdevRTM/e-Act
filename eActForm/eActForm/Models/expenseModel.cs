using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eActForm.Models
{
    public class exPerryCasheModel
    {
        public string actId { get; set; }
        public string companyId { get; set; }
        public string objective { get; set; }
        public DateTime exMonth { get; set; }
        public string dear { get; set; }
        public string detail { get; set; }
        public decimal total { get; set; }
        public List<exDetail> exDetailList { get; set; }

        public exPerryCasheModel()
        {
            exDetailList = new List<exDetail>();
        }
    }

    public class exDetail
    {
        public DateTime exDate { get; set; }
        public string place { get; set; }
        public string objective { get; set; }
        public decimal price { get; set; }
        public string remark { get; set; }
    }



}