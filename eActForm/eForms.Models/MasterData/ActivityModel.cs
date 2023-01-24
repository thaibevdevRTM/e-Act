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
        public string brandId { get; set; }
        public string channelId { get; set; }
        public string activityGroupId { get; set; }
        public string actType { get; set; }
        public string statusId { get; set; }
        public string activityNo { get; set; }
        public DateTime documentDate { get; set; }
        public DateTime activityPeriodSt { get; set; }
        public DateTime activityPeriodEnd { get; set; }
        public string activityName { get; set; }
        public string objective { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime createdByUserId { get; set; }
        public DateTime updatedDate { get; set; }
    }

}
