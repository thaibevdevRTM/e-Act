using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eActForm.Models
{
    public class SentKafkaLogModel
    {
        public string empId { get; set; }
        public string activityId { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public DateTime? createdDate { get; set; }
        public string path { get; set; }
        public string statusKafka { get; set; }
        public string massage { get; set; }

        public SentKafkaLogModel(string p_empId , string p_activityId, string p_status, string p_type, DateTime? p_createdDate, string p_path, string p_statusKafka, string massage)
        {
            this.empId = p_empId;
            this.activityId = p_activityId;
            this.status = p_status;
            this.type = p_type;
            this.createdDate = p_createdDate;
            this.path = p_path;
            this.statusKafka = p_statusKafka;
            this.massage = massage;
        }
    }
}