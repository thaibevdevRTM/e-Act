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
                                 activityIdAndEO = DocumentsAppCode.formatValueSelectEO_PVForm(d["activityId"].ToString(), d["EO"].ToString())
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("GetQueryDataEOPaymentVoucher => " + ex.Message);
                return new List<GetDataEO>();
            }
        }
        public static List<GetDataIO> GetQueryDataIOPaymentVoucher(ObjGetDataIO objGetDataIO)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getDataIOPaymentVoucher", new SqlParameter("@ActivityByEOSelect", objGetDataIO.ActivityByEOSelect), new SqlParameter("@EOSelect", objGetDataIO.EOSelect));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new GetDataIO()
                             {
                                 IO = d["IO"].ToString(),
                                 totalPayByIO = d["totalPayByIO"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["totalPayByIO"].ToString())),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("GetQueryDataIOPaymentVoucher => " + ex.Message);
                return new List<GetDataIO>();
            }
        }
        public static List<GetDataGL> GetQueryDataGLPaymentVoucher(ObjGetDataGL objGetDataGL)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getDataGLPaymentVoucher", new SqlParameter("@IOCode", objGetDataGL.IOCode), new SqlParameter("@SubGroupCode", objGetDataGL.SubGroupCode));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new GetDataGL()
                             {
                                 GL = d["GL"].ToString()
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("GetQueryDataIOPaymentVoucher => " + ex.Message);
                return new List<GetDataGL>();
            }
        }

        public static List<GetDataPVPrevious> GetQueryDataPVPrevious(ObjGetDataPVPrevious objGetGetDataPVPrevious)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_GetQueryDataPVPrevious", new SqlParameter("@master_type_form_id", objGetGetDataPVPrevious.master_type_form_id), new SqlParameter("@payNo", objGetGetDataPVPrevious.payNo));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new GetDataPVPrevious()
                             {
                                 activityNo = d["activityNo"].ToString(),
                                 activityId = d["activityId"].ToString(),
                                 payNo = d["payNo"].ToString(),
                                 statusId = d["statusId"].ToString(),
                                 totalallPayByIO = d["totalallPayByIO"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["totalallPayByIO"].ToString())),
                                 totalallPayNo = d["totalallPayNo"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["totalallPayNo"].ToString()))
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("GetQueryDataPVPrevious => " + ex.Message);
                return new List<GetDataPVPrevious>();
            }
        }

        public static List<GetDataDetailPaymentAll> GetDetailPaymentAll(ObjGetDataDetailPaymentAll objGetDataDetailPaymentAll)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_GetDetailPaymentAll", new SqlParameter("@activityId", objGetDataDetailPaymentAll.activityId), new SqlParameter("@payNo", objGetDataDetailPaymentAll.payNo));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new GetDataDetailPaymentAll()
                             {
                                 payNo = d["payNo"].ToString(),
                                 rowNo = int.Parse(d["rowNo"].ToString()),
                                 IO = d["IO"].ToString(),
                                 productDetail = d["productDetail"].ToString(),
                                 vat = d["vat"].ToString(),
                                 normalCost = d["normalCost"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["normalCost"].ToString())),
                                 documentDate = DateTime.Parse(d["documentDate"].ToString()),
                                 totalnormalCostEstimate = d["totalnormalCostEstimate"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["totalnormalCostEstimate"].ToString())),
                                 activityNo = d["activityNo"].ToString(),
                                 activityId = d["activityId"].ToString(),
                                 activityIdNoSub = d["activityIdNoSub"].ToString()
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("GetDetailPaymentAll => " + ex.Message);
                return new List<GetDataDetailPaymentAll>();
            }
        }


        public static List<DataRequesterToShow> GetDataRequesterToShow(string activityId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_GetDataRequesterToShow", new SqlParameter("@activityId", activityId));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new DataRequesterToShow()
                             {
                                 empName = d["empName"].ToString(),
                                 empId = d["empId"].ToString(),
                                 empDepartment = d["empDepartment"].ToString(),
                                 empPhone = d["empPhone"].ToString(),
                                 empCompany = d["empCompany"].ToString(),
                                 empEmail = d["empEmail"].ToString(),
                                 languageDoc = d["languageDoc"].ToString()
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("GetDataRequesterToShow => " + ex.Message);
                return new List<DataRequesterToShow>();
            }
        }

    }
}