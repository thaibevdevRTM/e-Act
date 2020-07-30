namespace eActForm.Models
{
    public class TB_Reg_Subject_Model : ActBaseModel
    {
        public string id { get; set; }
        public string companyId { get; set; }
        public string nameTH { get; set; }
        public string nameEn { get; set; }
        public string description { get; set; }
        public string typeFormId { get; set; }
    }
}