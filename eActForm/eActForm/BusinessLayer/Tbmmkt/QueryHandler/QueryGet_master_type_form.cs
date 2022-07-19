using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    public class QueryGet_master_type_form
    {
        public static List<Master_type_form_Model> get_master_type_form(string master_type_form_id)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_get_master_type_form"
                  , new SqlParameter("@master_type_form_id", master_type_form_id));

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new Master_type_form_Model()
                              {
                                  id = d["id"].ToString(),
                                  nameForm = d["nameForm"].ToString(),
                                  nameForm_EN = d["nameForm_EN"].ToString(),
                                  department = d["department"].ToString(),
                                  useIn = d["useIn"].ToString(),
                                  companyId = d["companyId"].ToString(),
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
                ExceptionManager.WriteError("get_master_type_form => " + ex.Message);
                return new List<Master_type_form_Model>();
            }
        }

        public static List<Master_type_form_Model> getmastertypeformByEmpId(string empId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getmastertypeformByEmpId"
                  , new SqlParameter("@empId", empId));

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new Master_type_form_Model()
                              {
                                  id = d["id"].ToString(),
                                  nameForm = d["nameForm"].ToString(),
                                  nameForm_EN = d["nameForm_EN"].ToString(),
                                  department = d["department"].ToString(),
                                  useIn = d["useIn"].ToString(),
                                  companyId = d["companyId"].ToString(),
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
                ExceptionManager.WriteError("get_master_type_form => " + ex.Message);
                return new List<Master_type_form_Model>();
            }
        }

    }
}