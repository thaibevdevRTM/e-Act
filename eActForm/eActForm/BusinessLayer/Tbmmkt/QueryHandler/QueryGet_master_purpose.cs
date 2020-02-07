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
    public class QueryGet_master_purpose
    {
        public static List<PurposeModel> getAllPurpose()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getAllPurpose"
                     );
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new PurposeModel()
                             {
                                 id = d["id"].ToString(),
                                 detailTh = d["detailTh"].ToString()+" : "+ d["detailEn"].ToString(),
                                 detailEn = d["detailEn"].ToString(),
                                 chk = false,
                             });
                return lists.OrderBy(x => x.detailTh).ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getAllPurpose => " + ex.Message);
                return new List<PurposeModel>();
            }
        }
        public static List<PurposeModel> getPurposeByActivityId(string activiityId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getPurposeByActivityId"
                   , new SqlParameter("@activityId", activiityId));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new PurposeModel()
                             {
                                 id = d["id"].ToString(),
                                 detailTh = d["detailTh"].ToString() + " : " + d["detailEn"].ToString(),
                                 detailEn = d["detailEn"].ToString(),
                                 chk = d["chk"].ToString()=="1" ? true : false ,
                             });
                return lists.OrderBy(x => x.detailTh).ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getAllPurpose => " + ex.Message);
                return new List<PurposeModel>();
            }
        }

        
    }
}