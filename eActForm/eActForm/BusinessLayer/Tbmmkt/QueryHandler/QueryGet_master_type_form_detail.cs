﻿using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    public class QueryGet_master_type_form_detail
    {
        public static List<Master_type_form_detail_Model> get_master_type_form_detail(string master_type_form_id)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_get_master_type_form_detail"
                 , new SqlParameter("@master_type_form_id", master_type_form_id));

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new Master_type_form_detail_Model()
                              {
                                  id = d["id"].ToString(),
                                  master_type_form_id = d["master_type_form_id"].ToString(),
                                  orderNo = int.Parse(d["orderNo"].ToString()),
                                  path_partial = d["path_partial"].ToString(),
                                  delFlag = bool.Parse(d["delFlag"].ToString()),
                                  createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                  createdByUserId = d["createdByUserId"].ToString(),
                                  updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                  updatedByUserId = d["updatedByUserId"].ToString(),
                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getActivityById => " + ex.Message);
                return new List<Master_type_form_detail_Model>();
            }
        }

    }
}