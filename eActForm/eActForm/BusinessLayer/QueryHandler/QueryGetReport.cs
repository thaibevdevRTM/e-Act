using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebLibrary;

namespace eActForm.BusinessLayer.QueryHandler
{
    public class QueryGetReport
    {
        public static List<ReportTypeModel> getReportTypeByTypeFormId(string typeFormId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getReportTypeByTypeFormId"
                    , new SqlParameter("@typeFormId", typeFormId));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new ReportTypeModel()
                             {
                                 id = d["id"].ToString(),
                                 reportName = d["reportName"].ToString(),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getReportTypeByTypeFormId => " + ex.Message);
                return new List<ReportTypeModel>();
            }
        }

        public static List<MedIndividualDetail> getReportMedIndividualDetail(string empId, string typeFormId, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getReportMedIndividualDetail"
                     , new SqlParameter("@empId", empId)
                     , new SqlParameter("@typeFormId", typeFormId)
                      , new SqlParameter("@startDate", startDate)
                       , new SqlParameter("@endDate", endDate));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new MedIndividualDetail()
                             {
                                 activityNo = d["activityNo"].ToString(),
                                 documentDate = d["documentDate"].ToString(),
                                 hospPercent = d["hospPercent"].ToString() == "" ? 0 : int.Parse(AppCode.checkNullorEmpty(d["hospPercent"].ToString())),
                                 amount = d["amount"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["amount"].ToString())),
                                 amountLimit = d["amountLimit"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["amountLimit"].ToString())),
                                 amountCumulative = d["amountCumulative"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["amountCumulative"].ToString())),
                                 amountBalance = d["amountBalance"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["amountBalance"].ToString())),
                                 amountReceived = d["amountReceived"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["amountReceived"].ToString())),
                                 hospId = d["hospId"].ToString(),
                                 typeName = d["typeName"].ToString(),
                                 hospNameTH = d["hospNameTH"].ToString(),
                                 rowNo = d["rowNo"].ToString() == "" ? 0 : int.Parse(AppCode.checkNullorEmpty(d["rowNo"].ToString())),
                                 treatmentDate = DocumentsAppCode.convertDateTHToShowCultureDateTH(DateTime.Parse(d["treatmentDate"].ToString()), ConfigurationManager.AppSettings["formatDateUse"]),
                                 detail = d["detail"].ToString(),
                                 unitPrice = d["unitPrice"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["unitPrice"].ToString())),
                                 total = d["total"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["total"].ToString())),
                                 amountByDetail = d["amountByDetail"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["amountByDetail"].ToString()))
                             });
                //BaseAppCodes.converStrToDatetimeWithFormat(Request.Form["startDate"], ConfigurationManager.AppSettings["formatDateUse"])
                //.OrderBy(x=>x.documentDate).OrderBy(x => x.rowNo)
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getReportMedIndividualDetail => " + ex.Message);
                return new List<MedIndividualDetail>();
            }
        }
        public static List<MedAllDetail> getReportMedAllDetail(string empCompanyId, string empDepartmentEN, string typeFormId, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getReportMedAllDetail"
                     , new SqlParameter("@empCompanyId", empCompanyId)
                      , new SqlParameter("@empDepartmentEN", empDepartmentEN)
                     , new SqlParameter("@typeFormId", typeFormId)
                      , new SqlParameter("@startDate", startDate)
                       , new SqlParameter("@endDate", endDate));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new MedAllDetail()
                             {
                                 empId = d["empId"].ToString(),
                                 empName = d["empName"].ToString(),
                                 position = d["empPositionTitleTH"].ToString(),
                                 level = d["empLevel"].ToString(),
                                 department = d["empDepartmentEN"].ToString(),
                                 amount = d["amount"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["amount"].ToString())),
                                 amountReceived = d["amountReceived"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["amountReceived"].ToString())),
                                 amountLimit = d["amountLimit"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["amountLimit"].ToString())),
                                 amountBalance = d["amountBalance"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["amountBalance"].ToString())),
                             });

                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getReportMedAllDetail => " + ex.Message);
                return new List<MedAllDetail>();
            }
        }

    }
}