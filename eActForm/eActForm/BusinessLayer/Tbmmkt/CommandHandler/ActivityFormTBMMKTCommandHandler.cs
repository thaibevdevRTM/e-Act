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

                model.activityFormModel.id = activityId;
                model.activityFormModel.documentDate = DateTime.ParseExact(model.activityFormModel.documentDate.ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                model.activityFormModel.activityPeriodSt = string.IsNullOrEmpty(model.activityFormModel.str_activityPeriodSt) ? (DateTime?)null :
                   DateTime.ParseExact(model.activityFormModel.str_activityPeriodSt, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                model.activityFormModel.activityPeriodEnd = string.IsNullOrEmpty(model.activityFormModel.str_activityPeriodEnd) ? (DateTime?)null :
                   DateTime.ParseExact(model.activityFormModel.str_activityPeriodEnd, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                //model.activityFormModel.costPeriodSt = string.IsNullOrEmpty(model.activityFormModel.str_costPeriodSt) ? (DateTime?)null :
                //   DateTime.ParseExact(model.activityFormModel.str_costPeriodSt, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                //model.activityFormModel.costPeriodEnd = string.IsNullOrEmpty(model.activityFormModel.str_costPeriodEnd) ? (DateTime?)null :
                //   DateTime.ParseExact(model.activityFormModel.str_costPeriodEnd, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                model.activityFormModel.activityNo = string.IsNullOrEmpty(model.activityFormModel.activityNo) ? "---" : model.activityFormModel.activityNo;
                model.activityFormModel.createdByUserId = model.activityFormModel.createdByUserId != null ? model.activityFormModel.createdByUserId : UtilsAppCode.Session.User.empId;
                model.activityFormModel.createdDate = model.activityFormModel.createdDate == null ? DateTime.Now : model.activityFormModel.createdDate;
                model.activityFormModel.updatedByUserId = UtilsAppCode.Session.User.empId;
                model.activityFormModel.updatedDate = DateTime.Now;
                model.activityFormModel.delFlag = false;
                model.activityFormModel.companyId = UtilsAppCode.Session.User.empCompanyId;
                rtn = insertActivityForm(model.activityFormModel);


                int insertIndex = 1;
                if (model.TB_Act_ActivityForm_DetailOther != null)
                {
                    rtn += deleteusp_deleteTB_Act_ActivityForm_DetailOther(activityId);

                    TB_Act_ActivityForm_DetailOther tB_Act_ActivityForm_DetailOther = new TB_Act_ActivityForm_DetailOther();
                    tB_Act_ActivityForm_DetailOther.Id = Guid.NewGuid().ToString();
                    tB_Act_ActivityForm_DetailOther.activityId = activityId;
                    tB_Act_ActivityForm_DetailOther.activityProduct = model.TB_Act_ActivityForm_DetailOther.activityProduct;
                    tB_Act_ActivityForm_DetailOther.activityTel = model.TB_Act_ActivityForm_DetailOther.activityTel;
                    tB_Act_ActivityForm_DetailOther.EO = model.TB_Act_ActivityForm_DetailOther.EO;
                    tB_Act_ActivityForm_DetailOther.delFlag = false;
                    tB_Act_ActivityForm_DetailOther.createdByUserId = model.activityFormModel.createdByUserId;
                    tB_Act_ActivityForm_DetailOther.createdDate = model.activityFormModel.createdDate == null ? DateTime.Now : model.activityFormModel.createdDate;
                    tB_Act_ActivityForm_DetailOther.updatedByUserId = UtilsAppCode.Session.User.empId;
                    tB_Act_ActivityForm_DetailOther.updatedDate = DateTime.Now;

                    rtn += usp_insertTB_Act_ActivityForm_DetailOther(tB_Act_ActivityForm_DetailOther);

                    insertIndex++;
                }


                insertIndex = 1;
                if (model.CostThemeDetailOfGroupByPriceTBMMKT != null)
                {
                    rtn += deleteActivityOfEstimateByActivityId(activityId);
                    foreach (var item in model.CostThemeDetailOfGroupByPriceTBMMKT.ToList())
                    {

                        CostThemeDetailOfGroupByPriceTBMMKT costThemeDetail = new CostThemeDetailOfGroupByPriceTBMMKT();
                        costThemeDetail.id = Guid.NewGuid().ToString();
                        //costThemeDetail.productGroupId = item.productGroupId;
                        costThemeDetail.activityId = activityId;
                        costThemeDetail.activityTypeId = item.activityTypeId;
                        //costThemeDetail.typeTheme = item.typeTheme;
                        costThemeDetail.productDetail = item.productName;
                        //costThemeDetail.productId = item.productId;
                        //costThemeDetail.normalCost = item.normalCost;
                        //costThemeDetail.brandId = item.brandId;
                        //costThemeDetail.smellId = item.smellId;
                        //costThemeDetail.themeCost = item.themeCost;
                        //costThemeDetail.growth = item.growth;
                        costThemeDetail.total = item.total;
                        //costThemeDetail.perTotal = item.perTotal;
                        //costThemeDetail.unit = item.unit;
                        //costThemeDetail.compensate = item.compensate;
                        //costThemeDetail.LE = item.LE;
                        costThemeDetail.IO = item.IO;
                        costThemeDetail.rowNo = insertIndex;
                        costThemeDetail.delFlag = false;
                        //costThemeDetail.isShowGroup = item.isShowGroup;
                        costThemeDetail.createdByUserId = model.activityFormModel.createdByUserId;
                        costThemeDetail.createdDate = model.activityFormModel.createdDate == null ? DateTime.Now : model.activityFormModel.createdDate;
                        costThemeDetail.updatedByUserId = UtilsAppCode.Session.User.empId;
                        costThemeDetail.updatedDate = DateTime.Now;
                        costThemeDetail.unitPrice = item.unitPrice;

                        rtn += insertEstimate(costThemeDetail);

                        insertIndex++;
                    }
                }


                //DataTable dt = AppCode.ToDataTable(insertProductlist);
                //rtn += deleteActivityOfProductByActivityId(activityId);
                //rtn += insertProductCost(dt);


                return rtn;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> insertAllActivity");
                return rtn;
            }


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
                    //,new SqlParameter("@reference",model.reference)
                    //,new SqlParameter("@customerId",model.customerId)
                    //,new SqlParameter("@productCateId",model.productCateId)
                    //,new SqlParameter("@productGroupId",model.productGroupId)
                    //,new SqlParameter("@brandId",model.productBrandId)
                    ,new SqlParameter("@activityPeriodST",model.activityPeriodSt)
                    ,new SqlParameter("@activityPeriodEND",model.activityPeriodEnd)
                    //,new SqlParameter("@costPeriodST",model.costPeriodSt)
                    //,new SqlParameter("@costPeriodEND",model.costPeriodEnd)
                    ,new SqlParameter("@activityName",model.activityName)
                    //,new SqlParameter("@theme",model.theme)
                    ,new SqlParameter("@objective",model.objective)
                    //,new SqlParameter("@trade",model.trade)
                    //,new SqlParameter("@activityDetail",model.activityDetail)
                    ,new SqlParameter("@companyId",model.companyId)
                    ,new SqlParameter("@delFlag",model.delFlag)
                    ,new SqlParameter("@createdDate",model.createdDate)
                    ,new SqlParameter("@createdByUserId",model.createdByUserId)
                    ,new SqlParameter("@updatedDate",model.updatedDate)
                    ,new SqlParameter("@updatedByUserId",model.updatedByUserId)
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

        protected static int insertEstimate(CostThemeDetailOfGroupByPriceTBMMKT model)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertCostThemeDetail"
                    , new SqlParameter[] {new SqlParameter("@id",model.id)
                    //,new SqlParameter("@productGroupId",model.productGroupId)
                    ,new SqlParameter("@activityId",model.activityId)
                    //,new SqlParameter("@activityTypeId",model.activityTypeId)
                    //,new SqlParameter("@typeTheme",model.typeTheme)
                    ,new SqlParameter("@productDetail",model.productDetail)
                    //,new SqlParameter("@productId",model.productId)
                    //,new SqlParameter("@productName",model.productName)
                    //,new SqlParameter("@normalCost",model.normalCost)
                    //,new SqlParameter("@themeCost",model.themeCost)
                    //,new SqlParameter("@growth",model.growth)
                    //,new SqlParameter("@unit",model.unit)
                    //,new SqlParameter("@compensate",model.compensate)
                    //,new SqlParameter("@le",model.LE)
                    ,new SqlParameter("@IO",model.IO)
                    ,new SqlParameter("@total",model.total)
                    //,new SqlParameter("@perTotal",model.perTotal)
                    //,new SqlParameter("@isShowGroup",model.isShowGroup)
                    ,new SqlParameter("@rowNo",model.rowNo)
                    ,new SqlParameter("@delFlag",model.delFlag)
                    ,new SqlParameter("@createdDate",model.createdDate)
                    ,new SqlParameter("@createdByUserId",model.createdByUserId)
                    ,new SqlParameter("@updatedDate",model.updatedDate)
                    ,new SqlParameter("@updatedByUserId",model.updatedByUserId)
                    ,new SqlParameter("@unitPrice",model.unitPrice)
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

    }

}