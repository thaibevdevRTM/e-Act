using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebLibrary;

namespace eActForm.BusinessLayer.QueryHandler
{
    public class QueryGetFlow
    {
        public static List<ApproveFlowModel.flowApproveDetail> getFlowDetailBytypeFormId(string typeFormId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getFlowDetailByTypeFormId"
                    , new SqlParameter("@typeFormId", typeFormId));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new ApproveFlowModel.flowApproveDetail()
                             {                                 
                                 id = d["flowId"].ToString(),                    
                                 description = d["description"].ToString(),                              
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getFlowDetailBytypeFormId => " + ex.Message);
                return new List<ApproveFlowModel.flowApproveDetail>();
            }
        }


    }
}