﻿using eActForm.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    public class AuthenAppCode
    {
        public static ActUserModel.ResponseUserAPI doAuthen(string strUser, string strPass)
        {
            try
            {
                ActUserModel.ResponseUserAPI response = null;
                ActUserModel.RequestUsreAPI request = new ActUserModel.RequestUsreAPI(strUser, strPass);
                string rtn = OkHTTP.SendPost(ConfigurationManager.AppSettings["urlAuthen"], JsonConvert.SerializeObject(request).ToString());
                JObject json = JObject.Parse(rtn);
                if (json.Count > 0)
                {
                    response = JsonConvert.DeserializeObject<ActUserModel.ResponseUserAPI>(rtn);
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("doAuthen >> " + ex.Message);
            }
        }
    }
}