using System;
using System.Collections.Generic;

namespace eActForm.Models
{
    public class HospitalModel
    {
        public string id { get; set; }
        public string hospNameEN { get; set; }
        public string hospNameTH { get; set; }
        public string provinceId { get; set; }
        public string provName { get; set; }
        public string region { get; set; }
        public string hospTypeId { get; set; }
        public string hospTypeName { get; set; }
        public int percentage { get; set; }
        public string delFlag { get; set; }
     

    }
}