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
                                 region = d["region"].ToString(),
                                 hospTypeId = d["hospTypeId"].ToString(),
                                 hospTypeName = d["hospTypeName"].ToString(),
                                 percentage = int.Parse(d["percentage"].ToString()),
                                 delFlag = d["delFlag"].ToString(),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("QueryGetAllHospital.getAllHospital => " + ex.Message);
                return new List<HospitalModel>();
            }
        }
        public static List<HospitalModel> getHospitalType()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getHospitalType");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new HospitalModel()
                             {
                                 id = d["id"].ToString(),
                                 percentage = int.Parse(d["percentage"].ToString()),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getHospitalType => " + ex.Message);
                return new List<HospitalModel>();
            }
        }
        public static List<HospitalModel> exportHospital()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_exportAllHospital");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new HospitalModel()
                             {
                                 id = d["id"].ToString(),
                                 hospNameTH = d["hospNameTH"].ToString(),
                                 provName = d["provName"].ToString(),
                                 region = d["region"].ToString(),
                                 percentage = int.Parse(d["percentage"].ToString()),
                                 delFlag = d["status"].ToString(),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("exportHospital => " + ex.Message);
                return new List<HospitalModel>();
            }
        }
    }
}