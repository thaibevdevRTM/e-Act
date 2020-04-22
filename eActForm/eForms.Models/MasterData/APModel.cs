namespace eForms.Models.MasterData
{
    public class APModel : DefaultFieldModel
    {
        public string id { get; set; }
        public string APCode { get; set; }
        public string Name1 { get; set; }
        public string CoNo { get; set; }
        public string HouseNo { get; set; }
        public string Street { get; set; }
        public string Street4 { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Tel { get; set; }
        public string FaxNo { get; set; }
        public string FullAddress { get; set; }
        public string TelAndFax { get; set; }
    }
}