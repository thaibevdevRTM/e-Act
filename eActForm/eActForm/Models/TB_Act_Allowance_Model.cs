using System;
using System.Collections.Generic;

namespace eActForm.Models
{

    public class TB_Act_Allowance_Model : ActBaseModel
    {
        public string id { get; set; }
        public string activityId { get; set; }
        public DateTime? date { get; set; }
        public bool chkPersonal { get; set; }
        public bool chkBreakfast { get; set; }
        public bool chkLunch { get; set; }
        public bool chkDinner { get; set; }
    }

    

}