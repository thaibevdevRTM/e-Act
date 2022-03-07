﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eForms.Models.MasterData
{
    public class TB_Act_Area_Model : DefaultFieldModel
    {
        public string id { get; set; }
        public string companyId { get; set; }
        public string region { get; set; }
        public string area { get; set; }
        public string province { get; set; }
        public string condition { get; set; }

    }
}
