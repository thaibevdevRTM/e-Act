namespace eActForm.Models
{
    public class TB_Reg_FlowModel : ActBaseModel
    {
        public string id { get; set; }
        public string subjectId { get; set; }
        public string companyId { get; set; }
        public string customerId { get; set; }
        public string productCatId { get; set; }
        public string productTypeId { get; set; }
        public string flowLimitId { get; set; }
        public string channelId { get; set; }
        public string productBrandId { get; set; }

    }
}