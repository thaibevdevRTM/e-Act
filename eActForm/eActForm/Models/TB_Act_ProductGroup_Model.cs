﻿using System;

namespace eActForm.Models
{
    public class TB_Act_ProductGroup_Model
    {
        public string id { get; set; }
        public string groupName { get; set; }
        public string cateId { get; set; }
        public bool delFlag { get; set; }
        public DateTime createdDate { get; set; }
        public string createdByUserId { get; set; }
        public DateTime updatedDate { get; set; }
        public string updatedByUserId { get; set; }

    }
}