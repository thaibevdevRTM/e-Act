using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eActForm.Models
{
    public class TB_Act_master_list_choiceModel : ActBaseModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string sub_name { get; set; }
        public string type { get; set; }
        public string master_type_form_id { get; set; }     
    }
}