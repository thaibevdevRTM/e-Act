﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eForms.Models.MasterData
{
    public class departmentMasterModel : DefaultFieldModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string companyId { get; set; }
    }
}