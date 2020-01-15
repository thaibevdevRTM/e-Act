﻿using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    public class QueryGetSelectAllTB_Reg_Subject
    {
        public static List<TB_Reg_Subject> GetAllQueryGetSelectAllTB_Reg_Subject()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "uspgetAllSubject");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Reg_Subject()
                             {
                                 id = d["id"].ToString(),
                                 companyId = d["companyId"].ToString(),
                                 nameTH = d["nameTH"].ToString(),
                                 nameEN = d["nameEN"].ToString(),
                                 description = d["description"].ToString(),
                                 master_type_form_id = d["master_type_form_id"].ToString(),
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
                ExceptionManager.WriteError("GetAllQueryGetSelectAllTB_Reg_Subject => " + ex.Message);
                return new List<TB_Reg_Subject>();
            }
        }
    }
}