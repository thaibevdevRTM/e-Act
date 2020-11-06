using eForms.Models.MasterData;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eForms.Presenter.AppCode
{
    public class pUserAppCode
    {
        public static int insertTokenByEmpId(string strCon, string empId, string tokenAccess, string tokenType)
        {
            try
            {
                int result = 0;
                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_insertUsersToken"
                     , new SqlParameter[] {new SqlParameter("@id",Guid.NewGuid())
                     ,new SqlParameter("@empId",empId)
                     ,new SqlParameter("@tokenAccess",tokenAccess)
                     ,new SqlParameter("@tokenType",tokenType)
                     ,new SqlParameter("@createDate",DateTime.Now) 
                     });
 

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("insertTokenByEmpId >>" + ex.Message);
            }
        }


        public static List<tokenModel> getTokenByEmpId(string strCon, string empId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strCon, CommandType.StoredProcedure, "usp_getUsersToken"
                     , new SqlParameter[] { new SqlParameter("@empId", empId) }) ;
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new tokenModel()
                             {
                                 id = d["id"].ToString(),
                                 empId = d["empId"].ToString(),
                                 tokenAccess = d["tokenAccess"].ToString(),
                                 tokenType = d["tokenType"].ToString(),
                                 createDate = DateTime.Parse(d["createDate"].ToString()),

                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("getTokenByEmpId >>" + ex.Message);
            }
        }
    }
}
