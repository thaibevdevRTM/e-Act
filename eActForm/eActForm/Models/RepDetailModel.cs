using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eActForm.Models
{
    public class RepDetailModel
    {
        public class actFormRepDetails
        {
            public List<actFormRepDetail> repDetailLists { get; set; }
        }
        public class actFormRepDetail : Activity_Model.actForm
        {
            public string cusNameTH { get; set; }
            public string productId { get; set; }
            public string productName { get; set; }
            public string size { get; set; }
            public string typeTheme { get; set; }
            public string normalSale { get; set; }
            public string promotionSale { get; set; }
            public string total { get; set; }
        }
    }
}