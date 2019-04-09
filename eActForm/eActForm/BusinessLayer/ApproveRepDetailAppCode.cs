using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using eActForm.Models;
namespace eActForm.BusinessLayer
{
    public class ApproveRepDetailAppCode
    {

        public static List<RepDetailModel.actApproveRepDetailModel> getApproveRepDetailListsByEmpId()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getApproveRepDetailFormByEmpId"
                    , new SqlParameter[] { new SqlParameter("@empId", UtilsAppCode.Session.User.empId) });

                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new RepDetailModel.actApproveRepDetailModel()
                             {
                                 id = dr["id"].ToString(),
                                 statusId = dr["statusId"].ToString(),
                                 statusName = dr["statusName"].ToString(),
                                 startDate = dr["startDate"] is DBNull ? null : (DateTime?)dr["startDate"],
                                 endDate = dr["endDate"] is DBNull ? null : (DateTime?)dr["endDate"],
                                 customerId = dr["customerId"].ToString(),
                                 productTypeId = dr["productTypeId"].ToString(),
                                 productTypeName = dr["productTypeName"].ToString(),
                                 customerName = dr["customerName"].ToString(),
                                 delFlag = (bool)dr["delFlag"],
                                 createdDate = (DateTime?)dr["createdDate"],
                                 createdByUserId = dr["createdByUserId"].ToString(),
                                 updatedDate = (DateTime?)dr["updatedDate"],
                                 updatedByUserId = dr["updatedByUserId"].ToString()
                             }).ToList();
                return lists;

            }catch(Exception ex)
            {
                throw new Exception("getApproveRepDetailListsByEmpId >>" + ex.Message);
            }
        }
        public static string insertActivityRepDetail(string customerId,string productTypeId,string startDate,string endDate)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertActivityRepDetail"
                    , new SqlParameter[] {
                        new SqlParameter("@id",id)
                        ,new SqlParameter("@statusId",(int)AppCode.ApproveStatus.รออนุมัติ)
                        ,new SqlParameter("@startDate",DateTime.ParseExact(startDate,"MM/dd/yyyy",null))
                        ,new SqlParameter("@endDate",DateTime.ParseExact(endDate,"MM/dd/yyyy",null))
                        ,new SqlParameter("@reference","")
                        ,new SqlParameter("@customerId",customerId)
                        ,new SqlParameter("@productTypeId",productTypeId)
                        ,new SqlParameter("@delFlag",false)
                        ,new SqlParameter("@createdDate",DateTime.Now)
                        ,new SqlParameter("@createdByUserId",UtilsAppCode.Session.User.empId)
                        ,new SqlParameter("@updatedDate",DateTime.Now)
                        ,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
                    });
                    return id;
            }
            catch(Exception ex)
            {
                throw new Exception("insertActivityRepDetail >>" + ex.Message);
            }
        }
        public static int insertApproveForReportDetail(string customerId, string productTypeId,string actId)
        {
            try
            {
                int rtn = 0;
                ApproveFlowModel.approveFlowModel flowModel = ApproveFlowAppCode.getFlowForReportDetail(
                    ConfigurationManager.AppSettings["subjectReportDetailId"]
                    , customerId
                    , productTypeId);
                if( ApproveAppCode.insertApproveByFlow(flowModel, actId) > 0)
                {
                    rtn = ApproveAppCode.updateApproveWaitingByRangNo(actId);
                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception("insertApproveForReportDetail >>" + ex.Message);
            }
        }

    }
}