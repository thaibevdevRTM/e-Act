using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;

namespace eActForm.BusinessLayer
{
    public class ApproveFlowAppCode
    {
        public static ApproveFlowModel.approveFlowModel getFlowId(string subId, string actFormId)
        {
            try
            {
                ApproveFlowModel.approveFlowModel model = new ApproveFlowModel.approveFlowModel();
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getFlowIdByActFormId"
                    , new SqlParameter[] {new SqlParameter("@subId",subId)
                    ,new SqlParameter("@actFormId",actFormId)});
                var lists = (from DataRow dr in ds.Tables[0].Rows
                                                            select new ApproveFlowModel.flowApprove()
                                                            {
                                                                id = dr["id"].ToString(),
                                                            }).ToList();
                model.flowMain = lists[0];
                model.flowDetail = getFlowDetail(model.flowMain.id);
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getFlow by actFormId >>" + ex.Message);
            }
        }
        public static ApproveFlowModel.approveFlowModel getFlow(string subId, string customerId, string productTypeId)
        {
            try
            {
                ApproveFlowModel.approveFlowModel model = new ApproveFlowModel.approveFlowModel();
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getFlowMain"
                    , new SqlParameter[] {new SqlParameter("@subjectId",subId)
                    ,new SqlParameter("@customerId",customerId)
                    ,new SqlParameter("@productTypeId",productTypeId)});
                var lists = (from DataRow dr in ds.Tables[0].Rows
                                                            select new ApproveFlowModel.flowApprove()
                                                            {
                                                                id = dr["id"].ToString(),
                                                                flowNameTH = dr["flowNameTH"].ToString(),
                                                                cusNameTH = dr["cusNameTH"].ToString(),
                                                                cusNameEN = dr["cusNameEN"].ToString(),
                                                                nameTH = dr["nameTH"].ToString(),
                                                            }).ToList();
                model.flowMain = lists[0];
                model.flowDetail = getFlowDetail(model.flowMain.id);
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getFlow >> " + ex.Message);
            }
        }

        public static List<ApproveFlowModel.flowApproveDetail> getFlowDetail(string flowId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getFlowApproveDetail"
                    , new SqlParameter[] { new SqlParameter("@flowId", flowId) });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ApproveFlowModel.flowApproveDetail()
                             {
                                 id = dr["id"].ToString(),
                                 rangNo = (int)dr["rangNo"],
                                 empId = dr["empId"].ToString(),
                                 empEmail = dr["empEmail"].ToString(),
                                 empFNameTH = dr["empFNameTH"].ToString(),
                                 empLNameTH = dr["empLNameTH"].ToString(),
                                 empPositionTitleTH = dr["empPositionTitleTH"].ToString(),
                                 approveGroupName = dr["approveGroupName"].ToString(),
                                 approveGroupNameEN = dr["approveGroupNameEN"].ToString(),
                                 isShowInDoc = (bool)dr["showInDoc"],
                                 description = dr["description"].ToString(),
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getFlowDetail >>" + ex.Message);
            }
        }
    }
}