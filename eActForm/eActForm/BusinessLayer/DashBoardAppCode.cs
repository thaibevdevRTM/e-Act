using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;

namespace eActForm.BusinessLayer
{
    public class DashBoardAppCode
    {
        public static List<DashBoardModel.infoDashBoardModel> getInfoDashBoard()
        {
            try
            {

                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getDashBoardInfo"
                    , new SqlParameter[] {
                        new SqlParameter("@empId",UtilsAppCode.Session.User.empId)
                    });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new DashBoardModel.infoDashBoardModel()
                             {
                                 countApprove = (int)dr["countApprove"],
                                 countCreated = (int)dr["countCreated"],
                                 countWaitingApprove = (int)dr["countWaitingApprove"],
                                 countOverSLA = (int)dr["countOverSLA"]
                             }).ToList();
                return lists;

            }
            catch (Exception ex)
            {
                throw new Exception("getInfoDashBoard >> " + ex.Message);
            }
        }

        public static List<DashBoardModel.infoMonthTotalSpending> getInfoMonthSpending()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getDashBoardMonthTotalSpending");
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new DashBoardModel.infoMonthTotalSpending()
                             {
                                 monthDate = dr["monthDate"].ToString(),
                                 sumTotal = dr["totalSpending"] is DBNull ? 0 : decimal.ToInt32( (decimal)dr["totalSpending"]),
                             }).ToList();
                return lists;

            }
            catch (Exception ex)
            {
                throw new Exception("getInfoMonthSpending >> " + ex.Message);
            }
        }
        public static List<DashBoardModel.infoGroupCustomerSpending> getInfoGroupCustomerSpending()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getDashBoardGroupCustomerSpending");
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new DashBoardModel.infoGroupCustomerSpending()
                             {
                                 customerId = dr["customerId"].ToString(),
                                 customerName = dr["cusName"].ToString(),
                                 sumSpending = dr["sumSpending"] is DBNull ? 0 : (decimal)dr["sumSpending"],
                             }).ToList();
                return lists;

            }
            catch (Exception ex)
            {
                throw new Exception("getInfoGroupCustomerSpending >> " + ex.Message);
            }
        }
    }
}