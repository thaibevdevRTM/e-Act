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
                else if (typeForm == Activity_Model.activityType.EXPENSE.ToString())
                {
                    strCall = "usp_getExpensePerryFormByEmpId";
                }
                else
                {
                    strCall = "usp_tbm_getActivityFormByEmpId";
                }

                if (isAdmin())
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


        public static bool _isOtherCompanyMT()
        {
            return UtilsAppCode.Session.User.empCompanyId == ConfigurationManager.AppSettings["companyId_MT"] ||
                UtilsAppCode.Session.User.empCompanyId == ConfigurationManager.AppSettings["companyId_OMT"] ? false : true;
        }
        public static bool isOtherCompanyMTOfDoc(string compId)
        {
            return compId == ConfigurationManager.AppSettings["companyId_MT"] ||
               compId == ConfigurationManager.AppSettings["companyId_OMT"] ? false : true;
        }
        public static bool isOtherCompanyMTOfDocByActId(string actId)
        {
            string compId = "";
            if (actId != "")
            {
                compId = QueryGetActivityByIdTBMMKT.getActivityById(actId).FirstOrDefault().companyId;
            }
            return compId == ConfigurationManager.AppSettings["companyId_MT"] ||
                 compId == ConfigurationManager.AppSettings["companyId_OMT"] ? false : true;
        }
        public static bool isAdmin()
        {
            return
                UtilsAppCode.Session.User.isAdminOMT
                || UtilsAppCode.Session.User.isAdmin
                || UtilsAppCode.Session.User.isAdminTBM
                || UtilsAppCode.Session.User.isAdminHCM
                || UtilsAppCode.Session.User.isAdminNUM
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
            if (val == ConfigurationManager.AppSettings["normal"])
                val = "1d8110";
            else if (val == ConfigurationManager.AppSettings["urgently"])
                val = "fba222";
            else if (val == ConfigurationManager.AppSettings["veryUrgently"])
                val = "f80014";

            return val;
        }
        public static bool checkGrpCompByUser(string typeComp)
        {
            List<ActUserModel.UserAuthorized> lst = new List<ActUserModel.UserAuthorized>();
            lst = UserAppCode.GetUserAuthorizedsByCompany(typeComp);
            return lst.Count > 0 ? true : false;

        }
        public static bool checkGrpComp(string compId, string typeComp)
        {
       List<ActUserModel.UserAuthorized> lst = new List<ActUserModel.UserAuthorized>();
            lst = UserAppCode.GetUserAuthorizedsByCompany(Activity_Model.activityType.NUM.ToString());
            return lst.Count > 0 ? true : false;

        }
    }
}