﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eActForm.Models
{
    public class TB_Act_ActivityGroup_Model
    {
        public string id { get; set; }
        public string activitySales { get; set; }
        public string activityAccount { get; set; }
        public string gl { get; set; }
        public bool delFlag { get; set; }
        public DateTime createdDate { get; set; }
        public string createdByUserId { get; set; }
        public DateTime updatedDate { get; set; }
        public string updatedByUserId { get; set; }

    }
}