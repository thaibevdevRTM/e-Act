using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eActForm.Models
{
    public class AdminUserModel
    {

        public List<User> userLists { get; set; }
        public List<User> userRoleLists { get; set; }
        public List<Customer> customerLists { get; set; }
        
        public AdminUserModel()
        {
            userLists = new List<User>();
        }


        public class AdminUserModels
        {
            public List<string> custLi { get; set; }
            public List<string> chkProductType { get; set; }
            public List<string> chkRole { get; set; }

        }

        public class User
        {
            public string empId { get; set; }
            public string userName { get; set; }
            public string userLName { get; set; }
            public string teamName { get; set; }
            public string roleId { get; set; }
            public string companyId { get; set; }
        }

        public class Customer
        {
            public string cusId { get; set; }
            public string customerName { get; set; }
            public string productTypeId { get; set; }
            public string companyId { get; set; }

        }
    }
}