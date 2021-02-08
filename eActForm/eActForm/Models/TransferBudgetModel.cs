using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eActForm.Models
{
    public class TransferBudgetModel
    {
        public TransferBudgetModels transferBudgetModels { get; set; }
    }

    public class TransferBudgetModels
    {
        public string activityId { get; set; }
        public decimal? total { get; set; }
        public decimal? paymentBlance { get; set; }
    }
}