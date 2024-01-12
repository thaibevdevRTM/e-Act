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
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.Globalization;

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
                DateTime? date = BaseAppCodes.converStrToDatetimeWithFormat(docDate, ConfigurationManager.AppSettings["formatDateUse"]);
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




        public static List<TB_Act_CountryModels> getCountry()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getCountry");
                if (ds.Tables.Count > 0)
                {
                    var lists = (from DataRow dr in ds.Tables[0].Rows
                                 select new TB_Act_CountryModels()
                                 {
                                     id = dr["id"].ToString(),
                                     country = dr["country"].ToString(),
                                     countryGroup = dr["countryGroup"].ToString(),
                                 }).ToList();
                    return lists;
                }
                else
                {
                    return new List<TB_Act_CountryModels>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("getCountry >>" + ex.Message);
            }
        }


        public static async System.Threading.Tasks.Task<EXG_Rate_Model> api_ExchangeRate(DateTime? conDate)
        {

            EXG_Rate_Model eXG_Rate_Model = new EXG_Rate_Model();
            try
            {
                
                switch (conDate.Value.DayOfWeek.ToString().ToLower())
                {
                    case "saturday":
                        conDate = conDate.Value.AddDays(-1);
                        break;
                    case "sunday":
                        conDate = conDate.Value.AddDays(-2);
                        break;

                }

                string st_date = conDate.Value.ToString("yyyy-MM-dd");

                EXG_Rate_Model responseModel = new EXG_Rate_Model();   
                string urlExchange = "https://apigw1.bot.or.th/bot/public/Stat-ExchangeRate/v2/DAILY_AVG_EXG_RATE/";
                string urlParameters = string.Format("?start_period={0}&end_period={1}&currency=USD", st_date, st_date,"USD");
                // "?start_period=2023-09-25&end_period=2023-09-25";

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

                if(responseModel.result.data.data_detail.Any())
                {
                    foreach(var item in responseModel.result.data.data_detail)
                    {
                        if(!string.IsNullOrEmpty(item.selling))
                        {
                            eXG_Rate_Model = responseModel;
                        }
                        else
                        {
                            eXG_Rate_Model = await api_ExchangeRate(conDate.Value.AddDays(-1));
                        }
                    }
           
                }



                return eXG_Rate_Model;

            }
            catch (Exception ex)
            {
                throw new Exception("api_ExchangeRate >> " + ex.Message);
            }
        }

        public static async System.Threading.Tasks.Task<TB_Act_CountryDetailModels> callGetAllowance(string countryId,string Lvl,string typeDay)
        {
            TB_Act_CountryDetailModels tB_Act_CountryDetailModels = new TB_Act_CountryDetailModels();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getCountryDetail"
                 , new SqlParameter[] { new SqlParameter("@countryId", countryId),
                      new SqlParameter("@Lvl", Lvl),
                 new SqlParameter("@type", typeDay)});
                if (ds.Tables.Count > 0)
                {
                    var lists = (from DataRow dr in ds.Tables[0].Rows
                                 select new TB_Act_CountryDetailModels()
                                 {
                                     id = dr["id"].ToString(),
                                     max_amount = decimal.Parse(dr["max_amount"].ToString()),
                                     countryGroup = dr["countryGroup"].ToString(),
                                 }).ToList();

                    return lists.ToList().FirstOrDefault();
                }


                return tB_Act_CountryDetailModels;
            }
            catch (Exception ex)
            {
                throw new Exception("getCountry >>" + ex.Message);
            }
        }



    }
}