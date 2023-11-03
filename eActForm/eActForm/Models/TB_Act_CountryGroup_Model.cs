using System;
using System.Collections.Generic;

namespace eActForm.Models
{

    public class TB_Act_CountryModels : ActBaseModel
    {
        public string id { get; set; }
        public string country { get; set; }
        public string countryGroup { get; set; }
    }

    public class TB_Act_CountryDetailModels : ActBaseModel
    {
        public string id { get; set; }
        public string detail { get; set; }
        public string typeDay { get; set; }
        public int? min_level { get; set; }
        public int? max_level { get; set; }
        public string countryGroup { get; set; }
        public decimal? max_amount { get; set; }

    }


}