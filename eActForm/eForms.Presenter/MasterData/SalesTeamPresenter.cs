using eForms.Models.MasterData;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;

namespace eForms.Presenter.MasterData
{
    public class SalesTeamPresenter : BasePresenter
    {
        public static List<SalesTeamCVMModel> getSalesTeamCVM(string strConn,string provinceId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_getAmphuresMaster"
                    ,new SqlParameter[] {new SqlParameter("@provinceId", provinceId) });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new SalesTeamCVMModel()
                             {
                                 id = dr["id"].ToString(),
                                 nameTH = dr["nameTH"].ToString()
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getSalesTeamCVM >>" + ex.Message);
            }
        }
    }
}
