using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace eForms.Models.MasterData
{
    public class ImportBudgetControlModel
    {
        public List<TB_Act_Other_Model> companyList { get; set; }
        public BudgetControlModels budgetControlModels { get; set; }

        public ImportBudgetControlModel()
        {
            budgetControlModels = new BudgetControlModels();
        }
        public class BudgetControlModels : DefaultFieldModel
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
            public string startDateStr { get; set; }
            public DateTime? startDate { get; set; }
            public DateTime? endDate { get; set; }
            public string endDateStr { get; set; }
            public decimal? amount { get; set; }
            public decimal? balance { get; set; }
            public decimal? amountTotal { get; set; }
            public decimal? totalChannel { get; set; }
            public decimal? totalBG { get; set; }
            public string description { get; set; }
            public int LE { get; set; }
            public decimal? reserve { get; set; }
            public decimal? reserveTotal { get; set; }
            public decimal? balanceTotal { get; set; }

            public decimal? totalBudgetChannel { get; set; }
            public List<HttpPostedFileBase> InputFiles { get; set; }

        }

        public class chanelBudgetModel
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class BudgetControl_LEModel : DefaultFieldModel
        {
            public string id { get; set; }
            public string budgetId { get; set; }
            public DateTime? startDate { get; set; }
            public DateTime? endDate { get; set; }
            public string descripion { get; set; }

        }

        public class BudgetControl_ActType : DefaultFieldModel
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
