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
    public class QueryGetGL
    {
        public static string getGLTypeByEmpGroupName()
        {
            try
            {
                if (UtilsAppCode.Session.User.empGroupName.Equals("Spirits Product"))
                {
                    return AppCode.GLType.GLSale;
                }
                else
                {
                    return AppCode.GLType.GLSaleSupport;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("getGLTypeByEmpGroupName >> " + ex.Message);
            }
        }
        private static string getDivisionIdByEmpGroupName()
        {
            try
            {
                if (UtilsAppCode.Session.User.empGroupName.Equals("Spirits Product"))
                {
                    return AppCode.Division.sales;
                }
                else
                {
                    return AppCode.Division.salesSupport;
                }
            }catch(Exception ex)
            {
                throw new Exception("getDivisionIdByEmpGroupName >> " + ex.Message);
            }
        }
        public static List<GetDataGL> getGLMasterByDivisionId()
        {
            try
            {
                string divisionId = getDivisionIdByEmpGroupName();
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getDataGLByDivisionId"
                    , new SqlParameter("@divisionId", divisionId));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new GetDataGL()
                             {
                                 id = d["id"].ToString(),
                                 GL = d["GL"].ToString(),
                                 groupGL = d["GroupGL"].ToString(),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("GetGLMasterByDivisionId => " + ex.Message);
                return new List<GetDataGL>();
            }
        }
    }
}