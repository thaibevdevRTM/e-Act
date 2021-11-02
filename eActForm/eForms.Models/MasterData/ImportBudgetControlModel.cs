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
        public List<BudgetControlModels> budgetReportList { get; set; }
        public List<BudgetControlModels> budgetReportChannelList { get; set; }
        public List<BudgetControl_LEModel> BudgetLEList { get; set; }
        public List<BudgetControl_ActType> bgActTypeList { get; set; }


        public ImportBudgetControlModel()
        {
            budgetControlModels = new BudgetControlModels();
            budgetReportList = new List<BudgetControlModels>();
            budgetReportChannelList = new List<BudgetControlModels>();
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
            public string dateStr { get; set; }
            public DateTime? date { get; set; }
            public string budNum { get; set; }
            public string b_Code { get; set; }
            public string bnamEng { get; set; }
            public string type { get; set; }
            public string budget_Activity { get; set; }
            public decimal? budget_Amount2 { get; set; }
            public string transaction { get; set; }
            public decimal? budgetAmount { get; set; }
            public string RefDoc { get; set; }
            public decimal? commitAmount { get; set; }
            public decimal? actual { get; set; }
            public decimal? accrued { get; set; }
            public decimal? commitment { get; set; }
            public decimal? PR_PO { get; set; }
            public decimal? prepaid { get; set; }
            public decimal? returnAmount { get; set; }
            public decimal? returnAmountBrand { get; set; }
            public string remark { get; set; }
            public string activityTypeId { get; set; }
            public string typeImport { get; set; }
            public string replaceEO { get; set; }
            public string activityNo { get; set; }
            public string approveNo { get; set; }
            public string orderNo { get; set; }
            public decimal? available { get; set; }
            public decimal? originalBudget { get; set; }
            public decimal? LE_Amount { get; set; }
            public decimal? approve_Amount { get; set; }
            public decimal? balanceLEApprove { get; set; }
            public decimal? trf_BG { get; set; }
            public decimal? actualTotal { get; set; }
            public double runingNo { get; set; }
            public string fiscalYear { get; set; }

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
