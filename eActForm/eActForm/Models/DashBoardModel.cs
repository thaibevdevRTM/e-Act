using System.Collections.Generic;

namespace eActForm.Models
{
    public class DashBoardModel
    {
        public class infoDashBoardModels
        {
            public List<infoDashBoardModel> dashBoardLists { get; set; }
            public List<infoGroupCustomerSpending> customerSpendingLists { get; set; }
            public List<infoSumSpendindOfTheYear> sumSpendingOfYear { get; set; }
        }
        public class infoDashBoardModel
        {
            public int countCreated { get; set; }
            public int countWaitingApprove { get; set; }
            public int countApprove { get; set; }
            public int countOverSLA { get; set; }

        }

        public class infoMonthTotalSpending
        {
            public string monthDate { get; set; }
            public int sumTotal { get; set; }
        }

        public class infoGroupCustomerSpending
        {
            public string customerId { get; set; }
            public string customerName { get; set; }
            public decimal? sumSpending { get; set; }
        }

        public class infoSumSpendindOfTheYear
        {
            public decimal? sumAlcoholSpending { get; set; }
            public decimal? sumNonAlcoholSpending { get; set; }
            public decimal? sumSpending { get; set; }
        }
    }
}