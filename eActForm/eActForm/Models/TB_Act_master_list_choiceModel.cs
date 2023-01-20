namespace eActForm.Models
{
    public class TB_Act_master_list_choiceModel : ActBaseModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string nameEN { get; set; }
        public string sub_name { get; set; }
        public string type { get; set; }
        public string master_type_form_id { get; set; }
        public string orderNum { get; set; }
        public string displayType { get; set; }
        public string subDisplayType { get; set; }
        public string glCodeId { get; set; }

    }
}