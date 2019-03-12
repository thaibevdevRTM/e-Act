using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eActForm.Models;
namespace eActForm.Models
{
    public class ApproveModel
    {
        public class approveModels
        {
            public approveModel approveModel { get; set; }
            public List<approveDetailModel> approveDetailLists { get; set; }
            public List<approveStatus> approveStatusLists { get; set; }
            public List<ApproveFlowModel.flowApproveDetail> approveFlowDetail { get; set; }
        }
        public class approveModel : ActBaseModel
        {
            public string id { get; set; }
            public string flowId { get; set; }
            public string actFormId { get; set; }
            public bool isPermisionApprove { get; set; } // current login can be approve
        }
        public class approveDetailModel : ActBaseModel
        {
            public string id { get; set; }
            public string approveId { get; set; }
            public int? rangNo { get; set; }
            public string ImgName { get; set; }
            public string empId { get; set; }
            public string empName { get; set; }
            public string statusId { get; set; }
            public string statusName { get; set; }
            public bool? isSendEmail { get; set; }
            public string remark { get; set; }
            public byte[] signature { get; set; }
        }
        public class approveStatus : ActBaseModel
        {
            public string id { get; set; }
            public string nameTH { get; set; }
            public string nameEN { get; set; }
            public string description { get; set; }
        }
    }
}