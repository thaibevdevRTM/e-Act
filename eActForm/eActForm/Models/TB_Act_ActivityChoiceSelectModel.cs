﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eActForm.Models
{
    public class TB_Act_ActivityChoiceSelectModel : ActBaseModel
    {
        public string id { get; set; }
        public string actFormId { get; set; }
        public string select_list_choice_id { get; set; }  
    }
}