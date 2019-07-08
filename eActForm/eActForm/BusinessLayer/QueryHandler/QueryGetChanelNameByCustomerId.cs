using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    public class QueryGetChanelNameByCustomerId
    {
        public static List<TB_Act_Chanel_Model.Chanel_Model> getChanelNameByCusId(string customerId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getChanelNamebyCustomerId"
                 , new SqlParameter("@customerId", customerId));

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new TB_Act_Chanel_Model.Chanel_Model()
                              {
                                  chanelGroup = d["chanelGroup"].ToString(),
                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getChanelNameByCusId => " + ex.Message);
                return new List<TB_Act_Chanel_Model.Chanel_Model>();
            }
        }
    }
}