using eActForm.BusinessLayer.Appcodes;
using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    public class ActivityFormCommandHandler
    {
        public static int insertAllActivity(Activity_Model model, string activityId)
        {
            int rtn = 0;
            int rtnIO = 0;
            try
            {
                model.activityFormModel.id = activityId;
                model.activityFormModel.documentDate = BaseAppCodes.converStrToDatetimeWithFormat(model.activityFormModel.dateDoc, ConfigurationManager.AppSettings["formatDateUse"]);
                model.activityFormModel.activityPeriodSt = string.IsNullOrEmpty(model.activityFormModel.str_activityPeriodSt) ? (DateTime?)null :
                  BaseAppCodes.converStrToDatetimeWithFormat(model.activityFormModel.str_activityPeriodSt, ConfigurationManager.AppSettings["formatDateUse"]);
                model.activityFormModel.activityPeriodEnd = string.IsNullOrEmpty(model.activityFormModel.str_activityPeriodEnd) ? (DateTime?)null :
                   BaseAppCodes.converStrToDatetimeWithFormat(model.activityFormModel.str_activityPeriodEnd, ConfigurationManager.AppSettings["formatDateUse"]);
                model.activityFormModel.costPeriodSt = string.IsNullOrEmpty(model.activityFormModel.str_costPeriodSt) ? (DateTime?)null :
                   BaseAppCodes.converStrToDatetimeWithFormat(model.activityFormModel.str_costPeriodSt, ConfigurationManager.AppSettings["formatDateUse"]);
                model.activityFormModel.costPeriodEnd = string.IsNullOrEmpty(model.activityFormModel.str_costPeriodEnd) ? (DateTime?)null :
                   BaseAppCodes.converStrToDatetimeWithFormat(model.activityFormModel.str_costPeriodEnd, ConfigurationManager.AppSettings["formatDateUse"]);
                model.activityFormModel.activityNo = string.IsNullOrEmpty(model.activityFormModel.activityNo) ? "---" : model.activityFormModel.activityNo;
                model.activityFormModel.createdByUserId = model.activityFormModel.createdByUserId != null ? model.activityFormModel.createdByUserId : UtilsAppCode.Session.User.empId;
                model.activityFormModel.createdDate = model.activityFormModel.createdDate == null ? DateTime.Now : model.activityFormModel.createdDate;
                model.activityFormModel.updatedByUserId = UtilsAppCode.Session.User.empId;
                model.activityFormModel.updatedDate = DateTime.Now;
                model.activityFormModel.companyId = BaseAppCodes.getCompanyIdByactivityType(model.activityFormModel.typeForm);
                rtn = insertActivityForm(model.activityFormModel);
                rtnIO = insertCliamIO(model.activityFormModel);

                int insertIndex = 1;
                List<ProductCostOfGroupByPrice> insertProductlist = new List<ProductCostOfGroupByPrice>();
                if (model.productcostdetaillist1 != null)
                {
                    foreach (var item in model.productcostdetaillist1)
                    {
                        foreach (var itemIn in item.detailGroup)
                        {
                            ProductCostOfGroupByPrice productcostdetail = new ProductCostOfGroupByPrice();
                            productcostdetail.id = itemIn.id;
                            productcostdetail.productGroupId = item.productGroupId;
                            productcostdetail.activityId = activityId;
                            productcostdetail.productId = itemIn.productId;
                            productcostdetail.wholeSalesPrice = item.wholeSalesPrice;
                            productcostdetail.saleIn = item.saleIn;
                            productcostdetail.saleOut = item.saleNormal;
                            productcostdetail.disCount1 = item.disCount1;
                            productcostdetail.disCount2 = item.disCount2;
                            productcostdetail.disCount3 = item.disCount3;
                            productcostdetail.normalCost = item.normalCost;
                            productcostdetail.normalGp = item.normalGp;
                            productcostdetail.promotionGp = item.promotionGp;
                            productcostdetail.specialDisc = item.specialDisc;
                            productcostdetail.specialDiscBaht = item.specialDiscBaht;
                            productcostdetail.promotionCost = item.promotionCost;
                            productcostdetail.isShowGroup = item.isShowGroup;
                            productcostdetail.rowNo = insertIndex;
                            productcostdetail.delFlag = itemIn.delFlag;
                            productcostdetail.createdByUserId = model.activityFormModel.createdByUserId;
                            productcostdetail.createdDate = model.activityFormModel.createdDate == null ? DateTime.Now : model.activityFormModel.createdDate;
                            productcostdetail.updatedByUserId = UtilsAppCode.Session.User.empId;
                            productcostdetail.updatedDate = DateTime.Now;
                            insertProductlist.Add(productcostdetail);
                        }
                        insertIndex++;
                    }
                }

                insertIndex = 1;
                if (model.activitydetaillist != null)
                {
                    rtn += deleteActivityOfEstimateByActivityId(activityId);
                    foreach (var item in model.activitydetaillist.ToList())
                    {

                        CostThemeDetail costThemeDetail = new CostThemeDetail();
                        costThemeDetail.id = item.id;
                        costThemeDetail.productGroupId = item.productGroupId;
                        costThemeDetail.activityId = activityId;
                        costThemeDetail.activityTypeId = item.activityTypeId;
                        costThemeDetail.typeTheme = item.typeTheme;
                        costThemeDetail.productDetail = item.productName;
                        costThemeDetail.productId = item.productId;
                        costThemeDetail.normalCost = item.normalCost;
                        costThemeDetail.brandId = item.brandId;
                        costThemeDetail.smellId = item.smellId;
                        costThemeDetail.themeCost = item.themeCost;
                        costThemeDetail.growth = item.growth;
                        costThemeDetail.total = item.total;
                        costThemeDetail.perTotal = item.perTotal;
                        costThemeDetail.unit = item.unit;
                        costThemeDetail.compensate = item.compensate;
                        costThemeDetail.LE = item.LE;

                        DateTime getDoc = DateTime.Parse(model.activityFormModel.activityPeriodSt.ToString());
                        string getYear = getDoc.Month > 9 ? getDoc.AddYears(1).ToString("yy") : getDoc.Year.ToString().Substring(2);
                        costThemeDetail.IO = "56S0" + getYear + ActFormAppCode.getDigitGroup(item.activityTypeId) + ActFormAppCode.getDigitRunnigGroup(item.productId);

                        //costThemeDetail.IO = item.IO;
                        costThemeDetail.mechanics = item.mechanics;
                        costThemeDetail.rowNo = insertIndex;
                        costThemeDetail.delFlag = item.delFlag;
                        costThemeDetail.isShowGroup = item.isShowGroup;
                        costThemeDetail.createdByUserId = model.activityFormModel.createdByUserId;
                        costThemeDetail.createdDate = model.activityFormModel.createdDate == null ? DateTime.Now : model.activityFormModel.createdDate;
                        costThemeDetail.updatedByUserId = UtilsAppCode.Session.User.empId;
                        costThemeDetail.updatedDate = DateTime.Now;
                        // model.costthemedetail.Add(costThemeDetail);

                        rtn += insertEstimate(costThemeDetail);

                        insertIndex++;
                    }
                }
                DataTable dt = AppCode.ToDataTable(insertProductlist);
                rtn += deleteActivityOfProductByActivityId(activityId);
                rtn += insertProductCost(dt);



                //DataTable dt1 = AppCode.ToDataTable(model.costthemedetail);




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
                rtn = updateClaimForm(model.activityFormModel);
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

                        string getYear = "";
                        if (getActList.FirstOrDefault().activityPeriodSt != null)
                        {
                            getYear = getActList.FirstOrDefault().activityPeriodSt.Value.Month > 9 ?
                                   new ThaiBuddhistCalendar().GetYear(getActList.FirstOrDefault().activityPeriodSt.Value.AddYears(1)).ToString().Substring(2, 2)
                                 : new ThaiBuddhistCalendar().GetYear(getActList.FirstOrDefault().activityPeriodSt.Value).ToString().Substring(2, 2);
                        }

                        if (getActList.FirstOrDefault().companyId == ConfigurationManager.AppSettings["companyId_MT"])
                        {
                            int genNumber = int.Parse(getActivityDoc(getActList.FirstOrDefault().chanel_Id, activityId).FirstOrDefault().docNo);

                            result[0] += getActList.FirstOrDefault().trade == "term" ? "W" : "S";
                            result[0] += getActList.FirstOrDefault().shortBrand.Trim();
                            result[0] += getActList.FirstOrDefault().chanelShort.Trim();
                            result[0] += getActList.FirstOrDefault().cusShortName.Trim();
                            result[0] += getYear;
                            result[0] += string.Format("{0:0000}", genNumber);
                            result[1] = Activity_Model.activityType.MT.ToString();
                        }
                        else if (getActList.FirstOrDefault().companyId == ConfigurationManager.AppSettings["companyId_OMT"])
                        {
                            int genNumber = int.Parse(getActivityDoc("running_OMT", activityId).FirstOrDefault().docNo);
                            result[0] += getActList.FirstOrDefault().trade == "term" ? "W" : "S";
                            result[0] += getActList.FirstOrDefault().shortBrand.Trim();
                            result[0] += getActList.FirstOrDefault().regionShort.Trim();
                            result[0] += getActList.FirstOrDefault().cusShortName.Trim();
                            result[0] += getYear;
                            result[0] += string.Format("{0:0000}", genNumber);
                            result[1] = Activity_Model.activityType.OMT.ToString();
                        }
                        else//other company
                        {
                            //==========แบบเก่า================
                            //int genNumber = int.Parse(getActivityDoc(Activity_Model.activityType.OtherCompany.ToString(), activityId).FirstOrDefault().docNo);
                            //var model = QueryGetActivityFormDetailOtherByActivityId.getByActivityId(activityId);
                            //result[0] += !string.IsNullOrEmpty(model.FirstOrDefault().channelId) ?
                            //    QueryGetAllChanel.getAllChanel().Where(x => x.id.Equals(model.FirstOrDefault().channelId)).FirstOrDefault().no_tbmmkt
                            //    : QueryGetAllBrand.GetAllBrand().Where(x => x.id.Equals(model.FirstOrDefault().productBrandId)).FirstOrDefault().no_tbmmkt;
                            //result[0] += getActList.FirstOrDefault().documentDate.Value.Year.ToString();
                            //result[0] += "/";
                            //result[0] += string.Format("{0:0000}", genNumber);
                            //==========แบบเก่า================

                            //=========แบบใหม่ Gen In USP=======By Peerapop=========
                            result[0] += getActivityDoc(Activity_Model.activityType.OtherCompany.ToString(), activityId).FirstOrDefault().docNo;
                            if (getActList.FirstOrDefault().companyId == ConfigurationManager.AppSettings["companyId_TBM"])
                            {
                                result[1] = Activity_Model.activityType.TBM.ToString();
                            }
                            else if (getActList.FirstOrDefault().companyId == ConfigurationManager.AppSettings["companyId_HCM"])
                            {
                                result[1] = Activity_Model.activityType.HCM.ToString();
                            }
                            else { 
                             //ffffffffff
                            }
                            //====END=====แบบใหม่ Gen In USP=======By Peerapop=========
                        }
                    }
                    else
                    {
                        result[0] = getActList.FirstOrDefault().activityNo.ToString();
                        //=====update by fream devDate 20200214=======
                        string typeFormCompany = "";
                        if (UtilsAppCode.Session.User.empCompanyId == ConfigurationManager.AppSettings["companyId_OMT"])
                        {
                            typeFormCompany = Activity_Model.activityType.OMT.ToString();
                        }
                        else if (UtilsAppCode.Session.User.empCompanyId == ConfigurationManager.AppSettings["companyId_MT"])
                        {
                            typeFormCompany = Activity_Model.activityType.MT.ToString();
                        }
                        else
                        {
                            if (getActList.FirstOrDefault().companyId == ConfigurationManager.AppSettings["companyId_TBM"])
                            {
                                typeFormCompany = Activity_Model.activityType.TBM.ToString();
                            }
                            else if (getActList.FirstOrDefault().companyId == ConfigurationManager.AppSettings["companyId_HCM"])
                            {
                                typeFormCompany = Activity_Model.activityType.HCM.ToString();
                            }
                        }
                        //==END===update by fream devDate 20200214=======
                        result[1] = typeFormCompany;
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

        public static int insertActivityForm(ActivityForm model)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertAllActivity"
                    , new SqlParameter[] {new SqlParameter("@id",model.id)
                     ,new SqlParameter("@statusId",model.statusId)
                    ,new SqlParameter("@activityNo",model.activityNo)
                    ,new SqlParameter("@documentDate",model.documentDate)
                    ,new SqlParameter("@reference",model.reference)
                    ,new SqlParameter("@customerId",model.customerId)
                    ,new SqlParameter("@productCateId",model.productCateId)
                    ,new SqlParameter("@productGroupId",model.productGroupId)
                    ,new SqlParameter("@brandId",model.productBrandId)
                    ,new SqlParameter("@activityPeriodST",model.activityPeriodSt)
                    ,new SqlParameter("@activityPeriodEND",model.activityPeriodEnd)
                    ,new SqlParameter("@costPeriodST",model.costPeriodSt)
                    ,new SqlParameter("@costPeriodEND",model.costPeriodEnd)
                    ,new SqlParameter("@activityName",model.activityName)
                    ,new SqlParameter("@theme",model.theme)
                    ,new SqlParameter("@objective",model.objective)
                    ,new SqlParameter("@trade",model.trade)
                    ,new SqlParameter("@activityDetail",model.activityDetail)
                    ,new SqlParameter("@companyId",model.companyId)
                    ,new SqlParameter("@empId",model.empId)
                    ,new SqlParameter("@remark",model.remark)
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

        protected static int insertCliamIO(ActivityForm model)
        {
            int result = 0;
            try
            {

                if (model.chkAddIO == false)
                {
                    model.actClaim = "";
                    model.actIO = "";
                }

                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertClaimIO"
                    , new SqlParameter[] {new SqlParameter("@actId",model.id)
                     ,new SqlParameter("@claim",model.actClaim)
                    ,new SqlParameter("@IO",model.actIO)
                    ,new SqlParameter("@checkbox",model.chkAddIO)
                    ,new SqlParameter("@delFlag",model.delFlag)
                    ,new SqlParameter("@createdDate",model.createdDate)
                    ,new SqlParameter("@createdByUserId",model.createdByUserId)
                    ,new SqlParameter("@updatedDate",model.updatedDate)
                    ,new SqlParameter("@updatedByUserId",model.updatedByUserId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> insertCliamIO");
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

        protected static int updateClaimForm(ActivityForm model)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_updateActivityClaim"
                    , new SqlParameter[] {new SqlParameter("@actId",model.id)
                    ,new SqlParameter("@chkAddIO",model.chkAddIO)
                    ,new SqlParameter("@actCliam",model.actClaim)
                    ,new SqlParameter("@actIO",model.actIO)
                    ,new SqlParameter("@updatedDate",model.updatedDate)
                    ,new SqlParameter("@updatedByUserId",model.updatedByUserId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> updateClaimForm");
            }

            return result;
        }

        public static int insertEstimate(CostThemeDetail model)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertCostThemeDetail"
                    , new SqlParameter[] {new SqlParameter("@id",model.id)
                     ,new SqlParameter("@productGroupId",model.productGroupId)
                    ,new SqlParameter("@activityId",model.activityId)
                    ,new SqlParameter("@activityTypeId",model.activityTypeId)
                    ,new SqlParameter("@typeTheme",model.typeTheme)
                    ,new SqlParameter("@productDetail",model.productDetail)
                    ,new SqlParameter("@productId",model.productId)
                    ,new SqlParameter("@productName",model.productName)
                    ,new SqlParameter("@normalCost",model.normalCost)
                    ,new SqlParameter("@themeCost",model.themeCost)
                    ,new SqlParameter("@growth",model.growth)
                    ,new SqlParameter("@unit",model.unit)
                    ,new SqlParameter("@compensate",model.compensate)
                    ,new SqlParameter("@le",model.LE)
                    ,new SqlParameter("@IO",model.IO)
                    ,new SqlParameter("@total",model.total)
                    ,new SqlParameter("@perTotal",model.perTotal)
                    ,new SqlParameter("@isShowGroup",model.isShowGroup)
                    ,new SqlParameter("@rowNo",model.rowNo)
                    ,new SqlParameter("@mechanics",model.mechanics)
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




        public static List<TB_Act_ActivityFormDocNo_Model> getActivityDoc(string chanel_Id, string activityId)
        {
            //ถ้ามาจาก other company คือบริษัที่ไม่ใช่ OMT กับ MT ตัวแปรchanel_Idคือส่ง ActFromId ไป By Peerapop dev date 20200214
            try
            {
                DataSet ds;
                if (chanel_Id == Activity_Model.activityType.OtherCompany.ToString())
                {
                    ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "uspInsertDocnoByDocNoTxt", new SqlParameter("@activityId", activityId));
                }
                else
                {
                    ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertDocNoByChanelId", new SqlParameter("@chanel_Id", chanel_Id));
                }

                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_ActivityFormDocNo_Model()
                             {
                                 docNo = d["docNo"].ToString()
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getActivityDoc => " + ex.Message);
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