using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    public class QueryGet_empDetailById
    {
        public static List<RequestEmpModel> getEmpDetailById(string empId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getEmpDetaitById"
                     , new SqlParameter("@empId", empId));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new RequestEmpModel()
                             {
                                 id = d["id"].ToString(),
                                 empId = d["empId"].ToString(),
                                 empName = d["empFNameTH"].ToString() + " " + d["empLNameTH"].ToString(),
                                 position = d["empPositionTitleTH"].ToString(),
                                 level = d["empLevel"].ToString(),
                                 department = d["empDepartmentTH"].ToString(),
                                 bu = d["empDivisionTH"].ToString(),
                                 companyName = "บริษัท" + d["companyNameTH"].ToString(),

                                 empNameEN = d["empFNameEN"].ToString() + " " + d["empLNameEN"].ToString(),
                                 positionEN = d["empPositionTitleEN"].ToString(),
                                 departmentEN = d["empDepartmentEN"].ToString(),
                                 buEN = d["empDivisionEN"].ToString(),
                                 companyNameEN = d["companyNameEN"].ToString(),
                                 compId = d["empCompanyId"].ToString(),
                             });
                return lists.OrderBy(x => x.empName).ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getEmpDetailById => " + ex.Message);
                return new List<RequestEmpModel>();
            }
        }
    }
}