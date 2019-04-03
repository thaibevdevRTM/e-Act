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
            public List<actFormRepDetailModel> actFormRepDetailLists { get; set; }
        }
        public class actFormRepDetailModel : Activity_Model.actForm
        {
            public string cusNameTH { get; set; }
            public string productId { get; set; }
            public string productName { get; set; }
            public string size { get; set; }
            public string typeTheme { get; set; }
            public string normalSale { get; set; }
            public string promotionSale { get; set; }
            public decimal? total { get; set; }
            public decimal? specialDisc { get; set; }
            public decimal? specialDiscBaht { get; set; }
            public decimal? promotionCost { get; set; }
        }
    }
}