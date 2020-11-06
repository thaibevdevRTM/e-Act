using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eForms.Models.MasterData
{
    public class tokenModel
    {
        public string id { get; set; }
        public string empId { get; set; }
        public string tokenAccess { get; set; }
        public string tokenType { get; set; }
        public DateTime? createDate { get; set; }
    }
}
