using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace eActForm.BusinessLayer
{
    public class DocumentsAppCode
    {
        public static List<DocumentsModel.actRepDetailModel> getActRepDetailLists(DateTime startDate, DateTime endDate, string typeForm)
        {
            try
            {
                string stored = "";
                if (typeForm == Activity_Model.activityType.MT.ToString())
                {
                    stored = "usp_GetActivityRepDetailAll";
                }
                else if (typeForm == Activity_Model.activityType.OMT.ToString())
                {
                    stored = "usp_GetActivityRepDetailOMTAll";
                }
                else if (typeForm == Activity_Model.activityType.MT_AddOn.ToString())
                {
                    stored = "usp_GetActivityRepDetailAddOn";
                }
                else if (typeForm == Activity_Model.activityType.OMT_AddOn.ToString())
                {
                    stored = "usp_GetActivityRepDetailAddOn_OMT";
                }
                else
                {
                    stored = "usp_GetActivityRepDetailSetPriceAll";
                }
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, stored
                    , new SqlParameter[] {
                        new SqlParameter("@startDate",startDate)
                        ,new SqlParameter("@endDate",endDate.AddDays(1))
                    });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new DocumentsModel.actRepDetailModel()
                             {
                                 id = dr["id"].ToString(),
                                 statusId = dr["statusId"].ToString(),
                                 activityNo = dr["activityNo"].ToString(),
                                 statusName = dr["statusName"].ToString(),
                                 customerId = dr["customerId"].ToString(),
                                 customerName = dr["customerName"].ToString(),
                                 productTypeId = dr["productTypeId"].ToString(),
                                 ProductTypeName = dr["ProductTypeName"].ToString(),
                                 startDate = dr["startDate"] is DBNull ? null : (DateTime?)dr["startDate"],
                                 endDate = dr["endDate"] is DBNull ? null : (DateTime?)dr["endDate"],
                                 delFlag = (bool)dr["delFlag"],
                                 createdDate = (DateTime?)dr["createdDate"],
                                 createdByUserId = dr["createdByUserId"].ToString(),
                                 updatedDate = (DateTime?)dr["updatedDate"],
                                 updatedByUserId = dr["updatedByUserId"].ToString(),
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public static string ThaiBahtText(string strNumber, bool IsTrillion = false)
        {
            string BahtText = "";
            string strTrillion = "";
            string[] strThaiNumber = { "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า", "สิบ" };
            string[] strThaiPos = { "", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน", "ล้าน" };

            decimal decNumber = 0;
            decimal.TryParse(strNumber, out decNumber);

            if (decNumber == 0)
            {
                return "ศูนย์บาทถ้วน";
            }

            strNumber = decNumber.ToString("0.00");
            string strInteger = strNumber.Split('.')[0];
            string strSatang = strNumber.Split('.')[1];

            if (strInteger.Length > 13)
                throw new Exception("รองรับตัวเลขได้เพียง ล้านล้าน เท่านั้น!");

            bool _IsTrillion = strInteger.Length > 7;
            if (_IsTrillion)
            {
                strTrillion = strInteger.Substring(0, strInteger.Length - 6);
                BahtText = ThaiBahtText(strTrillion, _IsTrillion);
                strInteger = strInteger.Substring(strTrillion.Length);
            }

            int strLength = strInteger.Length;
            for (int i = 0; i < strInteger.Length; i++)
            {
                string number = strInteger.Substring(i, 1);
                if (number == "-")
                {
                    number = strInteger.Substring(i + 1, 1);
                }


                if (number != "0")
                {
                    if (i == strLength - 1 && number == "1" && strLength != 1)
                    {
                        BahtText += "เอ็ด";
                    }
                    else if (i == strLength - 2 && number == "2" && strLength != 1)
                    {
                        BahtText += "ยี่";
                    }
                    else if (i != strLength - 2 || number != "1")
                    {
                        BahtText += strThaiNumber[int.Parse(number)];
                    }

                    BahtText += strThaiPos[(strLength - i) - 1];
                }
            }

            if (IsTrillion)
            {
                return BahtText + "ล้าน";
            }

            if (strInteger != "0")
            {
                BahtText += "บาท";
            }

            if (strSatang == "00")
            {
                BahtText += "ถ้วน";
            }
            else
            {
                strLength = strSatang.Length;
                for (int i = 0; i < strSatang.Length; i++)
                {
                    string number = strSatang.Substring(i, 1);
                    if (number != "0")
                    {
                        if (i == strLength - 1 && number == "1" && strSatang[0].ToString() != "0")
                        {
                            BahtText += "เอ็ด";
                        }
                        else if (i == strLength - 2 && number == "2" && strSatang[0].ToString() != "0")
                        {
                            BahtText += "ยี่";
                        }
                        else if (i != strLength - 2 || number != "1")
                        {
                            BahtText += strThaiNumber[int.Parse(number)];
                        }

                        BahtText += strThaiPos[(strLength - i) - 1];
                    }
                }

                BahtText += "สตางค์";
            }

            return BahtText;
        }

        public static bool checkLanguageDoc(string cultureDoc, string culture, int statusId)
        {

            //เพื่อเช็คการใช้ภาษาในหน้า input form
            string cultureLocal = "";

            if (HttpContext.Current != null)
            {
                try
                {
                    cultureLocal = HttpContext.Current.Request.Cookies[ConfigurationManager.AppSettings["nameCookieLanguageEact"]].Value.ToString();
                }
                catch (Exception ex)
                {
                    cultureLocal = cultureDoc;
                }
            }
            else
            {
                //เกิดกรณี approve เรียก fn ผ่าน API ใช้ Cookies ไม่ได้
                cultureLocal = cultureDoc;
            }

            //   Resources.Global.cultureLocal; 
            //   Resources.Global.cultureLocal ;
            bool chk = false;
            try
            {

                if (HttpContext.Current != null)
                {
                    //ถ้าเป็นโหมดแก้ไขได้ ใช้ภาษาเครื่อง
                    if (culture == cultureLocal) chk = true;
                }
                else
                {

                    //แก้ไขไม่ได้ต้องใช้ภาษาใน DB
                    if (culture == cultureDoc) chk = true;

                }

                //if (culture == "en-US")
                //{ chk = true; }

                //DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getActIDUseEng"
                //    , new SqlParameter[] { new SqlParameter("@activityId", activityId) });
                //if (ds.Tables[0].Rows.Count > 0) chk = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return chk;
        }

        public static bool checkModeEdit(int statusId, string formTYpeId)
        {
            bool chk = true; //แก้ไขได้

            try
            {
                if (UtilsAppCode.Session.User != null)
                {
                    if ((statusId == 2 && (UtilsAppCode.Session.User.isAdminTBM == false || formTYpeId == ConfigurationManager.AppSettings["formExpTrvNumId"] || formTYpeId == ConfigurationManager.AppSettings["formPaymentVoucherTbmId"]))

                        || (statusId == 3))
                    {
                        chk = false;//แก้ไข้ไม่ได้
                    }
                    else
                    {
                        chk = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return chk;
        }
        public static void setCulture(string culture)
        {
            if (culture == "" || culture == null)
            {
                culture = ConfigurationManager.AppSettings["cultureThai"]; // base Language
            }

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(culture);
        }
        public static string convertDateTHToShowCultureDateEN(DateTime? dateToShow, string formatDatetime)
        {
            string valResult = "";
            if (dateToShow != null)
            {
                valResult = dateToShow.Value.ToString(formatDatetime, new CultureInfo(ConfigurationManager.AppSettings["cultureEng"], true));
            }
            return valResult;
        }
        public static string convertDateTHToShowCultureDateTH(DateTime? dateToShow, string formatDatetime)
        {
            string valResult = "";

            if (dateToShow != null)
            {
                valResult = dateToShow.Value.ToString(formatDatetime, new CultureInfo(ConfigurationManager.AppSettings["cultureThai"], true));
            }
            return valResult;
        }



        public static string formatValueSelectEO_PVForm(string activityIdEO, string EO)
        {
            string valResult = (activityIdEO + "|" + EO);
            return valResult;
        }


        public static bool filterActivityForPayment_MT(string activityTxt)
        {
            bool result;
            result = QueryOtherMaster.getOhterMaster("activityMT", "").Where(x => x.val1.Equals(activityTxt)).Any();
            return result;
        }


    }
}