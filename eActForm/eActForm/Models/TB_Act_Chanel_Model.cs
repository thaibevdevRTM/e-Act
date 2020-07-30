using System;

namespace eActForm.Models
{
    public class TB_Act_Chanel_Model
    {

        public class Chanel_Model
        {
            public string id { get; set; }
            public string chanelGroup { get; set; }
            public string cust { get; set; }
            public string tradingPartner { get; set; }
            public string no_tbmmkt { get; set; }
            public string typeChannel { get; set; }
            public string delFlag { get; set; }
            public DateTime createdDate { get; set; }
            public string createdByUserId { get; set; }
            public DateTime updatedDate { get; set; }
            public string updatedByUserId { get; set; }
        }



    }
}