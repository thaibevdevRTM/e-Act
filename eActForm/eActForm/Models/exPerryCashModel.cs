﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eActForm.Models
{
    public class exPerryCashModel
    {
        public string cashLimitId { get; set; }
        public string cashName { get; set; }
        public string positionId { get; set; }
        public string positionName { get; set; }
        public decimal? cash { get; set; }
        public string empId { get; set; }
        public decimal? rulesCash { get; set; }

    }


}