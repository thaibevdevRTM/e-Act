﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eActForm.Models
{
    public class RepDetailModel
    {
        public class actFormRepDetails
        {
            public List<actFormRepDetailModel> actFormRepDetailLists { get; set; }
            public ApproveFlowModel.approveFlowModel flowList { get; set; }
            public List<actFormRepDetailModel> actFormRepDetailGroupLists { get; set; }
            public string typeForm { get; set; }
            public string setShowPDF { get; set; }
            public string dateReport { get; set; }

        }
        public class actFormRepDetailModel : Activity_Model.actForm
        {
            public string cusNameTH { get; set; }
            public string productId { get; set; }
            public string productName { get; set; }
            public string size { get; set; }
            public string typeTheme { get; set; }
            public decimal? normalSale { get; set; }
            public decimal? promotionSale { get; set; }
            public decimal? total { get; set; }
            public decimal? specialDisc { get; set; }
            public decimal? specialDiscBaht { get; set; }
            public decimal? promotionCost { get; set; }
            public decimal? compensate { get; set; }
            public string reference { get; set; }
            public decimal? perGrowth { get; set; }
            public decimal? perSE { get; set; }
            public decimal? perToSale { get; set; }
        }

        public class actApproveRepDetailModels
        {
            public List<actApproveRepDetailModel> repDetailLists { get; set; }
        }
        public class actApproveRepDetailModel : ActBaseModel
        {
            public string id { get; set; }
            public string statusId { get; set; }
            public string activityNo { get; set; }
            public string statusName { get; set; }
            public DateTime? startDate { get; set; }
            public DateTime? endDate { get; set; }
            public string customerId { get; set; }
            public string customerName { get; set; }
            public string productTypeId { get; set; }
            public string productTypeName { get; set; }
        }
    }
}