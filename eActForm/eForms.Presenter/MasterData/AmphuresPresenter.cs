using eForms.Models.MasterData;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace eForms.Presenter.MasterData
{
    public class AmphuresPresenter : BasePresenter
    {
        public List<AmphuresModel> getAmphures(string strConn, string provinceId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_getAmphuresMaster"
                    , new SqlParameter[] { new SqlParameter("@provinceId", provinceId) });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new AmphuresModel()
                             {
                                 id = dr["id"].ToString(),
                                 nameTH = dr["nameTH"].ToString()
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getAmphures >>" + ex.Message);
            }
        }
    }
}
