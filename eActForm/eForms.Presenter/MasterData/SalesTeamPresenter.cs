﻿using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using eForms.Models.MasterData;

namespace eForms.Presenter.MasterData
{
    public class SalesTeamPresenter : BasePresenter
    {
        public static List<SalesTeamCVMModel> getSalesTeamCVM(string strConn,string provinceId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_getSalesTeamCVMMaster"
                    , new SqlParameter[] {new SqlParameter("@provinceId", provinceId) });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new SalesTeamCVMModel()
                             {
                                 id = dr["saleTeamId"].ToString(),
                                 nameTH = dr["nameTH"].ToString()
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getSalesTeamCVM >>" + ex.Message);
            }
        }

        public static List<SalesTeamCVMModel>getSalesTeamCVMBySaleTeamId(string strConn, string saleTeamId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_getSalesTeamCVMMasterBySaleTeamId"
                    , new SqlParameter[] { new SqlParameter("@saleTeamId", saleTeamId) });
                var list = (from DataRow dr in ds.Tables[0].Rows
                            select new SalesTeamCVMModel()
                            {
                                id = dr["id"].ToString(),
                                nameTH = dr["nameTH"].ToString(),
                                telContact = dr["telContact"].ToString(),
                                emailContact = dr["emailContact"].ToString(),
                                nameContact = dr["nameContact"].ToString(),

                                address = dr["address"].ToString(),
                                latitude = dr["latitude"].ToString(),
                                longitude = dr["longitude"].ToString(),

                                telCashier = dr["telManager"].ToString(),
                                emailCashier = dr["emailManager"].ToString(),
                                nameCashier = dr["nameCashier"].ToString()
                            }).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception("getSalesTeamCVM >>" + ex.Message);
            }
        }
    }
}
