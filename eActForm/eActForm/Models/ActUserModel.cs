using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eActForm.Models
{
    public class ActUserModel
    {
        public class RequestUsreAPI
        {
            public string username { get; set; }
            public string password { get; set; }
            public RequestUsreAPI(string user, string pass)
            {
                username = user;
                password = pass;
            }
        }
        public class ResponseUserAPI
        {
            public string code { get; set; }
            public string mess { get; set; }
            public List<User> userModel { get; set; }
        }
        public class User
        {

            public string empId { get; set; }
            public string empStatus { get; set; }
            public string empPrefix { get; set; }
            public string empGender { get; set; }
            public string empFNameEN { get; set; }
            public string empLNameEN { get; set; }
            public string empFNameTH { get; set; }
            public string empLNameTH { get; set; }
            public string empDateofBirth { get; set; }
            public string empMaritalStatus { get; set; }
            public string empNationality { get; set; }
            public string empPosition { get; set; }
            public string empPositionTitleEN { get; set; }
            public string empPositionTitleTH { get; set; }
            public string empLocalPositionEN { get; set; }
            public string empLocalPositionTH { get; set; }
            public string empExternalPositionEN { get; set; }
            public string empExternalPositionTH { get; set; }
            public string empPositionLevel { get; set; }
            public string empPositionRange { get; set; }
            public string empLevel { get; set; }
            public string empClass { get; set; }
            public string empType { get; set; }
            public string empCategory { get; set; }
            public string empCompanyGroup { get; set; }
            public string empCompanyId { get; set; }
            public string empCompanyName { get; set; }
            public string empDivisionEN { get; set; }
            public string empDivisionTH { get; set; }
            public string empDepartmentEN { get; set; }
            public string empDepartmentTH { get; set; }
            public string empManagerId { get; set; }
            public string empEmail { get; set; }
            public bool isCreator { get; set; }
            public bool isApprove { get; set; }
            public bool isAdmin { get; set; }
            public bool isSuperAdmin { get; set; }
        }
    }
}