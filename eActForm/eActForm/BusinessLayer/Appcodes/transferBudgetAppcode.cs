using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace eActForm.BusinessLayer.Appcodes
{
    public class TransferBudgetAppcode
    {

        public static List<TransferBudgetModels> GetBudgetBalanceByEOIO(string EO, string IO)
        {
            try
            {

                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetBalanceByEOIO"
                    , new SqlParameter[] { new SqlParameter("@EO", EO),
                    new SqlParameter("@IO", IO)});
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new TransferBudgetModels
                             {
                                 activityId = dr["activityId"].ToString(),
                                 total = dr["total"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(dr["total"].ToString())),
                                 paymentBlance = dr["paymentBlance"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(dr["paymentBlance"].ToString())),
                             }).ToList();

                return lists.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("GetBudgetBalanceByEOIO>>" + ex.Message);
            }
        }

        public static List<TransferBudgetModels> GetIOByEO(string EO)
        {
            try
            {
                var retList = new List<string>();
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getIOByEO"
                    , new SqlParameter[] { new SqlParameter("@EO", EO) });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new TransferBudgetModels
                             {
                                 IO = dr["IO"].ToString(),

                             }).ToList();


                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("GetIOByEO>>" + ex.Message);
            }
        }

        public static List<TransferBudgetModels> GetBudgetBalanceNonEO(string brandId, string channelId)
        {
            try
            {

                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetBalanceNonEO"
                    , new SqlParameter[] { new SqlParameter("@brandId", brandId)
                    ,new SqlParameter("@channelId", channelId)});
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new TransferBudgetModels
                             {
                                 amount = dr["amount"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(dr["amount"].ToString())),
                                 EO = dr["EO"].ToString(),
                             }).ToList();

                return lists.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("GetBudgetBalanceNonEO>>" + ex.Message);
            }
        }

        public static bool transferBudgetAllApprove(string actId)
        {

            try
            {
                bool result = false;
                int rtn = 0;
                rtn = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_transferBudgetAllApprove"
                    , new SqlParameter[] { new SqlParameter("@activityId", actId) });
                if (rtn > 0)
                {
                    result = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("transferBudgetAllApprove >>" + ex.Message);
            }

        }
    }
}