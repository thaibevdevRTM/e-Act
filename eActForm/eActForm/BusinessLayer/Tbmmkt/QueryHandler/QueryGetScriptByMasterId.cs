using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebLibrary;

namespace eActForm.BusinessLayer.Tbmmkt.QueryHandler
{
    public class QueryGetScriptByMasterFormId
    {
        public static List<scriptModel> getScriptByMasterFormId(string master_type_form_id)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getScriptByMasterFormId"
                  , new SqlParameter("@master_type_form_id", master_type_form_id));

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new scriptModel()
                              {

                                  masterFormId = d["masterForm_Id"].ToString(),
                                  scriptsName = d["scriptsName"].ToString(),
                                  folder = d["folder"].ToString(),
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
                ExceptionManager.WriteError("getScriptByMasterFormId => " + ex.Message);
                return new List<scriptModel>();
            }
        }
    }
}