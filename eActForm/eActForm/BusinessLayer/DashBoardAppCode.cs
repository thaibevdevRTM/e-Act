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
                             select new DashBoardModel.infoDashBoardModel() {
                                 countApprove = (int)dr["countApprove"],
                                 countCreated = (int)dr["countCreated"],
                                 countWaitingApprove = (int)dr["countWaitingApprove"],
                                 countOverSLA = (int)dr["countOverSLA"]
                             }).ToList();
                return lists;

            }catch(Exception ex)
            {
                throw new Exception("getInfoDashBoard >> " + ex.Message);
            }
        }
    }
}