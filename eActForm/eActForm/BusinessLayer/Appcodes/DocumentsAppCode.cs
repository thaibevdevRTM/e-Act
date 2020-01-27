using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using eActForm.Models;
namespace eActForm.BusinessLayer
{
    public class DocumentsAppCode
    {
        public static List<DocumentsModel.actRepDetailModel> getActRepDetailLists(DateTime startDate, DateTime endDate, string typeForm)
        {
            try
            {
                string stored = typeForm == Activity_Model.activityType.MT.ToString() ? "usp_GetActivityRepDetailAll" : "usp_GetActivityRepDetailOMTAll";
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, stored);
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
            catch(Exception ex)
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

    }
}