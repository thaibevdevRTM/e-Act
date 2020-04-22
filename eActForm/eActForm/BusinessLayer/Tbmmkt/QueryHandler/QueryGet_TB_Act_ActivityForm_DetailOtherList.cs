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
    public class QueryGet_TB_Act_ActivityForm_DetailOtherList
    {
        public static List<TB_Act_ActivityForm_DetailOtherList> get_TB_Act_ActivityForm_DetailOtherList(string activityId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_get_TB_Act_ActivityForm_DetailOtherList"
                       , new SqlParameter("@activityId", activityId));
                var result = (from DataRow d in ds.Tables[0].Rows
                              select new TB_Act_ActivityForm_DetailOtherList()
                              {
                                  id = d["id"].ToString(),
                                  activityId = d["activityId"].ToString(),
                                  typeKeep = d["typeKeep"].ToString(),
                                  rowNo = int.Parse(d["rowNo"].ToString()),
                                  activityIdEO = d["activityIdEO"].ToString(),
                                  IO = d["IO"].ToString(),
                                  GL = d["GL"].ToString(),
                                  select_list_choice_id_ChReg = d["select_list_choice_id_ChReg"].ToString(),
                                  productBrandId = d["productBrandId"].ToString(),
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
                ExceptionManager.WriteError("get_TB_Act_ActivityForm_DetailOtherList => " + ex.Message);
                return new List<TB_Act_ActivityForm_DetailOtherList>();
            }
        }

    }
}