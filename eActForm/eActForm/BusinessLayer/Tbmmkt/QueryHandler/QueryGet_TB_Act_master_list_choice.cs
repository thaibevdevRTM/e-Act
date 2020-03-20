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
    public class QueryGet_TB_Act_master_list_choice
    {
        public static List<TB_Act_master_list_choiceModel> get_TB_Act_master_list_choice(string master_type_form_id, string type)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_get_TB_Act_master_list_choice"
                       , new SqlParameter("@master_type_form_id", master_type_form_id)
                           , new SqlParameter("@type", type));
                var result = (from DataRow d in ds.Tables[0].Rows
                              select new TB_Act_master_list_choiceModel()
                              {
                                  id = d["id"].ToString(),
                                  name = d["name"].ToString(),
                                  nameEN = d["nameEN"].ToString(),
                                  sub_name = d["sub_name"].ToString(),
                                  type = d["type"].ToString(),
                                  master_type_form_id = d["master_type_form_id"].ToString(),
                                  delFlag = bool.Parse(d["delFlag"].ToString()),
                                  createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                  createdByUserId = d["createdByUserId"].ToString(),
                                  updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                  updatedByUserId = d["updatedByUserId"].ToString(),
                                  orderNum = d["orderNum"].ToString(),
                              });
                return result.OrderBy(x => x.orderNum).OrderBy(x => x.id).ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("get_channelMasterType => " + ex.Message);
                return new List<TB_Act_master_list_choiceModel>();
            }
        }
    }
}