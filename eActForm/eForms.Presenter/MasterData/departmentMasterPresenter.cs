﻿using eForms.Models.MasterData;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace eForms.Presenter.MasterData
{
    public class departmentMasterPresenter
    {
        public static List<departmentMasterModel> getdepartmentMaster(string strConn, string companyId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_getdepartmentMaster", new SqlParameter("@companyId", companyId));
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new departmentMasterModel()
                             {
                                 id = dr["id"].ToString(),
                                 name = dr["name"].ToString(),
                                 companyId = dr["companyId"].ToString(),
                                 delFlag = bool.Parse(dr["delFlag"].ToString()),
                                 createdDate = DateTime.Parse(dr["createdDate"].ToString()),
                                 createdByUserId = dr["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(dr["updatedDate"].ToString()),
                                 updatedByUserId = dr["updatedByUserId"].ToString()
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getdepartmentMaster >>" + ex.Message);
            }
        }

    }
}