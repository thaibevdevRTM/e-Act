namespace eForms.Models.MasterData
{
    public class BaseMasterModel
    {
        public string id { get; set; }
        public string nameTH { get; set; }
        public string nameEN { get; set; }
    }

    public class TB_Act_Other_Model : DefaultFieldModel
    {
        public string id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string displayVal { get; set; }
        public string subType { get; set; }
        public string val1 { get; set; }
        public string val2 { get; set; }
        public string sort { get; set; }
        public string glCode { get; set; }

    }
}
