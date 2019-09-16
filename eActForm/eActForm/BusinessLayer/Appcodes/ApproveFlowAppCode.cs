using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Configuration;
using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using WebLibrary;
namespace eActForm.BusinessLayer
{
    public class ApproveFlowAppCode
    {
        public static ApproveFlowModel.approveFlowModel getFlowByActFormId(string actId)
        {
            try
            {
                ApproveFlowModel.approveFlowModel model = new ApproveFlowModel.approveFlowModel();
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getFlowMainByActFormId"
                    , new SqlParameter[] { new SqlParameter("@actFormId", actId) });
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
                model.flowDetail = getFlowDetailWithApproveDetail(model.flowMain.id, actId);
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getFlowByActFormId >>" + ex.Message);
            }
        }

        public static ApproveFlowModel.approveFlowModel getFlowForReportDetail(string subId, string customerId, string productTypeId)
        {
            ApproveFlowModel.approveFlowModel model = new ApproveFlowModel.approveFlowModel();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getFlowMainForReportDetail"
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
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getFlowForReportDetail >> flow detail report not found : " + ex.Message);
            }
            return model;
        }

        /// <summary> 
        /// get flow for type the activity form
        /// </summary>
        /// <param name="subId"></param>
        /// <param name="actFormId"></param>
        /// <returns></returns>
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
                if (lists.Count > 0)
                {
                    model.flowMain = lists[0];
                    string checkFlowApprove = checkFlowBeforeByActId(actFormId);
                    if (!string.IsNullOrEmpty(checkFlowApprove))
                    {
                        model.flowDetail = getFlowDetail(checkFlowApprove, actFormId);
                    }
                    else
                    {
                        model.flowDetail = getFlowDetail(model.flowMain.id, actFormId);
                    }

                }
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getFlow by actFormId >>" + ex.Message);
            }
        }

        public static string checkFlowBeforeByActId(string actFormId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_CheckFlowHistoryByActFormId"
                    , new SqlParameter[] { new SqlParameter("@actFormId", actFormId) });

                return ds.Tables[0].Rows[0]["flowId"].ToString();

            }
            catch (Exception ex)
            {
                return null;
                throw new Exception("checkFlowBeforeByActId >>" + ex.Message);
            }
        }

        /// <summary>
        /// get flow for type the activity form
        /// </summary>
        /// <param name="subId"></param>
        /// <param name="customerId"></param>
        /// <param name="productCatId"></param>
        /// <returns></returns>
        public static ApproveFlowModel.approveFlowModel getFlow(string subId, string customerId, string productCatId)
        {
            try
            {
                ApproveFlowModel.approveFlowModel model = new ApproveFlowModel.approveFlowModel();
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getFlowMain"
                    , new SqlParameter[] {new SqlParameter("@subjectId",subId)
                    ,new SqlParameter("@customerId",customerId)
                    ,new SqlParameter("@productCatId",productCatId)});
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

        public static List<ApproveFlowModel.flowApproveDetail> getFlowDetailWithApproveDetail(string flowId, string actId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getFlowApproveWithStatusDetail"
                    , new SqlParameter[] {
                        new SqlParameter("@flowId", flowId)
                        ,new SqlParameter("@actFormId", actId)
                    });
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
                                 statusId = dr["statusId"].ToString(),
                                 remark = dr["remark"].ToString(),
                                 imgSignature = string.Format(ConfigurationManager.AppSettings["rootgetSignaURL"], dr["empId"].ToString()),
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getFlowDetailWithApproveDetail >>" + ex.Message);
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
        public static List<ApproveFlowModel.flowApproveDetail> getFlowDetail(string flowId, string actId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getFlowApproveDetailForActForm"
                    , new SqlParameter[] { new SqlParameter("@flowId", flowId)
                                            , new SqlParameter("@actFormId",actId)
                    });
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