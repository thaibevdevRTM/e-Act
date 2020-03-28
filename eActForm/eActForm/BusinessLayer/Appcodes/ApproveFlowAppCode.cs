using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebLibrary;
using static eActForm.Models.ApproveFlowModel;

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


        public static ApproveFlowModel.approveFlowModel getFlowForReportDetailOMT(string subId, string customerId, string productTypeId)
        {
            ApproveFlowModel.approveFlowModel model = new ApproveFlowModel.approveFlowModel();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getFlowMainForReportDetailOMT"
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
                ExceptionManager.WriteError("getFlowForReportDetailOMT >> flow detail report not found : " + ex.Message);
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

                var getMasterType = QueryGetActivityByIdTBMMKT.getActivityById(actFormId).FirstOrDefault().master_type_form_id;
                string stor = ConfigurationManager.AppSettings["masterEmpExpense"] == getMasterType ? "usp_getFlowIdExpenseByActFormId" : "usp_getFlowIdByActFormId";

                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, stor
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
                        if (ConfigurationManager.AppSettings["masterEmpExpense"] == getMasterType)
                        {
                            model.flowDetail = getFlowDetailExpense(model.flowMain.id ,actFormId);
                        }
                        else
                        {
                            model.flowDetail = getFlowDetail(model.flowMain.id, actFormId);
                        }
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
                                 isApproved = dr["isApproved"] != null ? (bool)dr["isApproved"] : true,
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
                                 approveGroupId = dr["approveGroupId"].ToString(),//เฟรมเพิ่ม 20200113 เพิ่มapproveGroupId ไว้ใช้ดึงชือ่ผู้อนุมัติแสดงบนหนังสือฟอร์ม
                                 approveGroupName = dr["approveGroupName"].ToString(),
                                 approveGroupNameEN = dr["approveGroupNameEN"].ToString(),
                                 isShowInDoc = (bool)dr["showInDoc"],
                                 description = dr["description"].ToString(),
                                 isApproved = dr["isApproved"] != null ? (bool)dr["isApproved"] : true,
                                 bu = dr["empDivisionTH"].ToString(),
                                 buEN = dr["empDivisionEN"].ToString(),
                                 empFNameEN = dr["empFNameEN"].ToString(),
                                 empLNameEN = dr["empLNameEN"].ToString(),
                                 empPositionTitleEN = dr["empPositionTitleEN"].ToString()
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getFlowDetail >>" + ex.Message);
            }
        }

        public static List<ApproveFlowModel.flowApproveDetail> getFlowDetailExpense(string flowId ,string actId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getFlowApproveDetailExpense"
                    , new SqlParameter[]{ new SqlParameter("@flowId", flowId)
                                   , new SqlParameter("@actFormId", actId)});
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
                                 approveGroupId = dr["approveGroupId"].ToString(),
                                 approveGroupName = dr["approveGroupName"].ToString(),
                                 approveGroupNameEN = dr["approveGroupNameEN"].ToString(),
                                 isShowInDoc = (bool)dr["showInDoc"],
                                 description = dr["description"].ToString(),
                                 isApproved = dr["isApproved"] != null ? (bool)dr["isApproved"] : true,
                                 bu = dr["empDivisionTH"].ToString(),
                                 buEN = dr["empDivisionEN"].ToString(),
                                 empFNameEN = dr["empFNameEN"].ToString(),
                                 empLNameEN = dr["empLNameEN"].ToString(),
                                 empPositionTitleEN = dr["empPositionTitleEN"].ToString()
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getFlowDetailExpense >>" + ex.Message);
            }
        }

        public static ApproveFlowModel.approveFlowModel getFlowApproveGroupByType(getDataList_Model model)
        {
            try
            {
                ApproveFlowModel.approveFlowModel approveFlow_Model = new ApproveFlowModel.approveFlowModel();
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getFlowApproveByAllType"
                    , new SqlParameter[] {new SqlParameter("@companyId",model.companyId)
                    ,new SqlParameter("@subjectId",model.subjectId)
                    ,new SqlParameter("@customerId",model.customerId)
                    ,new SqlParameter("@productCatId",model.productCatId)
                    ,new SqlParameter("@flowLimitId",model.flowLimitId)
                    ,new SqlParameter("@channelId",model.channelId)
                    ,new SqlParameter("@productBrandId",model.productBrandId)
                    ,new SqlParameter("@productType",model.productTypeId)});
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ApproveFlowModel.flowApproveDetail()
                             {
                                 id = dr["id"].ToString(),
                                 companyId = dr["companyId"].ToString(),
                                 flowId = dr["flowId"].ToString(),
                                 empId = dr["empId"].ToString(),
                                 empFNameTH = dr["empName"].ToString(),
                                 approveGroupId = dr["approveGroupId"].ToString(),
                                 rangNo = int.Parse(dr["rangNo"].ToString()),
                                 isShowInDoc = !string.IsNullOrEmpty(dr["showInDoc"].ToString()) ? bool.Parse(dr["showInDoc"].ToString()) : true,
                                 isApproved = !string.IsNullOrEmpty(dr["isApproved"].ToString()) ? bool.Parse(dr["isApproved"].ToString()) : true,
                             }).ToList();
                approveFlow_Model.flowDetail = lists.ToList();
                return approveFlow_Model;
            }
            catch (Exception ex)
            {
                throw new Exception("getFlowApproveGroupByType >>" + ex.Message);
            }
        }


        public static List<TB_Reg_FlowModel> getMainFlowByMasterTypeId(string masterTypeId)
        {
            try
            {
                List<TB_Reg_FlowModel> regMainFlow = new List<TB_Reg_FlowModel>();
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_get_FlowMainByMasterTypeId"
                    , new SqlParameter[] { new SqlParameter("@masterTypeId", masterTypeId) });
                regMainFlow = (from DataRow dr in ds.Tables[0].Rows
                               select new TB_Reg_FlowModel()
                               {
                                   id = dr["id"].ToString(),
                                   subjectId = dr["subjectId"].ToString(),
                                   companyId = dr["companyId"].ToString(),
                                   customerId = dr["customerId"].ToString(),
                                   productCatId = dr["productCatId"].ToString(),
                                   productTypeId = dr["productTypeId"].ToString(),
                                   flowLimitId = dr["flowLimitId"].ToString(),
                                   channelId = dr["channelId"].ToString(),
                                   productBrandId = dr["productBrandId"].ToString(),
                               }).ToList();

                return regMainFlow;
            }
            catch (Exception ex)
            {
                throw new Exception("getMainFlowByMasterTypeId >>" + ex.Message);
            }
        }

    }
}