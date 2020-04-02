using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
namespace eActConsoleService
{
    public class AppCode
    {
        public static List<ApproveModel.approveWaitingModel> getAllWaitingApproveGroupByEmpId()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(Properties.Settings.Default.strConn, CommandType.StoredProcedure, "usp_getCountWaitingApproveGroupByEmpId");
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ApproveModel.approveWaitingModel()
                             {
                                 empId = dr["empId"].ToString()
                                 ,
                                 waitingCount = dr["waitingCount"].ToString()
                                 ,
                                 empPrefix = dr["empPrefix"].ToString()
                                 ,
                                 empFNameTH = dr["empFNameTH"].ToString()
                                 ,
                                 empLNameTH = dr["empLNameTH"].ToString()
                                 ,
                                 empEmail = dr["empEmail"].ToString()
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getAllWaitingApproveGroupByEmpId >> " + ex.Message);
            }
        }
    }
}
