using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eForms.Models.MasterData
{
    public class SalesTeamCVMModel : BaseMasterModel
    {
        public string provinceId { get; set; }
        public string saleTeamId { get; set; }
        public string telContact { get; set; }
        public string telManager { get; set; }
        public string emailContact { get; set; }
        public string emailManager { get; set; }
        public string address { get; set; }
        public string nameManager { get; set; }
        public string nameContact { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string nameCashier { get; set; }
        public string telCashier { get; set; }
        public string emailCashier { get; set; }
    }
}
