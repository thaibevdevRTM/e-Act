﻿using eActForm.BusinessLayer.Appcodes;
using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    public class ApproveAppCode
    {


        public static void setCountWatingApprove()
        {
            try
            {
                if (UtilsAppCode.Session.User != null)
                {
                    DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getCountWatingApproveByEmpId"
                        , new SqlParameter[] { new SqlParameter("@empId", UtilsAppCode.Session.User.empId) });
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        UtilsAppCode.Session.User.countWatingActForm = ds.Tables[0].Rows[0]["actFormId"].ToString();
                        UtilsAppCode.Session.User.counteatingRepDetail = ds.Tables[0].Rows[0]["repFormId"].ToString();
                        UtilsAppCode.Session.User.counteatingSummaryDetail = ds.Tables[0].Rows[0]["sumFormId"].ToString();
                    }
                    else
                    {
                        UtilsAppCode.Session.User.countWatingActForm = "0";
                        UtilsAppCode.Session.User.counteatingRepDetail = "0";
                        UtilsAppCode.Session.User.counteatingSummaryDetail = "0";
                    }


                    DataSet dsReject = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getCountRejectByEmpId"
                        , new SqlParameter[] { new SqlParameter("@empId", UtilsAppCode.Session.User.empId) });
                    if (dsReject.Tables.Count > 0 && dsReject.Tables[0].Rows.Count > 0)
                    {
                        UtilsAppCode.Session.User.countRejectAct = dsReject.Tables[0].Rows[0]["countRejectAct"].ToString();
                        UtilsAppCode.Session.User.countApproveReject = dsReject.Tables[0].Rows[0]["countApprove"].ToString();
                    }
                    else
                    {
                        UtilsAppCode.Session.User.countApproveReject = "0";
                        UtilsAppCode.Session.User.countRejectAct = "0";
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("setCountWatingApprove >>" + ex.Message);
            }
        }


        public static bool manageApproveEmpExpense(ApproveModel.approveModels model, string actId)
        {
            int resultInsert = 0;
            string newID = Guid.NewGuid().ToString();
            try
            {
                model.activity_TBMMKT_Model.activityFormModel = QueryGetActivityById.getActivityById(actId).FirstOrDefault();

                //---- ApprovePass-----//
                var dataApproveList = model.activity_TBMMKT_Model.activityOfEstimateList.Where(w => w.chkBox == true).ToList();
                var dataUnApproveList = model.activity_TBMMKT_Model.activityOfEstimateList.Where(w => w.chkBox == false).ToList();

                model.activity_TBMMKT_Model.activityOfEstimateList = dataApproveList;
                resultInsert = ActivityFormTBMMKTCommandHandler.ProcessInsertEstimate(0, model.activity_TBMMKT_Model, actId);

                //---- ApproveUnPass-----//
                model.activity_TBMMKT_Model.activityOfEstimateList = dataUnApproveList;
                resultInsert = ActivityFormTBMMKTCommandHandler.ProcessInsertEstimate(0, model.activity_TBMMKT_Model, newID);

                model.activity_TBMMKT_Model.activityFormModel.reference = model.activity_TBMMKT_Model.activityFormModel.activityNo;
                model.activity_TBMMKT_Model.activityFormModel.id = newID;
                model.activity_TBMMKT_Model.activityFormModel.activityNo = "";
                model.activity_TBMMKT_Model.activityFormModel.statusId = 1;
                model.activity_TBMMKT_Model.activityFormModel.updatedByUserId = UtilsAppCode.Session.User.empId;
                model.activity_TBMMKT_Model.activityFormModel.updatedDate = DateTime.Now;
                resultInsert += ActivityFormTBMMKTCommandHandler.insertActivityForm(model.activity_TBMMKT_Model.activityFormTBMMKT);
                return true;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("manageApproveEmpExpense => " + ex.Message);
                return false;
            }
        }

        public static string getEmailCCByActId(string actId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getEmailCC"
                     , new SqlParameter[] { new SqlParameter("@actId", actId) });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ApproveModel.approveDetailModel("")
                             {
                                 empEmail = dr["empEmail"].ToString()
                             }).ToList();

                string emailCC = "";
                if (lists.Any())
                {
                    emailCC = (string.Join(",", lists.Select(x => x.empEmail.ToString()).ToArray()));
                }



                return emailCC;
            }
            catch (Exception ex)
            {
                throw new Exception("getEmailCCByActId >> " + ex.Message);
            }
        }

        public static List<ApproveModel.approveDetailModel> getRemarkApprovedByEmpId(string actId, string empId)
        {
            try
            {
                ApproveModel.approveModels models = ApproveAppCode.getApproveByActFormId(actId);
                var empUser = models.approveDetailLists.Where(r => r.empId == empId).ToList();
                return empUser;
            }
            catch (Exception ex)
            {
                throw new Exception("getRemarkApprovedByEmpId >> " + ex.Message);
            }
        }


        public static List<ApproveModel.approveWaitingModel> getAllWaitingApproveGroupByEmpId()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getCountWaitingApproveGroupByEmpId");
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ApproveModel.approveWaitingModel()
                             {
                                 empId = dr["empId"].ToString()
                                 ,
                                 waitingCount = dr["waitingCount"].ToString()
                                 ,
                                 empPrefix = dr["empPrefix"].ToString()
                                 ,
                                 empFNameTH = dr["empFNameTH"].ToString()
                                 ,
                                 empLNameTH = dr["empLNameTH"].ToString()
                                 ,
                                 empEmail = dr["empEmail"].ToString()
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getAllWaitingApproveGroupByEmpId >> " + ex.Message);
            }
        }

        public static int updateApproveWaitingByRangNo(string actId)
        {
            try
            {

                return SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_updateWaitingApproveByRangNo"
                    , new SqlParameter[] {new SqlParameter("@rangNo",1)
                    ,new SqlParameter("@actId",actId)
                    ,new SqlParameter("@updateBy",UtilsAppCode.Session.User.empId)
                    ,new SqlParameter("@updateDate",DateTime.Now)
                    });
            }
            catch (Exception ex)
            {
                throw new Exception("updateApproveWaitingByRangNo >>" + ex.Message);
            }
        }
        public static bool getPremisionApproveByEmpid(List<ApproveModel.approveDetailModel> lists, string empId)
        {
            try
            {
                bool rtn = false;
                if (lists != null)
                {
                    var model = (from x in lists where x.empId.Equals(empId) select x).ToList();
                    rtn = model.Count > 0 ? true : false;
                }

                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception("fillterApproveByEmpid >>" + ex.Message);
            }
        }
        public static int updateApprove(string actFormId, string statusId, string remark, string approveType)
        {
            try
            {
                // update approve detail
                int rtn = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_updateApprove"
                        , new SqlParameter[] {new SqlParameter("@actFormId",actFormId)
                    , new SqlParameter("@empId",UtilsAppCode.Session.User.empId)
                    ,new SqlParameter("@statusId",statusId)
                    ,new SqlParameter("@remark",remark)
                    ,new SqlParameter("@updateDate",DateTime.Now)
                    ,new SqlParameter("@updateBy",UtilsAppCode.Session.User.empId)
                        });

                if (approveType == AppCode.ApproveType.Report_Detail.ToString())
                {
                    rtn = updateActRepDetailStatus(statusId, actFormId);
                }
                else if (approveType == AppCode.ApproveType.Report_Summary.ToString())
                {
                    rtn = updateSummaryDetailStatus(statusId, actFormId);
                }
                else
                {
                    // default Activity Form
                    rtn = updateActFormStatus(statusId, actFormId);
                }

                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception("updateApprove >> " + ex.Message);
            }
        }

        private static int updateSummaryDetailStatus(string statusId, string actFormId)
        {
            try
            {
                int rtn = 0;
                // update activity form
                if (statusId == ConfigurationManager.AppSettings["statusReject"])
                {
                    // update reject
                    rtn += ReportSummaryAppCode.updateSummaryReportWithApproveReject(actFormId);
                }
                else if (statusId == ConfigurationManager.AppSettings["statusApprove"])
                {
                    // update approve
                    rtn += ReportSummaryAppCode.updateSummaryReportWithApproveDetail(actFormId);

                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static int updateActRepDetailStatus(string statusId, string actFormId)
        {
            try
            {
                int rtn = 0;
                // update activity form
                if (statusId == ConfigurationManager.AppSettings["statusReject"])
                {
                    // update reject
                    rtn += ApproveRepDetailAppCode.updateActRepDetailByReject(actFormId);
                }
                else if (statusId == ConfigurationManager.AppSettings["statusApprove"])
                {
                    // update approve
                    rtn += ApproveRepDetailAppCode.updateActRepDetailByApproveDetail(actFormId);

                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private static int updateActFormStatus(string statusId, string actFormId)
        {
            try
            {
                int rtn = 0;
                // update activity form
                if (statusId == ConfigurationManager.AppSettings["statusReject"])
                {
                    // update reject
                    rtn += updateActFormWithApproveReject(actFormId);

                    List<ActivityForm> getActList = QueryGetActivityById.getActivityById(actFormId);
                    if (getActList.FirstOrDefault().master_type_form_id == ConfigurationManager.AppSettings["formTransferbudget"])
                    {
                        //waiting update budgetControl
                        bool resultTransfer = TransferBudgetAppcode.transferBudgetForReject(actFormId);
                    }
                }
                else if (statusId == ConfigurationManager.AppSettings["statusApprove"])
                {
                    // update approve
                    rtn += updateActFormWithApproveDetail(actFormId);
                }

                //var result = updateBudgetControl_Balance(actFormId);
               
                


                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static int updateBudgetControl_Balance(string actId)
        {
            try
            {
                return SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_updateBudgetControl_Balance"
                    ,new SqlParameter[] { new SqlParameter("@actFormId", actId)
                    ,new SqlParameter("@updateBy",UtilsAppCode.Session.User.empId)});
            }
            catch (Exception ex)
            {
                throw new Exception("updateBudgetControl_Balance >> " + ex.Message);
            }
        }

        public static int updateActFormWithApproveReject(string actId)
        {
            try
            {
                return SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_updateStatusActFormByApproveReject"
                    , new SqlParameter[] { new SqlParameter("@actFormId", actId)
                    ,new SqlParameter("@updateDate",DateTime.Now)
                    ,new SqlParameter("@updateBy",UtilsAppCode.Session.User.empId)});
            }
            catch (Exception ex)
            {
                throw new Exception("updateActFormWithApproveReject >> " + ex.Message);
            }
        }
        public static int updateActFormWithApproveDetail(string actId)
        {
            try
            {
                return SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_updateStatusActFormByApproveDetail"
                    , new SqlParameter[] { new SqlParameter("@actFormId", actId)
                    ,new SqlParameter("@updateDate",DateTime.Now)
                    ,new SqlParameter("@updateBy",UtilsAppCode.Session.User.empId)});
            }
            catch (Exception ex)
            {
                throw new Exception("updateActFormWithApproveDetail >> " + ex.Message);
            }
        }




        /// <summary>
        /// for Activity Form
        /// </summary>
        /// <param name="actId"></param>
        /// <returns></returns>
        public static int insertApproveForActivityForm(string actId)
        {
            int success = 0;
            try
            {
                if (getApproveByActFormId(actId).approveDetailLists.Count == 0)
                {
                    ApproveFlowModel.approveFlowModel flowModel = ApproveFlowAppCode.getFlowId(ConfigurationManager.AppSettings["subjectActivityFormId"], actId);
                    success = insertApproveByFlow(flowModel, actId);
                }
                else
                {
                    success = clearStatusApproveAndInsertHistory(actId);
                }
                return success; // alredy approve
            }
            catch (Exception ex)
            {
                throw new Exception("insertApprove >> " + ex.Message);
            }
        }

        /// <summary>
        /// insertApproveByFlow
        /// </summary>
        /// <param name="flowModel"></param>
        /// <param name="actId"></param>
        /// <returns></returns>
        public static int insertApproveByFlow(ApproveFlowModel.approveFlowModel flowModel, string actId)
        {
            try
            {
                int rtn = 0;
                List<ApproveModel.approveModel> list = new List<ApproveModel.approveModel>();
                ApproveModel.approveModel model = new ApproveModel.approveModel();
                model.id = Guid.NewGuid().ToString();
                model.flowId = flowModel.flowMain.id;
                model.actFormId = actId;
                model.delFlag = false;
                model.createdDate = DateTime.Now;
                model.createdByUserId = UtilsAppCode.Session.User.empId;
                model.updatedDate = DateTime.Now;
                model.updatedByUserId = UtilsAppCode.Session.User.empId;
                list.Add(model);
                DataTable dt = AppCode.ToDataTable(list);
                foreach (DataRow dr in dt.Rows)
                {
                    rtn += SqlHelper.ExecuteNonQueryTypedParams(AppCode.StrCon, "usp_insertApprove", dr);
                }

                // insert approve detail
                foreach (ApproveFlowModel.flowApproveDetail m in flowModel.flowDetail)
                {
                    rtn += SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertApproveDetail"
                        , new SqlParameter[] {new SqlParameter("@id",Guid.NewGuid().ToString())
                            ,new SqlParameter("@approveId",model.id)
                            ,new SqlParameter("@rangNo",m.rangNo)
                            ,new SqlParameter("@empId",m.empId)
                            ,new SqlParameter("@statusId","")
                            ,new SqlParameter("@isSendEmail",false)
                            ,new SqlParameter("@remark","")
                            ,new SqlParameter("@isApproved",m.isApproved)
                            ,new SqlParameter("@delFlag",false)
                            ,new SqlParameter("@createdDate",DateTime.Now)
                            ,new SqlParameter("@createdByUserId",UtilsAppCode.Session.User.empId)
                            ,new SqlParameter("@updatedDate",DateTime.Now)
                            ,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
                            ,new SqlParameter("@showInDoc",m.isShowInDoc)
                            ,new SqlParameter("@approveGroupId",m.approveGroupId)
                        });
                }

                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static ApproveModel.approveModels getApproveByActFormId(string actFormId, string empId)
        {
            try
            {
                ApproveModel.approveModels models = new ApproveModel.approveModels();
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getApproveDetailByActFormId"
                    , new SqlParameter[] { new SqlParameter("@actFormId", actFormId) });
                models.approveDetailLists = (from DataRow dr in ds.Tables[0].Rows
                                             select new ApproveModel.approveDetailModel(dr["empId"].ToString())
                                             {
                                                 id = dr["id"].ToString(),
                                                 approveId = dr["approveId"].ToString(),
                                                 rangNo = (int)dr["rangNo"],
                                                 empId = dr["empId"].ToString(),
                                                 statusId = dr["statusId"].ToString(),
                                                 statusName = dr["statusName"].ToString(),
                                                 statusNameEN = dr["statusNameEN"].ToString(),
                                                 isSendEmail = (bool)dr["isSendEmail"],
                                                 remark = dr["remark"].ToString(),
                                                 signature = (dr["signature"] == null || dr["signature"] is DBNull) ? new byte[0] : (byte[])dr["signature"],
                                                 ImgName = string.Format(ConfigurationManager.AppSettings["rootgetSignaByActURL"], actFormId, dr["empId"].ToString()),
                                                 isApprove = (bool)dr["isApproved"],
                                                 approveGroupId = dr["approveGroupId"].ToString(),
                                                 delFlag = (bool)dr["delFlag"],
                                                 createdDate = (DateTime?)dr["createdDate"],
                                                 createdByUserId = dr["createdByUserId"].ToString(),
                                                 updatedDate = (DateTime?)dr["updatedDate"],
                                                 updatedByUserId = dr["updatedByUserId"].ToString(),
                                                 approveGroupNameTH = dr["groupNameTH"].ToString(),
                                                 approveGroupnameEN = dr["groupNameEN"].ToString(),
                                                 isShowInDoc = dr["showInDoc"] is DBNull ? null : (bool?)dr["showInDoc"],
                                             }).ToList();


                if (models.approveDetailLists.Count > 0)
                {
                    ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getApproveByActFormId"
                   , new SqlParameter[] { new SqlParameter("@actFormId", actFormId) });

                    var empDetail = models.approveDetailLists.Where(r => r.empId == empId).ToList(); //

                    if (empDetail.Count > 1)
                    {
                        foreach (var item in empDetail)
                        {
                            if (item.statusId == "2")
                            {
                                empDetail[0] = item;
                                break;
                            }
                        }
                    }

                    var lists = (from DataRow dr in ds.Tables[0].Rows
                                 select new ApproveModel.approveModel()
                                 {
                                     id = dr["id"].ToString(),
                                     flowId = dr["flowId"].ToString(),
                                     actFormId = dr["actFormId"].ToString(),
                                     actNo = dr["activityNo"].ToString(),
                                     statusId = (empDetail.Count > 0) ? empDetail.FirstOrDefault().statusId : "",
                                     delFlag = (bool)dr["delFlag"],
                                     createdDate = (DateTime?)dr["createdDate"],
                                     createdByUserId = dr["createdByUserId"].ToString(),
                                     updatedDate = (DateTime?)dr["updatedDate"],
                                     updatedByUserId = dr["updatedByUserId"].ToString(),
                                     isPermisionApprove = getPremisionApproveByEmpid(models.approveDetailLists, empId)
                                 }).ToList();
                    models.approveModel = lists[0];

                    models.isApproveDetailListsShowDocHaveNull = models.approveDetailLists.Where(x => x.isShowInDoc == null).Count() > 0 ? true : false;

                }

                return models;
            }
            catch (Exception ex)
            {
                throw new Exception("getCountApproveByActFormId >>" + ex.Message);
            }
        }
        public static ApproveModel.approveModels getApproveByActFormId(string actFormId)
        {
            try
            {
                return getApproveByActFormId(actFormId, UtilsAppCode.Session.User.empId);
            }
            catch (Exception ex)
            {
                throw new Exception("getCountApproveByActFormId >>" + ex.Message);
            }
        }
        public static List<ApproveModel.approveStatus> getApproveStatus(AppCode.StatusType type)
        {
            try
            {

                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getApproveStatusAll"
                    , new SqlParameter("@type", type.ToString()));
                var list = (from DataRow dr in ds.Tables[0].Rows
                            select new ApproveModel.approveStatus()
                            {
                                id = dr["id"].ToString(),
                                nameEN = dr["nameEN"].ToString(),
                                nameTH = dr["nameTH"].ToString(),
                                description = dr["description"].ToString(),
                                type = dr["type"].ToString(),
                                delFlag = (bool)dr["delFlag"],
                                createdDate = (DateTime?)dr["createdDate"],
                                createdByUserId = dr["createdByUserId"].ToString(),
                                updatedDate = (DateTime?)dr["updatedDate"],
                                updatedByUserId = dr["updatedByUserId"].ToString(),
                            }).ToList();
                return list;

            }
            catch (Exception ex)
            {
                throw new Exception("getApproveStatus >> " + ex.Message);
            }
        }

        public static int clearStatusApproveAndInsertHistory(string activityId)
        {
            int result = 0;
            try
            {

                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_clearStatusApproveAndInsertHis"
                    , new SqlParameter[] {new SqlParameter("@activityId",activityId)
                    });
            }
            catch (Exception ex)
            {
                throw new Exception("clearStatusApproveAndInsertHistory >> " + ex.Message);
            }

            return result;
        }

    }
}