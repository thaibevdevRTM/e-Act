﻿using System.Collections.Generic;

namespace eActForm.Models
{
    public class ActSignatureModel
    {
        public class SignModels
        {
            public List<SignModel> lists { get; set; }
        }
        public class SignModel : ActBaseModel
        {
            public string id { get; set; }
            public string empId { get; set; }
            public byte[] signature { get; set; }
            public string empName { get; set; }
            public string empCompanyName { get; set; }
            public string positionTitle { get; set; }
        }
    }
}