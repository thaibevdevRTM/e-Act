using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebLibrary;
using eForms.Models.MasterData;
using System.Net.Http.Headers;
using System.Net.Http;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Numeric;
using System.Web.Mvc;
using System.Security.Cryptography.Xml;
using System.Runtime.InteropServices.ComTypes;

namespace eActForm.BusinessLayer.Appcodes
{
    public class expensesEntertainAppCode
    {
        public static List<exPerryCashModel> getLimitByEmpId(string empId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getLimitByEmpId"
                    , new SqlParameter[] { new SqlParameter("@empId", empId) });
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var lists = (from DataRow dr in ds.Tables[0].Rows
                                 select new exPerryCashModel("")
                                 {
                                     id = dr["idCashType"].ToString(),
                                     cashName = dr["displayVal"].ToString(),
                                     cash = dr["cash"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(dr["cash"].ToString())),
                                 }).ToList();
                    return lists;
                }
                else
                {
                    return new List<exPerryCashModel>();
                }


            }
            catch (Exception ex)
            {
                throw new Exception("getLimitByEmpId >>" + ex.Message);
            }
        }

        public static List<exPerryCashModel> getAmountLimitByEmpId(string empId, string docDate)
        {
            try
            {
                DateTime date = BaseAppCodes.converStrToDatetimeWithFormat(docDate, ConfigurationManager.AppSettings["formatDateUse"]);
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_exGetAmountLimitByEmpId"
                    , new SqlParameter[] { new SqlParameter("@empId", empId),
                      new SqlParameter("@docDate", date)});
                if (ds.Tables.Count > 0)
                {
                    var lists = (from DataRow dr in ds.Tables[0].Rows
                                 select new exPerryCashModel("")
                                 {
                                     cash = dr["total"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(dr["total"].ToString())),
                                     cashTypeId = dr["productId"].ToString(),
                                 }).ToList();
                    return lists;
                }
                else
                {
                    return new List<exPerryCashModel>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("getAmountLimitByEmpId >>" + ex.Message);
            }
        }



        public static async System.Threading.Tasks.Task<EXG_Rate_Model> api_ExchangeRate(string date)
        {
            try
            {
                EXG_Rate_Model responseModel = new EXG_Rate_Model();   
                string urlExchange = "https://apigw1.bot.or.th/bot/public/Stat-ExchangeRate/v2/DAILY_AVG_EXG_RATE/";
                string urlParameters = "?start_period=2023-09-25&end_period=2023-09-25";

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(urlExchange);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-IBM-Client-Id", "ec5040dd-ebe7-45c3-9d14-00fa8c1c4cb1");


                HttpResponseMessage response = client.GetAsync(urlParameters).Result;
                var jsonString = response.Content.ReadAsStringAsync();
                jsonString.Wait();
                responseModel = JsonConvert.DeserializeObject<EXG_Rate_Model>(jsonString.Result);

                client.Dispose();


                return responseModel;

            }
            catch (Exception ex)
            {
                throw new Exception("api_ExchangeRate >> " + ex.Message);
            }
        }
    }
}