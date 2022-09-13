using eForms.Models.MasterData;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace eForm.Approve
{
    public class AppCode
    {

        public static List<ApproverModel> getWaitApprove()
        {
            try
            {
                Console.Write("waiting for update data");

                DataSet ds = SqlHelper.ExecuteDataset(Properties.Resources.strConn, CommandType.StoredProcedure, "usp_getWaitApprover");
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ApproverModel()
                             {
                                 appId = dr["appId"].ToString(),
                                 appName = dr["appName"].ToString(),
                                 docNo = dr["docNo"].ToString(),
                                 refId = dr["refId"].ToString(),
                                 orderRank = dr["orderRank"].ToString(),
                                 subject = dr["subject"].ToString(),
                                 requesterDate = DateTime.Parse(dr["requesterDate"].ToString()),
                                 totalAmount = dr["totalAmount"].ToString(),
                                 currency = dr["currency"].ToString(),
                                 approver = dr["approver"].ToString(),
                                 companyName = dr["companyName"].ToString(),
                                 organizationUnitName = dr["organizationUnitName"].ToString(),
                                 detail = dr["detail"].ToString(),
                                 attachedFileName = dr["attachedFileName"].ToString(),
                                 attachedUrl = string.Format(Properties.Resources.controllerGetFile, dr["attachedFileName"].ToString()),
                             }).ToList();

                return lists;


            }
            catch (Exception ex)
            {
                throw new Exception("getWaitApprove >>" + ex.Message);
            }

        }
    }
}
