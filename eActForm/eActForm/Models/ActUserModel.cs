﻿using System.Collections.Generic;

namespace eActForm.Models
{
    public class ActUserModel
    {
        public class UserAuthorized
        {
            public string empId { get; set; }
            public string regionId { get; set; }
            public string customerId { get; set; }
            public string productTypeId { get; set; }
            public string productCateId { get; set; }
            public string companyId { get; set; }
        }
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
        public class RequestUserInfoAPI
        {
            public string userName { get; set; }
            public string keyToken { get; set; }
            public string tokenType { get; set; }
            public RequestUserInfoAPI(string user, string token,string tokenType)
            {
                userName = user;
                keyToken = token;
                this.tokenType = tokenType;
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
            public string empCompanyNameTH { get; set; }
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
            public bool isAdminOMT { get; set; }
            public bool isAdminBeer { get; set; }
            public bool isAdminTBM { get; set; }
            public bool isAdminHCM { get; set; }
            public bool isAdminNUM { get; set; }
            public bool isAdminPOM { get; set; }
            public bool isAdminChangInter { get; set; }
            public bool isAdminCVM { get; set; }
            public bool isAdminHCBP { get; set; }
            public bool isCreatorMT_OMT { get; set; }
            public string countWatingActFormBeer { get; set; }
            public string countWatingActForm { get; set; }
            public string counteatingRepDetail { get; set; }
            public string counteatingSummaryDetail { get; set; }
            public string countWatingBudgetForm { get; set; }
            public string countApproveReject { get; set; }
            public string countRejectAct { get; set; }
            public string exception { get; set; }
            public string regionId { get; set; }
            public string regionNum { get; set; }
            public string customerId { get; set; }
            public string empProbationEndDate { get; set; }
            public string empGroupName { get; set; }
            public string empGroupNameTH { get; set; }
            public string tokenAccess { get; set; }
            public string tokenType { get; set; }
            public string DepartmentName { get; set; }
            public string DepartmentNameTH { get; set; }
        }
    }
}