using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eActForm.Models
{
    public class AdminUserModel
    {
        public class AdminUserModels
        {
            public List<string> custLi { get; set; }
            public List<string> chkProductType { get; set; }
            public List<string> chkRole { get; set; }
            
        }

        public class UserList
        {
            public string empId { get; set; }
            public string userName { get; set; }

        }
    }
}