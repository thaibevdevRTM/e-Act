﻿using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    //public class QueryGetInvoiceActivityStstus
    //{
    //    public static List<Budget_Activity_Model.Budget_Activity_Status_Att> getInvoiceActivityStstus()
    //    {
    //        try
    //        {
    //            DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getInvoiceActivityStstus");

    //            var result = (from DataRow d in ds.Tables[0].Rows
    //                          select new Budget_Activity_Model.Budget_Activity_Status_Att()
    //                          {
    //                              id = d["id"].ToString(),
    //                              nameEN = d["NameEN"].ToString(),
    //                              nameTH = d["NameTH"].ToString(),
    //                              description = d["description"].ToString(),
    //                              delFlag = bool.Parse(d["delFlag"].ToString()),
    //                              createdDate = DateTime.Parse(d["createdDate"].ToString()),
    //                              createdByUserId = d["createdByUserId"].ToString(),
    //                              updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
    //                              updatedByUserId = d["updatedByUserId"].ToString(),
    //                          });

    //            return result.ToList();
    //        }
    //        catch (Exception ex)
    //        {
    //            ExceptionManager.WriteError("getActivityByApproveStatusId => " + ex.Message);
    //            return new List<Budget_Activity_Model.Budget_Activity_Status_Att>();
    //        }
    //    }
    //}

}