using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    public class QueryGet_ReqEmpByActivityId
    {
        public static List<RequestEmpModel> getReqEmpByActivityId(string activityId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getRequestEmpByActivityId"
                     , new SqlParameter("@activityId", activityId));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new RequestEmpModel()
                             {
                                 id = d["id"].ToString(),
                                 rowNo = Convert.ToInt32(d["rowNo"].ToString()),
                                 empId = d["empId"].ToString(),
                                 empName = "คุณ" + d["empFNameTH"].ToString() + " " + d["empLNameTH"].ToString(),
                                 position = d["empPositionTitleTH"].ToString(),
                                 level = d["empLevel"].ToString(),
                                 department = d["empDepartmentTH"].ToString(),
                                 bu = d["empDivisionTH"].ToString(),
                             });
                return lists.OrderBy(x => x.rowNo).ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getAllActivityGroup => " + ex.Message);
                return new List<RequestEmpModel>();
            }
        }
    }
}