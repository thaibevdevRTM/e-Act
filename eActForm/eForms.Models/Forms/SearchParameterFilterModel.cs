using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eForms.Models.Forms
{
    public class searchParameterFilterModel
    {
        public bool isShowActNo { get; set; }
        public bool isShowStatus { get; set; }
        public bool isShowDate { get; set; }
        public bool isShowCustomer { get; set; }
        public bool isShowActType { get; set; }
        public bool isShowProductType { get; set; }
        public bool isShowProductGroup { get; set; }
        public bool isShowMonthText { get; set; }
        public searchParameterFilterModel()
        {
            isShowActNo = true;
            isShowStatus = true;
            isShowDate = true;
            isShowCustomer = true;
            isShowActType = true;
            isShowProductType = true;
            isShowProductGroup = true;
            isShowMonthText = true;
        }
    }
}
