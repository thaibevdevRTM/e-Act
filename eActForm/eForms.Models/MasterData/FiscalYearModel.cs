using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eForms.Models.MasterData
{
    public class FiscalYearModel : DefaultFieldModel
    {
        public string id { get; set; }
        public string FromMonthYear { get; set; }
        public string ToMonthYear { get; set; }
        public string UseYear { get; set; }
        public string typePeriod{ get; set; }
    }
}