﻿using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    public class QueryGetAllHospital
    {

        public static List<HospitalModel> getAllHospital()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getAllHospital");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new HospitalModel()
                             {
                                 id = d["id"].ToString(),
                                 hospNameEN = d["hospNameEN"].ToString(),
                                 hospNameTH = d["hospNameTH"].ToString(),
                                 provName = d["provName"].ToString(),
                                 region = d["hospNameTH"].ToString(),
                                 hospTypeId = d["hospTypeId"].ToString(),
                                 hospTypeName = d["hospTypeName"].ToString(),
                                 percentage = int.Parse(d["percentage"].ToString()),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getActivityById => " + ex.Message);
                return new List<HospitalModel>();
            }
        }

    }
}