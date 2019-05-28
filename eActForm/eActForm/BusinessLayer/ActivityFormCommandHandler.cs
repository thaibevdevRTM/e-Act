using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    public class ActivityFormCommandHandler
    {
        public static int insertAllActivity(Activity_Model model, string activityId)
        {
            int rtn = 0;
            try
            {

                model.activityFormModel.id = activityId;
                model.activityFormModel.documentDate = DateTime.ParseExact(model.activityFormModel.dateDoc, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                model.activityFormModel.activityPeriodSt = string.IsNullOrEmpty(model.activityFormModel.str_activityPeriodSt) ? (DateTime?)null :
                   DateTime.ParseExact(model.activityFormModel.str_activityPeriodSt, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                model.activityFormModel.activityPeriodEnd = string.IsNullOrEmpty(model.activityFormModel.str_activityPeriodEnd) ? (DateTime?)null :
                   DateTime.ParseExact(model.activityFormModel.str_activityPeriodEnd, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                model.activityFormModel.costPeriodSt = string.IsNullOrEmpty(model.activityFormModel.str_costPeriodSt) ? (DateTime?)null :
                   DateTime.ParseExact(model.activityFormModel.str_costPeriodSt, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                model.activityFormModel.costPeriodEnd = string.IsNullOrEmpty(model.activityFormModel.str_costPeriodEnd) ? (DateTime?)null :
                   DateTime.ParseExact(model.activityFormModel.str_costPeriodEnd, "dd-MM-yyyy", CultureInfo.InvariantCulture); ;
                model.activityFormModel.activityNo = model.activityFormModel.activityNo != null ? model.activityFormModel.activityNo : "---";
                model.activityFormModel.createdByUserId = model.activityFormModel.createdByUserId != null ? model.activityFormModel.createdByUserId : UtilsAppCode.Session.User.empId;
                model.activityFormModel.createdDate = model.activityFormModel.createdDate == null ? DateTime.Now : model.activityFormModel.createdDate;
                model.activityFormModel.updatedByUserId = UtilsAppCode.Session.User.empId;
                model.activityFormModel.updatedDate = DateTime.Now;
                rtn = insertActivityForm(model.activityFormModel);


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

        public static string genNumberActivity(string activityId)
        {
            try
            {
                string result = string.Empty;

                List<ActivityForm> getActList = QueryGetActivityById.getActivityById(activityId);
                if (getActList.Any())
                {
                    if (getActList.FirstOrDefault().activityNo.ToString() == "---")
                    {
                        int genNumber = int.Parse(getActivityDoc(getActList.FirstOrDefault().chanel_Id).FirstOrDefault().docNo);

                        result += getActList.FirstOrDefault().trade == "term" ? "W" : "S";
                        result += getActList.FirstOrDefault().shortBrand.Trim() == "" ? getActList.FirstOrDefault().groupShort.Trim() : getActList.FirstOrDefault().shortBrand.Trim();
                        result += getActList.FirstOrDefault().chanelShort.Trim();
                        result += getActList.FirstOrDefault().cusShortName.Trim();
                        result += new ThaiBuddhistCalendar().GetYear(DateTime.Now).ToString().Substring(2, 2);
                        result += string.Format("{0:0000}", genNumber);

                    }
                    else
                    {
                        result = getActList.FirstOrDefault().activityNo.ToString();
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

        public static int insertImageForm(TB_Act_Image_Model.ImageModel model)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertImageForm"
                    , new SqlParameter[] {new SqlParameter("@activityId",model.activityId)
                    ,new SqlParameter("@imageType",model.imageType)
                    ,new SqlParameter("@image",model._image)
                    ,new SqlParameter("@fileName",model._fileName)
                    ,new SqlParameter("@extension",model.extension)
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
                ExceptionManager.WriteError(ex.Message + ">> insertImageForm");
            }

            return result;
        }


        public static int deleteImg(string fileName, string activityId)
        {

            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_deleteImgbyactivityIdName"
                    , new SqlParameter[] {new SqlParameter("@activityId",activityId)
                    ,new SqlParameter("@fileName",fileName)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> deleteImg");
            }

            return result;
        }

        public static int deleteImgById(string id)
        {

            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_deleteImgbyId"
                    , new SqlParameter[] {new SqlParameter("@id",id)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> deleteImg");
            }

            return result;
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

        protected static int insertEstimate(CostThemeDetail model)
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
                    ,new SqlParameter("@total",model.total)
                    ,new SqlParameter("@perTotal",model.perTotal)
                    ,new SqlParameter("@isShowGroup",model.isShowGroup)
                    ,new SqlParameter("@rowNo",model.rowNo)
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

    }

}