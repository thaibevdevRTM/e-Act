using System;

namespace eActForm.Models
{
    public class Master_type_form_detail_Model
    {
        public string id { get; set; }
        public string master_type_form_id { get; set; }
        public string layout_type { get; set; }
        public int orderNo { get; set; }
        public string path_partial { get; set; }
        public string path_controller { get; set; }
        public string path_action { get; set; }
        public string function_name { get; set; }
        public Boolean delFlag { get; set; }
        public DateTime? createdDate { get; set; }
        public string createdByUserId { get; set; }
        public DateTime? updatedDate { get; set; }
        public string updatedByUserId { get; set; }
    }
}