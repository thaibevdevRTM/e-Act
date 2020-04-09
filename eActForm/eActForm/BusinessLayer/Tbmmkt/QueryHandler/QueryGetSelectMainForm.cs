using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebLibrary;
using static eActForm.Controllers.GetDataMainFormController;

namespace eActForm.BusinessLayer
{
    public class QueryGetSelectMainForm
    {
        public static List<GetDataEO> GetQueryDataEOPaymentVoucher(ObjGetDataEO objGetDataEO)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getDataEOPaymentVoucher", new SqlParameter("@fiscalYear", objGetDataEO.fiscalYear), new SqlParameter("@master_type_form_id", objGetDataEO.master_type_form_id), new SqlParameter("@productBrandId", objGetDataEO.productBrandId), new SqlParameter("@channelId", objGetDataEO.channelId));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new GetDataEO()
                             {
                                 EO = d["EO"].ToString(),
                                 activityId = d["activityId"].ToString(),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("GetQueryDataEOPaymentVoucher => " + ex.Message);
                return new List<GetDataEO>();
            }
        }

    }
}