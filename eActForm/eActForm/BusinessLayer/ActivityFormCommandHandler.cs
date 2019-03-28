using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            model.activityFormModel.id = activityId;

            model.activityFormModel.documentDate = DateTime.ParseExact(model.activityFormModel.dateDoc, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            model.activityFormModel.activityPeriodSt = string.IsNullOrEmpty(model.activityFormModel.str_activityPeriodSt) ? (DateTime?)null : 
                DateTime.ParseExact(model.activityFormModel.str_activityPeriodSt, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            model.activityFormModel.activityPeriodEnd = string.IsNullOrEmpty(model.activityFormModel.str_activityPeriodEnd) ? (DateTime?)null : 
                DateTime.ParseExact(model.activityFormModel.str_activityPeriodEnd, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            model.activityFormModel.costPeriodSt = string.IsNullOrEmpty(model.activityFormModel.str_costPeriodSt) ? (DateTime?)null : 
                DateTime.ParseExact(model.activityFormModel.str_costPeriodSt, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            model.activityFormModel.costPeriodEnd = string.IsNullOrEmpty(model.activityFormModel.str_costPeriodEnd) ? (DateTime?)null :
                DateTime.ParseExact(model.activityFormModel.str_costPeriodEnd, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            model.activityFormModel.activityNo = model.activityFormModel.activityNo != null ? model.activityFormModel.activityNo : "---";
            model.activityFormModel.createdByUserId = UtilsAppCode.Session.User.empId;
            model.activityFormModel.createdDate = model.activityFormModel.createdDate == null ? DateTime.Now : model.activityFormModel.createdDate;
            model.activityFormModel.updatedByUserId = UtilsAppCode.Session.User.empId;
            model.activityFormModel.updatedDate = DateTime.Now;
            rtn = insertActivityForm(model.activityFormModel);

           
            foreach (var item in model.productcostdetaillist1)
            {
                if (model.activityFormModel.mode != Activity_Model.modeForm.edit.ToString())
                {
                    foreach (var itemIn in item.detailGroup)
                    {
                        Productcostdetail productcostdetail = new Productcostdetail();
                        productcostdetail.id = itemIn.id;
                        productcostdetail.activityId = activityId;
                        productcostdetail.productId = itemIn.productId;
                        productcostdetail.wholeSalesPrice = item.wholeSalesPrice;
                        productcostdetail.saleIn = item.saleIn;
                        productcostdetail.saleOut = item.saleOut;
                        productcostdetail.disCount1 = item.disCount1;
                        productcostdetail.disCount2 = item.disCount2;
                        productcostdetail.disCount3 = item.disCount3;
                        productcostdetail.normalCost = item.normalCost;
                        productcostdetail.normalGp = item.normalGp;
                        productcostdetail.promotionGp = item.promotionGp;
                        productcostdetail.specialDisc = item.specialDisc;
                        productcostdetail.specialDisBaht = item.specialDiscBaht;
                        productcostdetail.promotionCost = item.promotionCost;
                        productcostdetail.delFlag = itemIn.delFlag;
                        productcostdetail.createdByUserId = UtilsAppCode.Session.User.empId;
                        productcostdetail.createdDate = model.activityFormModel.createdDate == null ? DateTime.Now : model.activityFormModel.createdDate;
                        productcostdetail.updatedByUserId = UtilsAppCode.Session.User.empId;
                        productcostdetail.updatedDate = DateTime.Now;
                        model.productcostdetaillist.Add(productcostdetail);
                    }
                }
                else
                {
                    Productcostdetail productcostdetail = new Productcostdetail();
                    productcostdetail.id = item.id;
                    productcostdetail.activityId = activityId;
                    productcostdetail.productId = item.productId;
                    productcostdetail.wholeSalesPrice = item.wholeSalesPrice;
                    productcostdetail.saleIn = item.saleIn;
                    productcostdetail.saleOut = item.saleOut;
                    productcostdetail.disCount1 = item.disCount1;
                    productcostdetail.disCount2 = item.disCount2;
                    productcostdetail.disCount3 = item.disCount3;
                    productcostdetail.normalCost = item.normalCost;
                    productcostdetail.normalGp = item.normalGp;
                    productcostdetail.promotionGp = item.promotionGp;
                    productcostdetail.specialDisc = item.specialDisc;
                    productcostdetail.specialDisBaht = item.specialDiscBaht;
                    productcostdetail.promotionCost = item.promotionCost;
                    productcostdetail.delFlag = item.delFlag;
                    productcostdetail.createdByUserId = UtilsAppCode.Session.User.empId;
                    productcostdetail.createdDate = model.activityFormModel.createdDate == null ? DateTime.Now : model.activityFormModel.createdDate;
                    productcostdetail.updatedByUserId = UtilsAppCode.Session.User.empId;
                    productcostdetail.updatedDate = DateTime.Now;
                    model.productcostdetaillist.Add(productcostdetail);
                }
            }



            foreach (var item in model.activitydetaillist)
            {
                if (model.activityFormModel.mode != Activity_Model.modeForm.edit.ToString())
                {
                    foreach (var itemIn in item.detailGroup)
                    {
                        CostThemeDetail costThemeDetail = new CostThemeDetail();
                        costThemeDetail.id = itemIn.id;
                        costThemeDetail.activityId = activityId;
                        costThemeDetail.activityTypeId = item.activityTypeId;
                        costThemeDetail.typeTheme = item.typeTheme;
                        costThemeDetail.productId = itemIn.productId;
                        costThemeDetail.normalCost = item.normalCost;
                        costThemeDetail.themeCost = item.themeCost;
                        costThemeDetail.growth = item.growth;
                        costThemeDetail.total = item.total;
                        costThemeDetail.perTotal = item.perTotal;
                        costThemeDetail.delFlag = itemIn.delFlag;
                        costThemeDetail.createdByUserId = UtilsAppCode.Session.User.empId;
                        costThemeDetail.createdDate = model.activityFormModel.createdDate == null ? DateTime.Now : model.activityFormModel.createdDate;
                        costThemeDetail.updatedByUserId = UtilsAppCode.Session.User.empId;
                        costThemeDetail.updatedDate = DateTime.Now;
                        model.costthemedetail.Add(costThemeDetail);
                    }
                }
                else
                {
                    CostThemeDetail costThemeDetail = new CostThemeDetail();
                    costThemeDetail.id = item.id;
                    costThemeDetail.activityId = activityId;
                    costThemeDetail.activityTypeId = item.activityTypeId;
                    costThemeDetail.typeTheme = item.typeTheme;
                    costThemeDetail.productId = item.productId;
                    costThemeDetail.normalCost = item.normalCost;
                    costThemeDetail.themeCost = item.themeCost;
                    costThemeDetail.growth = item.growth;
                    costThemeDetail.total = item.total;
                    costThemeDetail.perTotal = item.perTotal;
                    costThemeDetail.delFlag = item.delFlag;
                    costThemeDetail.createdByUserId = UtilsAppCode.Session.User.empId;
                    costThemeDetail.createdDate = model.activityFormModel.createdDate == null ? DateTime.Now : model.activityFormModel.createdDate;
                    costThemeDetail.updatedByUserId = UtilsAppCode.Session.User.empId;
                    costThemeDetail.updatedDate = DateTime.Now;
                    model.costthemedetail.Add(costThemeDetail);
                }
            }

            DataTable dt = AppCode.ToDataTable<Productcostdetail>(model.productcostdetaillist);
            rtn += deleteActivityOfProductByActivityId(activityId);
            rtn += insertProductCost(dt);

            DataTable dt1 = AppCode.ToDataTable<CostThemeDetail>(model.costthemedetail);
            rtn += deleteActivityOfEstimateByActivityId(activityId);
            rtn += insertCostThemeDetail(dt1);

            return rtn;
        }





        public static string genNumberActivity(string activityId)
        {
            try
            {
                string result = string.Empty;

                List<ActivityForm> getActList = QueryGetActivityById.getActivityById(activityId);
                int genNumber = int.Parse(getActivityDoc(getActList.FirstOrDefault().customerId).FirstOrDefault().docNo);

                result += getActList.FirstOrDefault().trade == "term" ? "S" : "W";
                result += getActList.FirstOrDefault().groupShort.Trim();
                result += getActList.FirstOrDefault().chanelShort.Trim();
                result += getActList.FirstOrDefault().cusShortName.Trim();
                result += DateTime.Today.Year.ToString().Substring(2, 2);
                result += string.Format("{0:0000}", genNumber);

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

                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_updateStatusActivityForm"
                    , new SqlParameter[] {new SqlParameter("@statusId",statusId)
                    ,new SqlParameter("@activityId",activityId)
                    ,new SqlParameter("@genActivityDoc",genNumDoc)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> updateStatusActivity");
            }

            return result;
        }




        public static List<TB_Act_ActivityFormDocNo_Model> getActivityDoc(string cusId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertDocNoByCusId"
                , new SqlParameter("@cusId", cusId));

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