using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static eActForm.Models.TB_Act_Customers_Model;
using static eActForm.Models.TB_Act_Product_Cate_Model;
using static eActForm.Models.TB_Act_Product_Model;

namespace eActForm.Models
{
    public class Activity_TBMMKT_Model : Activity_Model
    {
        public TB_Act_ActivityForm_DetailOther TB_Act_ActivityForm_DetailOther { get; set; }
    }

    public class ActivityFormTBMMKT : ActivityForm
    {

    }

    public class TB_Act_ActivityForm_DetailOther
    {
        public string Id { get; set; }
        public string activityNo { get; set; }
        public string activityProduct { get; set; }
        public string activityTel { get; set; }
        public string EO { get; set; }
        public Boolean delFlag { get; set; }
        public DateTime? createdDate { get; set; }
        public string createdByUserId { get; set; }
        public DateTime? updatedDate { get; set; }
        public string updatedByUserId { get; set; }
    }

}