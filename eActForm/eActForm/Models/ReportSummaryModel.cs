using eActForm.BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eActForm.Models
{
    public class ReportSummaryModels
    {

        public List<ReportSummaryModel> activitySummaryList { get; set; }
        public List<ReportSummaryModel> activitySummaryGroupList { get; set; }
        public List<ReportSummaryModel> activitySummaryGroupActivityList { get; set; }
        public List<ReportSummaryModel> activitySummaryForecastList { get; set; }
        public ApproveFlowModel.approveFlowModel flowList { get; set; }
        public List<actApproveSummaryDetailModel> summaryDetailLists { get; set; }
        public List<ReportSummaryModel> activitySummaryFullList { get; set; }
        public string producttype_id { get; set; }
        public string subId { get; set; }
        public string cusId { get; set; }
        public ReportSummaryModels()
        {
            activitySummaryList = new List<ReportSummaryModel>();
            summaryDetailLists = new List<actApproveSummaryDetailModel>();
            activitySummaryForecastList = new List<ReportSummaryModel>();
            activitySummaryFullList = new List<ReportSummaryModel>();

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
            public string status { get; set; }
            public string brandId { get; set; }
            public string year { get; set; }
            public decimal? month { get; set; }
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
            public decimal? beer { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? changclassic { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? federbrau { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? archa { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? whitespirits { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? brownspirits { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? hongthong { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? blend285 { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? sangsom { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? oishiFood { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? readytodrink { get; set; }



            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? total { get; set; }
            public string remark { get; set; }
            public int rowNo { get; set; }
            public string companyId { get; set; }
        }

        public class actApproveSummaryDetailList
        {
            public List<actApproveSummaryDetailModel> summaryDetailLists { get; set; }
        }

        public class actApproveSummaryDetailModel : ActBaseModel
        {
            public actApproveSummaryDetailModel(string empId)
            {
                List<RequestEmpModel> model = QueryGet_empDetailById.getEmpDetailById(empId);
                if (model.Count > 0)
                {
                    this.createName = model.Count > 0 ? model[0].empName : "";
                }
            }
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
            public string createName { get; set; }
        }
    }
}