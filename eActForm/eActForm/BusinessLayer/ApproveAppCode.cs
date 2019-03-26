using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using eActForm.Models;
using System.Configuration;
namespace eActForm.BusinessLayer
{
    public class ApproveAppCode
    {
        public static List<ApproveModel.approveWaitingModel> getAllWaitingApproveGroupByEmpId()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getCountWaitingApproveGroupByEmpId");
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ApproveModel.approveWaitingModel()
                             {
                                 empId = dr["empId"].ToString()
                                 ,waitingCount = dr["waitingCount"].ToString()
                                 ,empPrefix = dr["empPrefix"].ToString()
                                 ,empFNameTH = dr["empFNameTH"].ToString()
                                 ,empLNameTH = dr["empLNameTH"].ToString()
                                 ,empEmail = dr["empEmail"].ToString()
                             }).ToList();
                return lists;
            }
            catch(Exception ex)
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
        public static int updateApprove(string actFormId, string statusId, string remark)
        {
            try
            {
                int rtn = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_updateApprove"
                        , new SqlParameter[] {new SqlParameter("@actFormId",actFormId)
                    , new SqlParameter("@empId",UtilsAppCode.Session.User.empId)
                    ,new SqlParameter("@statusId",statusId)
                    ,new SqlParameter("@remark",remark)
                    ,new SqlParameter("@updateDate",DateTime.Now)
                    ,new SqlParameter("@updateBy",UtilsAppCode.Session.User.empId)
                        });

                if (statusId == ConfigurationManager.AppSettings["statusReject"])
                {
                    // update reject
                    rtn += updateActFormWithApproveReject(actFormId);
                }
                else
                {
                    // update approve
                    rtn += updateActFormWithApproveDetail(actFormId);

                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception("updateApprove >> " + ex.Message);
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
        public static int insertApprove(string actId)
        {
            try
            {
                int rtn = 0;
                if (getApproveByActFormId(actId).approveDetailLists.Count == 0 )
                {
                    List<ApproveModel.approveModel> list = new List<ApproveModel.approveModel>();
                    ApproveFlowModel.approveFlowModel flowModel = ApproveFlowAppCode.getFlowId(ConfigurationManager.AppSettings["subjectActivityFormId"], actId);
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
                            ,new SqlParameter("@delFlag",false)
                            ,new SqlParameter("@createdDate",DateTime.Now)
                            ,new SqlParameter("@createdByUserId",UtilsAppCode.Session.User.empId)
                            ,new SqlParameter("@updatedDate",DateTime.Now)
                            ,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
                            });
                    }
                }
                else rtn = 999; // alredy approve
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception("insertApprove >> " + ex.Message);
            }
        }
        public static ApproveModel.approveModels getApproveByActFormId(string actFormId)
        {
            try
            {
                ApproveModel.approveModels models = new ApproveModel.approveModels();
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getApproveDetailByActFormId"
                    , new SqlParameter[] { new SqlParameter("@actFormId", actFormId) });
                models.approveDetailLists = (from DataRow dr in ds.Tables[0].Rows
                                             select new ApproveModel.approveDetailModel()
                                             {
                                                 id = dr["id"].ToString(),
                                                 approveId = dr["approveId"].ToString(),
                                                 rangNo = (int)dr["rangNo"],
                                                 empId = dr["empId"].ToString(),
                                                 empName = dr["empName"].ToString(),
                                                 statusId = dr["statusId"].ToString(),
                                                 statusName = dr["statusName"].ToString(),
                                                 isSendEmail = (bool)dr["isSendEmail"],
                                                 remark = dr["remark"].ToString(),
                                                 signature = (dr["signature"] == null || dr["signature"] is DBNull) ? new byte[0] : (byte[])dr["signature"],
                                                 ImgName = string.Format(ConfigurationManager.AppSettings["rootgetSignaURL"], dr["empId"].ToString()),
                                                 delFlag = (bool)dr["delFlag"],
                                                 createdDate = (DateTime?)dr["createdDate"],
                                                 createdByUserId = dr["createdByUserId"].ToString(),
                                                 updatedDate = (DateTime?)dr["updatedDate"],
                                                 updatedByUserId = dr["updatedByUserId"].ToString(),

                                             }).ToList();

                
                if (models.approveDetailLists.Count > 0)
                {
                    ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getApproveByActFormId"
                   , new SqlParameter[] { new SqlParameter("@actFormId", actFormId) });

                    var empDetail = models.approveDetailLists.Where(r => r.empId == UtilsAppCode.Session.User.empId).ToList();
                    var lists = (from DataRow dr in ds.Tables[0].Rows
                                 select new ApproveModel.approveModel()
                                 {
                                     id = dr["id"].ToString(),
                                     flowId = dr["flowId"].ToString(),
                                     actFormId = dr["actFormId"].ToString(),
                                     statusId = (empDetail.Count > 0) ? empDetail.FirstOrDefault().statusId : "",
                                     delFlag = (bool)dr["delFlag"],
                                     createdDate = (DateTime?)dr["createdDate"],
                                     createdByUserId = dr["createdByUserId"].ToString(),
                                     updatedDate = (DateTime?)dr["updatedDate"],
                                     updatedByUserId = dr["updatedByUserId"].ToString(),
                                     isPermisionApprove = getPremisionApproveByEmpid(models.approveDetailLists, UtilsAppCode.Session.User.empId)
                                 }).ToList();

                    models.approveModel = lists[0];

                }

                return models;
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
                    ,new SqlParameter("@type",type.ToString()));
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


    }
}