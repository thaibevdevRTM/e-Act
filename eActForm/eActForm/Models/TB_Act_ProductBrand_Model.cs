using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eActForm.Models
{
    public class TB_Act_ProductBrand_Model : ActBaseModel
    {
        public string id { get; set; }
        public string brandName { get; set; }
        public string productGroupId { get; set; }
        public string digit_IO { get; set; }
        public string digit_EO { get; set; }
        public string no_tbmmkt { get; set; }
    }
}