using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    public class ActivityFormTBMMKTCommandHandler
    {
        public static int insertAllActivity(Activity_TBMMKT_Model model, string activityId)
        {
            int rtn = 0;
            try
            {

                if (model.activityFormModel.mode == "edit" && model.activityFormTBMMKT.statusId == 2 && UtilsAppCode.Session.User.isAdminTBM)//ถ้าเป็น บัญชีเข้ามาเพื่อกรอก IO
                {
                    rtn = ProcessInsertEstimate(rtn, model, activityId);
                    rtn = ProcessInsertTB_Act_ActivityForm_DetailOther(rtn, model, activityId);
                }
                else
                {
                    model.activityFormModel.id = activityId;
                    model.activityFormModel.statusId = 1;
                    model.activityFormModel.documentDate = model.activityFormModel.documentDate;
                    model.activityFormModel.activityPeriodSt = string.IsNullOrEmpty(model.activityFormModel.activityPeriodSt.ToString()) ? (DateTime?)null : model.activityFormModel.activityPeriodSt;
                    model.activityFormModel.activityPeriodEnd = string.IsNullOrEmpty(model.activityFormModel.activityPeriodEnd.ToString()) ? (DateTime?)null : model.activityFormModel.activityPeriodEnd;
                    model.activityFormModel.activityNo = string.IsNullOrEmpty(model.activityFormModel.activityNo) ? "---" : model.activityFormModel.activityNo;
                    model.activityFormModel.createdByUserId = model.activityFormModel.createdByUserId != null ? model.activityFormModel.createdByUserId : UtilsAppCode.Session.User.empId;
                    model.activityFormModel.createdDate = model.activityFormModel.createdDate == null ? DateTime.Now : model.activityFormModel.createdDate;
                    model.activityFormModel.updatedByUserId = UtilsAppCode.Session.User.empId;
                    model.activityFormModel.updatedDate = DateTime.Now;
                    model.activityFormModel.delFlag = false;
                    model.activityFormModel.companyId = UtilsAppCode.Session.User.empCompanyId;
                    model.activityFormModel.remark = model.activityFormModel.remark;
                    model.activityFormModel.master_type_form_id = model.activityFormTBMMKT.master_type_form_id == null ? "" : model.activityFormTBMMKT.master_type_form_id;

                    rtn = insertActivityForm(model.activityFormModel);

                    rtn = ProcessInsertTB_Act_ActivityForm_DetailOther(rtn, model, activityId);

                    rtn = ProcessInsertEstimate(rtn, model, activityId);

                }



                return rtn;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> insertAllActivity");
                return rtn;
            }


        }

        public static int ProcessInsertEstimate(int rtn,Activity_TBMMKT_Model model, string activityId)
        {
            int insertIndex = 1;
            if (model.costThemeDetailOfGroupByPriceTBMMKT != null)
            {
                rtn += deleteActivityOfEstimateByActivityId(activityId);
                foreach (var item in model.costThemeDetailOfGroupByPriceTBMMKT.ToList())
                {

                    CostThemeDetailOfGroupByPriceTBMMKT costThemeDetail = new CostThemeDetailOfGroupByPriceTBMMKT();
                    costThemeDetail.id = Guid.NewGuid().ToString();
                    costThemeDetail.activityId = activityId;
                    costThemeDetail.activityTypeId = item.activityTypeId;
                    costThemeDetail.productDetail = item.productDetail;
                    costThemeDetail.total = item.total;
                    costThemeDetail.IO = item.IO;
                    costThemeDetail.rowNo = insertIndex;
                    costThemeDetail.delFlag = false;
                    costThemeDetail.createdByUserId = model.activityFormModel.createdByUserId;
                    costThemeDetail.createdDate = model.activityFormModel.createdDate == null ? DateTime.Now : model.activityFormModel.createdDate;
                    costThemeDetail.updatedByUserId = UtilsAppCode.Session.User.empId;
                    costThemeDetail.updatedDate = DateTime.Now;
                    costThemeDetail.unit = item.unit;
                    costThemeDetail.unitPrice = item.unitPriceDisplay == null ? 0 : decimal.Parse(item.unitPriceDisplay.Replace(",", ""));
                    costThemeDetail.QtyName = item.QtyName;
                    costThemeDetail.remark = item.remark == null ? "" : item.remark;

                    rtn += insertEstimate(costThemeDetail);

                    insertIndex++;
                }
            }
            return rtn;
        }

        public static int ProcessInsertTB_Act_ActivityForm_DetailOther(int rtn, Activity_TBMMKT_Model model, string activityId)
        {
            int insertIndex = 1;
            if (model.tB_Act_ActivityForm_DetailOther != null)
            {
                rtn += deleteusp_deleteTB_Act_ActivityForm_DetailOther(activityId);

                TB_Act_ActivityForm_DetailOther tB_Act_ActivityForm_DetailOther = new TB_Act_ActivityForm_DetailOther();
                tB_Act_ActivityForm_DetailOther.Id = Guid.NewGuid().ToString();
                tB_Act_ActivityForm_DetailOther.activityId = activityId;

                if (model.activityFormTBMMKT.selectedBrandOrChannel == "Brand")
                {
                    tB_Act_ActivityForm_DetailOther.productBrandId = model.activityFormTBMMKT.BrandlId;
                }
                else if (model.activityFormTBMMKT.selectedBrandOrChannel == "Channel")
                {
                    tB_Act_ActivityForm_DetailOther.channelId = model.activityFormTBMMKT.channelId;
                }

                tB_Act_ActivityForm_DetailOther.SubjectId = model.activityFormTBMMKT.SubjectId;
                tB_Act_ActivityForm_DetailOther.activityProduct = model.tB_Act_ActivityForm_DetailOther.activityProduct;
                tB_Act_ActivityForm_DetailOther.activityTel = model.tB_Act_ActivityForm_DetailOther.activityTel;
                tB_Act_ActivityForm_DetailOther.IO = model.tB_Act_ActivityForm_DetailOther.IO;
                tB_Act_ActivityForm_DetailOther.EO = model.tB_Act_ActivityForm_DetailOther.EO;
                tB_Act_ActivityForm_DetailOther.descAttach = model.tB_Act_ActivityForm_DetailOther.descAttach;
                tB_Act_ActivityForm_DetailOther.delFlag = false;
                tB_Act_ActivityForm_DetailOther.createdByUserId = model.activityFormModel.createdByUserId;
                tB_Act_ActivityForm_DetailOther.createdDate = model.activityFormModel.createdDate == null ? DateTime.Now : model.activityFormModel.createdDate;
                tB_Act_ActivityForm_DetailOther.updatedByUserId = UtilsAppCode.Session.User.empId;
                tB_Act_ActivityForm_DetailOther.updatedDate = DateTime.Now;
                tB_Act_ActivityForm_DetailOther.BudgetNumber = model.tB_Act_ActivityForm_DetailOther.BudgetNumber;

                rtn += usp_insertTB_Act_ActivityForm_DetailOther(tB_Act_ActivityForm_DetailOther);

                insertIndex++;
            }
            return rtn;
        }

        public static Activity_TBMMKT_Model getDataForEditActivity(string activityId)
        {
            Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
            try
            {
                activity_TBMMKT_Model.activityFormTBMMKT = QueryGetActivityByIdTBMMKT.getActivityById(activityId).FirstOrDefault(); // TB_Act_ActivityForm
                activity_TBMMKT_Model.activityFormModel = activity_TBMMKT_Model.activityFormTBMMKT;
                activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther = QueryGetActivityFormDetailOtherByActivityId.getByActivityId(activityId).FirstOrDefault(); // TB_Act_ActivityForm_DetailOther                
                activity_TBMMKT_Model.costThemeDetailOfGroupByPriceTBMMKT = QueryGetActivityEstimateByActivityId.getByActivityId(activityId);  //TB_Act_ActivityOfEstimate

                Decimal? totalCostThisActivity = 0;
                foreach (var item in activity_TBMMKT_Model.costThemeDetailOfGroupByPriceTBMMKT)
                {
                    totalCostThisActivity += item.total;
                }

                activity_TBMMKT_Model.totalCostThisActivity = totalCostThisActivity;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> getDataForEditActivityTBMMKT");
            }
            return activity_TBMMKT_Model;
        }

        public static int updateActivityForm(Activity_Model model, string activityId)
        {
            int rtn = 0;
            try
            {
                model.activityFormModel.id = activityId;
                model.activityFormModel.activityPeriodSt = string.IsNullOrEmpty(model.activityFormModel.str_activityPeriodSt) ? (DateTime?)null :
                 DateTime.ParseExact(model.activityFormModel.str_activityPeriodSt, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                model.activityFormModel.activityPeriodEnd = string.IsNullOrEmpty(model.activityFormModel.str_activityPeriodEnd) ? (DateTime?)null :
                DateTime.ParseExact(model.activityFormModel.str_activityPeriodEnd, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                model.activityFormModel.costPeriodSt = string.IsNullOrEmpty(model.activityFormModel.str_costPeriodSt) ? (DateTime?)null :
                 DateTime.ParseExact(model.activityFormModel.str_costPeriodSt, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                model.activityFormModel.costPeriodEnd = string.IsNullOrEmpty(model.activityFormModel.str_costPeriodEnd) ? (DateTime?)null :
                  DateTime.ParseExact(model.activityFormModel.str_costPeriodEnd, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                model.activityFormModel.updatedByUserId = UtilsAppCode.Session.User.empId;
                model.activityFormModel.updatedDate = DateTime.Now;
                rtn = updateActivityForm(model.activityFormModel);
                return rtn;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> updateActivityForm");
                return rtn;
            }
        }

        public static string[] genNumberActivity(string activityId)
        {
            try
            {

                String[] result = new String[2];
                List<ActivityForm> getActList = QueryGetActivityById.getActivityById(activityId);
                if (getActList.Any())
                {
                    if (getActList.FirstOrDefault().activityNo.ToString() == "---")
                    {
                        if (getActList.FirstOrDefault().chanel_Id != "")
                        {
                            int genNumber = int.Parse(getActivityDoc(getActList.FirstOrDefault().chanel_Id).FirstOrDefault().docNo);

                            result[0] += getActList.FirstOrDefault().trade == "term" ? "W" : "S";
                            result[0] += getActList.FirstOrDefault().shortBrand.Trim();
                            result[0] += getActList.FirstOrDefault().chanelShort.Trim();
                            result[0] += getActList.FirstOrDefault().cusShortName.Trim();
                            result[0] += new ThaiBuddhistCalendar().GetYear(DateTime.Now).ToString().Substring(2, 2);
                            result[0] += string.Format("{0:0000}", genNumber);
                            result[1] = Activity_Model.activityType.MT.ToString();
                        }
                        else
                        {
                            int genNumber = int.Parse(getActivityDoc("region").FirstOrDefault().docNo);
                            result[0] += getActList.FirstOrDefault().trade == "term" ? "W" : "S";
                            result[0] += getActList.FirstOrDefault().shortBrand.Trim();
                            result[0] += getActList.FirstOrDefault().regionShort.Trim();
                            result[0] += getActList.FirstOrDefault().cusShortName.Trim();
                            result[0] += new ThaiBuddhistCalendar().GetYear(DateTime.Now).ToString().Substring(2, 2);
                            result[0] += string.Format("{0:0000}", genNumber);
                            result[1] = Activity_Model.activityType.OMT.ToString();
                        }
                    }
                    else
                    {
                        result[0] = getActList.FirstOrDefault().activityNo.ToString();
                        result[1] = UtilsAppCode.Session.User.empCompanyId == ConfigurationManager.AppSettings["companyId_OMT"] ? Activity_Model.activityType.OMT.ToString() : Activity_Model.activityType.MT.ToString();

                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> genNumberActivity");
                return null;
            }
        }







        public static int deleteActivityOfProductByActivityId(string activityId)
        {

            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_deleteActivityOfProductByActivityId"
                    , new SqlParameter[] {new SqlParameter("@activityId",activityId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> deleteActivityOfProductByActivityId");
            }

            return result;
        }

        public static int deleteusp_deleteTB_Act_ActivityLayout(string activityId)
        {

            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_deleteTB_Act_ActivityLayout"
                    , new SqlParameter[] {new SqlParameter("@activityId",activityId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> deleteusp_deleteTB_Act_ActivityLayout");
            }

            return result;
        }

        public static int deleteActivityOfEstimateByActivityId(string activityId)
        {

            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_deleteActivityOfEstimateByActivityId"
                    , new SqlParameter[] {new SqlParameter("@activityId",activityId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> deleteActivityOfEstimateByActivityId");
            }

            return result;
        }


        public static int deleteusp_deleteTB_Act_ActivityForm_DetailOther(string activityId)
        {

            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_deleteTB_Act_ActivityForm_DetailOther"
                    , new SqlParameter[] {new SqlParameter("@activityId",activityId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> deleteusp_deleteTB_Act_ActivityForm_DetailOther");
            }

            return result;
        }


        protected static int insertActivityForm(ActivityForm model)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertAllActivity"
                    , new SqlParameter[] {new SqlParameter("@id",model.id)
                     ,new SqlParameter("@statusId",model.statusId)
                    ,new SqlParameter("@activityNo",model.activityNo)
                    ,new SqlParameter("@documentDate",model.documentDate)
                    ,new SqlParameter("@activityPeriodST",model.activityPeriodSt)
                    ,new SqlParameter("@activityPeriodEND",model.activityPeriodEnd)
                    ,new SqlParameter("@activityName",model.activityName)
                    ,new SqlParameter("@objective",model.objective)
                    ,new SqlParameter("@companyId",model.companyId)
                    ,new SqlParameter("@delFlag",model.delFlag)
                    ,new SqlParameter("@createdDate",model.createdDate)
                    ,new SqlParameter("@createdByUserId",model.createdByUserId)
                    ,new SqlParameter("@updatedDate",model.updatedDate)
                    ,new SqlParameter("@updatedByUserId",model.updatedByUserId)
                    ,new SqlParameter("@master_type_form_id",model.master_type_form_id)
                    ,new SqlParameter("@remark",model.remark)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> insertAllActivity");
            }

            return result;
        }


        protected static int updateActivityForm(ActivityForm model)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_updateActivityForm"
                    , new SqlParameter[] {new SqlParameter("@actId",model.id)
                    ,new SqlParameter("@activityPeriodST",model.activityPeriodSt)
                    ,new SqlParameter("@activityPeriodEND",model.activityPeriodEnd)
                    ,new SqlParameter("@costPeriodST",model.costPeriodSt)
                    ,new SqlParameter("@costPeriodEND",model.costPeriodEnd)
                    ,new SqlParameter("@updatedDate",model.updatedDate)
                    ,new SqlParameter("@updatedByUserId",model.updatedByUserId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> updateActivityForm");
            }

            return result;
        }


        protected static int usp_insertTB_Act_ActivityForm_DetailOther(TB_Act_ActivityForm_DetailOther model)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertTB_Act_ActivityForm_DetailOther"
                    , new SqlParameter[] {new SqlParameter("@id",model.Id)
                    ,new SqlParameter("@activityId",model.activityId)
                    ,new SqlParameter("@productBrandId",model.productBrandId)
                    ,new SqlParameter("@channelId",model.channelId)
                    ,new SqlParameter("@SubjectId",model.SubjectId)
                    ,new SqlParameter("@activityProduct",model.activityProduct)
                    ,new SqlParameter("@activityTel",model.activityTel)
                    ,new SqlParameter("@EO",model.EO)
                    ,new SqlParameter("@descAttach",model.descAttach)
                    ,new SqlParameter("@BudgetNumber",model.BudgetNumber)
                    ,new SqlParameter("@IO",model.IO)
                    ,new SqlParameter("@delFlag",model.delFlag)
                    ,new SqlParameter("@createdDate",model.createdDate)
                    ,new SqlParameter("@createdByUserId",model.createdByUserId)
                    ,new SqlParameter("@updatedDate",model.updatedDate)
                    ,new SqlParameter("@updatedByUserId",model.updatedByUserId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> insertEstimateTBMMKT");
            }

            return result;
        }

        protected static int usp_insertusp_insertTB_Act_ActivityLayout(TB_Act_ActivityLayout model)
        {
            int result = 0;
            if (model.io == null)
            {
                model.io = "";
            }
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertTB_Act_ActivityLayout"
                    , new SqlParameter[] {new SqlParameter("@id",model.id)
                    ,new SqlParameter("@activityId",model.activityId)
                    ,new SqlParameter("@no",model.no)
                    ,new SqlParameter("@io",model.io)
                    ,new SqlParameter("@activity",model.activity)
                    ,new SqlParameter("@amount",model.amount)
                    ,new SqlParameter("@delFlag",model.delFlag)
                    ,new SqlParameter("@createdDate",model.createdDate)
                    ,new SqlParameter("@createdByUserId",model.createdByUserId)
                    ,new SqlParameter("@updatedDate",model.updatedDate)
                    ,new SqlParameter("@updatedByUserId",model.updatedByUserId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> usp_insertusp_insertTB_Act_ActivityLayout");
            }

            return result;
        }

        protected static int insertEstimate(CostThemeDetailOfGroupByPriceTBMMKT model)
        {
            int result = 0;

            if (model.IO == null)
            {
                model.IO = "";
            }

            if (model.unitPrice == null)
            {
                model.unitPrice = 0;
            }

            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertCostThemeDetail"
                    , new SqlParameter[] {new SqlParameter("@id",model.id)
                    ,new SqlParameter("@activityId",model.activityId)
                    ,new SqlParameter("@activityTypeId",model.activityTypeId)
                    ,new SqlParameter("@productDetail",model.productDetail)
                    ,new SqlParameter("@IO",model.IO)
                    ,new SqlParameter("@remark",model.remark)
                    ,new SqlParameter("@total",decimal.Parse(string.Format("{0:0.00000}", model.total)))
                    ,new SqlParameter("@rowNo",model.rowNo)
                    ,new SqlParameter("@delFlag",model.delFlag)
                    ,new SqlParameter("@createdDate",model.createdDate)
                    ,new SqlParameter("@createdByUserId",model.createdByUserId)
                    ,new SqlParameter("@updatedDate",model.updatedDate)
                    ,new SqlParameter("@updatedByUserId",model.updatedByUserId)
                    ,new SqlParameter("@unit",Convert.ToInt32(model.unit))
                    ,new SqlParameter("@unitPrice", decimal.Parse(string.Format("{0:0.00000}", model.unitPrice)))
                    ,new SqlParameter("@QtyName",model.QtyName)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> insertEstimateTBMMKT");
            }

            return result;
        }

        protected static int insertProductCost(DataTable dt)
        {
            try
            {
                int rtn = 0;
                foreach (DataRow dr in dt.Rows)
                {

                    rtn += SqlHelper.ExecuteNonQueryTypedParams(AppCode.StrCon, "usp_insertProductCostdetail", dr);
                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception("insertProductCost >> " + ex.Message);
            }
        }

        protected static int insertCostThemeDetail(DataTable dt)
        {
            try
            {
                int rtn = 0;
                foreach (DataRow dr in dt.Rows)
                {

                    rtn += SqlHelper.ExecuteNonQueryTypedParams(AppCode.StrCon, "usp_insertCostThemeDetail", dr);
                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception("insertCostThemeDetail >> " + ex.Message);
            }
        }


        public static int updateStatusGenDocActivity(string statusId, string activityId, string genNumDoc)
        {

            int result = 0;
            try
            {

                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_InsertNumDocAndStatusActFormByActId"
                    , new SqlParameter[] {new SqlParameter("@statusId",statusId)
                    ,new SqlParameter("@activityId",activityId)
                    ,new SqlParameter("@genActivityDoc",genNumDoc)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> updateStatusGenDocActivity");
            }

            return result;
        }




        public static List<TB_Act_ActivityFormDocNo_Model> getActivityDoc(string chanel_Id)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertDocNoByChanelId"
                , new SqlParameter("@chanel_Id", chanel_Id));

                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_ActivityFormDocNo_Model()
                             {
                                 docNo = d["docNo"].ToString()
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getAllProductCate => " + ex.Message);
                return new List<TB_Act_ActivityFormDocNo_Model>();
            }
        }


        public static string getStatusActivity(string actId)
        {
            string result = "";
            try
            {
                object obj = SqlHelper.ExecuteScalar(AppCode.StrCon, CommandType.StoredProcedure, "usp_getCheckStatusActivity"
                    , new SqlParameter[] { new SqlParameter("@actId", actId) });
                result = obj.ToString();
                return result;

            }
            catch (Exception ex)
            {
                return result = "";
                throw new Exception("getStatusActivity >>" + ex.Message);
            }

        }

        public static int updateIOActivity(Activity_TBMMKT_Model model, string activityId)
        {
            int rtn = 0;
            try
            {
                int insertIndex = 1;

                if (model.costThemeDetailOfGroupByPriceTBMMKT != null)
                {
                    foreach (var item in model.costThemeDetailOfGroupByPriceTBMMKT.ToList())
                    {
                        CostThemeDetailOfGroupByPriceTBMMKT costThemeDetail = new CostThemeDetailOfGroupByPriceTBMMKT();
                        costThemeDetail.id = item.id;// ไท่แน่ใจค่าจะมาหรือป่าว                       
                        costThemeDetail.IO = item.IO;
                        costThemeDetail.updatedByUserId = UtilsAppCode.Session.User.empId;
                        costThemeDetail.updatedDate = DateTime.Now;
                        rtn += usp_updateIOActivity(costThemeDetail);
                        insertIndex++;
                    }
                }
                return rtn;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> updateIOActivity");
                return rtn;
            }


        }

        protected static int usp_updateIOActivity(CostThemeDetailOfGroupByPriceTBMMKT model)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_updateIOActivity"
                    , new SqlParameter[] {new SqlParameter("@id",model.id)
                    ,new SqlParameter("@IO",model.IO)
                    ,new SqlParameter("@updatedDate",model.updatedDate)
                    ,new SqlParameter("@updatedByUserId",model.updatedByUserId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> usp_UpdateIOActivity");
            }

            return result;
        }

    }

}