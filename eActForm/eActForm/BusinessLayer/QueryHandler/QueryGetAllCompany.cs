using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebLibrary;

namespace eActForm.BusinessLayer.QueryHandler
{
    public class QueryGetAllCompany
    {
        public static List<CompanyModel> getAllCompany()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getAllCompany");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new CompanyModel()
                             {
                                 id = d["id"].ToString(),
                                 companyId = d["companyId"].ToString(),
                                 companyNameEN = d["companyNameEN"].ToString(),
                                 companyNameTH = d["companyNameTH"].ToString(),

                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getAllCompany => " + ex.Message);
                return new List<CompanyModel>();
            }
        }

        public static List<CompanyModel> getCompanyByTypeFormId(string typeFormId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getCompanyByTypeFormId"
                    , new SqlParameter("@typeFormId", typeFormId));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new CompanyModel()
                             {
                                 id = d["id"].ToString(),
                                 companyId = d["companyId"].ToString(),
                                 companyNameEN = d["companyNameEN"].ToString(),
                                 companyNameTH = d["companyNameTH"].ToString(),

                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getCompanyByTypeFormId => " + ex.Message);
                return new List<CompanyModel>();
            }
        }
    }
}