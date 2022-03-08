using eActForm.BusinessLayer.Appcodes;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using eForms.Presenter.AppCode;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using static eForms.Models.MasterData.ImportBudgetControlModel;

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
                             select new ApproveModel.approveDetailModel(dr["empId"].ToString())
                             {
                                 empId = dr["empId"].ToString(),
                                 empName = dr["empName"].ToString(),
                                 empName_EN = dr["empName_EN"].ToString(),
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

                if (typeForm == Activity_Model.activityType.MT.ToString() || typeForm == Activity_Model.activityType.OMT.ToString())
                {
                    strCall = "usp_getActivityCustomersFormByEmpId";
                    if (isAdmin())
                    {
                        strCall = "usp_getActivityFormAll";
                    }
                }
                else if (typeForm == Activity_Model.activityType.SetPrice.ToString())
                {
                    strCall = "usp_getActivitySetPriceByEmpId";
                    if (isAdmin())
                    {
                        strCall = "usp_getActivitySetPriceFormAll";
                    }
                }
                else if (typeForm == Activity_Model.activityType.SetPriceOMT.ToString())
                {
                    strCall = "usp_getActivitySetPriceOMTByEmpId";
                    if (isAdmin() && typeForm == Activity_Model.activityType.SetPriceOMT.ToString())
                    {
                        strCall = "usp_getActivitySetPriceOMTFormAll";
                    }
                }
                else if (typeForm == Activity_Model.activityType.ITForm.ToString())
                {
                    strCall = "usp_getActivityFormByEmpId_ITForm";
                    if (isAdmin())
                    {
                        strCall = "usp_getActivityFormByEmpId_ITForm_Admin";
                    }
                }
                else if (typeForm == Activity_Model.activityType.HCForm.ToString())
                {
                    strCall = "usp_getActivityFormByEmpId_HCPomNum";
                    if (isAdmin())
                    {
                        strCall = "usp_getActivityFormAll_HCPomNum";
                    }
                }
                else if (typeForm == Activity_Model.activityType.EXPENSE.ToString())
                {
                    strCall = "usp_getActivityExpenseEntertainByEmpId";
                    if (isAdmin())
                    {
                        strCall = "usp_getActivityFormAll_Expense";
                    }
                }
                else
                {
                    strCall = "usp_tbm_getActivityFormByEmpId";
                    if (isAdmin())
                    {
                        strCall = "usp_getActivityFormAll";
                    }
                }
                
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, strCall
                , new SqlParameter[] {
                         new SqlParameter("@empId", UtilsAppCode.Session.User.empId)
                        ,new SqlParameter("@startDate", startDate)
                        ,new SqlParameter("@endDate", endDate)
                        ,new SqlParameter("@companyId",BaseAppCodes.getCompanyIdByactivityType(typeForm))
                });

                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new Activity_Model.actForm(dr["createdByUserId"].ToString())
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
                                 master_type_form_id = dr["master_type_form_id"].ToString(),
                                 companyId = BaseAppCodes.getCompanyIdByactivityType(typeForm),
                                 channelId = dr["channelId"].ToString(),
                                 brandId = dr["productBrandId"].ToString(),
                             }).ToList();




                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getActFormByEmpId >> " + ex.Message);
            }
        }


        public static List<Activity_Model.actForm> getActFormRejectByEmpId()
        {
            try
            {

                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getActRejectByEmpId"
                , new SqlParameter[] {
                         new SqlParameter("@empId", UtilsAppCode.Session.User.empId)
                });

                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new Activity_Model.actForm(dr["createdByUserId"].ToString())
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
                                 master_type_form_id = dr["master_type_form_id"].ToString(),

                             }).ToList();

                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getActFormRejectByEmpId >> " + ex.Message);
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
                || UtilsAppCode.Session.User.isAdminPOM
                || UtilsAppCode.Session.User.isAdminCVM
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
            try
            {
                List<ActUserModel.UserAuthorized> lst = new List<ActUserModel.UserAuthorized>();
                lst = UserAppCode.GetUserAuthorizedsByCompany(typeComp);
                return lst.Count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw new Exception("checkGrpCompByUser >>" + ex.Message);
            }
        }
        public static bool checkGrpComp(string compId, string typeComp)
        {
            try
            {
                List<TB_Act_Other_Model> lst = new List<TB_Act_Other_Model>();
                lst = QueryOtherMaster.getOhterMaster("company", typeComp).ToList();
                if (lst.Count > 0)
                {
                    lst = lst.Where(x => x.val1 == compId).ToList();
                }
                return lst.Count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw new Exception("checkGrpComp >>" + ex.Message);
            }
        }

        public static bool checkGroupApproveByUser(string actId, string groupApprove)
        {
            try
            {
                switch (groupApprove)
                {
                    case "Recorder":
                        groupApprove = AppCode.ApproveGroup.Recorder;
                        break;
                    case "PettyCashVerify":
                        groupApprove = AppCode.ApproveGroup.PettyCashVerify;
                        break;
                    default:
                        break;
                }

                ApproveModel.approveModels models = new ApproveModel.approveModels();
                models = ApproveAppCode.getApproveByActFormId(actId, "");

                if (models.approveDetailLists.Count > 0)
                {
                    models.approveDetailLists = models.approveDetailLists
                        .Where(x => x.approveGroupId == groupApprove)
                        .Where(x => x.empId == UtilsAppCode.Session.User.empId).ToList();
                }
                return models.approveDetailLists.Count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw new Exception("checkGroupApproveByUser >>" + ex.Message);
            }

        }
        public static bool checkCanEditByUser(string actId)
        {
            try
            {
                ApproveModel.approveModels models = new ApproveModel.approveModels();
                models = ApproveAppCode.getApproveByActFormId(actId, "");

                if (models.approveDetailLists.Count > 0)
                {
                    models.approveDetailLists = models.approveDetailLists
                        .Where(x => x.approveGroupId == AppCode.ApproveGroup.Recorder || x.approveGroupId == AppCode.ApproveGroup.PettyCashVerify)
                        .Where(x => x.empId == UtilsAppCode.Session.User.empId).ToList();
                }
                return models.approveDetailLists.Count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw new Exception("checkCanEditByUser >>" + ex.Message);
            }

        }

        public static string getGrpCompByCompId(string compId)
        {
            try
            {
                string grpComp = "";
                List<TB_Act_Other_Model> lst = new List<TB_Act_Other_Model>();
                lst = AdminUserAppCode.getCompany();

                if (lst.Count > 0)
                {
                    lst = lst.Where(x => x.val1 == compId).ToList();
                }
                if (lst.Count > 0)
                {
                    if (string.IsNullOrEmpty(lst[0].subType))
                    {
                        grpComp = lst[0].displayVal;
                    }
                    else
                    {
                        grpComp = lst[0].subType;
                    }


                }
                return grpComp;
            }
            catch (Exception ex)
            {
                throw new Exception("getGrpCompByCompId >>" + ex.Message);
            }
        }
        public static bool checkFormAddTBDetailOther(string masterForm)
        {
            bool check = false;
            if (ConfigurationManager.AppSettings["masterEmpExpense"] == masterForm
                || ConfigurationManager.AppSettings["formSetPriceMT"] == masterForm
                || ConfigurationManager.AppSettings["formSetPriceOMT"] == masterForm
                || ConfigurationManager.AppSettings["formReceptions"] == masterForm)
            {
                check = true;
            }

            return check;
        }

        public static Activity_TBMMKT_Model addDataToDetailOther(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            try
            {
                activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther = new TB_Act_ActivityForm_DetailOther();
                activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.productBrandId = activity_TBMMKT_Model.activityFormTBMMKT.BrandlId;
                activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.channelId = activity_TBMMKT_Model.activityFormTBMMKT.channelId;

                //ค่าที่ insert จะไปยัดใน tB_Act_ActivityForm_DetailOther อีกที ไม่งั้น Get Flow ไม่ได้ ???????
                activity_TBMMKT_Model.activityFormTBMMKT.SubjectId = ApproveFlowAppCode.getMainFlowByMasterTypeId(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id).FirstOrDefault().subjectId;
                //activity_TBMMKT_Model.activityFormTBMMKT.objective = QueryGet_master_type_form.get_master_type_form(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id).FirstOrDefault().nameForm;

                if (ConfigurationManager.AppSettings["masterEmpExpense"] == activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id ||
                    ConfigurationManager.AppSettings["formReceptions"] == activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id)
                {
                    activity_TBMMKT_Model.activityFormModel.documentDateStr = BaseAppCodes.converStrToDatetimeWithFormat(activity_TBMMKT_Model.activityFormModel.documentDateStr + "-" + DateTime.Today.ToString("dd"), "yyyy-MM-dd").ToString("dd/MM/yyyy");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("addDataToDetailOther >>" + ex.Message);
            }
            return activity_TBMMKT_Model;
        }

        public static List<BudgetControlModels> getBalanceByEO(string EO, string companyId, string getActTypeId, string channelId, string brandId, string activityId,string fiscalYear)
        {
            try
            {
               DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetBalanceByEONew"
                   , new SqlParameter[] { new SqlParameter("@EO", EO)
               ,new SqlParameter("@companyId", companyId)
               ,new SqlParameter("@actTypeId", getActTypeId)
               ,new SqlParameter("@channelId", channelId)
               ,new SqlParameter("@brandId", brandId)
               ,new SqlParameter("@activityId", activityId)
               ,new SqlParameter("@fiscalYear", fiscalYear)});
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new BudgetControlModels
                             {
                                 balance = string.IsNullOrEmpty(dr["balance"].ToString()) ? 0 : (decimal?)dr["balance"],
                                 balanceTotal = string.IsNullOrEmpty(dr["balanceTotal"].ToString()) ? 0 : (decimal?)dr["balanceTotal"],
                                 amountTotal = string.IsNullOrEmpty(dr["amountTotal"].ToString()) ? 0 : (decimal?)dr["amountTotal"],
                                 amount = string.IsNullOrEmpty(dr["amountEvent"].ToString()) ? 0 : (decimal?)dr["amountEvent"],
                                 EO = dr["EO"].ToString(),
                                 LE = dr["LE"] is DBNull ? 0 : int.Parse(dr["LE"].ToString()),
                                 totalBudgetChannel = string.IsNullOrEmpty(dr["totalBrandChannel"].ToString()) ? 0 : (decimal?)dr["totalBrandChannel"],
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getBalanceByEO >>" + ex.Message);
            }

        }


        public static List<BudgetControlModels> getAmountReturn(string EO, string channelId, string brandId,string actTypeId,string fiscalYear)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getAmountReturnChannel"
               , new SqlParameter[] { new SqlParameter("@channelId", channelId)
               ,new SqlParameter("@brandId", brandId)
               ,new SqlParameter("@EO", EO)
               ,new SqlParameter("@fiscalYear", fiscalYear)
               ,new SqlParameter("@actTypeId", actTypeId)});
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new BudgetControlModels
                             {
                                 returnAmount = string.IsNullOrEmpty(dr["returnAmount"].ToString()) ? 0 : (decimal?)dr["returnAmount"],
                                 returnAmountBrand = string.IsNullOrEmpty(dr["returnBrand"].ToString()) ? 0 : (decimal?)dr["returnBrand"],
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                return new List<BudgetControlModels>();
                throw new Exception("getAmountReturn >>" + ex.Message);
            }

        }


        public static List<BudgetControlModels> getAmountReturnByEOIO(string EO, string IO)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getAmountReturnByEOIO"
               , new SqlParameter[] { new SqlParameter("@EO", EO)});
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new BudgetControlModels
                             {
                                 returnAmount = string.IsNullOrEmpty(dr["returnAmount"].ToString()) ? 0 : (decimal?)dr["returnAmount"],

                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                return new List<BudgetControlModels>();
                throw new Exception("getAmountReturn >>" + ex.Message);
            }

        }

        public static int insertReserveBudget(string activityId)
        {
            try
            {
                int rtn = 0;
                rtn = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_ReserveBudgetControl"
                    , new SqlParameter[] { new SqlParameter("@activityId", activityId)
                    , new SqlParameter("@createByUser" ,UtilsAppCode.Session.User.empId)});
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception("insertReserveBudget >>" + ex.Message);
            }

        }
    }
}