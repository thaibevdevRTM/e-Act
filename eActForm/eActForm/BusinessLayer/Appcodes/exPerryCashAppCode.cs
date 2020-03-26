﻿using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
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
            catch(Exception ex)
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
    }
}