using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eActForm.Models
{
    public class MainFormModel : Activity_TBMMKT_Model
    {
        public List<Master_type_form_detail_Model> master_Type_Form_Detail_Models { get; set; }
    }

}