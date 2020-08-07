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
                ExceptionManager.WriteError("getAllActivityGroup => " + ex.Message);
                return new List<RequestEmpModel>();
            }
        }
    }
}