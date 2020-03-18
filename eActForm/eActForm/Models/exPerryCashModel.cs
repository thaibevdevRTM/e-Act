using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eActForm.Models
{
    public class exPerryCashModels
    {
        public exPerryCashModel exPrettyModel {get;set;}
        public List<exPerryCashModel> exPrettyModelList { get; set; }

        public exPerryCashModels()
        {
            exPrettyModelList = new List<exPerryCashModel>();
        }
    }



    public class exPerryCashModel : ActBaseModel
    {
        public string id { get; set; }
        public string actNo { get; set; }
        public string detail { get; set; }
        public string status { get; set; }
        public string cashLimitId { get; set; }
        public string cashName { get; set; }
        public string positionId { get; set; }
        public string positionName { get; set; }
        public decimal? cash { get; set; }
        public string empId { get; set; }
        public decimal? rulesCash { get; set; }
        public string subject { get; set; }
        public DateTime? startDate { get; set; }
        public string activityNo { get; set; }
        public string createName { get; set; }
        public DateTime? monthCash { get; set; }
        public string createby { get; set; }
        public DateTime? createDate { get; set; }

    }





}