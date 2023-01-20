using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eActForm.Models
{
    public class TB_Act_AmountBudget : ActBaseModel
    {
        public string id { get; set; }
        public string EO { get; set; }
        public string activityId { get; set; }
        public decimal? budgetTotal { get; set; }
        public decimal? useAmount { get; set; }
        public decimal? returnAmount { get; set; }
        public decimal? amountBalance { get; set; }
        public string activityType { get; set; }
        public string typeShowBudget { get; set; }
        public string yearBG { get; set; }
        public string brandName { get; set; }

    }


}