namespace eActForm.Models
{
    public class TB_Act_master_material_Model : ActBaseModel
    {
        public string id { get; set; }
        public string plnt { get; set; }
        public string material { get; set; }
        public string materialDescription { get; set; }
        public string sloc { get; set; }
        public int qty { get; set; }
        public string tB_Act_master_list_choice_id { get; set; }
        public string tB_Act_master_list_choice_id_InOutStock { get; set; }
        public string qtyName { get; set; }

    }
}