using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eForms.Models.MasterData
{
    public class EXG_Rate_Model
    {
        public EXG_Rate_Models result { get; set; }
    }

    public class RequestEXG_Rate
    {
        public string start_period { get; set; }
        public string end_period { get; set; }
        public RequestEXG_Rate(string start, string end)
        {
            start_period = start;
            end_period = end;
        }
    }

    public class EXG_Rate_Models
    {
        public string timestamp { get; set; }
        public string api { get; set; }
        public Data_Model data { get; set; }
    }

    public class Data_Model
    {
        public List<Data_Detail_Model> data_detail { get; set; }
    }

    public class Data_Detail_Model
    {
        public DateTime? period { get; set; }
        public string currency_id { get; set; }
        public string currency_name_eng { get; set; }
        public string buying_sight { get; set; }
        public string buying_transfer { get; set; }
        public string selling { get; set; }
        public string mid_rate { get; set; }
    }
}
