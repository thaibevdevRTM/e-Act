using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebLibrary;

namespace eActForm.BusinessLayer.QueryHandler
{
    public class QueryGetPosition
    {
        public static List<positionModel> getNewlinePosition()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure,
                 "usp_getNewlinePosition");

                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new positionModel()
                             {
                                 newPositionName = d["newPositionName"].ToString(),
                                 positionName = d["positionName"].ToString(),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getAllSubject => " + ex.Message);
                return new List<positionModel>();
            }
        }
    }
}