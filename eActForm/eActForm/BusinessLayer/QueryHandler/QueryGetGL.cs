using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    public class QueryGetGL
    {
        public static string getGL(List<GetDataGL> glList,string glCodeId ,string empId)
        {
            try
            {
                string rtn = "";
                AppCode.Expenses expenseEnum = new AppCode.Expenses(empId);
                var list = glList.Where(x => x.id == glCodeId);
                if (list != null && list.Count() > 0)
                {
                    rtn = expenseEnum.groupName.Equals("Spirits Product") ? list.FirstOrDefault().GLSale : list.FirstOrDefault().GL;
                }
                return rtn;

            }
            catch(Exception ex)
            {
                throw new Exception("getGL >> " + ex.Message);
            }
        }
        public static List<GetDataGL> getGLMasterByDivisionId(string divisionId)
        {
            try
            {
                
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getDataGLByDivisionId"
                    , new SqlParameter("@divisionId", divisionId));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new GetDataGL()
                             {
                                 id = d["id"].ToString(),
                                 GL = d["GL"].ToString(),
                                 GLSale = d["GLSales"].ToString(),
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