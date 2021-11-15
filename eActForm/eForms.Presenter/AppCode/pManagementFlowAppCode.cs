using eForms.Models.MasterData;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLibrary;

namespace eForms.Presenter.AppCode
{
    public class pManagementFlowAppCode
    {
        public static List<ManagentFlowModel.flowSubject> getFlowApproveByEmpId(string strCon, string empId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strCon, CommandType.StoredProcedure, "usp_getFlowbyEmpId"
                    , new SqlParameter("@empId", empId));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new ManagentFlowModel.flowSubject()
                             {
                                 flowApproveId = d["flowApproveId"].ToString(),
                                 subjectName = d["subjectName"].ToString(),
                                 empId = d["empId"].ToString(),
                                 chanelGroup = d["chanelGroup"].ToString(),
                                 channelId = d["channelId"].ToString(),
                                 brandName = d["brandName"].ToString(),
                                 productBrandId = d["productBrandId"].ToString(),
                                 departmentId = d["departmentId"].ToString(),
                                 cateName = d["cateName"].ToString(),
                                 productCatId = d["productCatId"].ToString(),
                                 companyId = d["companyId"].ToString(),
                                 companyName = d["companyName"].ToString(),
                                 rangNo = d["rangNo"].ToString(),
                                 cusNameTH = d["cusNameTH"].ToString(),

                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getFlowApproveByEmpId => " + ex.Message);
                return new List<ManagentFlowModel.flowSubject>();
            }
        }

        public static int updateSwapByApproveId(string strCon, string approveId, string empId,string updateBy)
        {
            try
            {
                int result = 0;
                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_updateSwapByApproveId"
                     , new SqlParameter[] {new SqlParameter("@empId",empId)
                     ,new SqlParameter("@approveId",approveId)
                     ,new SqlParameter("@updateByUserId",updateBy)
                     });


                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("updateSwapByApproveId >>" + ex.Message);
            }
        }

        public static List<ManagentFlowModel.flowSubject> getFlowbyEmpId(string strCon,  string empId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strCon, CommandType.StoredProcedure, "usp_getFlowbyEmpId"
                    , new SqlParameter("@empId", empId));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new ManagentFlowModel.flowSubject()
                             {
                                 flowId = d["flowId"].ToString(),
                                 subjectName = d["subjectName"].ToString(),
                                 companyName = d["companyNameTH"].ToString(),
                                 limitName = d["limitName"].ToString(),

                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getFlowbyEmpId => " + ex.Message);
                return new List<ManagentFlowModel.flowSubject>();
            }
        }

        public static int updateCompanyFlowByEmpId(string strCon,string flowId,string companyId, string empId,string updatedByUserId)
        {
            try
            {
                int result = 0;
                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_updateFlowCompanyByEmpId"
                     , new SqlParameter[] {new SqlParameter("@flowId",flowId)
                     ,new SqlParameter("@companyId",companyId)
                     ,new SqlParameter("@empId",empId)
                     ,new SqlParameter("@updatedByUserId",updatedByUserId)
                     });


                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("updateCompanyFlowByEmpId >>" + ex.Message);

            }
        }
    }
}
