using eActForm.BusinessLayer.Appcodes;
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
    public class ActFormAppCode
    {
        public static int updateWaitingCancel(string actId, string remark, string statusNote)
        {
            try
            {
                int rtn = 0;
                rtn = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_updateWaitingCancelFlagActivityForm"
                    , new SqlParameter[] { new SqlParameter("@actId", actId)
                    ,new SqlParameter("@remark",remark)
                    ,new SqlParameter("@updateBy",UtilsAppCode.Session.User.empId)
                    ,new SqlParameter("@updateDate",DateTime.Now)
                    ,new SqlParameter("@statusNote",statusNote)
                    });
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool checkActInvoice(string actId)
        {
            try
            {
                bool result = false;
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_checkRowActivityInvoice"
                    , new SqlParameter[] { new SqlParameter("@actId", actId) });
                if (ds.Tables[0].Rows.Count > 0)
                {
                    result = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("checkActInvoice >>" + ex.Message);
            }
        }
        public static int deleteActForm(string actId, string remark, string statusNote)
        {
            try
            {
                int rtn = 0;
                rtn = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_updateDelFlagActivityForm"
                    , new SqlParameter[] { new SqlParameter("@actId", actId)
                    ,new SqlParameter("@remark",remark)
                    ,new SqlParameter("@updateBy",UtilsAppCode.Session.User.empId)
                    ,new SqlParameter("@updateDate",DateTime.Now)
                    ,new SqlParameter("@statusNote",statusNote)
                    });
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception("deleteActForm >>" + ex.Message);
            }
        }
        public static List<ApproveModel.approveDetailModel> getUserCreateActForm(string actId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getUserCreateActivityForm"
                    , new SqlParameter[] { new SqlParameter("@actFormId", actId) });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ApproveModel.approveDetailModel()
                             {
                                 empId = dr["empId"].ToString(),
                                 empName = dr["empName"].ToString(),
                                 empName_EN = dr["empName_EN"].ToString(),
                                 empEmail = dr["empEmail"].ToString(),
                                 activityNo = dr["activityNo"].ToString(),
                                 companyName = dr["companyName"].ToString(),
                                 companyNameEN = dr["companyNameEN"].ToString(),
                                 delFlag = (bool)dr["delFlag"],
                                 createdDate = (DateTime?)dr["createdDate"],
                                 createdByUserId = dr["createdByUserId"].ToString(),
                                 updatedDate = (DateTime?)dr["updatedDate"],
                                 updatedByUserId = dr["updatedByUserId"].ToString()
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getUserCreateActForm >>" + ex.Message);
            }
        }


        public static List<Activity_Model.actForm> getActFormByEmpId(DateTime startDate, DateTime endDate, string typeForm)
        {
            try
            {
                string strCall = "";

                if (typeForm == Activity_Model.activityType.MT.ToString())
                {
                    strCall = "usp_getActivityCustomersFormByEmpId";
                }
                else if (typeForm == Activity_Model.activityType.OMT.ToString())
                {
                    strCall = "usp_tbm_getActivityFormByEmpId";
                }
                else
                {
                    strCall = "usp_tbm_getActivityFormByEmpId";
                }


                if (UtilsAppCode.Session.User.isAdminOMT || UtilsAppCode.Session.User.isAdmin ||
                UtilsAppCode.Session.User.isSuperAdmin || UtilsAppCode.Session.User.isAdminTBM || UtilsAppCode.Session.User.isAdminHCM)
                {
                    strCall = "usp_getActivityFormAll";
                }

                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, strCall
                , new SqlParameter[] {
                         new SqlParameter("@empId", UtilsAppCode.Session.User.empId)
                        ,new SqlParameter("@startDate", startDate)
                        ,new SqlParameter("@endDate", endDate)
                        ,new SqlParameter("@companyId",BaseAppCodes.getCompanyIdByactivityType(typeForm))
                });

                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new Activity_Model.actForm()
                             {
                                 id = dr["id"].ToString(),
                                 statusId = dr["statusId"].ToString(),
                                 statusName = dr["statusName"].ToString(),
                                 activityNo = dr["activityNo"].ToString(),
                                 documentDate = dr["documentDate"] is DBNull ? null : (DateTime?)dr["documentDate"],
                                 reference = dr["reference"].ToString(),
                                 customerId = dr["customerId"].ToString(),
                                 channelName = dr["channelName"].ToString(),
                                 productTypeId = dr["productTypeId"].ToString(),
                                 productTypeNameEN = dr["nameEN"].ToString(),
                                 cusShortName = dr["cusShortName"].ToString(),
                                 productCategory = dr["productCateText"].ToString(),
                                 productGroupid = dr["productGroupId"].ToString(),
                                 groupName = dr["groupName"].ToString(),
                                 activityPeriodSt = dr["activityPeriodSt"] is DBNull ? null : (DateTime?)dr["activityPeriodSt"],
                                 activityPeriodEnd = dr["activityPeriodEnd"] is DBNull ? null : (DateTime?)dr["activityPeriodEnd"],
                                 costPeriodSt = dr["costPeriodSt"] is DBNull ? null : (DateTime?)dr["costPeriodSt"],
                                 costPeriodEnd = dr["costPeriodEnd"] is DBNull ? null : (DateTime?)dr["costPeriodEnd"],
                                 activityName = dr["activityName"].ToString(),
                                 theme = dr["theme"].ToString(),
                                 objective = dr["objective"].ToString(),
                                 trade = dr["trade"].ToString(),
                                 activityDetail = dr["activityDetail"].ToString(),
                                 delFlag = (bool)dr["delFlag"],
                                 createdDate = (DateTime?)dr["createdDate"],
                                 createdByUserId = dr["createdByUserId"].ToString(),
                                 updatedDate = (DateTime?)dr["updatedDate"],
                                 updatedByUserId = dr["updatedByUserId"].ToString(),
                                 normalCost = dr["normalCost"] is DBNull ? 0 : (decimal?)dr["normalCost"],
                                 themeCost = dr["themeCost"] is DBNull ? 0 : (decimal?)dr["themeCost"],
                                 totalCost = dr["totalCost"] is DBNull ? 0 : (decimal?)dr["totalCost"],
                                 createByUserName = dr["createByUserName"].ToString(),
                                 master_type_form_id = dr["master_type_form_id"].ToString(),

                             }).ToList();




                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getActFormByEmpId >> " + ex.Message);
            }
        }

        public static string getDigitGroup(string activityTypeId)
        {
            try
            {
                string result = "";
                result = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.id == activityTypeId).FirstOrDefault().digit_Group;
                return result;
            }
            catch (Exception ex)
            {
                return "";
                throw new Exception("getDigitGroup >>" + ex.Message);
            }
        }

        public static string getDigitRunnigGroup(string productId)
        {
            string result = "";
            try
            {
                result = QueryGetAllProduct.getProductById(productId).FirstOrDefault().digit_IO;
                return result;
            }
            catch (Exception ex)
            {
                return result;
                throw new Exception("getDigitGroup >>" + ex.Message);
            }
        }


        public static string convertThaiBaht(decimal? txbaht)
        {
            string result = "";
            try
            {
                result = GreatFriends.ThaiBahtText.ThaiBahtTextUtil.ThaiBahtText(txbaht);
                return result;
            }
            catch (Exception ex)
            {
                return result;
                throw new Exception("convertThaiBaht >>" + ex.Message);
            }
        }


        public static bool isOtherCompanyMT()
        {
            return UtilsAppCode.Session.User.empCompanyId == ConfigurationManager.AppSettings["companyId_TBM"] ||
                UtilsAppCode.Session.User.empCompanyId == ConfigurationManager.AppSettings["companyId_HCM"] ? true : false;
        }
        public static bool isOtherCompanyMTOfDoc(string compId)
        {
            return compId == ConfigurationManager.AppSettings["companyId_TBM"] ||
               compId == ConfigurationManager.AppSettings["companyId_HCM"] ? true : false;
        }
        public static bool isAdmin()
        {
            return
                UtilsAppCode.Session.User.isAdminOMT
                || UtilsAppCode.Session.User.isAdmin
                || UtilsAppCode.Session.User.isAdminTBM
                || UtilsAppCode.Session.User.isAdminHCM
                || UtilsAppCode.Session.User.isSuperAdmin ? true : false;
        }

        public static string getStatusNote(string actId)
        {
            try
            {
                string statusNote = "";
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getStatusNote"
                    , new SqlParameter[] { new SqlParameter("@actId", actId) });
                if (ds.Tables[0].Rows.Count > 0)
                {
                    statusNote = ds.Tables[0].Rows[0]["statusNote"].ToString();
                }
                return statusNote;
            }
            catch (Exception ex)
            {
                throw new Exception("getStatusNote >>" + ex.Message);
            }
        }
        public static string getStatusNeedDocColor(string val)
        {
            if (val == "AA21FB12-B80A-4A0B-B9CA-78B8D7899D44")
                val = "1d8110";
            else if (val == "D0298BD9-ACC2-4434-955F-40E8A7EE810D")
                val = "fba222";
            else if (val == "B3EDA054-49C8-4EEF-83F2-EB0D9F39AB02")
                val = "f80014";

            return val;
        }
    }
}