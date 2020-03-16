using System;
using System.Collections.Generic;

namespace eActForm.Models
{

    public class DocumentsModel
    {
        public class actRepDetailModels
        {
            public List<actRepDetailModel> actRepDetailLists { get; set; }
        }
        public class actRepDetailModel : ActBaseModel
        {
            public string id { get; set; }
            public string statusId { get; set; }
            public string activityNo { get; set; }
            public string statusName { get; set; }
            public string customerId { get; set; }
            public string customerName { get; set; }
            public string productTypeId { get; set; }
            public string ProductTypeName { get; set; }
            public DateTime? startDate { get; set; }
            public DateTime? endDate { get; set; }
            public string typeForm { get; set; }
        }
    }
}