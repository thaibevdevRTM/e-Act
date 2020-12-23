using eForms.Models.MasterData;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLibrary;

namespace eForms.Presenter.AppCode
{
    public class MainAppCode
    {

        public static string paymentVoucherFlowId = "B062F1B4-969E-457B-8559-FE50F2279682";
        public static string masterTypePaymentVoucher = "F957F1C0-EF1B-4DD2-B257-83153109726E";
        public static DateTime convertStrToDate(string p_date, string formatDate)
        {
            return DateTime.ParseExact(p_date, formatDate, CultureInfo.InvariantCulture);
        }
        public static List<Models.MasterData.TB_Act_Other_Model> getOhterMaster(string strCon ,string type, string subtype)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strCon, CommandType.StoredProcedure, "usp_getAllTOtherMaster"
                    , new SqlParameter("@type", type)
                    , new SqlParameter("@subType", subtype));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_Other_Model()
                             {
                                 id = d["id"].ToString(),
                                 type = d["type"].ToString(),
                                 name = d["name"].ToString(),
                                 displayVal = d["displayVal"].ToString(),
                                 subType = d["subType"].ToString(),
                                 val1 = d["val1"].ToString(),
                                 val2 = d["val2"].ToString(),
                                 sort = d["sort"].ToString(),
                                 delFlag = bool.Parse(d["delFlag"].ToString()),
                                 createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                 createdByUserId = d["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                 updatedByUserId = d["updatedByUserId"].ToString(),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getOhterMaster => " + ex.Message);
                return new List<TB_Act_Other_Model>();
            }
        }



    }
}
