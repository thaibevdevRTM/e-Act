using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eActForm.Models
{
    public class DashBoardModel
    {
        public class infoDashBoardModels
        {
            public List<infoDashBoardModel> dashBoardLists { get; set; }
            public List<infoGroupCustomerSpending> customerSpendingLists { get; set; }
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
    }
}