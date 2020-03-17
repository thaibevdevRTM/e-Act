﻿using System.Collections.Generic;

namespace eActForm.Models
{
    public class ApproveFlowModel
    {
        public class approveFlowModel
        {
            public flowApprove flowMain { get; set; }
            public List<flowApproveDetail> flowDetail { get; set; }
        }
        public class flowApprove : ActBaseModel
        {
            public string id { get; set; }
            public string flowNameTH { get; set; }
            public string cusNameTH { get; set; }
            public string cusNameEN { get; set; }
            public string nameTH { get; set; }
        }
        public class flowApproveDetail : ActBaseModel
        {
            public string id { get; set; }
            public string companyId { get; set; }
            public string flowId { get; set; }
            public int? rangNo { get; set; }
            public string empId { get; set; }
            public string empEmail { get; set; }
            public string empFNameTH { get; set; }
            public string empLNameTH { get; set; }
            public string empPositionTitleTH { get; set; }
            public string approveGroupId { get; set; }
            public string approveGroupName { get; set; }
            public string approveGroupNameEN { get; set; }
            public bool isShowInDoc { get; set; }
            public string description { get; set; }
            public string statusId { get; set; }
            public string remark;
            public string imgSignature { get; set; }
            public bool? isApproved { get; set; }
            public string bu { get; set; }
            public string buEN { get; set; }
            public string empFNameEN { get; set; }
            public string empLNameEN { get; set; }
            public string empPositionTitleEN { get; set; }
        }
    }
}