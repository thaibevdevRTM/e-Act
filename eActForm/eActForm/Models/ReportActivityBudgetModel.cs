using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eActForm.Models
{
    public class ReportActivityBudgetModels
    {

        public List<ReportActivityBudgetModel> activityBudgetList { get; set; }

        public ReportActivityBudgetModels()
        {
            activityBudgetList = new List<ReportActivityBudgetModel>();
        }

        public class ReportActivityBudgetModel
        {

            public string id { get; set; }
            public string customerId { get; set; }
            public string customerName { get; set; }
            public string activitySales { get; set; }
            public string activityId { get; set; }
            [DisplayFormat(DataFormatString = "{0:0.##}", ApplyFormatInEditMode = true)]
            public decimal? est { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? crystal { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? wranger { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? plus100 { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? jubjai { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? oishi { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? soda { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? water { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? total { get; set; }
            public string remark { get; set; }
        }
    }
}