using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eActForm.Models
{
    public class TB_Act_master_cost_centerModel : ActBaseModel
    {
        public string id { get; set; }
        public string costCenter { get; set; }
        public string productBrandId { get; set; }
        public string companyId { get; set; }
    }
}