using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eForms.Models.MasterData;
using static eForms.Models.MasterData.ImportBudgetControlModel;

namespace eActForm.Models
{
    public class ImportBudgetControlModel
    {
        public List<TB_Act_Other_Model> companyList { get; set; }
        public BudgetControlModels budgetControlModels { get; set; }
        public List<BudgetControlModels> budgetReportList { get; set; }
        public List<BudgetControlModels> budgetReportList2 { get; set; }
        public List<BudgetControlModels> budgetReportChannelList { get; set; }
        public List<BudgetControl_LEModel> BudgetLEList { get; set; }
        public List<BudgetControl_ActType> bgActTypeList { get; set; }
        public string typeImport { get; set; }
        public ImportBudgetControlModel()
        {
            budgetControlModels = new BudgetControlModels();
            budgetReportList = new List<BudgetControlModels>();
            budgetReportChannelList = new List<BudgetControlModels>();
            budgetReportList2 = new List<BudgetControlModels>();
        }
    }
}