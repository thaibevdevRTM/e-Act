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
    public class QueryGetActivityFormDetailOtherByActivityId
    {
        public static List<TB_Act_ActivityForm_DetailOther> getByActivityId(string activityId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getTB_Act_ActivityForm_DetailOtherByActivityId"
                 , new SqlParameter("@activityId", activityId));

                var result = (from DataRow d in ds.Tables[0].Rows
                              select new TB_Act_ActivityForm_DetailOther()
                              {
                                  Id = d["Id"].ToString(),
                                  activityId = d["activityId"].ToString(),
                                  channelId = d["channelId"].ToString(),
                                  productBrandId = d["productBrandId"].ToString(),
                                  SubjectId = d["SubjectId"].ToString(),
                                  activityProduct = d["activityProduct"].ToString(),
                                  activityTel = d["activityTel"].ToString(),
                                  EO = d["EO"].ToString(),
                                  IO = d["IO"].ToString(),
                                  descAttach = d["descAttach"].ToString(),
                                  BudgetNumber = d["BudgetNumber"].ToString(),
                                  delFlag = bool.Parse(d["delFlag"].ToString()),
                                  createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                  createdByUserId = d["createdByUserId"].ToString(),
                                  updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                  updatedByUserId = d["updatedByUserId"].ToString(),
                                  groupName = d["groupName"].ToString(),
                                  brand_select = d["brand_select"].ToString(),
                                  costCenter = d["costCenter"].ToString(),
                                  channelRegionName = d["channelRegionName"].ToString(),
                                  glNo = d["glNo"].ToString(),
                                  glName = d["glName"].ToString(),
                                  toName = d["toName"].ToString(),
                                  toAddress = d["toAddress"].ToString(),
                                  toContact = d["toContact"].ToString(),
                                  detailContact = d["detailContact"].ToString(),
                                  fiscalYear = d["fiscalYear"].ToString(),
                                  APCode = d["APCode"].ToString(),
                                  payNo = d["payNo"].ToString(),
                                  activityIdNoSub = d["activityIdNoSub"].ToString(),
                                  totalnormalCostEstimate = d["totalnormalCostEstimate"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["totalnormalCostEstimate"].ToString())),
                                  totalvat = d["totalvat"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["totalvat"].ToString())),
                                  totalnormalCostEstimateWithVat = d["totalnormalCostEstimateWithVat"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["totalnormalCostEstimateWithVat"].ToString())),
                                  totalallPayByIO = d["totalallPayByIO"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["totalallPayByIO"].ToString())),
                                  totalallPayNo = d["totalallPayNo"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["totalallPayNo"].ToString())),
                                  totalallPayByIOBalance = d["totalallPayByIOBalance"].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(d["totalallPayByIOBalance"].ToString())),
                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getActivityById => " + ex.Message);
                return new List<TB_Act_ActivityForm_DetailOther>();
            }
        }

    }
}