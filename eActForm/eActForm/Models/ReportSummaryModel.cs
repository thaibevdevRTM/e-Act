using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eActForm.Models
{
    public class ReportSummaryModels
    {

        public List<ReportSummaryModel> activitySummaryList { get; set; }
        public List<ReportSummaryModel> activitySummaryGroupList { get; set; }
        public List<ReportSummaryModel> activitySummaryGroupActivityList { get; set; }
        public ApproveFlowModel.approveFlowModel flowList { get; set; }
        public List<actApproveSummaryDetailModel> summaryDetailLists { get; set; }

        public ReportSummaryModels()
        {
            activitySummaryList = new List<ReportSummaryModel>();
        }


        public class ReportSummaryModel : ActBaseModel
        {
            public string id { get; set; }
            public string repDetailId { get; set; }
            public string customerId { get; set; }
            public string customerName { get; set; }
            public string activitySales { get; set; }
            public string activitySalesId { get; set; }
            public string activityId { get; set; }
            public string activityName { get; set; }
            public string productType { get; set; }
            public string productTypeId { get; set; }
            public string activityNo { get; set; }
            public string statusId { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? est { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? crystal { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? wranger { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? plus100 { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? jubjai { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? oishi { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? soda { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? water { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? total { get; set; }
            public string remark { get; set; }
        }

        public class actApproveSummaryDetailList
        {
            public List<actApproveSummaryDetailModel> summaryDetailLists { get; set; }
        }

        public class actApproveSummaryDetailModel : ActBaseModel
        {
            public string id { get; set; }
            public string statusId { get; set; }
            public string activityNo { get; set; }
            public string statusName { get; set; }
            public string summaryId { get; set; }
            public string productname { get; set; }
            public DateTime? startDate { get; set; }
            public DateTime? endDate { get; set; }
            public string customerId { get; set; }
            public string customerName { get; set; }
            public string productTypeId { get; set; }
            public string productTypeName { get; set; }
        }
    }
}