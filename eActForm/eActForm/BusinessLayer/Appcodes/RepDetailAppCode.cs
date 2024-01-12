using eActForm.Models;
using iTextSharp.text;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace eActForm.BusinessLayer
{
    public class RepDetailAppCode
    {
        public static void reGenPDFReportDetail(string actRepDetailId, string gridHtml)
        {
            try
            {
                string fileName = string.Format(ConfigurationManager.AppSettings["rootRepDetailPdftURL"], actRepDetailId);
                var rootPath = HttpContext.Current.Server.MapPath(fileName);
                List<Attachment> file = AppCode.genPdfFile(gridHtml, new Document(PageSize.A3.Rotate(), 25, 10, 10, 10), rootPath);
            }
            catch (Exception ex)
            {
                throw new Exception("genFilePDF >> " + ex.Message);
            }
        }

        public static void genFilePDFBrandGroup(string actRepDetailId, string gridHtml, string htmlOS, string htmlEst, string htmlWA, string htmlSO)
        {
            try
            {
                string fileName = string.Format(ConfigurationManager.AppSettings["rootRepDetailPdftURL"], actRepDetailId);
                var rootPath = HttpContext.Current.Server.MapPath(fileName);
                List<Attachment> file = AppCode.genPdfFile(gridHtml, new Document(PageSize.A3.Rotate(), 25, 10, 10, 10), rootPath);
                TB_Act_Image_Model.ImageModel imageFormModel = new TB_Act_Image_Model.ImageModel();
                int resultImg = 0;

                if (!string.IsNullOrEmpty(htmlOS))
                {
                    fileName = "OS&JJ_" + actRepDetailId;
                    rootPath = HttpContext.Current.Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootRepDetailPdftURL"], fileName));
                    file = AppCode.genPdfFile(htmlOS, new Document(PageSize.A3.Rotate(), 25, 10, 10, 10), rootPath);
                    imageFormModel.activityId = actRepDetailId;
                    imageFormModel.imageType = AppCode.ApproveType.Report_Detail.ToString();
                    imageFormModel._fileName = fileName;
                    resultImg = ImageAppCode.insertImageForm(imageFormModel);
                }

                if (!string.IsNullOrEmpty(htmlEst))
                {
                    fileName = "Est&HP_" + actRepDetailId;
                    rootPath = HttpContext.Current.Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootRepDetailPdftURL"], fileName));
                    file = AppCode.genPdfFile(htmlEst, new Document(PageSize.A3.Rotate(), 25, 10, 10, 10), rootPath);
                    imageFormModel._fileName = fileName;
                    resultImg = ImageAppCode.insertImageForm(imageFormModel);
                }

                if (!string.IsNullOrEmpty(htmlWA))
                {
                    fileName = "WA&CY_" + actRepDetailId;
                    rootPath = HttpContext.Current.Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootRepDetailPdftURL"], fileName));
                    file = AppCode.genPdfFile(htmlWA, new Document(PageSize.A3.Rotate(), 25, 10, 10, 10), rootPath);
                    imageFormModel._fileName = fileName;
                    resultImg = ImageAppCode.insertImageForm(imageFormModel);
                }

                if (!string.IsNullOrEmpty(htmlSO))
                {
                    fileName = "SO&WR_" + actRepDetailId;
                    rootPath = HttpContext.Current.Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootRepDetailPdftURL"], fileName));
                    file = AppCode.genPdfFile(htmlSO, new Document(PageSize.A3.Rotate(), 25, 10, 10, 10), rootPath);
                    imageFormModel._fileName = fileName;
                    resultImg = ImageAppCode.insertImageForm(imageFormModel);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("genFilePDF >> " + ex.Message);
            }
        }


        public static int getRepDetailStatus(string repDetailId)
        {
            try
            {

                object obj = SqlHelper.ExecuteScalar(AppCode.StrCon, CommandType.StoredProcedure, "usp_getRepDetailStatus"
                    , new SqlParameter[] { new SqlParameter("@repDetailId", repDetailId) });

                return int.Parse(obj.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("getRepDetailStatus >> " + ex.Message);
            }
        }
        public static List<ApproveModel.approveDetailModel> getUserCreateRepDetailForm(string actId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getUserCreateRepDetailForm"
                    , new SqlParameter[] { new SqlParameter("@actFormId", actId) });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ApproveModel.approveDetailModel(dr["createdByUserId"].ToString())
                             {
                                 empId = dr["empId"].ToString(),
                                 delFlag = (bool)dr["delFlag"],
                                 createdDate = (DateTime?)dr["createdDate"],
                                 createdByUserId = dr["createdByUserId"].ToString(),
                                 updatedDate = (DateTime?)dr["updatedDate"],
                                 updatedByUserId = dr["updatedByUserId"].ToString()
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getUserCreateRepDetailForm >>" + ex.Message);
            }
        }

        public static RepDetailModel.actFormRepDetails getFilterRepDetailByActNo(RepDetailModel.actFormRepDetails model, string actNo)
        {
            try
            {

                model.actFormRepDetailGroupLists = model.actFormRepDetailGroupLists.Where(r => r.activityNo == actNo).ToList();
                model.actFormRepDetailLists = model.actFormRepDetailLists.Where(r => r.activityNo == actNo).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterRepDetailByActNo >>" + ex.Message);
            }
        }
        public static RepDetailModel.actFormRepDetails getFilterRepDetailByStatusId(RepDetailModel.actFormRepDetails model, string statusId)
        {
            try
            {
                if (statusId == ((int)AppCode.ApproveStatus.เพิ่มเติม).ToString())
                {
                    model.actFormRepDetailGroupLists = model.actFormRepDetailGroupLists.Where(r => r.createdDate >= r.activityPeriodSt).ToList();
                    model.actFormRepDetailLists = model.actFormRepDetailLists.Where(r => r.createdDate >= r.activityPeriodSt).ToList();
                }
                else
                {
                    model.actFormRepDetailGroupLists = model.actFormRepDetailGroupLists.Where(r => r.statusId == statusId && r.createdDate < r.activityPeriodSt).ToList();
                    model.actFormRepDetailLists = model.actFormRepDetailLists.Where(r => r.statusId == statusId && r.createdDate < r.activityPeriodSt).ToList();
                }

                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterRepDetailByActNo >>" + ex.Message);
            }
        }
        public static RepDetailModel.actFormRepDetails getFilterRepDetailByCustomer(RepDetailModel.actFormRepDetails model, string customerId)
        {
            try
            {
                model.actFormRepDetailGroupLists = model.actFormRepDetailGroupLists.Where(r => r.customerId == customerId).ToList();
                model.actFormRepDetailLists = model.actFormRepDetailLists.Where(r => r.customerId == customerId).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterRepDetailByActNo >>" + ex.Message);
            }
        }

        public static RepDetailModel.actFormRepDetails getFilterRepDetailByRegion(RepDetailModel.actFormRepDetails model, string regionId)
        {
            try
            {
                model.actFormRepDetailGroupLists = model.actFormRepDetailGroupLists.Where(r => r.regionId == regionId).ToList();
                model.actFormRepDetailLists = model.actFormRepDetailLists.Where(r => r.regionId == regionId).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterRepDetailByActNo >>" + ex.Message);
            }
        }
        public static RepDetailModel.actFormRepDetails getFilterRepDetailByActivity(RepDetailModel.actFormRepDetails model, string activityId)
        {
            try
            {
                model.actFormRepDetailGroupLists = model.actFormRepDetailGroupLists.Where(r => r.theme == activityId).ToList();
                model.actFormRepDetailLists = model.actFormRepDetailLists.Where(r => r.theme == activityId).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterRepDetailByActNo >>" + ex.Message);
            }
        }
        public static RepDetailModel.actFormRepDetails getFilterRepDetailByProductType(RepDetailModel.actFormRepDetails model, string productType)
        {
            try
            {
                model.actFormRepDetailLists = model.actFormRepDetailLists.Where(r => r.productTypeId == productType).ToList();
                model.actFormRepDetailGroupLists = model.actFormRepDetailGroupLists.Where(r => r.productTypeId == productType).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterRepDetailByProductType >>" + ex.Message);
            }
        }
        public static RepDetailModel.actFormRepDetails getFilterRepDetailByProductGroup(RepDetailModel.actFormRepDetails model, string productGroup)
        {
            try
            {
                model.actFormRepDetailLists = model.actFormRepDetailLists.Where(r => r.productGroupid == productGroup).ToList();
                model.actFormRepDetailGroupLists = model.actFormRepDetailGroupLists.Where(r => r.productGroupid == productGroup).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterRepDetailByActNo >>" + ex.Message);
            }
        }
        public static RepDetailModel.actFormRepDetails getRepDetailReportByCreateDateAndStatusId(DateTime startDate, DateTime endDate, string typeForm, string productType)
        {
            try
            {
                DataSet ds = new DataSet();
                if (UtilsAppCode.Session.User.isAdmin || UtilsAppCode.Session.User.isSuperAdmin || UtilsAppCode.Session.User.isAdminOMT)
                {
                    string stored = "";
                    if (typeForm == Activity_Model.activityType.MT.ToString())
                    {
                        stored = "usp_getReportDetailByCreateDate";
                    }
                    else if (typeForm == Activity_Model.activityType.MT_AddOn.ToString())
                    {
                        stored = "usp_getReportDetail_MT_AddOn";
                    }
                    else if (typeForm == Activity_Model.activityType.OMT.ToString())
                    {
                        stored = "usp_getReportDetailOMTByCreateDate";
                    }
                    else if (typeForm == Activity_Model.activityType.OMT_AddOn.ToString())
                    {
                        stored = "usp_getReportDetail_OMT_AddOn";
                    }
                    else
                    {
                        stored = "usp_getReportDetailSetPriceByCreateDate"; // SetPrice  
                    }

                    ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, stored
                , new SqlParameter[] {
                        new SqlParameter("@startDate",startDate)
                        ,new SqlParameter("@endDate",endDate.AddDays(1))
                        ,new SqlParameter("@productType",productType)
                });
                }
                else
                {
                    if (typeForm == Activity_Model.activityType.MT.ToString())
                    {
                        ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getReportDetailByCreateDateAndEmp"
                        , new SqlParameter[] {
                        new SqlParameter("@startDate",startDate)
                        ,new SqlParameter("@endDate",endDate.AddDays(1))
                        ,new SqlParameter("@empId", UtilsAppCode.Session.User.empId)
                         ,new SqlParameter("@productType",productType)
                        });
                    }
                    else
                    {
                        ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getReportDetailOMTByCreateDateAndEmp"
                        , new SqlParameter[] {
                        new SqlParameter("@startDate",startDate)
                        ,new SqlParameter("@endDate",endDate.AddDays(1))
                        ,new SqlParameter("@empId", UtilsAppCode.Session.User.empId)
                         ,new SqlParameter("@productType",productType)
                        });
                    }
                }

                return typeForm == Activity_Model.activityType.MT.ToString()
                    || typeForm == Activity_Model.activityType.OMT.ToString()
                    || typeForm == Activity_Model.activityType.MT_AddOn.ToString()
                    | typeForm == Activity_Model.activityType.OMT_AddOn.ToString() ?
                    dataTableToRepDetailModels(ds) : dataTableToRepDetailSetPriceModels(ds);
            }
            catch (Exception ex)
            {
                throw new Exception("getRepDetailReportByCreateDateAndStatusId >>" + ex.Message);
            }
        }

        public static RepDetailModel.actFormRepDetails getRepDetailReportByCreateDateAndStatusId(string repDetailId)
        {
            try
            {

                string getStord = "";
                string getFormId = getActivityTypeReportDetailByActNo(repDetailId);
                if (getFormId == ConfigurationManager.AppSettings["formSetPriceMT"])
                {
                    getStord = "usp_getReportDetailSetPriceByRepDetailId";
                }
                else
                {
                    getStord = "usp_getReportDetailByRepDetailId";
                }

                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, getStord
                    , new SqlParameter[] {
                        new SqlParameter("@repDetailId",repDetailId)
                    });

                RepDetailModel.actFormRepDetails model = new RepDetailModel.actFormRepDetails();

                if (getFormId == ConfigurationManager.AppSettings["formSetPriceMT"])
                {
                    model = dataTableToRepDetailSetPriceModels(ds);
                    model.typeForm = Activity_Model.activityType.SetPrice.ToString();
                }
                else
                {
                    model = dataTableToRepDetailModels(ds);
                    model.typeForm = Activity_Model.activityType.MT.ToString();
                }

                model.actFormRepDetailGroupLists.Select(r => r.delFlag = false
                        ).ToList();
                model.actFormRepDetailLists.Select(r => r.delFlag = false
                        ).ToList();

                model.reportDetailId = repDetailId;
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getRepDetailReportByCreateDateAndStatusId >>" + ex.Message);
            }
        }


        public static List<string> getRepdetailByActNo(string actNo)
        {
            List<string> sList = new List<string>();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getRepDetailIdByActNo"
                    , new SqlParameter[] { new SqlParameter("@actNo", actNo) });

               var getList =  (from DataRow dr in ds.Tables[0].Rows
                                 select new 
                                 {
                                     id = dr["repDetailId"].ToString(),

                                 }).ToList();
                sList = getList.Select(x => x.id).ToList();

                return sList;

            }
            catch (Exception ex)
            {
                throw new Exception("getRepdetailByActNo >>" + ex.Message);
            }
        }

        public static string getActivityTypeReportDetailByActNo(string repDetailId)
        {
            string result = "";
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getActivityTypeReportByRepId"
                    , new SqlParameter[] { new SqlParameter("@repDetailId", repDetailId) });


                if (ds.Tables[0].Rows.Count > 0)
                {
                    result = ds.Tables[0].Rows[0]["master_type_form_id"].ToString();
                }
                return result;
            }
            catch (Exception ex)
            {
                result = "";
                throw new Exception("getStatusNote >>" + ex.Message);
            }
        }

        private static RepDetailModel.actFormRepDetails dataTableToRepDetailModels(DataSet ds)
        {
            try
            {
                RepDetailModel.actFormRepDetails actRepModel = new RepDetailModel.actFormRepDetails();
                actRepModel.actFormRepDetailLists = (from DataRow dr in ds.Tables[0].Rows
                                                     select new RepDetailModel.actFormRepDetailModel()
                                                     {
                                                         #region detail parse
                                                         id = dr["activityId"].ToString(),
                                                         reference = dr["reference"].ToString(),
                                                         statusId = dr["statusId"].ToString(),
                                                         statusName = dr["statusName"].ToString(),
                                                         activityNo = dr["activityNo"].ToString(),
                                                         documentDate = (DateTime?)dr["documentDate"] ?? null,
                                                         brandId = dr["brandId"].ToString(),
                                                         customerId = dr["customerId"].ToString(),
                                                         regionId = dr["regionId"].ToString(),
                                                         productCateId = dr["productCateId"].ToString(),
                                                         productGroupid = dr["productGroupid"].ToString(),
                                                         groupId = dr["groupId"].ToString(),
                                                         cusNameTH = dr["cusNameTH"].ToString(),
                                                         productId = dr["productId"].ToString(),
                                                         productName = dr["productName"].ToString(),
                                                         size = dr["size"].ToString(),
                                                         typeTheme = dr["typeTheme"].ToString(),
                                                         normalSale = dr["normalCase"] is DBNull ? 0 : Convert.ToDecimal(dr["normalCase"]),
                                                         promotionSale = dr["promotionCase"] is DBNull ? 0 : Convert.ToDecimal(dr["promotionCase"]),
                                                         total = dr["total"] is DBNull ? 0 : Convert.ToDecimal(dr["total"]),
                                                         totalCase = dr["totalCase"] is DBNull ? 0 : Convert.ToDecimal(dr["totalCase"]),
                                                         specialDisc = dr["specialDisc"] is DBNull ? 0 : Convert.ToDecimal(dr["specialDisc"]),
                                                         specialDiscBaht = dr["specialDiscBaht"] is DBNull ? 0 : Convert.ToDecimal(dr["specialDiscBaht"]),
                                                         promotionCost = dr["promotionCost"] is DBNull ? 0 : Convert.ToDecimal(dr["promotionCost"]),
                                                         channelName = dr["channelName"].ToString(),
                                                         productTypeId = dr["productTypeId"].ToString(),
                                                         activityPeriodSt = dr["activityPeriodSt"] is DBNull ? null : (DateTime?)dr["activityPeriodSt"],
                                                         activityPeriodEnd = dr["activityPeriodEnd"] is DBNull ? null : (DateTime?)dr["activityPeriodEnd"],
                                                         costPeriodSt = dr["costPeriodSt"] is DBNull ? null : (DateTime?)dr["costPeriodSt"],
                                                         costPeriodEnd = dr["costPeriodEnd"] is DBNull ? null : (DateTime?)dr["costPeriodEnd"],
                                                         activityName = dr["activityName"].ToString(),
                                                         theme = dr["theme"].ToString(),
                                                         activityDetail = dr["activityDetail"].ToString(),
                                                         compensate = dr["compensate"] is DBNull ? 0 : Convert.ToDecimal(dr["compensate"]),
                                                         delFlag = true,
                                                         createdDate = (DateTime?)dr["createdDate"],
                                                         perGrowth = dr["growth"] is DBNull ? 0 : Convert.ToDecimal(dr["growth"]),
                                                         perSE = dr["Le"] is DBNull ? 0 : Convert.ToDecimal(dr["Le"]),
                                                         perToSale = dr["perToSale"] is DBNull ? 0 : Convert.ToDecimal(dr["perToSale"]),
                                                         rowNo = int.Parse(dr["rowNo"].ToString()),
                                                         actRef = dr["actRef"].ToString(),
                                                         payment = dr["payment"].ToString(),
                                                         #endregion

                                                     }).ToList();

                actRepModel.actFormRepDetailGroupLists = actRepModel.actFormRepDetailLists
                    .GroupBy(item => new
                    {
                        item.activityNo,
                        item.activityPeriodSt,
                        item.groupId,
                        item.theme
                    })
                    .Select((group, index) => new RepDetailModel.actFormRepDetailModel
                    {

                        #region detail parse
                        id = group.First().id,
                        reference = group.First().reference,
                        statusId = group.First().statusId,
                        statusName = group.First().statusName,
                        activityNo = group.First().activityNo,
                        documentDate = group.First().documentDate,
                        brandId = group.First().brandId,
                        customerId = group.First().customerId,
                        regionId = group.First().regionId,
                        productCateId = group.First().productCateId,
                        productGroupid = group.First().productGroupid,
                        groupId = group.First().groupId,
                        cusNameTH = group.First().cusNameTH,
                        productId = group.First().productId,
                        productName = group.First().productName,
                        size = group.First().size,
                        typeTheme = group.First().typeTheme,
                        normalSale = group.Sum(x => x.normalSale),
                        promotionSale = group.Sum(x => x.promotionSale),
                        totalCase = group.Sum(x => x.totalCase),
                        total = group.Sum(x => x.total),
                        specialDisc = group.Sum(x => x.specialDisc),
                        specialDiscBaht = group.Sum(x => x.specialDiscBaht),
                        promotionCost = group.Sum(x => x.promotionCost),
                        channelName = group.First().channelName,
                        productTypeId = group.First().productTypeId,
                        activityPeriodSt = group.First().activityPeriodSt,
                        activityPeriodEnd = group.First().activityPeriodEnd,
                        costPeriodSt = group.First().costPeriodSt,
                        costPeriodEnd = group.First().costPeriodEnd,
                        activityName = group.First().activityName,
                        theme = group.First().theme,
                        activityDetail = group.First().activityDetail,
                        compensate = group.Sum(x => x.compensate),
                        delFlag = group.First().delFlag,
                        createdDate = group.First().createdDate,
                        perGrowth = group.Sum(x => x.perGrowth),
                        perSE = group.Sum(x => x.perSE),
                        perToSale = group.Sum(x => x.perToSale),
                        rowNo = group.First().rowNo,
                        actRef = group.First().actRef,
                        payment = group.First().payment
                        #endregion


                    }).OrderBy(x => x.activityNo).ThenBy(x => x.activityPeriodSt).ThenBy(x => x.rowNo).ToList();

                return actRepModel;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private static RepDetailModel.actFormRepDetails dataTableToRepDetailSetPriceModels(DataSet ds)
        {
            try
            {
                RepDetailModel.actFormRepDetails actRepModel = new RepDetailModel.actFormRepDetails();
                actRepModel.actFormRepDetailLists = (from DataRow dr in ds.Tables[0].Rows
                                                     select new RepDetailModel.actFormRepDetailModel()
                                                     {
                                                         #region detail parse
                                                         id = dr["activityId"].ToString(),
                                                         reference = dr["reference"].ToString(),
                                                         statusId = dr["statusId"].ToString(),
                                                         statusName = dr["statusName"].ToString(),
                                                         activityNo = dr["activityNo"].ToString(),
                                                         documentDate = (DateTime?)dr["documentDate"] ?? null,
                                                         brandId = dr["brandId"].ToString(),
                                                         customerId = dr["customerId"].ToString(),
                                                         regionId = dr["regionId"].ToString(),
                                                         productCateId = dr["productCateId"].ToString(),
                                                         productGroupid = dr["productGroupid"].ToString(),
                                                         cusNameTH = dr["cusNameTH"].ToString(),
                                                         productId = dr["productId"].ToString(),
                                                         productName = dr["productName"].ToString(),
                                                         size = dr["size"].ToString(),
                                                         typeTheme = dr["typeTheme"].ToString(),
                                                         specialDisc = dr["specialDisc"] is DBNull ? 0 : Convert.ToDecimal(dr["specialDisc"]),
                                                         specialDiscBaht = dr["specialDiscBaht"] is DBNull ? 0 : Convert.ToDecimal(dr["specialDiscBaht"]),
                                                         promotionCost = dr["promotionCost"] is DBNull ? 0 : Convert.ToDecimal(dr["promotionCost"]),
                                                         channelName = dr["channelName"].ToString(),
                                                         productTypeId = dr["productTypeId"].ToString(),
                                                         activityPeriodSt = dr["activityPeriodSt"] is DBNull ? null : (DateTime?)dr["activityPeriodSt"],
                                                         activityPeriodEnd = dr["activityPeriodEnd"] is DBNull ? null : (DateTime?)dr["activityPeriodEnd"],
                                                         costPeriodSt = dr["costPeriodSt"] is DBNull ? null : (DateTime?)dr["costPeriodSt"],
                                                         costPeriodEnd = dr["costPeriodEnd"] is DBNull ? null : (DateTime?)dr["costPeriodEnd"],
                                                         activityName = dr["activityName"].ToString(),
                                                         theme = dr["theme"].ToString(),
                                                         activityDetail = dr["activityDetail"].ToString(),
                                                         delFlag = true,
                                                         createdDate = (DateTime?)dr["createdDate"],
                                                         perGrowth = dr["growth"] is DBNull ? 0 : Convert.ToDecimal(dr["growth"]),
                                                         perSE = dr["Le"] is DBNull ? 0 : Convert.ToDecimal(dr["Le"]),
                                                         perToSale = dr["perToSale"] is DBNull ? 0 : Convert.ToDecimal(dr["perToSale"]),
                                                         wholeSalesPrice = dr["wholeSalesPrice"] is DBNull ? 0 : (decimal?)dr["wholeSalesPrice"],
                                                         saleIn = dr["saleIn"] is DBNull ? 0 : Convert.ToDecimal(dr["saleIn"]),
                                                         saleOut = dr["saleOut"] is DBNull ? 0 : Convert.ToDecimal(dr["saleOut"]),
                                                         discount1 = dr["discount1"] is DBNull ? 0 : Convert.ToDecimal(dr["discount1"]),
                                                         discount2 = dr["discount2"] is DBNull ? 0 : Convert.ToDecimal(dr["discount2"]),
                                                         discount3 = dr["discount3"] is DBNull ? 0 : Convert.ToDecimal(dr["discount3"]),
                                                         normalGp = dr["normalGp"] is DBNull ? 0 : Convert.ToDecimal(dr["normalGp"]),
                                                         normalCost = dr["normalCost"] is DBNull ? 0 : Convert.ToDecimal(dr["normalCost"]),
                                                         promotionGp = dr["promotionGp"] is DBNull ? 0 : Convert.ToDecimal(dr["promotionGp"]),
                                                         rsp = dr["RSP"] is DBNull ? 0 : Convert.ToDecimal(dr["RSP"]),
                                                         unitTxt = dr["unitTxt"].ToString(),
                                                         rowNo = int.Parse(dr["rowNo"].ToString()),
                                                         #endregion

                                                     }).ToList();

                actRepModel.actFormRepDetailGroupLists = actRepModel.actFormRepDetailLists
                    .GroupBy(item => new
                    {
                        item.activityNo,
                        item.productGroupid,
                        item.theme
                    })
                    .Select((group, index) => new RepDetailModel.actFormRepDetailModel
                    {

                        #region detail parse
                        id = group.First().id,
                        reference = group.First().reference,
                        statusId = group.First().statusId,
                        statusName = group.First().statusName,
                        activityNo = group.First().activityNo,
                        documentDate = group.First().documentDate,
                        brandId = group.First().brandId,
                        customerId = group.First().customerId,
                        regionId = group.First().regionId,
                        productCateId = group.First().productCateId,
                        productGroupid = group.First().productGroupid,
                        cusNameTH = group.First().cusNameTH,
                        productId = group.First().productId,
                        productName = group.First().productName,
                        size = group.First().size,
                        typeTheme = group.First().typeTheme,
                        specialDisc = group.Sum(x => x.specialDisc),
                        specialDiscBaht = group.Sum(x => x.specialDiscBaht),
                        promotionCost = group.Sum(x => x.promotionCost),
                        channelName = group.First().channelName,
                        productTypeId = group.First().productTypeId,
                        activityPeriodSt = group.First().activityPeriodSt,
                        activityPeriodEnd = group.First().activityPeriodEnd,
                        costPeriodSt = group.First().costPeriodSt,
                        costPeriodEnd = group.First().costPeriodEnd,
                        activityName = group.First().activityName,
                        theme = group.First().theme,
                        activityDetail = group.First().activityDetail,
                        delFlag = group.First().delFlag,
                        createdDate = group.First().createdDate,
                        perGrowth = group.Sum(x => x.perGrowth),
                        perSE = group.Sum(x => x.perSE),
                        perToSale = group.Sum(x => x.perToSale),
                        wholeSalesPrice = group.First().wholeSalesPrice,
                        saleIn = group.First().saleIn,
                        saleOut = group.First().saleOut,
                        discount1 = group.First().discount1,
                        discount2 = group.First().discount2,
                        discount3 = group.First().discount3,
                        normalGp = group.First().normalGp,
                        normalCost = group.First().normalCost,
                        promotionGp = group.First().promotionGp,
                        rsp = group.First().rsp,
                        unitTxt = group.First().unitTxt,
                        rowNo = group.First().rowNo,
                        #endregion


                    }).OrderBy(x => x.activityNo).ToList();

                return actRepModel;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<RepDetailModel.actApproveRepDetailModel> getActNoByRepId(string repId)
        {

            try
            {

                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getActNoByRepId"
                    , new SqlParameter[] { new SqlParameter("@repId", repId) });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new RepDetailModel.actApproveRepDetailModel()
                             {
                                 activityNo = dr["activityNo"].ToString(),

                             }).ToList();
                return lists;

            }
            catch (Exception ex)
            {
                throw new Exception("getActNoByRepId >>" + ex.Message);
            }
        }

    }
}