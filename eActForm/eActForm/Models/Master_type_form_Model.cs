using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eActForm.Models
{
    public class Master_type_form_Model
    {
        public string id { get; set; }
        public string nameForm { get; set; }
        public string nameForm_EN { get; set; }
        public string department { get; set; }
        public string useIn { get; set; }
        public string companyId { get; set; }
        public Boolean delFlag { get; set; }
        public DateTime? createdDate { get; set; }
        public string createdByUserId { get; set; }
        public DateTime? updatedDate { get; set; }
        public string updatedByUserId { get; set; }
    }
}