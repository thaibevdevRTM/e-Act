﻿using eActForm.BusinessLayer.Appcodes;
using eActForm.Controllers;
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
    public class QueryGet_ReqEmpByActivityId
    {
        public static List<RequestEmpModel> getReqEmpByActivityId(string activityId, bool langEn, bool typeForm = false)
        {
            try
            {
                string strore = typeForm ? "usp_getRequestEmpFlowByActivityId" : "usp_getRequestEmpByActivityId";
                BaseAppCodes.getEmpFromApi("");
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, strore
                     , new SqlParameter("@activityId", activityId));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new RequestEmpModel()
                             {
                                 id = d["id"].ToString(),
                                 rowNo = Convert.ToInt32(d["rowNo"].ToString()),
                                 empId = d["empId"].ToString(),
                                 empName = !langEn ? "คุณ" + d["empFNameTH"].ToString() + " " + d["empLNameTH"].ToString() : d["empFNameEN"].ToString() + " " + d["empLNameEN"].ToString(),
                                 position = !langEn ? d["empPositionTitleTH"].ToString() : d["empPositionTitleEN"].ToString(),
                                 level = d["empLevel"].ToString(),
                                 department = !langEn ? d["empDepartmentTH"].ToString() : d["empDepartmentEN"].ToString(),
                                 bu = !langEn ? d["empDivisionTH"].ToString() : d["empDivisionEN"].ToString(),
                                 empNameEN = d["empFNameEN"].ToString() + " " + d["empLNameEN"].ToString(),
                                 positionEN = d["empPositionTitleEN"].ToString(),
                                 departmentEN = d["empDepartmentEN"].ToString(),
                                 buEN = d["empDivisionEN"].ToString(),
                                 empTel = d["empTel"].ToString(),
                                 companyName = d["companyNameTH"].ToString(),
                                 companyNameEN = d["companyNameEN"].ToString(),
                                 detail = d["detail"].ToString(),
                                 hireDate = DocumentsAppCode.convertDateTHToShowCultureDateEN(Convert.ToDateTime(BaseAppCodes.getEmpFromApi(d["empId"].ToString()).empProbationEndDate), ConfigurationManager.AppSettings["formatDateUse"]),//  empProbationEndDate
                                 //!string.IsNullOrEmpty(d["hireDate"].ToString()) ? DateTime.Parse(d["hireDate"].ToString()).ToString(ConfigurationManager.AppSettings["formatDateUse"]) : "",
                             });

                return lists.OrderBy(x => x.rowNo).ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getReqEmpByActivityId => " + ex.Message);
                return new List<RequestEmpModel>();
            }
        }

        public static List<RequestEmpModel> getReqEmpByMainTableActivityId(string activityId, bool langEn)
        {
            try
            {
                string strore = "usp_getReqEmpByMainTableActivityId";

                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, strore
                     , new SqlParameter("@activityId", activityId));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new RequestEmpModel()
                             {
                                 id = d["id"].ToString(),
                                 rowNo = Convert.ToInt32(d["rowNo"].ToString()),
                                 empId = d["empId"].ToString(),
                                 empName = !langEn ? "คุณ" + d["empFNameTH"].ToString() + " " + d["empLNameTH"].ToString() : d["empFNameEN"].ToString() + " " + d["empLNameEN"].ToString(),
                                 position = !langEn ? d["empPositionTitleTH"].ToString() : d["empPositionTitleEN"].ToString(),
                                 level = d["empLevel"].ToString(),
                                 department = !langEn ? d["empDepartmentTH"].ToString() : d["empDepartmentEN"].ToString(),
                                 bu = !langEn ? d["empDivisionTH"].ToString() : d["empDivisionEN"].ToString(),
                                 empNameEN = d["empFNameEN"].ToString() + " " + d["empLNameEN"].ToString(),
                                 positionEN = d["empPositionTitleEN"].ToString(),
                                 departmentEN = d["empDepartmentEN"].ToString(),
                                 buEN = d["empDivisionEN"].ToString(),
                                 empTel = d["empTel"].ToString(),
                                 companyName = d["companyNameTH"].ToString(),
                                 companyNameEN = d["companyNameEN"].ToString(),
                             });
                return lists.OrderBy(x => x.rowNo).ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getReqEmpByMainTableActivityId => " + ex.Message);
                return new List<RequestEmpModel>();
            }
        }
    }
}