using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebLibrary;

namespace eActForm.BusinessLayer.QueryHandler
{
    public class QueryGetSubject
    {
        public static List<TB_Reg_Subject_Model> getAllSubject()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getAllSubject");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Reg_Subject_Model()
                             {
                                 id = d["id"].ToString(),
                                 companyId = d["companyId"].ToString(),
                                 nameTH = d["nameTH"].ToString(),
                                 nameEn = d["nameEn"].ToString(),
                                 description = d["description"].ToString(),
                                 typeFormId = d["master_type_form_id"].ToString(),
                                 delFlag = bool.Parse(d["delFlag"].ToString()),
                                 createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                 createdByUserId = d["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                 updatedByUserId = d["updatedByUserId"].ToString(),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getAllSubject => " + ex.Message);
                return new List<TB_Reg_Subject_Model>();
            }
        }


        public static List<TB_Reg_Subject_Model> getAllSubjectByFlowCompany(string companyId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getAllSubjectByFlowCompany"
                     , new SqlParameter[] { new SqlParameter("@companyId", companyId)
                     });
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Reg_Subject_Model()
                             {
                                 id = d["id"].ToString(),
                                 companyId = d["companyId"].ToString(),
                                 nameTH = d["nameTH"].ToString(),
                                 nameEn = d["nameEn"].ToString(),
                                 description = d["description"].ToString(),
                                 typeFormId = d["master_type_form_id"].ToString(),
                                 delFlag = bool.Parse(d["delFlag"].ToString()),
                                 createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                 createdByUserId = d["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                 updatedByUserId = d["updatedByUserId"].ToString(),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getAllSubject => " + ex.Message);
                return new List<TB_Reg_Subject_Model>();
            }
        }
    }
}