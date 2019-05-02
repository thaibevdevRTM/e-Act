using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eActForm.Models
{
    public class DashBoardModel
    {
        public class infoDashBoardModels
        {
            public List<infoDashBoardModel> dashBoardLists { get; set; }
        }
            public class infoDashBoardModel
        {
            public int countCreated { get;  set; }
            public int countWaitingApprove { get; set; }
            public int countApprove { get; set; }
            public int countOverSLA { get; set; }

        }
    }
}