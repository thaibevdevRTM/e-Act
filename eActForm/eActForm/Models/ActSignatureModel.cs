using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        }
    }
}