namespace eActForm.Models
{
    public class TB_Reg_FlowLimit_Model : ActBaseModel
    {
        public string id { get; set; }
        public string subjectId { get; set; }
        public string limitBegin { get; set; }
        public string limitTo { get; set; }
        public string displayVal { get; set; }
    }
}