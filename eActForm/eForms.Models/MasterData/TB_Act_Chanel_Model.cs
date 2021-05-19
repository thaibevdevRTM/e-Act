using System;

namespace eForms.Models.MasterData
{
    public class TB_Act_Chanel_Model : DefaultFieldModel
    {

        public string id { get; set; }
        public string chanelGroup { get; set; }
        public string cust { get; set; }
        public string tradingPartner { get; set; }
        public string no_tbmmkt { get; set; }
        public string typeChannel { get; set; }
        public string brandCode { get; set; }
    }
}