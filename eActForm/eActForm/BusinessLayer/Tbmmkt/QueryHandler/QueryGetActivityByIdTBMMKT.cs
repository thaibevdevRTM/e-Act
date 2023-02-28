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
    public class QueryGetActivityByIdTBMMKT
    {

        public static List<ActivityFormTBMMKT> getActivityById(string activityId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getActivityFormByIdTBMMKT"
                 , new SqlParameter("@activityId", activityId));

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new ActivityFormTBMMKT()
                              {
                                  id = d["Id"].ToString(),
                                  statusId = int.Parse(d["statusId"].ToString()),
                                  activityNo = d["activityNo"].ToString(),
                                  activityNoRef = d["referenceActNo"].ToString(),
                                  documentDate = !string.IsNullOrEmpty(d["documentDate"].ToString()) ? DateTime.Parse(d["documentDate"].ToString()) : (DateTime?)null,
                                  activityPeriodSt = !string.IsNullOrEmpty(d["activityPeriodSt"].ToString()) ? DateTime.Parse(d["activityPeriodSt"].ToString()) : (DateTime?)null,
                                  activityPeriodEnd = !string.IsNullOrEmpty(d["activityPeriodEnd"].ToString()) ? DateTime.Parse(d["activityPeriodEnd"].ToString()) : (DateTime?)null,
                                  activityName = d["activityName"].ToString(),
                                  objective = d["objective"].ToString(),
                                  remark = d["remark"].ToString(),
                                  companyId = d["companyId"].ToString(),
                                  master_type_form_id = d["master_type_form_id"].ToString(),
                                  benefit = d["benefit"].ToString(),
                                  languageDoc = d["languageDoc"].ToString(),
                                  companyNameEN = d["companyNameEN"].ToString(),
                                  companyNameTH = d["companyNameTH"].ToString(),
                                  reference = d["reference"].ToString(),
                                  customerId = d["customerId"].ToString(),
                                  costPeriodSt = !string.IsNullOrEmpty(d["costPeriodSt"].ToString()) ? DateTime.Parse(d["costPeriodSt"].ToString()) : (DateTime?)null,
                                  costPeriodEnd = !string.IsNullOrEmpty(d["costPeriodEnd"].ToString()) ? DateTime.Parse(d["costPeriodEnd"].ToString()) : (DateTime?)null,
                                  activityDetail = d["activityDetail"].ToString(),
                                  empId = d["empId"].ToString(),
                                  delFlag = bool.Parse(d["delFlag"].ToString()),
                                  createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                  createdByUserId = d["createdByUserId"].ToString(),
                                  updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                  updatedByUserId = d["updatedByUserId"].ToString(),
                                  createdByName = "คุณ" + d["createdByName"].ToString(),
                                  createdByNameEN = d["createdByNameEN"].ToString(),
                                  piorityDoc = d["piorityDoc"].ToString(),
                                  customerName = string.IsNullOrEmpty(d["cusShortName"].ToString()) ? d["customerName"].ToString() : d["customerName"].ToString() + "(" + d["cusShortName"].ToString() + ")",
                                  cusShortName = d["cusShortName"].ToString(),
                                  chanel = d["channelName"].ToString(),
                                  chanelShort = d["chanelShort"].ToString(),
                                  chanel_Id = d["chanel_Id"].ToString(),
                                  regionId = d["regionId"].ToString(),
                                  regionName = d["regionName"].ToString() + "(" + d["regionShort"].ToString() + ")",
                                  regionShort = d["regionShort"].ToString(),
                                  productCateText = d["productCateText"].ToString(),
                                  productCateId = d["productCateId"].ToString(),
                                  productGroupText = d["productGroupText"].ToString(),
                                  productGroupId = d["productGroupId"].ToString(),
                                  productBrandId = d["brandId"].ToString(),
                                  productTypeId = d["productTypeId"].ToString(),
                                  groupShort = d["groupShort"].ToString(),
                                  brandName = d["brandName"].ToString(),
                                  shortBrand = d["shortBrand"].ToString(),
                                  theme = d["theme"].ToString(),
                                  txttheme = d["activitySales"].ToString(),
                                  trade = d["trade"].ToString(),
                                  chkAddIO = !string.IsNullOrEmpty(d["chkAddIO"].ToString()) ? bool.Parse(d["chkAddIO"].ToString()) : false,
                                  actClaim = d["actClaim"].ToString(),
                                  actIO = d["actIO"].ToString(),
                                  actEO = d["EO"].ToString(),
                                  statusNote = d["statusNote"].ToString(),
                                  isTemp = !string.IsNullOrEmpty(d["isTemp"].ToString()) ? bool.Parse(d["isTemp"].ToString()) : false,
                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getActivityById => " + ex.Message);
                return new List<ActivityFormTBMMKT>();
            }
        }


        public static List<ActivityFormTBMMKT> getAllActivityFormByEmpId(String typeFormId, string empId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getAllActivityFormByEmpId"
                 , new SqlParameter("@typeFormId", typeFormId)
                 , new SqlParameter("@empId", empId));

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new ActivityFormTBMMKT()
                              {
                                  id = d["Id"].ToString(),
                                  statusId = int.Parse(d["statusId"].ToString()),
                                  activityNo = d["activityNo"].ToString(),
                                  documentDate = !string.IsNullOrEmpty(d["documentDate"].ToString()) ? DateTime.Parse(d["documentDate"].ToString()) : (DateTime?)null,
                                  companyId = d["companyId"].ToString(),
                                  master_type_form_id = d["master_type_form_id"].ToString(),
                                  empId = d["empId"].ToString(),

                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getActivityById => " + ex.Message);
                return new List<ActivityFormTBMMKT>();
            }
        }


    }
}