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
        public static List<ManagentFlowModel.flowSubject> getFlowApproveByEmpId(string strCon ,string empId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strCon, CommandType.StoredProcedure, "usp_getFlowApproveByEmpId"
                    , new SqlParameter("@empId", empId));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new ManagentFlowModel.flowSubject()
                             {
                                 flowId = d["flowId"].ToString(),
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

                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getFlowApproveByEmpId => " + ex.Message);
                return new List<ManagentFlowModel.flowSubject>();
            }
        }

      
    }
}
