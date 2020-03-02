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
    public class QueryGet_TB_Act_ActivityChoiceSelect
    {
        public static List<TB_Act_ActivityChoiceSelectModel> get_TB_Act_ActivityChoiceSelectModel(string actFormId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_get_TB_Act_ActivityChoiceSelect"
                       , new SqlParameter("@actFormId", actFormId));
                var result = (from DataRow d in ds.Tables[0].Rows
                              select new TB_Act_ActivityChoiceSelectModel()
                              {
                                  id = d["id"].ToString(),
                                  actFormId = d["actFormId"].ToString(),
                                  select_list_choice_id = d["select_list_choice_id"].ToString(),
                                  name = d["name"].ToString(),
                                  type = d["type"].ToString(),
                                  delFlag = bool.Parse(d["delFlag"].ToString()),
                                  createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                  createdByUserId = d["createdByUserId"].ToString(),
                                  updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                  updatedByUserId = d["updatedByUserId"].ToString(),
                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("get_TB_Act_ActivityChoiceSelectModel => " + ex.Message);
                return new List<TB_Act_ActivityChoiceSelectModel>();
            }
        }

    }
}