using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
namespace eActForm.BusinessLayer
{
    public class ApproveRepDetailAppCode
    {
        public static List<RepDetailModel.actApproveRepDetailModel> getFilterFormByStatusId(List<RepDetailModel.actApproveRepDetailModel> lists, int statusId)
        {
            try
            {
                return lists.Where(r => r.statusId == statusId.ToString()).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterFormByStatusId >> " + ex.Message);
            }
        }
        public static int updateActRepDetailByApproveDetail(string actId)
        {
            try
            {
                return SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_updateStatusActRepDetailByApproveDetail"
                    , new SqlParameter[] { new SqlParameter("@actFormId", actId)
                    ,new SqlParameter("@updateDate",DateTime.Now)
                    ,new SqlParameter("@updateBy",UtilsAppCode.Session.User.empId)});
            }
            catch (Exception ex)
            {
                throw new Exception("updateActRepDetailByApproveDetail >> " + ex.Message);
            }
        }
        public static int updateActRepDetailByReject(string actId)
        {
            try
            {
                return SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_updateStatusActRepDetailByApproveReject"
                    , new SqlParameter[] { new SqlParameter("@actFormId", actId)
                    ,new SqlParameter("@updateDate",DateTime.Now)
                    ,new SqlParameter("@updateBy",UtilsAppCode.Session.User.empId)});
            }
            catch (Exception ex)
            {
                throw new Exception("updateActRepDetailByReject >> " + ex.Message);
            }
        }
        public static List<RepDetailModel.actApproveRepDetailModel> getApproveRepDetailListsByEmpId()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getApproveRepDetailFormByEmpId"
                    , new SqlParameter[] { new SqlParameter("@empId", UtilsAppCode.Session.User.empId) });
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var lists = (from DataRow dr in ds.Tables[0].Rows
                                 select new RepDetailModel.actApproveRepDetailModel()
                                 {
                                     id = dr["id"].ToString(),
                                     activityNo = dr["activityNo"].ToString(),
                                     statusId = dr["statusId"].ToString(),
                                     statusName = dr["statusName"].ToString(),
                                     startDate = dr["startDate"] is DBNull ? null : (DateTime?)dr["startDate"],
                                     endDate = dr["endDate"] is DBNull ? null : (DateTime?)dr["endDate"],
                                     customerId = dr["customerId"].ToString(),
                                     productTypeId = dr["productTypeId"].ToString(),
                                     productTypeName = dr["productTypeName"].ToString(),
                                     customerName = dr["customerName"].ToString(),
                                     delFlag = (bool)dr["delFlag"],
                                     createdDate = dr["createdDate"] is DBNull ? null : (DateTime?)dr["createdDate"],
                                     createdByUserId = dr["createdByUserId"].ToString(),
                                     updatedDate = dr["updatedDate"] is DBNull ? null : (DateTime?)dr["updatedDate"],
                                     updatedByUserId = dr["updatedByUserId"].ToString()
                                 }).ToList();
                    return lists;
                }
                else
                {
                    return new List<RepDetailModel.actApproveRepDetailModel>();
                }


            }
            catch (Exception ex)
            {
                throw new Exception("getApproveRepDetailListsByEmpId >>" + ex.Message);
            }
        }
        public static string insertActivityRepDetail(string customerId, string productTypeId, string startDate, string endDate, RepDetailModel.actFormRepDetails model,string typeForm)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                string TxtDoc = "";
                if (typeForm == Activity_Model.activityType.SetPrice.ToString())
                {
                    typeForm = ConfigurationManager.AppSettings["reportSetPrice"];
                    TxtDoc = ConfigurationManager.AppSettings["getTxtDocReportSetPrice"];
                }
                else if(typeForm == Activity_Model.activityType.OMT.ToString())
                {
                     typeForm = ConfigurationManager.AppSettings["reportOMT"];
                    TxtDoc = ConfigurationManager.AppSettings["getTxtDocReportOMT"];
                }
                else
                {
                    typeForm = ConfigurationManager.AppSettings["reportMT"];
                    TxtDoc = ConfigurationManager.AppSettings["getTxtDocReportMT"];
                }

                string docNo = string.Format("{0:0000}", int.Parse(ActivityFormCommandHandler.getActivityDoc(typeForm, "","").FirstOrDefault().docNo));
                int rtn = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertActivityRepDetail"
                    , new SqlParameter[] {
                        new SqlParameter("@id",id)
                        ,new SqlParameter("@statusId",(int)AppCode.ApproveStatus.รออนุมัติ)
                        ,new SqlParameter("@typeForm",TxtDoc)
                        ,new SqlParameter("@actNo",docNo)
                        ,new SqlParameter("@startDate",DateTime.ParseExact(startDate,"MM/dd/yyyy",null))
                        ,new SqlParameter("@endDate",DateTime.ParseExact(endDate,"MM/dd/yyyy",null))
                        ,new SqlParameter("@reference","")
                        ,new SqlParameter("@customerId",model.actFormRepDetailLists.FirstOrDefault().customerId)
                        ,new SqlParameter("@productTypeId",productTypeId)
                        ,new SqlParameter("@delFlag",false)
                        ,new SqlParameter("@createdDate",DateTime.Now)
                        ,new SqlParameter("@createdByUserId",UtilsAppCode.Session.User.empId)
                        ,new SqlParameter("@updatedDate",DateTime.Now)
                        ,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
                    });

                if (rtn > 0)
                {
                    string actIdTemp = "";
                    foreach (RepDetailModel.actFormRepDetailModel item in model.actFormRepDetailLists)
                    {
                        if (actIdTemp != item.id)
                        {
                            SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertRepDetailMapActForm"
                            , new SqlParameter[] {
                                new SqlParameter("@id",Guid.NewGuid().ToString())
                                ,new SqlParameter("@repDetailId",id)
                                ,new SqlParameter("@actFormId",item.id)
                                ,new SqlParameter("@delFlag",false)
                                ,new SqlParameter("@createdDate",DateTime.Now)
                                ,new SqlParameter("@createdByUserId",UtilsAppCode.Session.User.empId)
                                ,new SqlParameter("@updatedDate",DateTime.Now)
                                ,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
                            });
                        }
                        actIdTemp = item.id;
                    }
                }
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception("insertActivityRepDetail >>" + ex.Message);
            }
        }
        public static int insertApproveForReportDetail(string customerId, string productTypeId, string actId, string typeForm)
        {
            try
            {
                int rtn = 0;
                ApproveFlowModel.approveFlowModel flowModel = new ApproveFlowModel.approveFlowModel();

                if (typeForm == Activity_Model.activityType.MT.ToString())
                {
                    flowModel = ApproveFlowAppCode.getFlowForReportDetail(
                    ConfigurationManager.AppSettings["subjectReportDetailId"]
                    , customerId
                    , productTypeId);
                }
                else
                {
                    flowModel = ApproveFlowAppCode.getFlowForReportDetailOMT(
                   ConfigurationManager.AppSettings["subjectReportDetailId"]
                   , customerId
                   , productTypeId);
                }


                if (ApproveAppCode.insertApproveByFlow(flowModel, actId) > 0)
                {
                    rtn = ApproveAppCode.updateApproveWaitingByRangNo(actId);
                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception("insertApproveForReportDetail >>" + ex.Message);
            }
        }

    }
}