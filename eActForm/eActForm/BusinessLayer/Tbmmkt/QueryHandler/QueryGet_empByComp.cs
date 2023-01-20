using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebLibrary;
using static eActForm.Models.ActUserModel;

namespace eActForm.BusinessLayer
{
    public class QueryGet_empByComp
    {
        public static List<RequestEmpModel> getEmpByComp(string companyId, bool langEn)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getEmpByComp"
                     , new SqlParameter("@empCompanyId", companyId));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new RequestEmpModel()
                             {
                                 id = d["id"].ToString(),
                                 empId = d["empId"].ToString(),
                                 level = d["empLevel"].ToString(),
                                 empName = d["empFNameTH"].ToString() + " " + d["empLNameTH"].ToString(),
                                 position = d["empPositionTitleTH"].ToString(),
                                 department = d["empDepartmentTH"].ToString(),
                                 bu = d["empDivisionTH"].ToString(),
                                 empNameEN = d["empFNameEN"].ToString() + " " + d["empLNameEN"].ToString(),
                                 positionEN = d["empPositionTitleEN"].ToString(),
                                 departmentEN = d["empDepartmentEN"].ToString(),
                                 buEN = d["empDivisionEN"].ToString(),
                             });
                return lists.OrderBy(x => x.empName).ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getEmpByComp => " + ex.Message);
                return new List<RequestEmpModel>();
            }
        }

        public static List<RequestEmpModel> getEmpByDepartment(string companyId, string department)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getEmpByDepartment"
                     , new SqlParameter("@companyId", companyId)
                      , new SqlParameter("@department", department));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new RequestEmpModel()
                             {
                                 empId = d["empId"].ToString(),
                                 empName = d["empName"].ToString(),
                                 departmentEN = d["empDepartmentEN"].ToString(),
                             });
                return lists.OrderBy(x => x.empName).ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getEmpByDepartment => " + ex.Message);
                return new List<RequestEmpModel>();
            }
        }

        public static List<User> getEmpGroupByChannelId(string channelId, string master_type_form_id)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getEmpGroupByChannel"
                     , new SqlParameter("@channelId", channelId)
                      , new SqlParameter("@master_type_form_id", master_type_form_id));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new User()
                             {
                                 empId = d["empId"].ToString(),
                                 empFNameTH = d["empFNameTH"].ToString(),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getEmpGroupByChannelId => " + ex.Message);
                return new List<User>();
            }
        }
    }
}