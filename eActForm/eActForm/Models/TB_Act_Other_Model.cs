using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eActForm.Models
{
    public class TB_Act_Other_Model
    {
        public string id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string displayVal { get; set; }
        public string subType { get; set; }
        public string val1 { get; set; }
        public string val2 { get; set; }
        public string sort { get; set; }
        public string delFlag { get; set; }
        public DateTime createdDate { get; set; }
        public string createdByUserId { get; set; }
        public DateTime updatedDate { get; set; }
        public string updatedByUserId { get; set; }


    }
}