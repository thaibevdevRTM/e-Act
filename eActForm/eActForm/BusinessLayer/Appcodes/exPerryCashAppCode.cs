using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.BusinessLayer.Appcodes
{
    public class exPerryCashAppCode
    {
        public static Activity_TBMMKT_Model getMaster(Activity_TBMMKT_Model model)
        {
            try
            {
                //model.requestEmpModel = QueryGet_empDetailById.getEmpDetailById(UtilsAppCode.Session.User.empId);
                //model.exPerryCashList = getCashPosition(UtilsAppCode.Session.User.empId);
                //model.exPerryCashModel.rulesCash = getCashPosition(UtilsAppCode.Session.User.empId).Where(x => x.cashLimitId.Equals("87757B5B-C946-4001-A74B-AB6C9003AD25")).FirstOrDefault().cash;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getMaster" + ex.Message);
            }

            return model;
        }

        public static List<exPerryCashModel> getCashPosition(string empId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getLimitPerryCash"
                    , new SqlParameter[] { new SqlParameter("@empId", empId) });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new exPerryCashModel()
                             {
                                 cashLimitId = dr["cashLimitId"].ToString(),
                                 cashName = dr["cashName"].ToString(),
                                 positionId = dr["positionId"].ToString(),
                                 positionName = dr["positionName"].ToString(),
                                 cash = decimal.Parse(dr["cash"].ToString()),
                                 empId = dr["empId"].ToString(),
                             }).ToList();

                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getCashPosition >> " + ex.Message);
            }
        }

        public static List<exPerryCashModel> getApproveExpenseListsByEmpId()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getApproveExpenseByEmpId"
                    , new SqlParameter[] { new SqlParameter("@empId", UtilsAppCode.Session.User.empId) });
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var lists = (from DataRow dr in ds.Tables[0].Rows
                                 select new exPerryCashModel()
                                 {
                                     id = dr["id"].ToString(),
                                     actNo = dr["statusId"].ToString(),
                                     detail = dr["statusName"].ToString(),
                                     status = dr["productTypeName"].ToString(),
                                     startDate = dr["startDate"] is DBNull ? null : (DateTime?)dr["startDate"],
                                     activityNo = dr["actNo"].ToString(),
                                     createName = dr["createByName"].ToString(),
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
                    return new List<exPerryCashModel>();
                }


            }
            catch (Exception ex)
            {
                throw new Exception("getApproveSummaryDetailListsByEmpId >>" + ex.Message);
            }
        }

        public static Activity_TBMMKT_Model prepareDataExpense_AddNew(Activity_TBMMKT_Model activity_TBMMKT_Model, string activityId)
        {
            try
            {
                activity_TBMMKT_Model.activityFormModel.id = Guid.NewGuid().ToString();
                //get empid from เงินทดรอง
                activity_TBMMKT_Model.activityFormTBMMKT.empId = ApproveAppCode.getApproveByActFormId(activityId).approveDetailLists.FirstOrDefault().empId;
                activity_TBMMKT_Model.activityFormTBMMKT.statusId = 1;
                activity_TBMMKT_Model.activityFormTBMMKT.reference = activity_TBMMKT_Model.activityFormTBMMKT.activityNo;
                activity_TBMMKT_Model.activityFormTBMMKT.activityNo = "";
                activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id = ConfigurationManager.AppSettings["masterEmpExpense"];
                activity_TBMMKT_Model.activityFormTBMMKT.benefit = activity_TBMMKT_Model.totalCostThisActivity.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("prepareDataExpense >>" + ex.Message);
            }
            return activity_TBMMKT_Model;
        }

        public static Activity_TBMMKT_Model processDataExpense(Activity_TBMMKT_Model activity_TBMMKT_Model, string activityId)
        {
            try
            {
                if (!checkActExpense(activity_TBMMKT_Model.activityFormTBMMKT.activityNo))
                {
                    if (ConfigurationManager.AppSettings["masterEmpExpense"] != activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id)
                    {
                        // case เอกสารใหม่
                        activity_TBMMKT_Model = prepareDataExpense_AddNew(activity_TBMMKT_Model, activityId);
                    }
                    else
                    {
                        // case คลิกจาก document ตรง
                        activity_TBMMKT_Model.totalCostThisActivity = !string.IsNullOrEmpty(activity_TBMMKT_Model.activityFormTBMMKT.benefit) ? decimal.Parse(activity_TBMMKT_Model.activityFormTBMMKT.benefit) : 0;
                    }
                }
                else
                {
                    // case คลิกจาก ยืมเงินทดรอง ต้องใช้ ActNo เพื่อ Get Data
                    activity_TBMMKT_Model = ActivityFormTBMMKTCommandHandler.getDataForEditActivityByActNo(activity_TBMMKT_Model.activityFormTBMMKT.activityNo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("processDataExpense >>" + ex.Message);
            }
            return activity_TBMMKT_Model;
        }

        public static bool checkActExpense(string actNo)
        {
            bool result = false;
            try
            {
                if (QueryGetActivityByActNo.getCheckRefActivityByActNo(actNo).Any())
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("checkActExpense >>" + ex.Message);
            }
            return result;
        }




        public static Activity_TBMMKT_Model addDataToDetailOther(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            //activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther = new TB_Act_ActivityForm_DetailOther();
            try
            {
                activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther = new TB_Act_ActivityForm_DetailOther();
                activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.productBrandId = activity_TBMMKT_Model.activityFormTBMMKT.BrandlId;
                activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.channelId = activity_TBMMKT_Model.activityFormTBMMKT.channelId;
                //activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.SubjectId = ApproveFlowAppCode.getMainFlowByMasterTypeId(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id).FirstOrDefault().subjectId;

                //ค่าที่ insert จะไปยัดใน tB_Act_ActivityForm_DetailOther อีกที
                activity_TBMMKT_Model.activityFormTBMMKT.SubjectId = ApproveFlowAppCode.getMainFlowByMasterTypeId(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id).FirstOrDefault().subjectId;
                activity_TBMMKT_Model.activityFormTBMMKT.objective = QueryGet_master_type_form.get_master_type_form(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id).FirstOrDefault().nameForm;
                activity_TBMMKT_Model.activityFormModel.documentDateStr = BaseAppCodes.converStrToDatetimeWithFormat(activity_TBMMKT_Model.activityFormModel.documentDateStr + "-01", "yyyy-MM-dd").ToString("dd/MM/yyyy") ; 
                
            }
            catch (Exception ex)
            {
                throw new Exception("addDataToDetailOther >>" + ex.Message);
            }
            return activity_TBMMKT_Model;
        }


        public static List<RequestEmpModel> getEmpByChannel(string subjectId, string channelId,string filter)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getEmpbyChannelId"
                    , new SqlParameter[] { new SqlParameter("@subjectId", subjectId),
                                           new SqlParameter("@channelId", channelId)
                    });
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var lists = (from DataRow dr in ds.Tables[0].Rows
                                 select new RequestEmpModel()
                                 {
                                     empId = dr["empId"].ToString(),
                                     empName = dr["empId"].ToString() + "  "+ dr["empName"].ToString(),             
                                 }).ToList();

                    lists = !string.IsNullOrEmpty(filter) ? lists.Where(x => x.empId.Contains(filter)).ToList() : lists;
                    return lists;
                }
                else
                {
                    return new List<RequestEmpModel>();
                }


            }
            catch (Exception ex)
            {
                throw new Exception("getApproveSummaryDetailListsByEmpId >>" + ex.Message);
            }
        }


    }
}