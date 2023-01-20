using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    public class QueryGetAllChanel
    {

        public static List<TB_Act_Chanel_Model.Chanel_Model> getAllChanel()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getAllChanel");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_Chanel_Model.Chanel_Model()
                             {
                                 id = d["id"].ToString(),
                                 chanelGroup = d["chanelGroup"].ToString(),
                                 cust = d["cust"].ToString(),
                                 tradingPartner = d["tradingPartner"].ToString(),
                                 no_tbmmkt = d["no_tbmmkt"].ToString(),
                                 delFlag = d["delFlag"].ToString(),
                                 typeChannel = d["typeChannel"].ToString(),
                                 createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                 createdByUserId = d["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                 updatedByUserId = d["updatedByUserId"].ToString(),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getAllChanel => " + ex.Message);
                return new List<TB_Act_Chanel_Model.Chanel_Model>();
            }
        }


        public static List<TB_Act_Chanel_Model.Chanel_Model> getChanelBySubjectId(string subjectId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getChanelBySubjectId"
                     , new SqlParameter("@subjectId", subjectId));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_Chanel_Model.Chanel_Model()
                             {
                                 id = d["id"].ToString(),
                                 chanelGroup = d["chanelGroup"].ToString(),

                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getChanelBySubjectId => " + ex.Message);
                return new List<TB_Act_Chanel_Model.Chanel_Model>();
            }
        }

        public static List<TB_Act_Chanel_Model.Chanel_Model> GetChannelBudgetControl()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getChanelBGControl");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_Chanel_Model.Chanel_Model()
                             {
                                 id = d["id"].ToString(),
                                 chanelGroup = d["chanelGroup"].ToString(),

                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("GetChannelBudgetControl => " + ex.Message);
                return new List<TB_Act_Chanel_Model.Chanel_Model>();
            }
        }


    }
}