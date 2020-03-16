using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace eActForm.BusinessLayer
{
    public class SignatureAppCode
    {
        public static int signatureInsert(DataTable dt)
        {
            try
            {
                int rtn = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    rtn += SqlHelper.ExecuteNonQueryTypedParams(AppCode.StrCon, "uspInsertSignature", dr);
                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception("signatureInsert >>" + ex.Message);
            }
        }

        public static ActSignatureModel.SignModels currentSignatureByEmpId(string empId)
        {
            try
            {
                ActSignatureModel.SignModels models = new ActSignatureModel.SignModels();
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, "usp_getSignatureByEmpId"
                    , new SqlParameter[] { new SqlParameter("@empId", empId) });
                models.lists = (from DataRow d in ds.Tables[0].Rows
                                select new ActSignatureModel.SignModel()
                                {
                                    id = d["id"].ToString(),
                                    empId = d["empId"].ToString(),
                                    signature = (d["signature"] == null || d["signature"] is DBNull) ? new byte[0] : (byte[])d["signature"],
                                    delFlag = (bool)d["delFlag"],
                                    createdByUserId = d["createdByUserId"].ToString(),
                                    createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                    updatedByUserId = d["updatedByUserId"].ToString(),
                                    updatedDate = DateTime.Parse(d["updatedDate"].ToString())
                                }).ToList();
                return models;
            }
            catch (Exception ex)
            {
                throw new Exception("signatureGetByEmpId >> " + ex.Message);
            }
        }

        public static List<ActSignatureModel.SignModel> signatureGetByEmpId(string empId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, "usp_getSignatureListsByEmpId"
                    , new SqlParameter[] { new SqlParameter("@empId", empId) });
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new ActSignatureModel.SignModel()
                             {
                                 id = d["id"].ToString(),
                                 empId = d["empId"].ToString(),
                                 signature = (d["signature"] == null || d["signature"] is DBNull) ? new byte[0] : (byte[])d["signature"],
                                 delFlag = (bool)d["delFlag"],
                                 createdByUserId = d["createdByUserId"].ToString(),
                                 createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                 updatedByUserId = d["updatedByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(d["updatedDate"].ToString())
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("signatureGetByEmpId >> " + ex.Message);
            }
        }
    }
}