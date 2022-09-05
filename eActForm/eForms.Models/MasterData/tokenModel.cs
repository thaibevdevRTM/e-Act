using System;

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
