using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.UI.WebControls.WebParts;

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
            public string reportDetailId { get; set; }
        }
        public class actFormRepDetailModel : ActBaseModel
        {
            public string cusNameTH { get; set; }
            public string productId { get; set; }
            public string productName { get; set; }
            public string size { get; set; }
            public string typeTheme { get; set; }
            public decimal? normalSale { get; set; }
            public decimal? promotionSale { get; set; }
            public decimal? total { get; set; }
            public decimal? totalCase { get; set; }
            public decimal? specialDisc { get; set; }
            public decimal? specialDiscBaht { get; set; }
            public decimal? promotionCost { get; set; }
            public decimal? compensate { get; set; }
            public decimal? perGrowth { get; set; }
            public decimal? perSE { get; set; }
            public decimal? perToSale { get; set; }
            public decimal? wholeSalesPrice { get; set; }
            public decimal? saleIn { get; set; }
            public decimal? saleOut { get; set; }
            public decimal? discount1 { get; set; }
            public decimal? discount2 { get; set; }
            public decimal? discount3 { get; set; }
            public decimal? normalGp { get; set; }
            public decimal? promotionGp { get; set; }
            public decimal? rsp { get; set; }
            public string unitTxt { get; set; }
            public int rowNo { get; set; }
            public string id { get; set; }
            public string statusId { get; set; }
            public string statusName { get; set; }
            public string statusNameEN { get; set; }
            public string languageDoc { get; set; }
            public string activityNo { get; set; }
            public DateTime? documentDate { get; set; }
            public string reference { get; set; }
            public string productCateId { get; set; }
            public string productGroupid { get; set; }
            public string customerId { get; set; }
            public string channelName { get; set; }
            public string productTypeId { get; set; }
            public string productTypeNameEN { get; set; }
            public string cusShortName { get; set; }
            public string productCategory { get; set; }
            public string productGroup { get; set; }
            public string groupName { get; set; }
            public DateTime? activityPeriodSt { get; set; }
            public DateTime? activityPeriodEnd { get; set; }
            public DateTime? costPeriodSt { get; set; }
            public DateTime? costPeriodEnd { get; set; }
            public string activityName { get; set; }
            public string theme { get; set; }
            public string objective { get; set; }
            public string trade { get; set; }
            public string activityDetail { get; set; }
            public decimal? normalCost { get; set; }
            public decimal? themeCost { get; set; }

            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? totalCost { get; set; }
            public decimal? perTotal { get; set; }
            public string createByUserName { get; set; }
            public string regionId { get; set; }
            public string brandId { get; set; }
            public string master_type_form_id { get; set; }
            public DateTime? dateSentApprove { get; set; }
            public string companyId { get; set; }
            public string brandName { get; set; }
            public string channelId { get; set; }
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