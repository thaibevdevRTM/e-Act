using System;
using System.Collections.Generic;

namespace eForms.Models.MasterData
{
    public class ActivityModel
    {
        public ActivityModels activityModels { get; set; }
        public List<TB_Act_ProductBrand_Model> brandList { get; set; }
        public List<TB_Act_Chanel_Model> chanelList { get; set; }
        public List<TB_Act_ActivityGroup_Model> activityGroupList { get; set; }

    }


    public class ActivityModels
    {
        public string activityId { get; set; }
        public string id { get; set; }
        public string statusName { get; set; }
        public string brandId { get; set; }
        public string brandName { get; set; }
        public string channelId { get; set; }
        public string channelName { get; set; }
        public string activityGroupId { get; set; }
        public string actType { get; set; }
        public string statusId { get; set; }
        public string EO { get; set; }
        public string IO { get; set; }
        public int? rowNo { get; set; }
        public string year { get; set; }
        public string productDetail { get; set; }
        public decimal? unitPrice { get; set; }
        public string activityNo { get; set; }
        public DateTime documentDate { get; set; }
        public DateTime activityPeriodSt { get; set; }
        public DateTime activityPeriodEnd { get; set; }
        public string activityName { get; set; }
        public string objective { get; set; }
        public DateTime createdDate { get; set; }
        public string createdByUserId { get; set; }
        public DateTime updatedDate { get; set; }
        public string updatedByUserId { get; set; }
        public decimal? amount { get; set; }
        public string budgetType { get; set; }
        public string yearBG { get; set; }
        public decimal? budgetTotal { get; set; }
        public decimal? useAmount { get; set; }
        public decimal? returnAmount { get; set; }
        public decimal? amountBalance { get; set; }

    }

}
