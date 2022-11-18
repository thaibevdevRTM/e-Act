using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using OfficeOpenXml.ConditionalFormatting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebLibrary;
using static eActForm.Models.ActUserModel;

namespace eActForm.BusinessLayer
{
    public class QueryGet_empDetailById
    {
        public static List<RequestEmpModel> getEmpDetailById(string empId)
        {
            try
            {
                //DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getEmpDetaitById"
                //     , new SqlParameter("@empId", empId));
                ResponseUserAPI response = null;
                try
                {
                    if (!string.IsNullOrEmpty(empId))
                    {
                        response = AuthenAppCode.doAuthenInfo(empId);
                    }

                }
                catch
                {

                }

                if (response != null)
                {
                    var lists = (from User d in response.userModel
                                 select new RequestEmpModel()
                                 {
                                     empId = d.empId,
                                     empName = d.empFNameTH + " " + d.empLNameTH,
                                     position = d.empPositionTitleTH,
                                     level = d.empLevel,
                                     department = d.empDepartmentTH,
                                     bu = d.empDivisionTH,
                                     companyName = "บริษัท " + d.empCompanyNameTH,
                                     empNameEN = d.empFNameEN + " " + d.empLNameEN,
                                     positionEN = d.empPositionTitleEN,
                                     departmentEN = d.empDepartmentEN,
                                     buEN = d.empDivisionEN,
                                     companyNameEN = d.empCompanyName,
                                     compId = d.empCompanyId,
                                     email = d.empEmail,
                                     hireDate = d.empProbationEndDate,
                                     empGroupName = d.empGroupName,
                                     empGroupNameTH = d.empGroupNameTH

                                 });
                    return lists.OrderBy(x => x.empName).ToList();
                }
                else {
                    return null;

                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getEmpDetailById => " + ex.Message);
                return new List<RequestEmpModel>();
            }
        }

        public static List<RequestEmpModel> getEmpDetailFlowById(string empId, string typeFormId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getEmpDetaitFlowById"
                     , new SqlParameter("@empId", empId)
                      , new SqlParameter("@typeFormId", typeFormId));

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return getEmpDetailById(empId);
                }
                else
                {
                    // emp can't map with flow
                    return new List<RequestEmpModel>();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getEmpDetailFlowById => " + ex.Message);
                return new List<RequestEmpModel>();
            }
        }

    }
}