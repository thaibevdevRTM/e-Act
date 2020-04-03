using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using eForms.Models.MasterData;
using Microsoft.ApplicationBlocks.Data;

namespace eForms.Presenter.MasterData
{
    public class ProvincePresenter: BasePresenter
    {
        public static List<ProvinceModel> getProvince(string strConn)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_getProvinceMaster");
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ProvinceModel()
                             {
                                 id = dr["id"].ToString(),
                                 nameTH = dr["nameTH"].ToString()
                             }).ToList();
                return lists;
            }
            catch(Exception ex)
            {
                throw new Exception("getProvince >>" + ex.Message);
            }
        }
    }
}
