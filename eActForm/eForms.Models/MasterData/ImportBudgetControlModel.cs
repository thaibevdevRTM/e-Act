﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace eForms.Models.MasterData
{
    public class ImportBudgetControlModel
    {
        public class BudgetControlModels
        {
            public string id { get; set; }
            public string budgetNo { get; set; }
            public string EO { get; set; }
            public string budgetGroupType { get; set; }
            public string customerId { get; set; }
            public string chanelId { get; set; }
            public string chanelName { get; set; }
            public string brandId { get; set; }
            public string brandName { get; set; }
            public string companyId { get; set; }
            public DateTime? startDate { get; set; }
            public DateTime? endDate { get; set; }
            public decimal? amountTT { get; set; }
            public decimal? amountCVM { get; set; }
            public decimal? amountMT { get; set; }
            public decimal? amountONT { get; set; }
            public decimal? amountSSC { get; set; }
            public string description { get; set; }
            public List<HttpPostedFileBase> InputFiles { get; set; }

        }

        public class BudgetControl_LEModel
        {
            public string id { get; set; }
            public string budgetId { get; set; }
            public DateTime? startDate { get; set; }
            public DateTime? endDate { get; set; }
            public decimal? amount { get; set; }
            public string descripion { get; set; }

        }

        public class BudgetControl_ActType
        {
            public string id { get; set; }
            public string budgetId { get; set; }
            public string budgetLEId { get; set; }
            public string actTypeId { get; set; }
            public decimal? amount { get; set; }
            public string description { get; set; }

        }

    }
}
